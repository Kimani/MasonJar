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
using MasonJar.Common;

namespace MasonJar
{
    [Activity(Label = "HistoryActivity")]
    public class HistoryActivity : ListActivity
    {
        private ViewModel.Jar _JarViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.History);
            ViewHelper.FixBackgroundRepeat(FindViewById<LinearLayout>(Resource.Id.maineditlayout));

            // Get the view model.
            _JarViewModel = ViewModel.Jar.GetInstance();

            // Set the list view content.
            HistoryAdapter adapter = new HistoryAdapter(_JarViewModel, (LayoutInflater)GetSystemService(Context.LayoutInflaterService));
            ListAdapter = adapter;
            adapter.NotifyDataSetChanged();
            ListView.RequestFocus();
        }

        public override void Finish()
        {
            base.Finish();
            OverridePendingTransition(0, 0);
        }
    }
}