using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Tournament.Droid.Adapters;
using Tournament.Droid.Dialog;
using Tournament.Portable.Models;
using Tournament.Portable.Services;

namespace Tournament.Droid.Activities
{
    [Activity(Label = "TourneeActivity")]
    public class TourneeActivity : Activity, SelectWinnerListener
    {
        private MatchListAdapter adapter;
        private Tournee tournament;
        private Match selectedMatch;
        private TextView roundTextView;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (tournament == null)
            {
                var tournamentId = Intent.Extras.GetString("Id");
                tournament = await MyWebService.GetTournament(tournamentId);
            }

            // Create your application here
            SetContentView(Resource.Layout.Tournee);

            var textView = FindViewById<TextView>(Resource.Id.tournamentNameText);
            textView.Text = tournament.DisplayName;

            this.roundTextView = FindViewById<TextView>(Resource.Id.roundText);
            adapter = new MatchListAdapter(this, null);
            SetList(tournament.CurrentRound);

            var matchListView = FindViewById<ListView>(Resource.Id.matchListView);
            matchListView.Adapter = this.adapter;
            matchListView.ItemClick += (sender, e) =>
                {
                    selectedMatch = adapter[e.Position];
                    new SelectWinnerDialog(this.selectedMatch).Show(FragmentManager, "winner");
                };

            var decreaseButton = FindViewById<Button>(Resource.Id.decreaseButton);
            decreaseButton.Click += (sender, e) =>
                {
                    if (this.tournament.CurrentRound > 1)
                    {
                        SetList(--this.tournament.CurrentRound);
                        this.adapter.NotifyDataSetChanged();
                    }
                };

            var increaseButton = FindViewById<Button>(Resource.Id.increaseButton);
            increaseButton.Click += (s, e) =>
                {
                    if (this.tournament.Matches.Any(m => m.Round > this.tournament.CurrentRound))
                    {
                        SetList(++this.tournament.CurrentRound);
                        this.adapter.NotifyDataSetChanged();
                    }
                };

            var saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Click += (s, e) => this.Save();
        }

        private void SetList(int round)
        {
            this.adapter.Matches = tournament.Matches.Where(match => match.Round == round).OrderBy(match => match.DisplayName).ToList();
            this.roundTextView.Text = this.tournament.CurrentRound.ToString();
        }

        private async void Save()
        {
            var roundBefore = this.tournament.CurrentRound;
            TournamentHelper.UpdateTournament(this.tournament);

            if (roundBefore != this.tournament.CurrentRound)
            {
                SetList(this.tournament.CurrentRound);
            }

            this.adapter.NotifyDataSetChanged();
            await MyWebService.UpdateTournament(this.tournament);
        }

        public void OnReturnValue(string winnerId)
        {
            this.selectedMatch.WinnerId = winnerId;
            this.tournament.Matches = this.tournament.Matches.Where(match => match.Round < tournament.CurrentRound).ToList();

            this.tournament.Matches.AddRange(adapter.Matches);
            
            this.adapter.NotifyDataSetChanged();
        }
    }
}