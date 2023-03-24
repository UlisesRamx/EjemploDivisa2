using EjemploDivisa2.Model.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EjemploDivisa2.Model.Api
{
    internal class DivisaServices
    {
        private static readonly string URL_BASE = "https://openexchangerates.org/api/";
        private static readonly string APP_ID = "e77f99c02f404d34a3631b67223d85e5";


        public static async Task<RespuestaTasas> getTasasConversion() {
        RespuestaTasas res = new RespuestaTasas();
        using (var httpClient = new HttpClient()) {
                        HttpRequestMessage request;
                        HttpResponseMessage response;
                        try
                        {
                            string url = string.Format("{0}latest.json?app_id={1}", URL_BASE, APP_ID);

                            request = new HttpRequestMessage(HttpMethod.Get,url);
                            response = await httpClient.SendAsync(request);

                            if (response != null) {
                                if (response.IsSuccessStatusCode)
                                {
                                    TasasConversion tasas = await response.Content.ReadFromJsonAsync<TasasConversion>();

                                    if (tasas != null && tasas.Disclaimer != null && tasas.Rates != null)
                                    {
                                        res.Error = false;
                                        res.Mensaje = "OK";
                                        res.Tasas = tasas;
                                    }
                                    else
                                    {
                                        res.Error = true;
                                        res.Mensaje = "No se deserializo la respuesta en JSON de las Tasas de Conversiòn...";
                                    }
                                }
                                else {
                                    res.Error = true;
                                    res.Mensaje = String.Format("Eror: (0) - (1)", (int)response.StatusCode, response.StatusCode);
                                }
                            }
                            else
                            {
                                res.Error = true;
                                res.Mensaje = "No se puede obtener respuesta del servicio web de las Tasas de Conversión";
                            }
                        }
                        catch (Exception ex){
                            res.Error = true;
                            res.Mensaje = ex.Message;
                        }
                        return res;
                    }

                }

        public static async Task<RespuestaMoneda> getMoneda(){ 
            RespuestaMoneda res = new RespuestaMoneda();
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage request;
                HttpResponseMessage response;
                try
                {
                    string url = string.Format("{0}currencies.json?app_id={1}", URL_BASE, APP_ID);
                    request = new HttpRequestMessage(HttpMethod.Get, url);
                    response = await httpClient.SendAsync(request);
                    //response = await httpClient.SendAsync(request);
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            Dictionary<string, string> monedas = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                            if (monedas != null)
                            {
                                res.Error = false;
                                res.Monedas = monedas;
                            }
                            else
                            {
                                res.Error = true;
                                res.Mensaje = "No se deserializo la respuesta en JSON de las monedas...";
                            }
                        }
                        else
                        {
                            res.Error = true;
                            HttpStatusCode httpStatusCode = response.StatusCode;
                            res.Mensaje = string.Format("Error {0} - {1}", (int)httpStatusCode, httpStatusCode);
                        }
                    }
                    else
                    {
                        res.Error = true;
                        res.Mensaje = "No se puede obtener respuesta del servicio web de las monedas";
                    }
                }
                catch (Exception exception)
                {
                    res.Error = true;
                    res.Mensaje = exception.Message;
                }
                return res;
            }

        }
    }
}
