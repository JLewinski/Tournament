using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Template10.Mvvm;

using Tournament.Portable.DesignTimeData;
using Tournament.Portable.Models;
using Tournament.Portable.Services;
using Tournament.Portable.ViewModels;
using Tournament.Windows10.Data;

using Windows.UI.Xaml.Navigation;

namespace Tournament.Windows10.ViewModels
{
    public class RoundPageViewModel : ViewModelBase, IRoundViewModel
    {
        // Set to true when either selected match or selected winner index is changing to avoid infinite changing
        private bool changing;

        public RoundPageViewModel()
        {
            this.SelectedWinnerIndex = -1;

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Tournament = Factory.CreateTournament();
            }
        }

        public Tournee Tournament { get; set; }

        private int round;

        public int Round
        {
            get
            {
                return round;
            }

            set
            {
                Set(ref round, value);
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            using (var db = new ApplicationDbContext())
            {
                Tournament = db.Tournaments.Include(tournament => tournament.Matches)
                    .ThenInclude(match => match.Connections)
                    .ThenInclude(team => team.Team)
                    .Single(t => t.Id == (string)parameter);
                if (Tournament == null)
                {
                    Tournament = await MyWebService.GetTournament((string)parameter);
                    if (Tournament != null)
                    {
                        db.Add(Tournament);
                    }
                }
            }

            if (Tournament == null)
            {
                NavigationService.GoBack();
            }
            else
            {
                SetMatches(Tournament.CurrentRound);
            }
        }

        private int selectedMatchIndex = -1;

        public int SelectedMatchIndex
        {
            get
            {
                return this.selectedMatchIndex;
            }

            set
            {
                Set(ref this.selectedMatchIndex, value);
                SelectedMatch = this.selectedMatchIndex == -1 ? null : Matches[this.selectedMatchIndex];
            }
        }

        private Match selectedMatch;

        public Match SelectedMatch
        {
            get
            {
                return this.selectedMatch;
            }

            set
            {
                if (!this.changing && value != null)
                {
                    this.changing = true;
                    Set(ref this.selectedMatch, value);
                    if (value.WinnerId == null)
                    {
                        this.SelectedWinnerIndex = -1;
                    }
                    else
                    {
                        for (var i = 0; i < value.Teams.Count; ++i)
                        {
                            if (value.Teams[i].Id == value.WinnerId)
                            {
                                this.SelectedWinnerIndex = i;
                            }
                        }
                    }
                }
                else
                {
                    Set(ref this.selectedMatch, value);
                }
                
                this.changing = false;
            }
        }

        private List<Match> matches = default(List<Match>);

        public List<Match> Matches
        {
            get
            {
                return this.matches;
            }

            set
            {
                Set(ref this.matches, value);
            }
        }

        private int selectedWinnerIndex = -1;

        public int SelectedWinnerIndex
        {
            get
            {
                return this.selectedWinnerIndex;
            }

            set
            {
                if (!this.changing)
                {
                    if (this.SelectedMatchIndex == -1) return;
                    this.changing = true;
                    Tournament.Matches[this.SelectedMatchIndex].WinnerId = value == -1 ?
                        null : Tournament.Matches[this.SelectedMatchIndex].Teams[value].Id;
                }

                Set(ref this.selectedWinnerIndex, value);
                this.changing = false;
            }
        }

        public void SaveLocal()
        {
            Tournament.Matches = Matches.Where(match => match.Round < Round).ToList();
            Tournament.CurrentRound = Round;
            Tournament.Matches.AddRange(Matches);
            using (var db = new ApplicationDbContext())
            {
                TournamentHelper.UpdateTournament(Tournament, db);
                db.SaveChanges();
                SetMatches(Tournament.CurrentRound);
            }
        }

        private void SetMatches(int newRound)
        {
            Round = newRound;
            Matches =
                    Tournament.Matches.Where(match => match.Round == Round)
                        .OrderBy(match => match.DisplayName)
                        .ToList();
        }

        public async void SaveRemote()
        {
            SaveLocal();
            if (await MyWebService.GetTournament(Tournament.Id) == null)
            {
                await MyWebService.InsertTournament(Tournament);
            }
            else
            {
                await MyWebService.UpdateTournament(Tournament);
            }
        }

        public void IncreaseRound()
        {
            if (Round < Tournament.CurrentRound)
            {
                SetMatches(++Round);
            }
        }

        public void DecreaseRound()
        {
            if (Round > 1)
            {
                SetMatches(--Round);
            }
        }

        public void GoBack()
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(typeof(Views.HomePage));
            }
        }
    }
}
