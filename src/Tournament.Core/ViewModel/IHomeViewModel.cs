using System.Collections.Generic;

namespace Tournament.Core.ViewModel
{
    public interface IHomeViewModel
    {
        ICollection<Models.Tournament> Tournaments { get; set; }
    }
}