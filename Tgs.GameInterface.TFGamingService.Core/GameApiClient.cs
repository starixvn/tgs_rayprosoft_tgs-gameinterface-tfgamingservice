using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.TFGamingService.Core.Exceptions;
using Tgs.GameInterface.TFGamingService.Core.Models;

namespace Tgs.GameInterface.TFGamingService.Core;

internal class GameApiClient(
    HttpClient client,
    ILogger<GameApiClient> logger) : IGameApiClient
{
    public async ValueTask CreateMemberAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["member_code"] = memberCode,
        };

        var url = new Uri(apiUrl, "/api/v2/members/");

        var jsonNode = await PostAsync(
            url,
            parameters,
            privateToken,
            "CreateMember",
            cancellationToken).ConfigureAwait(false);

        var errors = jsonNode["errors"]?.AsObject().ToJsonString();
        if (!string.IsNullOrEmpty(errors))
        {
            var code = jsonNode["code"]!.GetValue<int>();
            throw new GameProviderResponseErrorException(code.ToString(), errors);
        }
    }

    public async ValueTask<MemberBalanceResult> MemberBalanceAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["LoginName"] = memberCode,
        };

        var url = new Uri(apiUrl, "/api/v2/balance/");

        var jsonNode = await GetAsync(
            url,
            parameters,
            privateToken,
            "MemberBalance",
            cancellationToken).ConfigureAwait(false);

        var results = jsonNode["results"]!.AsArray();

        if (results.Count == 0)
            return new MemberBalanceResult(false, 0);

        var balance = results.First()?["balance"]?.GetValue<decimal>() ?? 0M;

        return new MemberBalanceResult(true, balance);
    }

    public async ValueTask<DepositResult> DepositAsync(Uri apiUrl, string privateToken, string operatorId, string member, decimal amount, string referenceNo, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["member"] = member,
            ["operator_id"] = operatorId,
            ["amount"] = amount.ToString(),
            ["reference_no"] = referenceNo,
        };

        var url = new Uri(apiUrl, "/api/v2/deposit/");

        var jsonNode = await PostAsync(
            url,
            parameters,
            privateToken,
            "Deposit",
            cancellationToken).ConfigureAwait(false);

        var errors = jsonNode["errors"]?.AsObject().ToJsonString();
        if (!string.IsNullOrEmpty(errors))
        {
            var code = jsonNode["code"]!.GetValue<int>();
            throw new GameProviderResponseErrorException(code.ToString(), errors);
        }

        var balance = jsonNode["balance_amount"]!.GetValue<decimal>();

        return new DepositResult(referenceNo, balance);
    }

    public async ValueTask<WithdrawResult> WithdrawAsync(Uri apiUrl, string privateToken, string operatorId, string member, decimal amount, string referenceNo, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["member"] = member,
            ["operator_id"] = operatorId,
            ["amount"] = amount.ToString(),
            ["reference_no"] = referenceNo,
        };

        var url = new Uri(apiUrl, "/api/v2/withdraw/");

        var jsonNode = await PostAsync(
            url,
            parameters,
            privateToken,
            "Withdraw",
            cancellationToken).ConfigureAwait(false);

        var errors = jsonNode["errors"]?.AsObject().ToJsonString();
        if (!string.IsNullOrEmpty(errors))
        {
            var code = jsonNode["code"]!.GetValue<int>();
            throw new GameProviderResponseErrorException(code.ToString(), errors);
        }

        var balance = jsonNode["balance_amount"]!.GetValue<decimal>();

        return new WithdrawResult(referenceNo, balance);
    }

    public async ValueTask<bool> PartnerAccountLogoutAsync(Uri apiUrl, string privateToken, string member, string operatorId, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["member"] = member,
            ["operator_id"] = operatorId,
        };

        var url = new Uri(apiUrl, "/api/v2/partner-account-logout/");

        var jsonNode = await PostAsync(
            url,
            parameters,
            privateToken,
            "PartnerAccountLogout",
            cancellationToken).ConfigureAwait(false);

        return jsonNode["member"] is not null;
    }

    public async IAsyncEnumerable<BetTransactionSettledResult> BetTransactionSettledAsync(Uri apiUrl, string privateToken, DateTime startTime, DateTime endTime, int pageSize = 1000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["from_settlement_datetime"] = startTime.ToUniversalTime().ToString("u").Replace(" ", "T"),
            ["to_settlement_datetime"] = endTime.ToUniversalTime().ToString("u").Replace(" ", "T"),
            ["page_size"] = pageSize.ToString(),
        };

        var url = new Uri(apiUrl, "/api/v2/bet-transaction/");

        var jsonNode = await GetAsync(
            url,
            parameters,
            privateToken,
            "BetTransaction",
            cancellationToken).ConfigureAwait(false);

        var results = jsonNode["results"]!.AsArray();

        var bets = results.Select(x => new BetTransactionSettledResult
        {
            PlayerCode = x!["member_code"]!.ToString(),
            ExternalRoundId = x["id"]!.ToString(),
            ExternalCreatedOn = DateTimeOffset.Parse(x["date_created"]!.ToString()),
            ExternalClosedOn = DateTimeOffset.Parse(x["settlement_datetime"]!.ToString()),
            Bet = x["amount"]!.GetValue<decimal>(),
            ResultStatus = x["result_status"] == null
                ? null
                : x["result_status"]!.ToString(),
            Earnings = x["earnings"] == null
                ? null
                : x["earnings"]!.GetValue<decimal>(),
            AdditionalData = x.ToString()
        }) ?? Enumerable.Empty<BetTransactionSettledResult>();

        if (!bets.Any())
        {
            yield break;
        }

        foreach (var bet in bets)
        {
            yield return bet;
        }
    }

    public async IAsyncEnumerable<BetTransactionUnSettledResult> BetTransactionUnSettledAsync(Uri apiUrl, string privateToken, DateTime startTime, DateTime endTime, int pageSize = 1000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["settlement_status"] = "confirmed",
            ["from_datetime"] = startTime.ToUniversalTime().ToString("u").Replace(" ", "T"),
            ["to_datetime"] = endTime.ToUniversalTime().ToString("u").Replace(" ", "T"),
            ["page_size"] = pageSize.ToString(),
        };

        var url = new Uri(apiUrl, "/api/v2/bet-transaction/");

        var document = await GetAsync(
            url,
            parameters,
            privateToken,
            "BetTransaction",
            cancellationToken).ConfigureAwait(false);

        var results = document["results"]!.AsArray();

        var openBets = results.Select(x => new BetTransactionUnSettledResult
        {
            MemberCode = x!["member_code"]!.ToString(),
            Id = x["id"]!.ToString(),
            DateCreated = DateTimeOffset.Parse(x["date_created"]!.ToString()),
            Amount = x["amount"]!.GetValue<decimal>(),
            AdditionalData = x.ToString()
        }) ?? Enumerable.Empty<BetTransactionUnSettledResult>();

        if (!openBets.Any())
        {
            yield break;
        }

        foreach (var bet in openBets)
        {
            yield return bet;
        }
    }

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string privateToken,
        string referenceNo,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["reference_no"] = referenceNo,
        };
        var url = new Uri(apiUrl, "/api/v2/transfer-status/");

        var jsonNode = await GetAsync(
            url,
            parameters,
            privateToken,
            "FundStatus",
            cancellationToken).ConfigureAwait(false);

        var jsonResults = jsonNode["results"]!.AsArray();

        var transfers = jsonResults.Select(x => new FundStatusResult
        {
            PartnerReferenceNo = x!["partner_reference_no"]!.ToString(),
            Status = x["status"]!.ToString(),
        }) ?? Enumerable.Empty<FundStatusResult>();

        if (!transfers.Any())
            return false;

        var transfer = transfers.FirstOrDefault(x => x.PartnerReferenceNo == referenceNo);

        return transfer is not null && transfer.Status == "success";
    }

    public async IAsyncEnumerable<TransferStatusResult> TransferHistoryAsync(
        Uri apiUrl,
        string privateToken,
        string? referenceNo,
        int? page,
        int? pageSize,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>();

        if (referenceNo is not null)
            parameters.Add("reference_no", referenceNo);

        if (page is not null)
            parameters.Add("page", page.Value.ToString());

        if (pageSize is not null)
            parameters.Add("page_size", pageSize.Value.ToString());

        var url = new Uri(apiUrl, "/api/v2/transfer-status/");

        var jsonNode = await GetAsync(
            url,
            parameters,
            privateToken,
            "TransferStatus",
            cancellationToken).ConfigureAwait(false);

        var jsonResults = jsonNode["results"]!.AsArray();

        var transfers = jsonResults.Select(x => new TransferStatusResult
        {
            Id = x!["id"]!.GetValue<int>(),
            PartnerReferenceNo = x["partner_reference_no"]!.ToString(),
            MemberCode = x["member_code"]!.ToString(),
            OperatorName = x["operator_id"]!.ToString(),
            TransactionType = x["transaction_type"]!.ToString(),
            TransactionDate = DateTimeOffset.Parse(x["transaction_date"]!.ToString()),
            Amount = x["amount"]!.GetValue<decimal>(),
            Status = x["status"]!.ToString(),
            Balance = x["balance"]!.GetValue<decimal>(),
        }) ?? Enumerable.Empty<TransferStatusResult>();

        if (!transfers.Any())
        {
            yield break;
        }

        foreach (var transfer in transfers)
        {
            yield return transfer;
        }
    }

    public async ValueTask<string?> MemberDirectLaunchAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["member_code"] = memberCode,
        };

        var url = new Uri(apiUrl, "/api/v2/member-login/");

        var jsonNode = await PostAsync(
            url,
            parameters,
            privateToken,
            "MemberDirectLaunch",
            cancellationToken).ConfigureAwait(false);

        var launchUrl = jsonNode["launch_url"]?.GetValue<string?>() ?? null;

        return launchUrl;
    }

    private async ValueTask<JsonNode> PostAsync(Uri apiUrl, IDictionary<string, string> parameters, string apiToken, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var content = new StringContent(JsonSerializer.Serialize(parameters));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiToken);
        var httpResponseMessage = await client.PostAsync(apiUrl, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr
                .ReadToEndAsync(
                cancellationToken: cancellationToken)
                .ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }

    private async ValueTask<JsonNode> GetAsync(Uri apiUrl, IEnumerable<KeyValuePair<string, string>> parameters, string apiToken, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var queryStrings = HttpUtility.ParseQueryString(string.Empty);

        foreach (var item in parameters)
        {
            queryStrings.Add(item.Key, item.Value);
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiToken);

        var httpResponseMessage = await client
            .GetAsync($"{apiUrl}?{queryStrings}", cancellationToken)
            .ConfigureAwait(false);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr
                .ReadToEndAsync(
                cancellationToken: cancellationToken)
                .ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = string.Empty,
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
