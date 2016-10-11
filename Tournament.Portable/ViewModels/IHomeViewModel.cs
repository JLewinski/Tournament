namespace Tournament.Portable.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IHomeViewModel
    {
        string Title { get; }

        ObservableCollection<Models.Tournee> Tournaments { get; set; }
    }
}