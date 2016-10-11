namespace Tournament.Portable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Tournament.Portable.Models;

    public static class MyWebService
    {
        // https://tournamenttracking.azurewebsites.net/
        // https://localhost:44323/
        public const string HomeAddress = "https://tournamenttracking.azurewebsites.net/Tournaments/";

        public static async Task<IEnumerable<Tournee>> GetTournaments()
        {
            using (var client = SetUpHttpClient())
            {
                try
                {
                    var response = await client.GetAsync("GetAll");
                    if (response.IsSuccessStatusCode)
                    {
                        var tous = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<IEnumerable<Tournee>>(tous);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return null;
        }

        public static async Task<Tournee> GetTournament(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HomeAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"Get/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var tous = await response.Content.ReadAsStringAsync();
                    var tour = JsonConvert.DeserializeObject<Tournee>(tous);

                    foreach (var match in tour.Matches)
                    {
                        match.Teams = new List<Team>();
                        foreach (var connection in match.Connections)
                        {
                            match.Teams.Add(connection.Team);
                        }
                    }

                    return tour;
                }
            }

            return null;
        }

        public static async Task<bool> InsertTournament(Tournee tournament)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HomeAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync("Create", SerializeTournament(tournament));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                return response?.IsSuccessStatusCode ?? false;
            }
        }

        public static async Task<bool> UpdateTournament(Tournee tournament)
        {
            using (var client = SetUpHttpClient())
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync("Update", SerializeTournament(tournament));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                return response?.IsSuccessStatusCode ?? false;
            }
        }

        private static HttpClient SetUpHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(HomeAddress) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static StringContent SerializeTournament(Tournee tournament)
        {
            var json = JsonConvert.SerializeObject(
                    tournament,
                    Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            return new StringContent(json);
        }
    }
}