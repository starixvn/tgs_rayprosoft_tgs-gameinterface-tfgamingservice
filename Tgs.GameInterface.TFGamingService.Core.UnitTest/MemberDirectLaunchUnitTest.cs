using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class MemberDirectLaunchUnitTest
{
	[TestMethod]
	public async Task 開啟遊戲正常()
	{
		var memberCode = "4ucmqplxb029";
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/member-login/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
		var launchUrl = "https://gc-test.r4espt.com/launch.html?game_launch_token=000211c425bb286d4221b16b6abf8310f7767199188ae57e42a4b1bc34b52c5e10ede5a624eeb1b7e886e64da6a6a1fdd3c636f2655ebbf50c6744a519e3516e924cd38ca8394dcbb3bba81ade62d6126a8cae19279237a373f08f679e3fd6df5a3fe0921e4823f72ecc0f24ecd93593f41738ea1c9381a43fae1a467405db3799a4";

		var response = """
            {
                "launch_url": "https://gc-test.r4espt.com/launch.html?game_launch_token=000211c425bb286d4221b16b6abf8310f7767199188ae57e42a4b1bc34b52c5e10ede5a624eeb1b7e886e64da6a6a1fdd3c636f2655ebbf50c6744a519e3516e924cd38ca8394dcbb3bba81ade62d6126a8cae19279237a373f08f679e3fd6df5a3fe0921e4823f72ecc0f24ecd93593f41738ea1c9381a43fae1a467405db3799a4"
            }
        """;

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, apiUrl.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.MemberDirectLaunchAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				memberCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(launchUrl, actual);
	}
}
