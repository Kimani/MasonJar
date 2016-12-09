// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MasonJar.Common;

namespace MasonJar
{
    [Activity(Label = "EditActivity")]
    public class EditActivity : ListActivity
    {
        private ViewModel.Jar _JarViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Setup the layout.
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Edit);
            ViewHelper.FixBackgroundRepeat(FindViewById<LinearLayout>(Resource.Id.maineditlayout));

            // Get the view model.
            _JarViewModel = ViewModel.Jar.GetInstance();

            // Create the headers and make the buttons hook up to this activity.
            LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);

            View categoriesHeaderView = inflater.Inflate(Resource.Layout.EditHeaderCategories, null);
            categoriesHeaderView.FindViewById<ImageButton>(Resource.Id.edit_button_add_category).Click += delegate { AddCategoryClicked(); };

            View itemsHeaderView = inflater.Inflate(Resource.Layout.EditHeaderItems, null);
            itemsHeaderView.FindViewById<ImageButton>(Resource.Id.edit_button_add_item).Click += delegate { AddItemClicked(); };

            // Set the list view content.
            EditAdapter adapter = new EditAdapter(_JarViewModel, categoriesHeaderView, itemsHeaderView, inflater);
            ListAdapter = adapter;
            adapter.NotifyDataSetChanged();
            ListView.RequestFocus();
        }

        public override void Finish()
        {
            base.Finish();
            OverridePendingTransition(0, 0);
        }

        private void AddCategoryClicked()
        {

        }

        private void AddItemClicked()
        {

        }
    }
}