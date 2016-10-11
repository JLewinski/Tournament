using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Tournament.Portable.Models;

namespace Tournament.Droid.Dialog
{
    using Dialog = Android.App.Dialog;

    public class SelectWinnerDialog : DialogFragment
    {
        private string[] ids;

        private readonly string[] names;

        private string winnerId;

        public SelectWinnerDialog(string[] ids, string[] names, string winnerId)
        {
            this.ids = ids;
            this.names = names;
            this.winnerId = winnerId;
        }

        public SelectWinnerDialog(Match match)
        {
            this.ids = match.Teams.Select(t => t.Id).ToArray();
            this.names = match.Teams.Select(t => t.Name).ToArray();
            this.winnerId = match.WinnerId;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            /*ids = savedInstanceState.GetStringArray("Ids");
            names = savedInstanceState.GetStringArray("Names");
            winnerId = savedInstanceState.GetString("WinnerId");*/

            builder.SetTitle("Pick a Winner")
                .SetItems(
                    names,
                    (sender, args) =>
                            ((SelectWinnerListener)this.Activity).OnReturnValue(ids[args.Which]))
                .SetNegativeButton(
                    "cancel",
                    (sender, args) => /*((SelectWinnerListener)this.Activity).OnReturnValue(winnerId)*/ { });

            return builder.Create();
        }
    }
}