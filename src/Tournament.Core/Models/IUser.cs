using System.Collections.Generic;

namespace Tournament.Core.Models
{
    public interface IUser : IModel
    {
        string UserName { get; set; }
        ICollection<Core.Models.Tournament> Tournaments { get; set; }
    }
}