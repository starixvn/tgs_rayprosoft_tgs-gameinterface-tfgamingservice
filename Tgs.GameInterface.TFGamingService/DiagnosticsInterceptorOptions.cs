using System.Diagnostics;

namespace Tgs.GameInterface.TFGamingService;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.TFGamingService.DiagnosticsInterceptor");
}
