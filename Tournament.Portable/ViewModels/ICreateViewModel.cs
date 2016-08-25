using System.Collections;
using System.Runtime.InteropServices.ComTypes;

namespace Tournament.Portable.ViewModels
{
    public interface ICreateViewModel
    {
        string Title { get; }
        int NumberTeams { get; set; }
        Models.Tournament Tournament { get; set; }
    }
}