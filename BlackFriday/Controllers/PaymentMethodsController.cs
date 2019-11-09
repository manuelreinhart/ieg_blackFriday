using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Polly;

namespace BlackFriday.Controllers
{
    [Produces("application/json")]
    [Route("api/PaymentMethods")]
    public class PaymentMethodsController : Controller
    {
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        private readonly ILogger<PaymentMethodsController> _logger;
        private static readonly int ServiceCount = 2;


        private static int _lastRoundRobinIndex = 0;       
        private static int RoundRobinIndex
        {
            get
            {
                return _lastRoundRobinIndex++ % ServiceCount + 1;
            }
        }

        public static string CreditcardServiceBaseAddress
        {
            get
            {
                return $"https://iegeasycreditcardservicemar{RoundRobinIndex}.azurewebsites.net/";
            }
        }


        public PaymentMethodsController(ILogger<PaymentMethodsController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            return await this.GetPaymentMethods();
        }

        private async Task<String[]> GetPaymentMethods()
        {
            List<string> acceptedPaymentMethods = null;//= new string[] { "Diners", "Master" };
            _logger.LogError("Accepted Paymentmethods");
            var httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(CreditcardServiceBaseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Policy
           .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
           .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
           {
               _logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
           })
           .ExecuteAsync(() => httpClient.GetAsync(CreditcardServiceBaseAddress + "/api/AcceptedCreditCards"));

            if (response.IsSuccessStatusCode)
            {
                acceptedPaymentMethods = await response.Content.ReadAsAsync<List<string>>();

                _logger.LogInformation("Response was successful.");
            }
            else
            {
                _logger.LogError($"Response failed. Status code {response.StatusCode}");
            }


            return acceptedPaymentMethods.ToArray();

        }               

    }
}