namespace Tournament.Portable.ViewModels
{
    public class RoundViewModel : IRoundViewModel
    {
        public RoundViewModel()
        {
        }

        public RoundViewModel(Models.Tournament tournament)
        {
            Tournament = tournament;
            Round = tournament.CurrentRound;
        }

        public RoundViewModel(Models.Tournament tournament, int round)
        {
            Tournament = tournament;
            Round = round;
        }

        public Models.Tournament Tournament { get; set; }
        public int Round { get; set; }
    }
}