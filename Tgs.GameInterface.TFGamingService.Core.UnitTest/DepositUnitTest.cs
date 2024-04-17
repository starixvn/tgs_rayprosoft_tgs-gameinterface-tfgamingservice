using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.TFGamingService.Core.Exceptions;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class DepositUnitTest
{
	[TestMethod]
	public async Task 主錢包轉入遊戲廠商成功()
	{
		var memberCode = "4ucmqplxb029";
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/deposit/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
		var operatorId = "3871";
		var amount = 1000;
		var referenceNo = "1237";
		var response = """
            {
                "member": "4ucmqplxb029",
                "operator_id": 3871,
                "amount": 2000.0,
                "reference_no": "1237",
                "currency": "RMB",
                "transaction_type": "deposit",
                "balance_amount": 11154.2
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
			.DepositAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				operatorId: operatorId,
				member: memberCode,
				amount: amount,
				referenceNo: referenceNo)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(referenceNo, actual.RefTransactionCode);
		Assert.AreEqual(11154.2M, actual.Balance);
	}

	[TestMethod]
	public async Task 錯誤的OperatorId_主錢包轉入遊戲廠商失敗()
	{
		var memberCode = "4ucmqplxb029";
		var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/deposit/");
		var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
		var operatorId = "2";
		var amount = 1000;
		var referenceNo = "1235";
		var response = """
            {
                "errors":{
                "operator_id":[
                    "Invalid operator id"
                    ]
                },
                "code": 6
            }
            """;

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, apiUrl.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		_ = await Assert
		.ThrowsExceptionAsync<GameProviderResponseErrorException>(() =>
			sut
			.DepositAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				operatorId: operatorId,
				member: memberCode,
				amount: amount,
				referenceNo: referenceNo)
			.AsTask())
		.ConfigureAwait(false);
	}
}
