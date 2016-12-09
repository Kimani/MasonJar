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

namespace MasonJar
{
    public class EditAdapter : BaseAdapter
    {
        public override int Count
        {
            get
            {
                // Items and Categories + Headers for the two groups.
                return 2 + _JarViewModel.Categories.Count + _JarViewModel.Items.Count;
            }
        }

        private ViewModel.Jar  _JarViewModel;
        private LayoutInflater _Inflater;
        private View           _CategoriesHeader;
        private View           _ItemsHeader;
        private Java.Lang.String _CategoriesHeaderDataItem = new Java.Lang.String("Categories");
        private Java.Lang.String _ItemsHeaderDataItem = new Java.Lang.String("Items");

        // Methods
        public EditAdapter(ViewModel.Jar jarViewModel, View categoriesHeaderView, View itemsHeaderView, LayoutInflater inflater)
        {
            _Inflater = inflater;
            _CategoriesHeader = categoriesHeaderView;
            _ItemsHeader = itemsHeaderView;
            _JarViewModel = jarViewModel;

            _JarViewModel.CategoryCollectionChanged += EditUpdated;
            _JarViewModel.ItemCollectionChanged += EditUpdated;
        }

        private void EditUpdated(object sender, EventArgs args)
        {
            NotifyDataSetChanged();
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return (position == 0)                                                     ? (Java.Lang.Object)_CategoriesHeaderDataItem :
                   ((position > 0) && position < (1 + _JarViewModel.Categories.Count)) ? (Java.Lang.Object)_JarViewModel.Categories[position - 1] :
                   (position == (1 + _JarViewModel.Categories.Count))                  ? (Java.Lang.Object)_ItemsHeaderDataItem : 
                                                                                         (Java.Lang.Object)_JarViewModel.Items[(position - (2 + _JarViewModel.Categories.Count))];
        }

        public override long GetItemId(int position)
        {
            return GetItem(position).GetHashCode();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View resultView;
            if (position == 0)
            {
                // Category header.
                resultView = _CategoriesHeader;
            }
            else if ((position > 0) && position < (1 + _JarViewModel.Categories.Count))
            {
                // Category.
                resultView = _Inflater.Inflate(Resource.Layout.EditCategory, null);
            }
            else if (position == (1 + _JarViewModel.Categories.Count))
            {
                // Item header.
                resultView = _ItemsHeader;
            }
            else
            {
                // Item.
                resultView = _Inflater.Inflate(Resource.Layout.EditItem, null);
            }
            return resultView;

            /*
            // Create the view based on if we have a category or not. 
            View rootView = _Inflater.Inflate((item.HasCategory ? Resource.Layout.HistoryItemWithCategory : Resource.Layout.HistoryItem), null);
            TextView content = (TextView)rootView.FindViewById(Resource.Id.history_content);
            content.Text = item.Content;
                ImageView swatch = (ImageView)rootView.FindViewById(Resource.Id.history_category_swatch);
                swatch.SetColorFilter(Android.Graphics.Color.Argb(255, item.Color.Value.R, item.Color.Value.G, item.Color.Value.B));
            */
        }
    }
}