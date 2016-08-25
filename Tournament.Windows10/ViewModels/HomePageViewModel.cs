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
    public class HomePageViewModel : ViewModelBase, IHomeViewModel
    {

        public HomePageViewModel()
        {
            Tournaments = new ObservableCollection<Portable.Models.Tournament>
            {
                Factory.CreateTournament(),
                Factory.CreateTournament(),
                Factory.CreateTournament()
            };
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            using (var db = new ApplicationDbContext())
            {
                Tournaments = db.Tournaments.ToObservableCollection();
            }
            return Task.CompletedTask;
        }

        private ObservableCollection<Portable.Models.Tournament> _tournaments;
        public ObservableCollection<Portable.Models.Tournament> Tournaments
        {
            get { return _tournaments; }
            set { Set(ref _tournaments, value); }
        }

        public string Title => "Home";

        public void CreateTournament()
        {
            NavigationService.Navigate(typeof(Views.CreatePage));
        }
    }
}