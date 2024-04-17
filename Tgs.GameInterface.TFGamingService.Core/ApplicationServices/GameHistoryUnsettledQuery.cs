using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record GameHistoryUnsettledQuery(
	Uri ApiUrl,
	string PrivateToken,
	DateTime StartTime,
	DateTime EndTime,
	[property: Range(minimum: 100,maximum: 1000)]
	int PageSize);
