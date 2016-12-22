// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.Content;
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
        private EditActivity     _Parent;

        public EditCategoryAdapter(ViewModel.Jar jarViewModel, CallbackSet callbacks, EditActivity parent)
        {
            _JarViewModel = jarViewModel;
            _Callbacks = callbacks;
            _Parent = parent;
        }

        public override Java.Lang.Object GetItem(int position) { return (position == 0) ? _NoCategoryDataItem : GetCategory(position); }
        private Java.Lang.Object GetCategory(int position)     {  return _JarViewModel.Categories[position - 1]; }
        public override long GetItemId(int position)           { return GetItem(position).GetHashCode(); }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_Parent.GetSystemService(Context.LayoutInflaterService);
            View resultView;

            if (position == 0)
            {
                resultView = inflater.Inflate(Resource.Layout.EditSelectableCategory, null);
                resultView.FindViewById<TextView>(Resource.Id.edit_selectable_category_title).Text = "No Category";
                resultView.FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category).Click += delegate { _Callbacks.CallbackItemCategorySelectionClicked(null); };

                ImageView imageView = resultView.FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch);
                ViewHelper.SetScaledImage(_Parent, imageView,Resource.Drawable.circle_dashed);
                imageView.SetColorFilter(Android.Graphics.Color.Argb(0x3A, 0, 0, 0), Android.Graphics.PorterDuff.Mode.Multiply);
            }
            else
            {
                var category = (ViewModel.Category)GetCategory(position);
                var color = Android.Graphics.Color.Argb(255, category.Color.R, category.Color.G, category.Color.B);

                resultView = inflater.Inflate(Resource.Layout.EditSelectableCategory, null);
                resultView.FindViewById<TextView>(Resource.Id.edit_selectable_category_title).Text = category.Title;
                resultView.FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category).Click += delegate { _Callbacks.CallbackItemCategorySelectionClicked(category); };
                resultView.FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch).SetColorFilter(color, Android.Graphics.PorterDuff.Mode.Multiply);
            }
            return resultView;
        }
    }
}