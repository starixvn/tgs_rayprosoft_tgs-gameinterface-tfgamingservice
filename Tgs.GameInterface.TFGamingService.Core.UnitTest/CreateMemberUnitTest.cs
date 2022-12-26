using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.TFGamingService.Core.Exceptions;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class CreateMemberUnitTest
{
    [TestMethod]
    public async Task 創建玩家成功()
    {
        var memberCode = "4ucmqplxkuyd";
        var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/members/");
        var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";

        var response = """
                {
                    "member_code": "4ucmqplxkuyd",
                    "combo_limit_amount": null,
                    "spread_group": "A",
                    "db_min_bet_amount": null,
                    "inplay_min_bet_amount": null,
                    "limit_amount_per_event": null,
                    "db_limit_amount_per_event": null,
                    "inplay_limit_amount_per_event": null,
                    "bet_type_group_limits": []
                }
            """;

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, apiUrl.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        await sut
            .CreateMemberAsync(
                apiUrl: apiUrl,
                privateToken: privateToken,
                memberCode: memberCode)
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task 玩家已經存在()
    {
        var memberCode = "4ucmqplxb029";
        var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/members/");
        var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";

        var response = """
                {
                    "errors": {
                        "member_code": [
                            "Already exists"
                        ]
                    },
                    "code": 1
                }
            """;
        var query = new Dictionary<string, string>();
        var url = QueryHelpers.AddQueryString(apiUrl.ToString(), query);
        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url)
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        _ = await Assert
        .ThrowsExceptionAsync<GameProviderResponseErrorException>(() =>
            sut
            .CreateMemberAsync(
                apiUrl: apiUrl,
                privateToken: privateToken,
                memberCode: memberCode)
            .AsTask())
        .ConfigureAwait(false);
    }
}
