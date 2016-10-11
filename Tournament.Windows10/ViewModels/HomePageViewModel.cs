using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Utils;
using Tournament.Portable.DesignTimeData;
using Tournament.Portable.ViewModels;
using Tournament.Windows10.Data;

namespace Tournament.Windows10.ViewModels
{
    using Tournament.Portable.Models;
    using Tournament.Portable.Services;

    /// <summary>
    /// The home page view model.
    /// </summary>
    public class HomePageViewModel : ViewModelBase, IHomeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageViewModel"/> class with design time data..
        /// </summary>
        public HomePageViewModel()
        {
            SelectedIndex = -1;
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Tournaments = new ObservableCollection<Tournee>
                {
                    Factory.CreateTournament(),
                    Factory.CreateTournament(),
                    Factory.CreateTournament()
                };
            }
        }

        /// <summary>
        /// Sets up the ViewModel every time the page is navigated to.
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
            using (var db = new ApplicationDbContext())
            {
                Tournaments = db.Tournaments.ToObservableCollection();
            }

            // TODO: Add a local only option
            ////Tournaments.AddRange(await MyWebService.GetTournaments());

            return Task.CompletedTask;
        }

        /// <summary>
        /// The tournaments in the data set the user has access to.
        /// </summary>
        private ObservableCollection<Tournee> tournaments;

        /// <summary>
        /// Gets or sets the tournaments.
        /// </summary>
        public ObservableCollection<Tournee> Tournaments
        {
            get { return this.tournaments; }
            set { Set(ref this.tournaments, value); }
        }

        /// <summary>
        /// The title of the page.
        /// </summary>
        public string Title => "Home";

        /// <summary>
        /// Go to the Create Page to create a new tournament.
        /// </summary>
        public void CreateTournament()
        {
            NavigationService.Navigate(typeof(Views.CreatePage));
        }

        public int SelectedIndex { get; set; }

        public void GoToTournament()
        {
            if (SelectedIndex != -1)
            {
                NavigationService.Navigate(typeof(Views.RoundPage), Tournaments[this.SelectedIndex].Id);
            }
        }
    }
}