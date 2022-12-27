using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, string?>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string?> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => _client.MemberDirectLaunchAsync(query.ApiUrl, query.PrivateToken, query.MemberCode, cancellationToken);
}
