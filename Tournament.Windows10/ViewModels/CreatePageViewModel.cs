using Template10.Mvvm;
using Tournament.Portable.ViewModels;

namespace Tournament.Windows10.ViewModels
{
    public class CreatePageViewModel : ViewModelBase, ICreateViewModel
    {
        public string Title => "Create New Tournament";

        private int _numberTeams;

        public int NumberTeams
        {
            get
            {
                return _numberTeams;
            }
            set
            {
                Set(ref _numberTeams, value);
            }
        }

        private Portable.Models.Tournament _tournament;

        public Portable.Models.Tournament Tournament
        {
            get { return _tournament; }
            set { Set(ref _tournament, value); }
        }
    }
}