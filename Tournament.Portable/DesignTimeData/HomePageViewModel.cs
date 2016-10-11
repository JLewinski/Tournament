using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Portable.Models;
using Tournament.Portable.ViewModels;

namespace Tournament.Portable.DesignTimeData
{
    public class HomePageViewModel : IHomeViewModel
    {
        public string Title => "Home";
        public ObservableCollection<Models.Tournee> Tournaments { get; set; }

    }
}
