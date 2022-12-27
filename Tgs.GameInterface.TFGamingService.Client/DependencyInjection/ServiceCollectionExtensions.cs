using Microsoft.Extensions.Options;
using Tgs.GameInterface.TFGamingService.Client;
using Tgs.GameInterface.TFGamingService.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceTFGamingServiceClient(
        this IServiceCollection services,
        Action<GameInterfaceTFGamingClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceTFGamingClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceTFGamingService.GameInterfaceTFGamingServiceClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceTFGamingClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceTFGamingClient, GameInterfaceTFGamingClient>();
}
