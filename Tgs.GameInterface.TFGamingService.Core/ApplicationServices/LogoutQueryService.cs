using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class LogoutQueryService : IGetService<LogoutCommand, bool>
{
    private readonly IGameApiClient _client;

    public LogoutQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(LogoutCommand command, CancellationToken cancellationToken = default)
        => _client.PartnerAccountLogoutAsync(
            apiUrl: command.ApiUrl,
            privateToken: command.PrivateToken,
            member: command.Member,
            operatorId: command.OperatorId,
            cancellationToken);
}
