namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record FundHistoryResult(
	string RefTransactionCode,
	string TransactionId,
	decimal Amount,
	string OperatorName,
	string MemberCode,
	string TransactionType,
	DateTimeOffset TransactionDate,
	string Status,
	decimal Balance);
