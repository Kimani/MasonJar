// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using Android.Views;
using Android.Widget;

namespace MasonJar
{
    public class EditAdapter : BaseAdapter
    {
        public delegate void CallbackDelegateCategory(ViewModel.Category item);
        public delegate void CallbackDelegateItem(ViewModel.Item item);

        public class CallbackSet
        {
            public CallbackDelegateCategory CallbackCategorySwatchClicked;
            public CallbackDelegateCategory CallbackCategoryDeleteClicked;
            public CallbackDelegateCategory CallbackCategoryTitleClicked;
            public CallbackDelegateItem     CallbackItemCategoryClicked;
            public CallbackDelegateItem     CallbackItemContentClicked;
            public CallbackDelegateItem     CallbackItemDeleteClicked;
        }

        public override int Count
        {
            get
            {
                // Items and Categories + Headers for the two groups.
                return 2 + _JarViewModel.Categories.Count + _JarViewModel.Items.Count;
            }
        }

        private ViewModel.Jar    _JarViewModel;
        private LayoutInflater   _Inflater;
        private View             _CategoriesHeader;
        private View             _ItemsHeader;
        private CallbackSet      _Callbacks;
        private Java.Lang.String _CategoriesHeaderDataItem = new Java.Lang.String("Categories");
        private Java.Lang.String _ItemsHeaderDataItem = new Java.Lang.String("Items");

        // Methods
        public EditAdapter(ViewModel.Jar jarViewModel, View categoriesHeaderView, View itemsHeaderView, CallbackSet callbacks, LayoutInflater inflater)
        {
            _Inflater = inflater;
            _CategoriesHeader = categoriesHeaderView;
            _ItemsHeader = itemsHeaderView;
            _JarViewModel = jarViewModel;
            _Callbacks = callbacks;

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
                ViewModel.Category category = (ViewModel.Category)GetItem(position);
                var color = Android.Graphics.Color.Argb(255, category.Color.R, category.Color.G, category.Color.B);

                resultView = _Inflater.Inflate(Resource.Layout.EditCategory, null);
                resultView.FindViewById<TextView>(Resource.Id.edit_category_title).Text = category.Title;
                resultView.FindViewById<TextView>(Resource.Id.edit_category_title).Click += delegate { _Callbacks.CallbackCategoryTitleClicked(category); };
                resultView.FindViewById<ImageButton>(Resource.Id.edit_category_swatch).SetColorFilter(color, Android.Graphics.PorterDuff.Mode.Multiply);
                resultView.FindViewById<ImageButton>(Resource.Id.edit_category_swatch).Click += delegate { _Callbacks.CallbackCategorySwatchClicked(category); };
                resultView.FindViewById<Button>(Resource.Id.edit_category_delete).Click += delegate { _Callbacks.CallbackCategoryDeleteClicked(category); };
            }
            else if (position == (1 + _JarViewModel.Categories.Count))
            {
                // Item header.
                resultView = _ItemsHeader;
            }
            else
            {
                // Item.
                ViewModel.Item item = (ViewModel.Item)GetItem(position);
                Android.Graphics.Color? color = null;
                if (item.Color != null)
                {
                    color = Android.Graphics.Color.Argb(255, item.Color.Value.R, item.Color.Value.G, item.Color.Value.B);
                }

                resultView = _Inflater.Inflate(Resource.Layout.EditItem, null);
                resultView.FindViewById<TextView>(Resource.Id.edit_item_content).Text = item.Content;
                resultView.FindViewById<TextView>(Resource.Id.edit_item_content).Click += delegate { _Callbacks.CallbackItemContentClicked(item); };
                resultView.FindViewById<Button>(Resource.Id.edit_item_delete).Click += delegate { _Callbacks.CallbackItemDeleteClicked(item); };
                resultView.FindViewById<ImageView>(Resource.Id.edit_item_swatch).Click += delegate { _Callbacks.CallbackItemCategoryClicked(item); };

                if (color != null)
                {
                    resultView.FindViewById<ImageView>(Resource.Id.edit_item_swatch).SetColorFilter(color.Value, Android.Graphics.PorterDuff.Mode.Multiply);
                }
                else
                {
                    resultView.FindViewById<ImageView>(Resource.Id.edit_item_swatch).SetImageResource(Resource.Drawable.circle_dashed);
                    resultView.FindViewById<ImageView>(Resource.Id.edit_item_swatch).SetColorFilter(Android.Graphics.Color.Argb(0x3A, 0, 0 , 0), Android.Graphics.PorterDuff.Mode.Multiply);
                }
            }
            return resultView;
        }
    }
}