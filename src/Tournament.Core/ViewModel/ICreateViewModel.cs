using System.Collections;
using System.Runtime.InteropServices.ComTypes;

namespace Tournament.Core.ViewModel
{
    public interface ICreateViewModel
    {
        int NumberTeams { get; set; }
        Models.Tournament Tournament { get; set; }
    }
}