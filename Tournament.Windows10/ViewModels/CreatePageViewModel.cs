using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Tournament.Portable.Models;
using Tournament.Portable.ViewModels;

namespace Tournament.Windows10.ViewModels
{
    using System;

    using Tournament.Portable.Services;
    using Tournament.Windows10.Data;
    using Tournament.Windows10.Views;

    /// <summary>
    /// The create page view model.
    /// </summary>
    public class CreatePageViewModel : ViewModelBase, ICreateViewModel
    {
        /// <summary>
        /// The title for the page.
        /// </summary>
        public string Title => "Create New Tournament";

        /// <summary>
        /// The number teams in the tournament.
        /// </summary>
        private int numberTeams;

        /// <summary>
        /// Gets or sets the number teams.
        /// </summary>
        public int NumberTeams
        {
            get
            {
                return numberTeams;
            }

            set
            {
                Set(ref numberTeams, value);
                while (value > Teams.Count)
                {
                    Teams.Add(new Team { Id = Guid.NewGuid().ToString(), Name = $"Team {Teams.Count + 1}" });
                }

                while (value < Teams.Count)
                {
                    Teams.RemoveAt(Teams.Count - 1);
                }
            }
        }

        /// <summary>
        /// Gets or sets the tournament.
        /// </summary>
        public Tournee Tournament { get; set; }

        /// <summary>
        /// The teams.
        /// </summary>
        private ObservableCollection<Team> teams = default(ObservableCollection<Team>);

        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        public ObservableCollection<Team> Teams
        {
            get { return teams; }
            set { Set(ref teams, value); }
        }

        /// <summary>
        /// Gets the names collection.
        /// </summary>
        public ObservableCollection<string> NamesCollection { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// The on navigated to async.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="mode">
        /// The mode.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            /*Tournament = new Portable.Models.Tournee { Teams = new List<Team>() };

            for (var i = 0; i < 16; i++)
            {
                Tournament.Teams.Add(new Team() { Name = $"Team {i + 1}" });
            }*/

            Teams = new ObservableCollection<Team>();

            NumberTeams = 16;

            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        /// <summary>
        /// Continues to the next phase in creating the tournament.
        /// </summary>
        public async void Continue()
        {
            Tournament = new Tournee { Id = Guid.NewGuid().ToString(), Teams = Teams.ToList(), DisplayName = "Made from Windows" };

            TournamentHelper.MakeNewTournament(Tournament);

            bool worked = await MyWebService.InsertTournament(Tournament);

            if (worked)
            {
                await NavigationService.NavigateAsync(typeof(HomePage));
            }
            else
            {
                NumberTeams = 8;
                return;
            }

            using (var dbcontext = new ApplicationDbContext())
            {
                dbcontext.Add(Tournament);
                dbcontext.SaveChanges();
            }

            await NavigationService.NavigateAsync(typeof(HomePage));
            // For now just make a single elimination tournament and give people a bye.
        }
    }
}