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
    [Activity(Label = "EditActivity")]
    public class EditActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Setup the layout.
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Edit);
            ViewHelper.FixBackgroundRepeat(FindViewById<LinearLayout>(Resource.Id.maineditlayout));
        }

        public override void Finish()
        {
            base.Finish();
            OverridePendingTransition(0, 0);
        }
    }
}