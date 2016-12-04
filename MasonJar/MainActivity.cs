// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using MasonJar.Common;

namespace MasonJar
{
    [Activity(Label = "MasonJar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private RelativeLayout _HelpContent;
        private bool           _TutorialShowing = false;

        protected override void OnCreate(Bundle bundle)
        {
            // Setup the layout.
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            ViewHelper.FixBackgroundRepeat(FindViewById<LinearLayout>(Resource.Id.mainlayout));

            // Get UI elements.
            _HelpContent = FindViewById<RelativeLayout>(Resource.Id.help_content_parent);

            // Add event handlers.
            FindViewById<ImageButton>(Resource.Id.button_edit).Click    += delegate { EditClicked(); };
            FindViewById<ImageButton>(Resource.Id.button_history).Click += delegate { HistoryClicked(); };
            FindViewById<ImageButton>(Resource.Id.button_help).Click    += delegate { HelpClicked(); };
            _HelpContent.Click                                          += delegate { DismissTutorial(); };
        }

        public override void OnBackPressed()
        {
            if (_TutorialShowing)
            {
                DismissTutorial();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public void DismissTutorial()
        {
            _HelpContent.Visibility = ViewStates.Gone;
            _TutorialShowing = false;
        }

        public void EditClicked()
        {
            StartActivity(new Intent(this, typeof(EditActivity)));
            OverridePendingTransition(0, 0);
        }

        public void HelpClicked()
        {
            _HelpContent.Visibility = ViewStates.Visible;
            _TutorialShowing = true;
        }

        public void HistoryClicked()
        {
            StartActivity(new Intent(this, typeof(HistoryActivity)));
            OverridePendingTransition(0, 0);
        }
    }
}

