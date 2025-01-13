using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Dapper;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalResponseBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                // Captura respostas com status diferente de 200 e registra
                if (context.Response.StatusCode != 200)
                {
                    await LogErrorResponseAsync(context, responseBody);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                // Restaura o corpo da resposta para o Swagger
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Configura a resposta para exceções não tratadas
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var response = new
        {
            error = "Ocorreu um erro interno no servidor.",
            details = exception.Message
        };

        // Registra o erro no banco
        await LogErrorToDatabase(context, exception.Message, exception.StackTrace);

        // Escreve a resposta de erro no corpo
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private async Task LogErrorResponseAsync(HttpContext context, MemoryStream responseBody)
    {
        // Lê o corpo da resposta
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();

        // Registra no banco de dados
        using var scope = context.RequestServices.CreateScope();
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using var connection = dbConnectionFactory.CreateConnection();

        var query = @"INSERT INTO LogsErro (mensagem, stack_trace, data_hora) 
                      VALUES (@Mensagem, NULL, NOW());";

        await connection.ExecuteAsync(query, new
        {
            Mensagem = $"Erro HTTP capturado no endpoint {context.Request.Path}, StatusCode: {context.Response.StatusCode}, Detalhes: {responseText}"
        });

        responseBody.Seek(0, SeekOrigin.Begin); // Reposiciona o stream para que o Swagger possa ler
    }

    private async Task LogErrorToDatabase(HttpContext context, string mensagem, string? stackTrace)
    {
        using var scope = context.RequestServices.CreateScope();
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using var connection = dbConnectionFactory.CreateConnection();

        var query = @"INSERT INTO LogsErro (mensagem, stack_trace, data_hora) 
                      VALUES (@Mensagem, @StackTrace, NOW());";

        await connection.ExecuteAsync(query, new
        {
            Mensagem = mensagem,
            StackTrace = stackTrace
        });
    }
}
