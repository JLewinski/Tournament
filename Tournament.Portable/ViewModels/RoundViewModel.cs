namespace Tournament.Portable.ViewModels
{
    public class RoundViewModel : IRoundViewModel
    {
        public RoundViewModel()
        {
        }

        public RoundViewModel(Models.Tournee tournament)
        {
            Tournament = tournament;
            Round = tournament.CurrentRound;
        }

        public RoundViewModel(Models.Tournee tournament, int round)
        {
            Tournament = tournament;
            Round = round;
        }

        public Models.Tournee Tournament { get; set; }
        public int Round { get; set; }
    }
}