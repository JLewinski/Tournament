namespace Tournament.Droid.Activities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Widget;

    using Java.Lang;

    using Tournament.Droid.Adapters;
    using Tournament.Portable.Models;
    using Tournament.Portable.Services;

    [Activity(Label = "Tournament.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private List<Tournee> tournaments;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            tournaments = new List<Tournee>();

            await SyncAsync();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (!tournaments.Any())
            {
                // Design Filler Data
                tournaments = new List<Tournee>();
                for (var i = 0; i < 5; i++)
                {
                    tournaments.Add(new Tournee { DisplayName = $"Design {i}", Id = $"i = {i}" });
                }
            }

            var tournamentListView = FindViewById<ListView>(Resource.Id.tournamentListView);
            tournamentListView.Adapter = new TournamentListAdapter(this, this.tournaments);
            tournamentListView.ItemClick += (sender, e) =>
                {
                    var tlv = (sender as ListView)?.Adapter as TournamentListAdapter;
                    GoToTournament(tlv?[e.Position]);
                };

            var addTournamentButton = FindViewById<Button>(Resource.Id.addTournamentButton);
            addTournamentButton.Click += (sender, e) => { this.CreateNewTournament(); };
        }

        private async void Sync()
        {
            var tournamentsFromServer = (await MyWebService.GetTournaments()).ToList();
            if (tournamentsFromServer.Any())
            {
                this.tournaments = tournamentsFromServer;
            }
        }

        private async Task SyncAsync()
        {
            var tournamentsFromServer = (await MyWebService.GetTournaments()).ToList();
            if (tournamentsFromServer.Any())
            {
                this.tournaments = tournamentsFromServer;
            }
        }

        private void CreateNewTournament()
        {
            var intent = new Intent(this, typeof(CreateNewTournamentActivity));
            StartActivity(intent);
        }

        private void GoToTournament(Tournee tournee)
        {
            var intent = new Intent(this, typeof(TourneeActivity));
            intent.PutExtra("Id", tournee?.Id);
            StartActivity(intent);
        }
    }
}

