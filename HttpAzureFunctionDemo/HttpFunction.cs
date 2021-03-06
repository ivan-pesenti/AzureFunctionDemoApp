using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using HttpAzureFunctionDemo.Services;
using System.Collections.Generic;

namespace HttpAzureFunctionDemo
{
    public class HttpFunction
    {
        private readonly IOptionsMonitor<MyConfiguration> _myConfig;
        private readonly IOptions<MyConfigurationSecrets> _mySecrets;
        private readonly IOptionsMonitor<Serilog> _serilog;
        private readonly ILogger<HttpFunction> _logger;
        private readonly IProduct _product;
        private readonly IOrder _order;
        private readonly IConfiguration _configuration;

        public HttpFunction(IOptionsMonitor<MyConfiguration> myConfig, IOptions<MyConfigurationSecrets> mySecrets, IOptionsMonitor<Serilog> serilog, ILogger<HttpFunction> logger, IProduct product, IOrder order, IConfiguration configuration)
        {
            _myConfig = myConfig;
            _mySecrets = mySecrets;
            _serilog = serilog;
            _logger = logger;
            _product = product;
            _order = order;
            _configuration = configuration;
        }

        [FunctionName("HttpFunction")]
        public void Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            var auth = _configuration.GetSection("MyConfiguration:Authorities").Get<List<string>>();
            var auth2 = _myConfig.CurrentValue.Authorities;

            _order.Run();
            _product.Run();
        }
    }
}
