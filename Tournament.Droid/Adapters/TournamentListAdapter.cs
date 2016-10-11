namespace Tournament.Droid.Adapters
{
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

    public class TournamentListAdapter : BaseAdapter<Tournee>
    {
        private Activity context;
        private List<Tournee> tournaments;
        
        public TournamentListAdapter(Activity context, ICollection<Tournee> items)
        {
            this.context = context;
            this.tournaments = items.ToList();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView
                        ?? this.context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = tournaments[position].DisplayName;

            return view;
        }

        public override int Count => this.tournaments.Count;

        public override Tournee this[int position] => this.tournaments[position];
    }
}