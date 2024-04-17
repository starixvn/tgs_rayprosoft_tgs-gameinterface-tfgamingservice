using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class PartnerAccountLogoutUnitTest
{
	[TestMethod]
	public async Task 會員登出成功()
	{
		var memberCode = "4ucmqplxb029";
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/partner-account-logout/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
		var operatorId = "3871";

		var response = """
                {
                    "member": "4ucmqplxb029"
                }
            """;

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, apiUrl.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		_ = await sut
			.PartnerAccountLogoutAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				member: memberCode,
				operatorId: operatorId)
			.ConfigureAwait(false);
	}
}
