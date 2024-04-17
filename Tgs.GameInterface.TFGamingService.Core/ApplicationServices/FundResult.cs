namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record FundResult(
	string RefTransactionCode,
	string TransactionId,
	decimal Balance);
