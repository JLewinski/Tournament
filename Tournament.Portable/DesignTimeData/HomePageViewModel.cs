using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Portable.ViewModels;

namespace Tournament.Portable.DesignTimeData
{
    class HomePageViewModel : IHomeViewModel
    {
        public string Title => "Home";
        public ObservableCollection<Models.Tournament> Tournaments { get; set; }
    }
}
