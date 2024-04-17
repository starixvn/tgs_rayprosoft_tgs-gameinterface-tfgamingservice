using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.TFGamingService.Core.UnitTest;

[TestClass]
public class TransferHistoryUnitTest
{
	[TestMethod]
	public async Task 轉帳紀錄查詢成功()
	{
		var apiUrl = new Uri("https://spi-test.r4espt.com/api/v2/transfer-status/");
		var privateToken = "0002fc0c96401e68419756f562689ed6e8e1ea398406f710d4d3f1c99b04f61b8fc0bea712b7d674f8be0dbb69a7a3aca0b02b423c73257eeb87521799ec7479218285597e314f5b18189eb8ebfc2bbf4dea799ad3b2fc74d99abb709ca86bc03d4daa5b17b078d4601152920903d18546c7335cb1008d2158f999ba0810a3643fcb";
		var referenceNo = "0001";

		var response = """
                {
                    "count": 1,
                    "next": null,
                    "previous": null,
                    "results": [
                        {
                            "id": 2264081,
                            "reference_no": "238964f2-c5f5-4760-a75d-88038326b700",
                            "partner_reference_no": "0001",
                            "operator_id": "CPI_RMB",
                            "member_code": "4ucmqplxb029",
                            "transaction_type": "IN",
                            "transaction_date": "2022-12-23T03:14:29.763649Z",
                            "amount": 2000.0,
                            "status": "success",
                            "balance": 2000.0
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
			.TransferHistoryAsync(
				apiUrl: apiUrl,
				privateToken: privateToken,
				referenceNo: referenceNo,
				page: null,
				pageSize: null)
			.ToArrayAsync()
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsTrue(actual.Length > 0);
	}
}
