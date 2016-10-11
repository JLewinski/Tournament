using System.ComponentModel.DataAnnotations.Schema;

namespace Tournament.Portable.Models
{
    public class Player: IModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Team Team { get; set; }
    }
}