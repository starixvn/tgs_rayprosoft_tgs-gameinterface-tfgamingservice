using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class BetTransactionSettledUnitTest
{
    [TestMethod]
    public async Task 查詢已結算注單()
    {
        var apiUrl = new Uri("https://spi-test.r4espt.com/api/v2/bet-transaction/");
        var privateToken = "0002fc0c96401e68419756f562689ed6e8e1ea398406f710d4d3f1c99b04f61b8fc0bea712b7d674f8be0dbb69a7a3aca0b02b423c73257eeb87521799ec7479218285597e314f5b18189eb8ebfc2bbf4dea799ad3b2fc74d99abb709ca86bc03d4daa5b17b078d4601152920903d18546c7335cb1008d2158f999ba0810a3643fcb";
        var startTime = DateTime.Parse("2022-12-23T05:43:37Z");
        var endTime = DateTime.Parse("2022-12-23T07:43:37Z");
        var page = 1;
        var pageSize = 100;

        var response = """
            {
                "count": 2,
                "next": null,
                "previous": null,
                "results": [
                    {
                        "id": "CCYCD185145H2900MGZHPZ494B",
                        "bet_selection": "attack",
                        "odds": 0.97,
                        "currency": "RMB",
                        "amount": 10.0,
                        "game_type_name": "DOTA 2 Attack",
                        "game_market_name": "Roll",
                        "market_option": "match",
                        "map_num": null,
                        "bet_type_name": "SPAD",
                        "competition_name": "LootBox",
                        "event_id": 12789000,
                        "event_name": "13751910",
                        "event_datetime": "2022-12-23T07:28:56.432111Z",
                        "date_created": "2022-12-23T07:29:01.892916Z",
                        "settlement_datetime": "2022-12-23T07:29:07.208033Z",
                        "modified_datetime": "2022-12-23T07:41:19.995275Z",
                        "settlement_status": "settled",
                        "result": "44",
                        "result_status": "WIN",
                        "earnings": 19.7,
                        "handicap": "50.0",
                        "is_combo": false,
                        "member_code": "4ucmqplxb029",
                        "is_unsettled": false,
                        "ticket_type": "live",
                        "malay_odds": 0.97,
                        "euro_odds": 1.97,
                        "member_odds": 0.97,
                        "member_odds_style": "hongkong",
                        "game_type_id": 39,
                        "request_source": "unknown"
                    },
                    {
                        "id": "CCYCD185145H2420MGFVMX4F01",
                        "bet_selection": "female",
                        "odds": 0.95,
                        "currency": "RMB",
                        "amount": 500.0,
                        "game_type_name": "KOG Draw (Fast)",
                        "game_market_name": "Gender",
                        "market_option": "match",
                        "map_num": null,
                        "bet_type_name": "SPXX",
                        "competition_name": "LootBox",
                        "event_id": 12788801,
                        "event_name": "13751713",
                        "event_datetime": "2022-12-23T07:24:14.867910Z",
                        "date_created": "2022-12-23T07:24:21.343061Z",
                        "settlement_datetime": "2022-12-23T07:24:30.257837Z",
                        "modified_datetime": "2022-12-23T07:33:44.682550Z",
                        "settlement_status": "settled",
                        "result": "female",
                        "result_status": "WIN",
                        "earnings": 975.0,
                        "handicap": null,
                        "is_combo": false,
                        "member_code": "4ucmqplxb029",
                        "is_unsettled": false,
                        "ticket_type": "live",
                        "malay_odds": 0.95,
                        "euro_odds": 1.95,
                        "member_odds": 0.95,
                        "member_odds_style": "hongkong",
                        "game_type_id": 27,
                        "request_source": "desktop-browser"
                    }
                ]
            }
            """;
        var query = new Dictionary<string, string>
        {
            { "from_settlement_datetime", startTime.ToUniversalTime().ToString("u").Replace(" ", "T") },
            { "to_settlement_datetime", endTime.ToUniversalTime().ToString("u").Replace(" ", "T") },
            { "page", page.ToString() },
            { "page_size", pageSize.ToString() }
        };
        var url = QueryHelpers.AddQueryString(apiUrl.ToString(), query);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Get, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .BetTransactionSettledAsync(
                apiUrl: apiUrl,
                privateToken: privateToken,
                startTime: startTime,
                endTime: endTime,
                page: page,
                pageSize: pageSize)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}
