using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.TFGamingService.Protos;

namespace Tgs.GameInterface.TFGamingService.Client
{
    internal class GameInterfaceTFGamingClient : IGameInterfaceTFGamingClient
    {
        private readonly GameInterfaceTFGamingService.GameInterfaceTFGamingServiceClient _client;

        public GameInterfaceTFGamingClient(GameInterfaceTFGamingService.GameInterfaceTFGamingServiceClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async IAsyncEnumerable<FundHistory> FundHistoryAsync(
            Uri apiUrl,
            string privateToken,
            string? refTransactionCode,
            int? page,
            int? pageSize,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var request = new FundHistoryRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                RefTransactionCode = refTransactionCode,
                Page = page.GetValueOrDefault(1),
                PageSize = pageSize.GetValueOrDefault(100),
            };

            var reply = await _client
                    .FundHistoryAsync(
                        request: request,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            if (!reply.Items.Any())
            {
                yield break;
            }

            foreach (var item in reply.Items)
            {
                yield return item;
            }
        }

        public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string operatorId, string privateToken, string member, string refTransactionCode, decimal amount, CancellationToken cancellationToken = default)
        {
            var request = new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                OperatorId = operatorId,
                PrivateToken = privateToken,
                Member = member,
                RefTransactionCode = refTransactionCode,
                Amount = amount
            };

            return await _client
                    .FundInAsync(
                        request: request,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
        }

        public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string operatorId, string privateToken, string member, string refTransactionCode, decimal amount, CancellationToken cancellationToken = default)
        {
            var request = new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                OperatorId = operatorId,
                PrivateToken = privateToken,
                Member = member,
                RefTransactionCode = refTransactionCode,
                Amount = amount
            };

            return await _client
                    .FundOutAsync(
                        request: request,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
        }

        public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string privateToken, string refTransactionCode, CancellationToken cancellationToken = default)
        {
            var request = new FundStatusRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                RefTransactionCode = refTransactionCode
            };

            var reply = await _client
                    .FundStatusAsync(
                        request: request,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            return reply.IsOrderExist;
        }

        public async IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(
            Uri apiUrl,
            string privateToken,
            DateTime startTime,
            DateTime endTime,            
            int pageSize = 1000,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var request = new GameHistorySettledRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                StartTime = startTime.ToTimestamp(),
                EndTime = endTime.ToTimestamp(),                
                PageSize = pageSize,
            };

            var reply = await _client
                    .GameHistorySettledAsync(
                        request: request,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            if (!reply.Items.Any())
            {
                yield break;
            }

            foreach (var item in reply.Items)
            {
                yield return item;
            }
        }

        public async IAsyncEnumerable<GameHistoryUnsettled> GameHistoryUnsettledAsync(
            Uri apiUrl,
            string privateToken,
            DateTime startTime,
            DateTime endTime,            
            int pageSize = 1000,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var request = new GameHistoryUnsettledRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                StartTime = startTime.ToTimestamp(),
                EndTime = endTime.ToTimestamp(),                
                PageSize = pageSize,
            };

            var reply = await _client
                    .GameHistoryUnsettledAsync(
                        request: request,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            if (!reply.Items.Any())
            {
                yield break;
            }

            foreach (var item in reply.Items)
            {
                yield return item;
            }
        }

        public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellation = default)
        {
            var request = new GameUrlRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                MemberCode = memberCode,
            };

            return await _client
                .GameUrlAsync(
                    request: request,
                    cancellationToken: cancellation)
                .ConfigureAwait(false);
        }

        public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default)
        {
            var request = new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                MemberCode = memberCode,
            };

            return await _client.PlayerBalanceAsync(
                request: request,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async ValueTask PlayerCreateAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default)
        {
            var request = new PlayerCreateRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                MemberCode = memberCode,
            };

            _ = await _client
                .PlayerCreateAsync(request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async ValueTask PlayerKickAsync(Uri apiUrl, string privateToken, string operatorId, string member, CancellationToken cancellationToken = default)
        {
            var request = new PlayerKickRequest
            {
                ApiUrl = apiUrl.ToString(),
                PrivateToken = privateToken,
                OperatorId = operatorId,
                Member = member,
            };

            _ = await _client
                .PlayerKickAsync(request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
