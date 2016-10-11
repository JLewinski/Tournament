namespace Tournament.Portable.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Tournament.Portable.Models;

    public class HomeViewModel : IHomeViewModel
    {
        public string Title { get; }

        public ObservableCollection<Tournee> Tournaments { get; set; }
    }
}
