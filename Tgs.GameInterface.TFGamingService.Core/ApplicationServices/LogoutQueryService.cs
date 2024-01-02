using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class LogoutQueryService(
    IGameApiClient client) : IGetService<LogoutCommand, bool>
{
    public ValueTask<bool> GetAsync(LogoutCommand command, CancellationToken cancellationToken = default)
        => client.PartnerAccountLogoutAsync(
            apiUrl: command.ApiUrl,
            privateToken: command.PrivateToken,
            member: command.Member,
            operatorId: command.OperatorId,
            cancellationToken);
}
