using Tgs.GameInterface.TFGamingService.Protos;

namespace Tgs.GameInterface.TFGamingService.Client;

public interface IGameInterfaceTFGamingClient
{
    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string operatorId,
        string privateToken,
        string member,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string operatorId,
        string privateToken,
        string member,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string privateToken,
        string refTransactionCode,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<FundHistory> FundHistoryAsync(
        Uri apiUrl,
        string privateToken,        
        string? refTransactionCode,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string privateToken,
        string memberCode,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string privateToken,
        string memberCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerKickAsync(
        Uri apiUrl,
        string privateToken,
        string operatorId,
        string member,
        CancellationToken cancellationToken = default);

    ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string privateToken,
        string memberCode,
        CancellationToken cancellation = default);

    IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(
        Uri apiUrl,
        string privateToken,
        DateTime startTime,
        DateTime endTime,
        int page = 1,
        int pageSize = 100,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameHistoryUnsettled> GameHistoryUnsettledAsync(
        Uri apiUrl,
        string privateToken,
        DateTime startTime,
        DateTime endTime,
        int page = 1,
        int pageSize = 100,
        CancellationToken cancellationToken = default);
}
