namespace Tournament.Droid.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Widget;

    using Tournament.Portable.Models;
    using Tournament.Portable.Services;

    [Activity(Label = "CreateNewTournamentActivity")]
    public class CreateNewTournamentActivity : Activity
    {
        private ListView newUserList;
        private ArrayAdapter<Team> adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreateNewTournament);

            newUserList = FindViewById<ListView>(Resource.Id.newUsersList);
            adapter = new ArrayAdapter<Team>(this, Android.Resource.Layout.SimpleListItem1, new List<Team>());
            newUserList.Adapter = adapter;
            adapter.NotifyDataSetChanged();

            var addUserButton = FindViewById<Button>(Resource.Id.addUserButton);
            addUserButton.Click += (sender, e) =>
                {
                    var text = FindViewById<EditText>(Resource.Id.newUserNameText);
                    AddUser(text.Text);
                    text.Text = string.Empty;
                };

            var createTournamentButton = FindViewById<Button>(Resource.Id.createNewTournamentButton);
            createTournamentButton.Click += (sender, e) => { CreateTournament(); };
        }

        private void AddUser(string name)
        {
            if (name != string.Empty)
            {
                adapter.Add(new Team { Name = name, Id = Guid.NewGuid().ToString() });
                adapter.NotifyDataSetChanged();
            }
        }

        private async void CreateTournament()
        {
            var tournee = new Tournee
                              {
                                  Id = Guid.NewGuid().ToString(),
                                  Teams = new List<Team>(),
                                  DisplayName = "Made from Android",
                                  CurrentRound = 0,
                                  Description = string.Empty,
                                  GamesPerMatch = 1,
                                  TeamsPerMatch = 2
                              };

            for (var i = 0; i < adapter.Count; i++)
            {
                tournee.Teams.Add(this.adapter.GetItem(i));
            }
            
            TournamentHelper.MakeNewTournament(tournee);

            var intent = new Intent(this, typeof(MainActivity));

            try
            {
                bool worked = await MyWebService.InsertTournament(tournee);
                if (worked)
                {
                    StartActivity(intent);
                }
            }
            catch (Exception)
            {
                // Can't access server
            }

            // StartActivity(intent);

            // TODO: Make sure the tournament is connecting to the teams when being added
        }

        // TODO: Make sure that the list of users is not deleted when exiting and opening the app.
    }
}