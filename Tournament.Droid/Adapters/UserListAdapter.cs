namespace Tournament.Droid.Adapters
{
    using System.Collections.Generic;

    using Android.App;
    using Android.Views;
    using Android.Widget;

    using Tournament.Portable.Models;

    public class UserListAdapter : BaseAdapter<User>
    {
        private Activity context;
        private List<User> users;

        public UserListAdapter(Activity context, List<User> users)
        {
            this.context = context;
            this.users = users;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView
                        ?? this.context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = this.users[position].ToString();

            return view;
        }

        public override int Count => this.users.Count;

        public override User this[int position] => this.users[position];
    }
}