using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.TFGamingService.Core.Exceptions;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class WithdrawUnitTest
{
    [TestMethod]
    public async Task 遊戲廠商包轉出主錢包成功()
    {
        var memberCode = "4ucmqplxb029";
        var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/withdraw/");
        var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
        var operatorId = "3871";
        var amount = 100;
        var referenceNo = "0001";
        var response = """
            {
                "member": "4ucmqplxkuyd",
                "operator_id": 3871,
                "amount": 100.0,
                "reference_no": "0001",
                "currency": "RMB",
                "transaction_type": "withdraw",
                "balance_amount": 1900.0
            }
            """;

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, apiUrl.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .WithdrawAsync(
                apiUrl: apiUrl,
                privateToken: privateToken,
                operatorId: operatorId,
                member: memberCode,
                amount: amount,
                referenceNo: referenceNo)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(referenceNo, actual.RefTransactionCode);
        Assert.AreEqual(1900M, actual.Balance);
    }

    [TestMethod]
    public async Task 不存在玩家提款()
    {
        var memberCode = "4ucmqplxhfoe";
        var apiUrl = new Uri($"https://spi-test.r4espt.com/api/v2/withdraw/");
        var privateToken = "00022362f50e16fa474c80fc24da29793420aff6ce4b5d05a4a7dc52a261078ce72ceef10e7dc28f7bcfe89fe4922624fe72992e1f2f4f78360e4360970ce1b3f2e4567d01b071190c9051058fc1c14202f4ea117497c29fdbe8c9eddfb0e4fa6a3efe3bcbc7cfd8665c5b311e4a7065b8ad37a1a30d64919d8a3f2ff82c2ead23eb";
        var operatorId = "3273";
        var amount = 1000;
        var referenceNo = "7890";
        var response = """
            {
                "errors": {
                    "non_field_errors": [
                        "Insufficient Funds."
                    ]
                },
                "code": 5
            }
            """;

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, apiUrl.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        _ = await Assert
        .ThrowsExceptionAsync<GameProviderResponseErrorException>(() =>
            sut
            .WithdrawAsync(
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
