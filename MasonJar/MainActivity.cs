// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MasonJar.Model;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace MasonJar
{
    [Activity(Label = "MasonJar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            // Ensure our model and view model.
            MasonJarModel.EnsureInstance();

            // Setup the layout.
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            FixBackgroundRepeat(FindViewById<LinearLayout>(Resource.Id.mainlayout));

            // Add event handlers.
            FindViewById<ImageButton>(Resource.Id.button_edit).Click    += delegate { EditClicked(); };
            FindViewById<ImageButton>(Resource.Id.button_history).Click += delegate { HistoryClicked(); };
            FindViewById<ImageButton>(Resource.Id.button_help).Click    += delegate { HelpClicked(); };
        }

        public void EditClicked()
        {

        }

        public void HelpClicked()
        {

        }

        public void HistoryClicked()
        {

        }

        private static void FixBackgroundRepeat(View view)
        {
            BitmapDrawable bd = view.Background as BitmapDrawable;
            if (bd != null)
            {
                bd.Mutate(); // Make sure that we aren't sharing state anymore.
                bd.SetTileModeXY(Shader.TileMode.Repeat, Shader.TileMode.Repeat);
                bd.SetTargetDensity(96);
            }
        }
    }
}

