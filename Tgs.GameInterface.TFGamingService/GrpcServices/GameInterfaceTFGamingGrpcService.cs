using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.TFGamingService.Core.ApplicationServices;
using Tgs.GameInterface.TFGamingService.Protos;
using Tgs.ServiceUtility;
using Enum = System.Enum;

namespace Tgs.GameInterface.TFGamingService.GrpcServices;

internal class GameInterfaceTFGamingGrpcService(
	ILogger<GameInterfaceTFGamingGrpcService> logger,
	IGetService<FundInQuery, FundResult> fundInService,
	IGetService<FundOutQuery, FundResult> fundOutService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	ICommandService<PlayerCreateCommand> playerCreateService,
	ICommandService<PlayerKickCommand> playerKickService,
	IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
	IGetService<GameUrlQuery, string?> gameUrlService,
	IQueryService<FundHistoryQuery, FundHistoryResult> fundHistoryService,
	IQueryService<GameHistorySettledQuery, GameHistorySettledResult> gameHistorySettledService,
	IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult> gameHistoryUnsettledService) : GameInterfaceTFGamingService.GameInterfaceTFGamingServiceBase
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName
		});
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var query = new FundInQuery(
			ApiUrl: new Uri(request.ApiUrl),
			OperatorId: request.OperatorId,
			PrivateToken: request.PrivateToken,
			Member: request.Member,
			RefTransactionCode: request.RefTransactionCode,
			Amount: request.Amount.ToDecimal().GetValueOrDefault(0)
		);

		var result = await fundInService
				.GetAsync(
					query: query,
					cancellationToken: context.CancellationToken)
				.ConfigureAwait(false);

		return new FundReply()
		{
			RefTransactionCode = result.RefTransactionCode,
			TransactionId = result.TransactionId,
			Balance = result.Balance
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var query = new FundOutQuery(
			ApiUrl: new(request.ApiUrl),
			OperatorId: request.OperatorId,
			PrivateToken: request.PrivateToken,
			Member: request.Member,
			RefTransactionCode: request.RefTransactionCode,
			Amount: request.Amount.ToDecimal().GetValueOrDefault(0));

		var result = await fundOutService
				.GetAsync(
					query: query,
					cancellationToken: context.CancellationToken)
				.ConfigureAwait(false);

		return new FundReply()
		{
			RefTransactionCode = result.RefTransactionCode,
			TransactionId = result.TransactionId,
			Balance = result.Balance
		};
	}

	public override async Task<FundStatusReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var query = new FundStatusQuery(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			RefTransactionCode: request.RefTransactionCode);

		var result = await fundStatusService
				.GetAsync(
					query: query,
					cancellationToken: context.CancellationToken)
				.ConfigureAwait(false);

		return new FundStatusReply()
		{
			IsOrderExist = result
		};
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var query = new GameUrlQuery(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			MemberCode: request.MemberCode);

		var result = await gameUrlService
			.GetAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply()
		{
			GameUrl = result
		};
	}

	public override async Task<GameHistorySettledReply> GameHistorySettled(GameHistorySettledRequest request, ServerCallContext context)
	{
		var query = new GameHistorySettledQuery(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			StartTime: request.StartTime.ToDateTime(),
			EndTime: request.EndTime.ToDateTime(),
			PageSize: request.PageSize);

		var gameHistorySettleds = await gameHistorySettledService
				.QueryAsync(
					query: query,
					cancellationToken: context.CancellationToken)
				.ToArrayAsync(context.CancellationToken)
				.ConfigureAwait(false);
		var reply = new GameHistorySettledReply();

		if (gameHistorySettleds.Length != 0)
		{
			foreach (var gameHistorySettled in gameHistorySettleds)
			{
				reply.Items.Add(new GameHistorySettled()
				{
					PlayerCode = gameHistorySettled.PlayerCode,
					ExternalRoundId = gameHistorySettled.ExternalRoundId,
					Status = (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameHistorySettled.Status.Replace(" ", string.Empty), true),
					ExternalCreatedOn = gameHistorySettled.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = gameHistorySettled.ExternalClosedOn.ToTimestamp(),
					Bet = gameHistorySettled.Bet,
					ValidBet = gameHistorySettled.ValidBet,
					PlayerWinLoss = gameHistorySettled.PlayerWinLoss,
					Turnover = gameHistorySettled.Turnover,
					Settle = gameHistorySettled.Settle,
					AdditionalData = gameHistorySettled.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<GameHistoryUnsettledReply> GameHistoryUnsettled(GameHistoryUnsettledRequest request, ServerCallContext context)
	{
		var query = new GameHistoryUnsettledQuery(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			StartTime: request.StartTime.ToDateTime(),
			EndTime: request.EndTime.ToDateTime(),
			PageSize: request.PageSize);

		var gameHistoryUnsettleds = await gameHistoryUnsettledService
				.QueryAsync(
					query: query,
					cancellationToken: context.CancellationToken)
				.ToArrayAsync(context.CancellationToken)
				.ConfigureAwait(false);
		var reply = new GameHistoryUnsettledReply();

		if (gameHistoryUnsettleds.Length != 0)
		{
			foreach (var gameHistoryUnsettled in gameHistoryUnsettleds)
			{
				reply.Items.Add(new GameHistoryUnsettled()
				{
					PlayerCode = gameHistoryUnsettled.PlayerCode,
					ExternalRoundId = gameHistoryUnsettled.ExternalRoundId,
					ExternalCreatedOn = gameHistoryUnsettled.ExternalCreatedOn.ToTimestamp(),
					Bet = gameHistoryUnsettled.Bet,
					ValidBet = gameHistoryUnsettled.ValidBet,
					AdditionalData = gameHistoryUnsettled.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			MemberCode: request.MemberCode);

		var result = await playerBalanceService
				.GetAsync(
					query: query,
					cancellationToken: context.CancellationToken)
				.ConfigureAwait(false);

		return new PlayerBalanceReply()
		{
			IsPlayerExist = result.IsPlayerExist,
			Balance = result.Balance
		};
	}

	public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
	{
		var command = new PlayerCreateCommand(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			MemberCode: request.MemberCode);

		await playerCreateService
				.ExecuteAsync(
					command: command,
					cancellationToken: context.CancellationToken)
				.ConfigureAwait(false);

		return new BaseReply()
		{
			Status = true
		};
	}

	public override async Task<BaseReply> PlayerKick(PlayerKickRequest request, ServerCallContext context)
	{
		var command = new PlayerKickCommand(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			OperatorId: request.OperatorId,
			Member: request.Member);

		await playerKickService
			.ExecuteAsync(
				command: command,
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply()
		{
			Status = true
		};
	}

	public override async Task<FundHistoryReply> FundHistory(FundHistoryRequest request, ServerCallContext context)
	{
		var query = new FundHistoryQuery(
			ApiUrl: new(request.ApiUrl),
			PrivateToken: request.PrivateToken,
			RefTransactionCode: request.RefTransactionCode,
			Page: request.Page,
			PageSize: request.PageSize);

		var fundHistorys = await fundHistoryService
				.QueryAsync(
					query: query,
					cancellationToken: context.CancellationToken)
				.ToArrayAsync(context.CancellationToken)
				.ConfigureAwait(false);
		var reply = new FundHistoryReply();

		if (fundHistorys.Length != 0)
		{
			foreach (var fundHistory in fundHistorys)
			{
				reply.Items.Add(new FundHistory()
				{
					RefTransactionCode = fundHistory.RefTransactionCode,
					Amount = fundHistory.Amount,
					Balance = fundHistory.Balance,
					MemberCode = fundHistory.MemberCode,
					OperatorName = fundHistory.OperatorName,
					Status = fundHistory.Status,
					TransactionDate = fundHistory.TransactionDate.ToTimestamp(),
					TransactionId = fundHistory.TransactionId,
					TransactionType = fundHistory.TransactionType,
				});
			}
		}

		return reply;
	}
}
