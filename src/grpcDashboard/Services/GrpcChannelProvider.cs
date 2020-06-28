namespace GrpcTools.GRpcDashboard.Services
{
    using System.Net.Http;
    using Grpc.Net.Client;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class GrpcChannelProvider
    {
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<GrpcChannelProvider> logger;
        private readonly ILoggerFactory loggerFactory;

        public GrpcChannelProvider(
            ILogger<GrpcChannelProvider> logger,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment environment)
        {
            this.loggerFactory = loggerFactory;
            this.environment = environment;
            this.logger = logger;
        }

        public GrpcChannel GetChannel(string address)
        {
            var options = new GrpcChannelOptions { LoggerFactory = loggerFactory };

            if (environment.IsDevelopment())
            {
                var httpHandler = new HttpClientHandler();

                // Return `true` to allow certificates that are untrusted/invalid
                httpHandler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                options.HttpClient = new HttpClient(httpHandler);
                options.DisposeHttpClient = true;
                logger.LogWarning("DEVELOPMENT: Trusting all certificates!");
            }

            logger.LogDebug("New Channel for {address}", address);
            return GrpcChannel.ForAddress(address, options);
        }
    }
}
