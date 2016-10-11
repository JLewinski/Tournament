using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tournament.Portable.ViewModels
{
    public interface IRoundViewModel
    {
        Models.Tournee Tournament { get; set; }
        int Round { get; set; }
    }
}