using System.ComponentModel.DataAnnotations.Schema;

namespace Tournament.Core.Models
{
    public interface IModel
    {
        string Id { get; set; }
        string Description { get; set; }
    }
}