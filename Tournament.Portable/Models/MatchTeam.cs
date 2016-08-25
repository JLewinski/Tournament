using System.ComponentModel.DataAnnotations.Schema;

namespace Tournament.Portable.Models
{
    public class MatchTeam
    {
        public Team Team { get; set; }
        public Match Match { get; set; }
        public string TeamId { get; set; }
        public string MatchId { get; set; }
    }
}