using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class BetTransactionUnSettledUnitTest
{
    [TestMethod]
    public async Task 查詢未結算注單()
    {
        var apiUrl = new Uri("https://spi-test.r4espt.com/api/v2/bet-transaction/");
        var privateToken = "0002fc0c96401e68419756f562689ed6e8e1ea398406f710d4d3f1c99b04f61b8fc0bea712b7d674f8be0dbb69a7a3aca0b02b423c73257eeb87521799ec7479218285597e314f5b18189eb8ebfc2bbf4dea799ad3b2fc74d99abb709ca86bc03d4daa5b17b078d4601152920903d18546c7335cb1008d2158f999ba0810a3643fcb";
        var startTime = DateTime.Parse("2022-12-23T05:43:37Z");
        var endTime = DateTime.Parse("2022-12-23T07:43:37Z");
        var pageSize = 100;

        var response = """
            {
                "count": 2,
                "next": null,
                "previous": null,
                "results": [
                    {
                        "id": "CCYCD185145H2111EFCBCEF",
                        "bet_selection": "home",
                        "odds": 0.29,
                        "currency": "RMB",
                        "amount": 50.0,
                        "game_type_name": "CS:GO",
                        "game_market_name": "WINNER",
                        "market_option": "match",
                        "map_num": null,
                        "bet_type_name": "WIN",
                        "competition_name": "2023 Gunz Blazing Tournament",
                        "event_id": 12549123,
                        "event_name": "100 Thieves vs 1337HUANIA",
                        "event_datetime": "2022-12-23T15:59:59Z",
                        "date_created": "2022-12-23T07:21:11.536249Z",
                        "settlement_datetime": null,
                        "modified_datetime": "2022-12-23T07:21:11.665549Z",
                        "settlement_status": "confirmed",
                        "result": null,
                        "result_status": null,
                        "earnings": null,
                        "handicap": null,
                        "is_combo": false,
                        "member_code": "4ucmqplxb029",
                        "is_unsettled": false,
                        "ticket_type": "db",
                        "malay_odds": 0.29,
                        "euro_odds": 1.29,
                        "member_odds": 0.29,
                        "member_odds_style": "hongkong",
                        "game_type_id": 1,
                        "request_source": "desktop-browser"
                    },
                    {
                        "id": "CCYCD185145H1911PUO9A14",
                        "bet_selection": "home",
                        "odds": 0.29,
                        "currency": "RMB",
                        "amount": 50.0,
                        "game_type_name": "CS:GO",
                        "game_market_name": "WINNER",
                        "market_option": "match",
                        "map_num": null,
                        "bet_type_name": "WIN",
                        "competition_name": "2023 Gunz Blazing Tournament",
                        "event_id": 12549070,
                        "event_name": "00Nation vs 0to100",
                        "event_datetime": "2022-12-23T15:59:59Z",
                        "date_created": "2022-12-23T07:19:11.147311Z",
                        "settlement_datetime": null,
                        "modified_datetime": "2022-12-23T07:19:11.297105Z",
                        "settlement_status": "confirmed",
                        "result": null,
                        "result_status": null,
                        "earnings": null,
                        "handicap": null,
                        "is_combo": false,
                        "member_code": "4ucmqplxb029",
                        "is_unsettled": false,
                        "ticket_type": "db",
                        "malay_odds": 0.29,
                        "euro_odds": 1.29,
                        "member_odds": 0.29,
                        "member_odds_style": "hongkong",
                        "game_type_id": 1,
                        "request_source": "desktop-browser"
                    }
                ]
            }
            """;

        var uriBuilder = new UriBuilder(apiUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["settlement_status"] = "confirmed";
        query["from_datetime"] = startTime.ToUniversalTime().ToString("u").Replace(" ", "T");
        query["to_datetime"] = endTime.ToUniversalTime().ToString("u").Replace(" ", "T");
        query["page_size"] = pageSize.ToString();
        uriBuilder.Query = query.ToString();

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, uriBuilder.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .BetTransactionUnSettledAsync(
                apiUrl: apiUrl,
                privateToken: privateToken,
                startTime: startTime,
                endTime: endTime,
                pageSize: pageSize)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}
