using System.ComponentModel.DataAnnotations.Schema;

namespace Tournament.Core.Models
{
    public class Game: IModel
    {
        public string Id { get; set; }
        public Match Match { get; set; }
        public Team Team { get; set; }
        public string Description { get; set; }
    }
}