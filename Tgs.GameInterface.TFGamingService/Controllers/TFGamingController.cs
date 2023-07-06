using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.TFGamingService.Core.ApplicationServices;
using Tgs.GameInterface.TFGamingService.Protos;
using Tgs.GameInterface.TFGamingService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Controllers;

[ApiController]
[Route("tf")]
public class TFGamingController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.(GP -> Tgs)
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "operatorId": "3871",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "member": "4ucmqplxb029",
    ///             "refTransactionCode": "7891",
    ///             "amount": 100
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            OperatorId: source.OperatorId,
            PrivateToken: source.PrivateToken,
            Member: source.Member,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the out asynchronous.(Tgs -> Gp)
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "operatorId": "3871",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "member": "4ucmqplxb029",
    ///             "refTransactionCode": "1234",
    ///             "amount": 2000
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            OperatorId: source.OperatorId,
            PrivateToken: source.PrivateToken,
            Member: source.Member,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "refTransactionCode": "7890"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
                ApiUrl: source.ApiUrl,
                PrivateToken: source.PrivateToken,
                RefTransactionCode: source.RefTransactionCode);

        return service.GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Funds the history asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "page": 1,
    ///             "pageSize": 100
    ///         }
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "refTransactionCode": "0001"
    ///         }
    ///
    /// </remarks>
    [HttpPost("fund/history", Name = "FundHistory")]
    public async IAsyncEnumerable<FundHistoryResultViewModel> FundHistoryAsync(
        [FromServices] IQueryService<FundHistoryQuery, FundHistoryResult> service,
        FundHistoryViewModel model)
    {
        var query = new FundHistoryQuery(
            ApiUrl: model.ApiUrl,
            model.PrivateToken,
            RefTransactionCode: model.RefTransactionCode,
            Page: model.Page,
            PageSize: model.PageSize);

        var fundHistorys = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!fundHistorys.Any())
        {
            yield break;
        }

        foreach (var fundHistory in fundHistorys)
        {
            yield return new FundHistoryResultViewModel
            {
                RefTransactionCode = fundHistory.RefTransactionCode,
                TransactionId = fundHistory.TransactionId,
                Amount = fundHistory.Amount,
                OperatorName = fundHistory.OperatorName,
                MemberCode = fundHistory.MemberCode,
                TransactionType = fundHistory.TransactionType,
                TransactionDate = fundHistory.TransactionDate,
                Status = fundHistory.Status,
                Balance = fundHistory.Balance
            };
        }
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "memberCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            PrivateToken: source.PrivateToken,
            MemberCode: source.MemberCode);

        return service.ExecuteAsync(
                command: command,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "memberCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
       [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
       PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            PrivateToken: source.PrivateToken,
            MemberCode: source.MemberCode);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Players the kick asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "operatorId": "3871",
    ///             "member": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/kick", Name = "PlayerKick")]
    public ValueTask PlayerKickAsync(
        [FromServices] ICommandService<PlayerKickCommand> service,
        PlayerKickViewModel source)
    => service
        .ExecuteAsync(
            command: new PlayerKickCommand(
                source.ApiUrl,
                source.PrivateToken,
                source.OperatorId,
                source.Member),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "memberCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("gameurl")]
    public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, string?> service,
        GameUrlQueryViewModel model)
    {
        var result = await service
            .GetAsync(new GameUrlQuery(
                model.ApiUrl,
                model.PrivateToken,
                model.MemberCode))
            .ConfigureAwait(false);

        return new GameUrlResultViewModel(result);
    }

    /// <summary>
    /// Games the history settled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "startTime": "2022-12-23T05:43:37.223Z",
    ///             "endTime": "2022-12-23T07:43:37.223Z",
    ///             "page": 1,
    ///             "pageSize": 100
    ///         }
    /// </remarks>
    [HttpPost("game/history/settled", Name = "GameHistorySettled")]
    public async IAsyncEnumerable<GameHistorySettledResultViewModel> GameHistorySettledAsync(
        [FromServices] IQueryService<GameHistorySettledQuery, GameHistorySettledResult> service,
        GameHistorySettledViewModel model)
    {
        var query = new GameHistorySettledQuery(
            ApiUrl: model.ApiUrl,
            model.PrivateToken,
            StartTime: model.StartTime,
            EndTime: model.EndTime,
            PageSize: model.PageSize);

        var gameRecords = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameHistorySettledResultViewModel(
                PlayerCode: gameRecord.PlayerCode,
                ExternalRoundId: gameRecord.ExternalRoundId,
                Status: (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameRecord.Status.Replace(" ", string.Empty), true),
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                ExternalClosedOn: gameRecord.ExternalClosedOn,
                Bet: gameRecord.Bet,
                ValidBet: gameRecord.ValidBet,
                PlayerWinLoss: gameRecord.PlayerWinLoss,
                Turnover: gameRecord.Turnover,
                Settle: gameRecord.Settle,
                AdditionalData: gameRecord.AdditionalData);
        }
    }

    /// <summary>
    /// Games the history unsettled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://spi-test.r4espt.com/",
    ///             "privateToken": "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb",
    ///             "startTime": "2022-12-23T05:43:37.223Z",
    ///             "endTime": "2022-12-23T07:43:37.223Z",
    ///             "page": 1,
    ///             "pageSize": 100
    ///         }
    /// </remarks>
    [HttpPost("game/history/unsettled", Name = "GameHistoryUnsettled")]
    public async IAsyncEnumerable<GameHistoryUnsettledResultViewModel> GameHistoryUnsettledAsync(
        [FromServices] IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult> service,
        GameHistoryUnsettledViewModel model)
    {
        var query = new GameHistoryUnsettledQuery(
            ApiUrl: model.ApiUrl,
            model.PrivateToken,
            StartTime: model.StartTime,
            EndTime: model.EndTime,
            PageSize: model.PageSize);

        var gameRecords = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameHistoryUnsettledResultViewModel(
                PlayerCode: gameRecord.PlayerCode,
                ExternalRoundId: gameRecord.ExternalRoundId,
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                Bet: gameRecord.Bet,
                ValidBet: gameRecord.ValidBet,
                AdditionalData: gameRecord.AdditionalData);
        }
    }
}
