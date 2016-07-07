using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tournament.Core.ViewModel
{
    public interface IRoundViewModel
    {
        Models.Tournament Tournament { get; set; }
        int Round { get; set; }
    }
}