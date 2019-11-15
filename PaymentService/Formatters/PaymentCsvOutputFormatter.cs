using Microsoft.AspNetCore.Mvc.Formatters;
using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Formatters
{
   
    public class PaymentCsvOutputFormatter : TextOutputFormatter
    {
        public PaymentCsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(Payment).IsAssignableFrom(type) || typeof(IEnumerable<Payment>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }

            return false;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<Payment>)
            {
                foreach (var payment in (IEnumerable<Payment>)context.Object)
                {
                    FormatCsv(buffer, payment);
                }
            }
            else
            {
                FormatCsv(buffer, (Payment)context.Object);
            }

            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                return writer.WriteAsync(buffer.ToString());
            }
        }

        private static void FormatCsv(StringBuilder buffer, Payment payment)
        {
             foreach (var product in payment.Products)
             {
                buffer.AppendLine($"{payment.Id},\"{payment.Subject}\",{product.Id},\"{product.Name}\",\"{product.Price}\",\"{payment.Total}\"");                
             }
        }
    }
}
