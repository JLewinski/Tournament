using System.Collections.Generic;

namespace Tournament.Portable.Models
{
    public interface IUser : IModel
    {
        string UserName { get; set; }
        ICollection<Portable.Models.Tournament> Tournaments { get; set; }
    }
}