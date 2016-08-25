using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tournament.Portable.ViewModels
{
    public interface IHomeViewModel
    {
        string Title { get; }
        ObservableCollection<Models.Tournament> Tournaments { get; set; }
    }
}