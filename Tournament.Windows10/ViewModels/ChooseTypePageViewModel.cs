using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Tournament.Portable.Models;

namespace Tournament.Windows10.ViewModels
{
    /// <summary>
    /// The choose type page view model.
    /// </summary>
    public class ChooseTypePageViewModel: ViewModelBase
    {
        /// <summary>
        /// Sets up the view model each time it's navigated to.
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
            Tournament = parameter as Tournee;

            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        /// <summary>
        /// Gets the tournament.
        /// </summary>
        public Tournee Tournament { get; private set; }

        // Type of tournament to set up based on the number of people
    }
}