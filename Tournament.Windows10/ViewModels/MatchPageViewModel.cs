using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Windows10.ViewModels
{
    using Windows.UI.Xaml.Navigation;

    using Template10.Mvvm;
    using Template10.Services.NavigationService;

    using Tournament.Portable.DesignTimeData;
    using Tournament.Portable.Models;
    using Tournament.Windows10.Data;

    public class MatchPageViewModel : ViewModelBase
    {
        public MatchPageViewModel(INavigationService navigationService)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Match = Factory.CreateMatch();
            }
        }

        public Match Match { get; set; }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Match = (Match)parameter;
            return Task.CompletedTask;
        }

        private int selectedIndex = default(int);

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                Set(ref this.selectedIndex, value);
                Match.WinnerId = Match.Teams[SelectedIndex].Id;
            }
        }

        public void ChooseWinner()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Update(Match);
            }

            this.NavigationService.GoBack();
        }
    }
}
