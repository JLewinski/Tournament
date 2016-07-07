using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tournament.Core.Services
{
    public static class MyWebService
    {
        public static async Task<IEnumerable<Models.Tournament>> GetTournaments()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44323/Tournaments/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var tous = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<Models.Tournament>>(tous);
                }
            }
            return null;
        }

        public static async Task<Models.Tournament> GetTournament(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44323/Tournaments/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"Get/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var tous = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Models.Tournament>(tous);
                }
            }
            return null;
        }

        public static async Task<bool> InsertTournament(Models.Tournament tournament)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44323/Tournaments/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(tournament, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                var content = new StringContent(json, Encoding.ASCII, "application/json");
                
                var response = await client.PostAsync("Create", content);
                return response.IsSuccessStatusCode;
            }
        }

        public static async Task<bool> UpdateTournament(Models.Tournament tournament)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44323/Tournaments/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(tournament, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                var content = new StringContent(json);

                var response = await client.PutAsync("Update", content);
                return response.IsSuccessStatusCode;
            }
        }
    }
}