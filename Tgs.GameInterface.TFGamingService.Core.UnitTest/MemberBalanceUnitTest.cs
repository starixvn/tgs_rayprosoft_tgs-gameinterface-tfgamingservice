using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class MemberBalanceUnitTest
{
	[TestMethod]
	public async Task 查詢餘額成功()
	{
		var memberCode = "1234";
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/balance/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";

		var response = """
            {
                "count": 1,
                "next": null,
                "previous": null,
                "results": [
                    {
                        "id": 185145,
                        "operator_id": "3871",
                        "member_id": "4ucmqplxb029",
                        "currency": "RMB",
                        "balance": 11154.2,
                        "last_bet_datetime": "2022-12-23T08:16:43.546609Z",
                        "last_bet_amount": 10.0
                    }
                ]
            }
            """;

		var uriBuilder = new UriBuilder(apiUrl);
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["LoginName"] = memberCode;
		uriBuilder.Query = query.ToString();

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.MemberBalanceAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				memberCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsTrue(actual.IsPlayerExist);
		Assert.AreEqual(11154.2M, actual.Balance);
	}

	[TestMethod]
	public async Task 玩家不存在()
	{
		var memberCode = "1234";
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/balance/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";

		var response = """
            {
                "count": 0,
                "next": null,
                "previous": null,
                "results": []
            }
            """;

		var uriBuilder = new UriBuilder(apiUrl);
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["LoginName"] = memberCode;
		uriBuilder.Query = query.ToString();

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.MemberBalanceAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				memberCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsFalse(actual.IsPlayerExist);
		Assert.IsTrue(actual.Balance == 0);
	}
}
