using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class PlayerKickCommandService : ICommandService<PlayerKickCommand>
{
    private readonly IGameApiClient _client;

    public PlayerKickCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
    {
        _ = await _client
            .PartnerAccountLogoutAsync(command.ApiUrl, command.PrivateToken, command.Member, command.OperatorId, cancellationToken)
            .ConfigureAwait(false);

        return;
    }
}
