// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace MasonJar.Common
{
    public class ViewHelper
    {
        public static void FixBackgroundRepeat(View view)
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