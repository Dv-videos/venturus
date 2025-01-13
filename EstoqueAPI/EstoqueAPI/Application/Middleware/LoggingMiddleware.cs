using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

public class LoggingMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public LoggingMiddleware(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            // Passa para o próximo handler na cadeia
            return await next();
        }
        catch (Exception ex)
        {
            // Registra o erro no banco de dados
            await LogErrorToDatabase(ex, request);
            throw; // Relança a exceção para que o comportamento esperado não seja interrompido
        }
    }

    private async Task LogErrorToDatabase(Exception exception, TRequest request)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var query = @"INSERT INTO LogsErro (mensagem, stack_trace, data_hora, request) 
                      VALUES (@Mensagem, @StackTrace, @DataHora, @Request);";
        await connection.ExecuteAsync(query, new
        {
            Mensagem = exception.Message,
            StackTrace = exception.StackTrace,
            DataHora = DateTime.Now,
            Request = request.ToString() // Certifique-se de que TRequest possui uma implementação significativa de ToString()
        });
    }
}
