using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CapaEntidad.Paypal;

namespace CapaNegocio
{
    public class CN_Paypal
    {
        private static readonly string urlPaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static readonly string clientId = ConfigurationManager.AppSettings["ClientId"];
        private static readonly string secret = ConfigurationManager.AppSettings["Secret"];

        private HttpClient ConfigureHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(urlPaypal) };
            var authToken = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            return client;
        }

        public async Task<Response_Paypal<Response_Checkout>> CrearSolicitud(Checkout_Order orden)
        {
            var responsePaypal = new Response_Paypal<Response_Checkout>();

            using (var client = ConfigureHttpClient())
            {
                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/v2/checkout/orders", data);
                responsePaypal.Status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    var jsonRespuesta = await response.Content.ReadAsStringAsync();
                    responsePaypal.Response = JsonConvert.DeserializeObject<Response_Checkout>(jsonRespuesta);
                }
            }

            return responsePaypal;
        }

        public async Task<Response_Paypal<Response_Capture>> AprobarPago(string token)
        {
            var responsePaypal = new Response_Paypal<Response_Capture>();

            using (var client = ConfigureHttpClient())
            {
                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);
                responsePaypal.Status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    var jsonRespuesta = await response.Content.ReadAsStringAsync();
                    responsePaypal.Response = JsonConvert.DeserializeObject<Response_Capture>(jsonRespuesta);
                }
            }

            return responsePaypal;
        }
    }
}
