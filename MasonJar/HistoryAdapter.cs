// [Ready Design Corps] - [Mason Jar] - Copyright 2016

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
using Java.Lang;

namespace MasonJar
{
    public class HistoryAdapter : BaseAdapter
    {
        public override int Count { get { return _JarViewModel.History.Count; } }

        private ViewModel.Jar _JarViewModel;
        private LayoutInflater _Inflater;

        // Methods
        public HistoryAdapter(ViewModel.Jar jarViewModel, LayoutInflater inflater)
        {
            _Inflater = inflater;
            _JarViewModel = jarViewModel;
            _JarViewModel.HistoryCollectionChanged += HistoryUpdated;
        }

        private void HistoryUpdated(object sender, EventArgs args)
        {
            NotifyDataSetChanged();
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return (position < Count) ? _JarViewModel.History[position] : null;
        }

        public override long GetItemId(int position)
        {
            return (position < Count) ? _JarViewModel.History[position].GetHashCode() : -1;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewModel.HistoryItem item = _JarViewModel.History[position];

            // Create the view based on if we have a category or not. 
            View rootView = _Inflater.Inflate((item.HasCategory ? Resource.Layout.HistoryItemWithCategory : Resource.Layout.HistoryItem), null);

            // Populate the view with history content.
            TextView content = (TextView)rootView.FindViewById(Resource.Id.history_content);
            content.Text = item.Content;

            TextView timestamp = (TextView)rootView.FindViewById(Resource.Id.history_category_timestamp);
            timestamp.Text = item.Timestamp.ToString("d");

            if (item.HasCategory)
            {
                TextView title = (TextView)rootView.FindViewById(Resource.Id.history_category_title);
                title.Text = item.CategoryTitle;

                ImageView swatch = (ImageView)rootView.FindViewById(Resource.Id.history_category_swatch);
                swatch.SetColorFilter(Android.Graphics.Color.Argb(255, item.Color.Value.R, item.Color.Value.G, item.Color.Value.B));
            }

            // Done!
            return rootView;
        }
    }
}