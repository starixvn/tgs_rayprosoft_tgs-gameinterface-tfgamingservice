using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class FundStatusUnitTest
{
	[TestMethod]
	public async Task 訂單存在()
	{
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/transfer-status/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
		var referenceNo = "7890";
		var response = """
            {
                "count": 1,
                "next": null,
                "previous": null,
                "results": [
                    {
                        "id": 2263122,
                        "reference_no": "2d4151ad-ad32-4f91-813c-5b082237e3e1",
                        "partner_reference_no": "7890",
                        "operator_id": "PEGA_RMB",
                        "member_code": "4ucmqplxkuyd",
                        "transaction_type": "OUT",
                        "transaction_date": "2022-12-22T06:20:26.115068Z",
                        "amount": 100.0,
                        "status": "success",
                        "balance": 1900.0
                    }
                ]
            }
            """;

		var uriBuilder = new UriBuilder(apiUrl);
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["reference_no"] = referenceNo;
		uriBuilder.Query = query.ToString();

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.FundStatusAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				referenceNo: referenceNo)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsTrue(actual);
	}

	[TestMethod]
	public async Task 訂單不存在()
	{
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/transfer-status/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
		var referenceNo = "9999";
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
		uriBuilder.Query = query.ToString();

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.FundStatusAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				referenceNo: referenceNo)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsFalse(actual);
	}
}
