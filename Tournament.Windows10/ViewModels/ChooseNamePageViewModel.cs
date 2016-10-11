using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Tournament.Portable.Models;

namespace Tournament.Windows10.ViewModels
{
    /// <summary>
    /// The choose name page view model.
    /// </summary>
    public class ChooseNamePageViewModel: ViewModelBase
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
            Name = Tournament?.DisplayName;
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        /// <summary>
        /// Gets or sets the tournament.
        /// </summary>
        public Tournee Tournament { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        private string name = default(string);

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { Set(ref this.name, value); }
        }
    }
}