using System.Collections.Generic;

using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;

using Tournament.Portable.Models;

namespace Tournament.Droid.Adapters
{
    

    public class MatchListAdapter : BaseAdapter<Match>
    {
        private readonly Activity context;

        public List<Match> Matches { get; set; }

        public MatchListAdapter(Activity context, List<Match> matches)
        {
            this.context = context;
            this.Matches = matches;
        }

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView
                       ?? this.context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = this.Matches[position].DisplayName;

            view.SetBackgroundColor(this.Matches[position].WinnerId != null ? Color.Green : Color.Transparent);

            return view;
        }

        public override int Count => this.Matches.Count;

        public override Match this[int position] => this.Matches[position];

        public void Update(string matchId, string winnerId)
        {
            var tempMatch = this.Matches.Find(m => m.Id == matchId);
            tempMatch.WinnerId = winnerId;
            tempMatch = this.Matches.Find(m => m.Id == matchId);
        }
    }
}