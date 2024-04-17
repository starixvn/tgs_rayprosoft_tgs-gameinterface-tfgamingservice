using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class PlayerCreateCommandService(
	IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> client.CreateMemberAsync(
			apiUrl: command.ApiUrl,
			privateToken: command.PrivateToken,
			memberCode: command.MemberCode,
			cancellationToken);
}
