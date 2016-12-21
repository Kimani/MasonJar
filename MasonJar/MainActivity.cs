// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using MasonJar.Common;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace MasonJar
{
    [Activity(Label = "MasonJar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, View.IOnTouchListener
    {
        private class KeyGroup
        {
            public RelativeLayout Group;
            public ImageView      Swatch;
            public TextView       Title;

            public KeyGroup(RelativeLayout g, ImageView s, TextView t)
            {
                Group = g;
                Swatch = s;
                Title = t;
            }
        }

        private ViewModel.Jar         _JarViewModel;
        private ImageView             _JarImage;
        private ImageView             _JarHintImage;
        private RelativeLayout        _HelpContent;
        private RelativeLayout        _SelectionOverlay;
        private ImageView             _SelectionSwatch;
        private TextView              _SelectionContent;
        private TextView              _SelectionCategory;
        private List<KeyGroup>        _KeyGroups = new List<KeyGroup>();
        private bool                  _TutorialShowing = false;
        private bool                  _JarInteractionEnabled = true;
        private List<ViewModel.Stick> _Sticks;
        private List<ImageView>       _JarCircles = new List<ImageView>();
        private Timer                 _ShakeTimer = null;
        private bool                  _ShakeLeft = false;
        private ViewModel.Item        _SelectedItem;

        private int   MAX_STICK_COUNT       = 4;
        private int   EDIT_REQUEST_CODE     = 100;
        private Color HITTEST_COLOR_JAR     = Color.FromArgb(0xFF, 0x00, 0x00);
        private Color HITTEST_COLOR_STICK_1 = Color.FromArgb(0x00, 0xFF, 0x00);
        private Color HITTEST_COLOR_STICK_2 = Color.FromArgb(0x00, 0x00, 0xFF);
        private Color HITTEST_COLOR_STICK_3 = Color.FromArgb(0xFF, 0xFF, 0x00);
        private Color HITTEST_COLOR_STICK_4 = Color.FromArgb(0xFF, 0x00, 0xFF);

        protected override void OnCreate(Bundle bundle)
        {
            // Setup the layout.
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            ViewHelper.FixBackgroundRepeat(FindViewById<LinearLayout>(Resource.Id.mainlayout));

            // Get a starting set of sticks.
            _JarViewModel = ViewModel.Jar.GetInstance();
            _Sticks = _JarViewModel.GetSticks(MAX_STICK_COUNT);

            // Get UI elements and initialize them.
            _HelpContent = FindViewById<RelativeLayout>(Resource.Id.help_content_parent);
            _JarImage = FindViewById<ImageView>(Resource.Id.jar_image);
            _JarHintImage = FindViewById<ImageView>(Resource.Id.jar_image_hit);
            _JarImage.SetOnTouchListener(this);

            _SelectionOverlay = FindViewById<RelativeLayout>(Resource.Id.main_overlay_stick_selection);
            _SelectionSwatch = FindViewById<ImageView>(Resource.Id.main_stick_selection_swatch);
            _SelectionContent = FindViewById<TextView>(Resource.Id.main_stick_selection_content);
            _SelectionCategory = FindViewById<TextView>(Resource.Id.main_stick_selection_category_title);
            FindViewById<Button>(Resource.Id.main_stick_selection_remove_stick).Click += DiscardStick;
            FindViewById<Button>(Resource.Id.main_stick_selection_replace_stick).Click += ReplaceStick;

            _JarCircles.Add(FindViewById<ImageView>(Resource.Id.jar_image_circle1));
            _JarCircles.Add(FindViewById<ImageView>(Resource.Id.jar_image_circle2));
            _JarCircles.Add(FindViewById<ImageView>(Resource.Id.jar_image_circle3));
            _JarCircles.Add(FindViewById<ImageView>(Resource.Id.jar_image_circle4));

            _KeyGroups.Add(new KeyGroup(FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category_1),
                                        FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch_1),
                                        FindViewById<TextView>(Resource.Id.edit_selectable_category_title_1)));
            _KeyGroups.Add(new KeyGroup(FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category_2),
                                        FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch_2),
                                        FindViewById<TextView>(Resource.Id.edit_selectable_category_title_2)));
            _KeyGroups.Add(new KeyGroup(FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category_3),
                                        FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch_3),
                                        FindViewById<TextView>(Resource.Id.edit_selectable_category_title_3)));
            _KeyGroups.Add(new KeyGroup(FindViewById<RelativeLayout>(Resource.Id.edit_selectable_category_4),
                                        FindViewById<ImageView>(Resource.Id.edit_selectable_category_swatch_4),
                                        FindViewById<TextView>(Resource.Id.edit_selectable_category_title_4)));
            ApplySticks();

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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == EDIT_REQUEST_CODE)
            {
                // Refresh the jar selection options now that we're back from the edit screen.
                _Sticks = _JarViewModel.GetSticks(MAX_STICK_COUNT);
                ApplySticks();
            }
        }

        public void DismissTutorial()
        {
            _HelpContent.Visibility = ViewStates.Gone;
            _TutorialShowing = false;
        }

        public void EditClicked()
        {
            StartActivityForResult(new Intent(this, typeof(EditActivity)), EDIT_REQUEST_CODE);
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

        public bool OnTouch(View v, MotionEvent e)
        {
            bool handled = false;
            if (_JarInteractionEnabled)
            {
                // Get the color of the pixel that was clicked. Enable the DrawingCache
                // so we have a resource to query, and then clean it up when done.
                _JarHintImage.DrawingCacheEnabled = true;
                Android.Graphics.Bitmap hotspots = _JarHintImage.GetDrawingCache(true);
                Color pixelColor = Color.FromArgb(hotspots.GetPixel((int)e.GetX(), (int)e.GetY()));
                _JarHintImage.DrawingCacheEnabled = false;

                bool hit = (pixelColor.Equals(HITTEST_COLOR_JAR)) ||
                           (pixelColor.Equals(HITTEST_COLOR_STICK_1)) ||
                           (pixelColor.Equals(HITTEST_COLOR_STICK_2)) ||
                           (pixelColor.Equals(HITTEST_COLOR_STICK_3)) ||
                           (pixelColor.Equals(HITTEST_COLOR_STICK_4));

                if (e.Action == MotionEventActions.Down && hit)
                {
                    handled = true;
                }
                else if (e.Action == MotionEventActions.Up)
                {
                    if      (pixelColor.Equals(HITTEST_COLOR_JAR))     { PerformJarShake(); }
                    else if (pixelColor.Equals(HITTEST_COLOR_STICK_1)) { StickSelected(0); }
                    else if (pixelColor.Equals(HITTEST_COLOR_STICK_2)) { StickSelected(1); }
                    else if (pixelColor.Equals(HITTEST_COLOR_STICK_3)) { StickSelected(2); }
                    else if (pixelColor.Equals(HITTEST_COLOR_STICK_4)) { StickSelected(3); }
                    handled = true;
                }
            }
            return handled;
        }

        private void ApplySticks()
        {
            // Update the images of the jar itself. Enable interaction since we're not in the middle of shaking.
            UpdateJarImages();
            _JarInteractionEnabled = true;

            // Go through the sticks and get all the categories.
            List<ViewModel.Category> categories = new List<ViewModel.Category>();
            bool randomCategory = false;

            foreach (ViewModel.Stick stick in _Sticks)
            {
                if (stick.Random)
                {
                    randomCategory = true;
                }
                else if ((stick.Category != null) && !categories.Contains(stick.Category))
                {
                    categories.Add(stick.Category);
                }
            }

            // Update the legend entries. If we have remaining key groups, make them invisible.
            int nextKey = 0;
            if (randomCategory)
            {
                _KeyGroups[0].Group.Visibility = ViewStates.Visible;
                _KeyGroups[0].Title.Text = "Random";
                _KeyGroups[0].Swatch.SetImageResource(Resource.Drawable.random);
                _KeyGroups[0].Swatch.SetColorFilter(Android.Graphics.Color.Argb(0xFF, 0xFF, 0xFF, 0xFF), Android.Graphics.PorterDuff.Mode.Multiply);
                ++nextKey;
            }

            foreach (ViewModel.Category category in categories)
            {
                _KeyGroups[nextKey].Group.Visibility = ViewStates.Visible;
                _KeyGroups[nextKey].Title.Text = category.Title;
                _KeyGroups[nextKey].Swatch.SetImageResource(Resource.Drawable.circle);

                var color = Android.Graphics.Color.Argb(255, category.Color.R, category.Color.G, category.Color.B);
                _KeyGroups[nextKey].Swatch.SetColorFilter(color, Android.Graphics.PorterDuff.Mode.Multiply);
                ++nextKey;
            }

            for (; nextKey < 4; ++nextKey)
            {
                _KeyGroups[nextKey].Group.Visibility = ViewStates.Gone;
            }

            // Update the stick circles.
            int circleIndex = 0;
            for (; circleIndex < _Sticks.Count; ++circleIndex)
            {
                if (_Sticks[circleIndex].Random)
                {
                    _JarCircles[circleIndex].Visibility = ViewStates.Visible;
                    _JarCircles[circleIndex].SetImageResource(GetCircleResource(_Sticks.Count, circleIndex, true));
                    _JarCircles[circleIndex].SetColorFilter(Android.Graphics.Color.Argb(0xFF, 0xFF, 0xFF, 0xFF), Android.Graphics.PorterDuff.Mode.Multiply);
                }
                else if (_Sticks[circleIndex].Category != null)
                {
                    _JarCircles[circleIndex].Visibility = ViewStates.Visible;
                    _JarCircles[circleIndex].SetImageResource(GetCircleResource(_Sticks.Count, circleIndex, false));

                    var color = Android.Graphics.Color.Argb(255, _Sticks[circleIndex].Category.Color.R, 
                                                                 _Sticks[circleIndex].Category.Color.G, 
                                                                 _Sticks[circleIndex].Category.Color.B);
                    _JarCircles[circleIndex].SetColorFilter(color, Android.Graphics.PorterDuff.Mode.Multiply);
                }
                else
                {
                    _JarCircles[circleIndex].Visibility = ViewStates.Gone;
                }
            }

            for (; circleIndex < 4; ++circleIndex)
            {
                _JarCircles[circleIndex].Visibility = ViewStates.Gone;
            }
        }

        private int GetCircleResource(int stickCount, int stickIndex, bool random)
        {
            return random ? (((stickCount == 1) && (stickIndex == 0)) ? Resource.Drawable.stick1_random1 :
                             ((stickCount == 2) && (stickIndex == 0)) ? Resource.Drawable.stick2_random1 :
                             ((stickCount == 2) && (stickIndex == 1)) ? Resource.Drawable.stick2_random2 :
                             ((stickCount == 3) && (stickIndex == 0)) ? Resource.Drawable.stick3_random1 :
                             ((stickCount == 3) && (stickIndex == 1)) ? Resource.Drawable.stick3_random2 :
                             ((stickCount == 3) && (stickIndex == 2)) ? Resource.Drawable.stick3_random3 :
                             ((stickCount == 4) && (stickIndex == 0)) ? Resource.Drawable.stick4_random1 :
                             ((stickCount == 4) && (stickIndex == 1)) ? Resource.Drawable.stick4_random2 :
                             ((stickCount == 4) && (stickIndex == 2)) ? Resource.Drawable.stick4_random3 :
                                                                        Resource.Drawable.stick4_random4) :
              /* !random */ (((stickCount == 1) && (stickIndex == 0)) ? Resource.Drawable.stick1_circle1 :
                             ((stickCount == 2) && (stickIndex == 0)) ? Resource.Drawable.stick2_circle1 :
                             ((stickCount == 2) && (stickIndex == 1)) ? Resource.Drawable.stick2_circle2 :
                             ((stickCount == 3) && (stickIndex == 0)) ? Resource.Drawable.stick3_circle1 :
                             ((stickCount == 3) && (stickIndex == 1)) ? Resource.Drawable.stick3_circle2 :
                             ((stickCount == 3) && (stickIndex == 2)) ? Resource.Drawable.stick3_circle3 :
                             ((stickCount == 4) && (stickIndex == 0)) ? Resource.Drawable.stick4_circle1 :
                             ((stickCount == 4) && (stickIndex == 1)) ? Resource.Drawable.stick4_circle2 :
                             ((stickCount == 4) && (stickIndex == 2)) ? Resource.Drawable.stick4_circle3 :
                                                                        Resource.Drawable.stick4_circle4);
        }

        private void UpdateJarImages()
        {
            int stickCount = _Sticks.Count;
            _JarImage.SetImageResource((stickCount == 0) ? Resource.Drawable.Jar_Empty :
                                       (stickCount == 1) ? Resource.Drawable.Jar_Stick1 :
                                       (stickCount == 2) ? Resource.Drawable.Jar_Stick2 :
                                       (stickCount == 3) ? Resource.Drawable.Jar_Stick3 :
                                                           Resource.Drawable.Jar_Stick4);
            _JarHintImage.SetImageResource((stickCount == 0) ? Resource.Drawable.hit_empty :
                                           (stickCount == 1) ? Resource.Drawable.hit_stick1 :
                                           (stickCount == 2) ? Resource.Drawable.hit_stick2 :
                                           (stickCount == 3) ? Resource.Drawable.hit_stick3 :
                                                               Resource.Drawable.hit_stick4);
        }

        private void JarShakeComplete(object state)
        {
            RunOnUiThread(delegate
            {
                _JarInteractionEnabled = true;
                _ShakeTimer.Dispose();
                _ShakeTimer = null;

                // Grab new sticks and populate the view.
                _Sticks = _JarViewModel.GetSticks(MAX_STICK_COUNT);
                ApplySticks();
            });
        }

        private void PerformJarShake()
        {
            if (_JarInteractionEnabled && (_JarViewModel.Items.Count > 0))
            {
                // Disable interaction so we don't get a second shake.
                _JarInteractionEnabled = false;

                // Setup a timer that fires when the jar should stop shaking.
                TimerCallback timerDelegate = new TimerCallback(JarShakeComplete);
                _ShakeTimer = new Timer(timerDelegate, null, 750, Timeout.Infinite);

                // Show a shaking jar.
                _JarImage.SetImageResource(_ShakeLeft ? Resource.Drawable.Jar_ShakeLeft : Resource.Drawable.Jar_ShakeRight);
                _ShakeLeft = !_ShakeLeft;

                // Clear the key and circles.
                _Sticks.Clear();

                foreach (KeyGroup group in _KeyGroups)
                {
                    group.Group.Visibility = ViewStates.Gone;
                }

                foreach (ImageView view in _JarCircles)
                {
                    view.Visibility = ViewStates.Gone;
                }
            }
        }

        private void StickSelected(int position)
        {
            _SelectionOverlay.Visibility = ViewStates.Visible;

            // Get an item from the jar that matches the stick that was selected.
            _SelectedItem = _JarViewModel.GetItemFromStick(_Sticks[position]);

            _SelectionContent.Text = _SelectedItem.Content;
            if (_SelectedItem.Category != null)
            {
                _SelectionCategory.Visibility = ViewStates.Visible;
                _SelectionCategory.Text = _SelectedItem.Category.Title;

                var color = Android.Graphics.Color.Argb(255, _SelectedItem.Category.Color.R, _SelectedItem.Category.Color.G, _SelectedItem.Category.Color.B);
                _SelectionSwatch.Visibility = ViewStates.Visible;
                _SelectionSwatch.SetColorFilter(color, Android.Graphics.PorterDuff.Mode.Multiply);
            }
            else
            {
                _SelectionCategory.Visibility = ViewStates.Invisible;
                _SelectionSwatch.Visibility = ViewStates.Invisible;
            }
        }

        private void ReplaceStick(object sender, EventArgs args)
        {
            DismissStickSelection();
        }

        private void DiscardStick(object sender, EventArgs args)
        {
            _JarViewModel.MoveItemToHistory(_SelectedItem);

            //_SelectionOverlay = FindViewById<RelativeLayout>(Resource.Id.main_overlay_stick_selection);
            //_SelectionSwatch = FindViewById<ImageView>(Resource.Id.main_stick_selection_swatch);
            //_SelectionContent = FindViewById<TextView>(Resource.Id.main_stick_selection_content);
            //_SelectionCategory = FindViewById<TextView>(Resource.Id.main_stick_selection_category_title);

            DismissStickSelection();
        }

        private void DismissStickSelection()
        {
            _SelectedItem = null;
            _SelectionOverlay.Visibility = ViewStates.Invisible;
            _JarInteractionEnabled = true;

            if (_JarViewModel.Items.Count == 0)
            {
                _Sticks.Clear();
                UpdateJarImages();
            }
            else
            {
                PerformJarShake();
            }
        }
    }
}

