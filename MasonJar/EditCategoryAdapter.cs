// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.Views;
using Android.Widget;
using MasonJar.Common;

namespace MasonJar
{
    public class EditCategoryAdapter : BaseAdapter
    {
        public override int Count
        {
            get
            {
                return _JarViewModel.Categories.Count + 1;
            }
        }

        private ViewModel.Jar    _JarViewModel;
        private Java.Lang.String _NoCategoryDataItem = new Java.Lang.String("NoCategory");
        private CallbackSet      _Callbacks;
        private LayoutInflater   _Inflater;

        public EditCategoryAdapter(ViewModel.Jar jarViewModel, CallbackSet callbacks, LayoutInflater inflater)
        {
            _JarViewModel = jarViewModel;
            _Callbacks = callbacks;
            _Inflater = inflater;
        }

        public override Java.Lang.Object GetItem(int position) { return (position == 0) ? _NoCategoryDataItem : GetCategory(position); }
        private Java.Lang.Object GetCategory(int position)     {  return _JarViewModel.Categories[position - 1]; }
        public override long GetItemId(int position)           { return GetItem(position).GetHashCode(); }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View resultView;
            if (position == 0)
            {
                resultView = _Inflater.Inflate(Resource.Layout.EditSelectableCategory, null);
                resultView.FindViewById<TextView>(Resource.Id.edit_selectable_category_title).Text = "No Category";
                resultView.FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch).SetImageResource(Resource.Drawable.circle_dashed);
                resultView.FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch).SetColorFilter(Android.Graphics.Color.Argb(0x3A, 0, 0, 0), Android.Graphics.PorterDuff.Mode.Multiply);
                resultView.FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category).Click += delegate { _Callbacks.CallbackItemCategorySelectionClicked(null); };
            }
            else
            {
                var category = (ViewModel.Category)GetCategory(position);
                var color = Android.Graphics.Color.Argb(255, category.Color.R, category.Color.G, category.Color.B);

                resultView = _Inflater.Inflate(Resource.Layout.EditSelectableCategory, null);
                resultView.FindViewById<TextView>(Resource.Id.edit_selectable_category_title).Text = category.Title;
                resultView.FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch).SetColorFilter(color, Android.Graphics.PorterDuff.Mode.Multiply);
                resultView.FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category).Click += delegate { _Callbacks.CallbackItemCategorySelectionClicked(category); };
            }
            return resultView;
        }
    }
}