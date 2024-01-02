using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class PlayerKickCommandService(
    IGameApiClient client) : ICommandService<PlayerKickCommand>
{
    public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
    {
        _ = await client
            .PartnerAccountLogoutAsync(command.ApiUrl, command.PrivateToken, command.Member, command.OperatorId, cancellationToken)
            .ConfigureAwait(false);

        return;
    }
}
