using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlackFriday.Controllers
{
    [Route("api/[controller]")]
    public class ProductListController : Controller
    {

        private static string productDbBaseUrl = "http://productdbservicemar1.azurewebsites.net/";
        private static string productFTPBaseUrl = "http://productftpservicemar1.azurewebsites.net/";

        // GET: http://iegblackfriday.azurewebsites.net/api/productlist
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            //return new string[] { "Windows Phone", "BlackBerry" };

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(productDbBaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync(productDbBaseUrl + "/api/Products");
            var products = new List<string>();
            if (response.IsSuccessStatusCode)
                products = await response.Content.ReadAsAsync<List<string>>();

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(productFTPBaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            response = await httpClient.GetAsync(productFTPBaseUrl + "/api/Products");
            if (response.IsSuccessStatusCode)
                products.AddRange(await response.Content.ReadAsAsync<List<string>>());
            
            return products;
        }
        
    }
}
