// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Widget;
using System;
using Android.Content;
using Android.App;
using Android.Content.Res;

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

        private static bool USE_IMAGE_SCALER = false;

        // http://stackoverflow.com/questions/10200256/out-of-memory-error-imageview-issue
        // SetScaledImage will first scale down the resource to the size of the ImageView on screen, which should aid in memory usage.
        public static void SetScaledImage(Context c, ImageView imageView, int resId)
        {
            if (USE_IMAGE_SCALER)
            {
                ((BitmapDrawable)imageView.Drawable).Bitmap.Recycle();
                imageView.ViewTreeObserver.AddOnPreDrawListener(new ViewTreePreDrawListener(c, imageView, resId));
            }
            else
            {
                imageView.SetImageResource(resId);
            }
        }

        private class ViewTreePreDrawListener : Java.Lang.Object, ViewTreeObserver.IOnPreDrawListener
        {
            ImageView _TargetView;
            Context   _TargetContext;
            int       _ResourceId;

            public ViewTreePreDrawListener(Context c, ImageView iv, int resId)
            {
                _TargetView = iv;
                _TargetContext = c;
                _ResourceId = resId;
            }

            public bool OnPreDraw()
            {
                _TargetView.ViewTreeObserver.RemoveOnPreDrawListener(this);
                int imageViewHeight = _TargetView.MeasuredHeight;
                int imageViewWidth = _TargetView.MeasuredWidth;
                _TargetView.SetImageBitmap(DecodeSampledBitmapFromResource(_TargetContext.Resources, _ResourceId, imageViewWidth, imageViewHeight));
                return true;
            }

            private static Bitmap DecodeSampledBitmapFromResource(Resources res, int resId, int reqWidth, int reqHeight)
            {
                // First decode with inJustDecodeBounds = true to check dimensions
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InJustDecodeBounds = true;
                BitmapFactory.DecodeResource(res, resId, options);

                // Calculate inSampleSize
                options.InSampleSize = calculateInSampleSize(options, reqWidth, reqHeight);

                // Decode bitmap with inSampleSize set
                options.InJustDecodeBounds = false;
                return BitmapFactory.DecodeResource(res, resId, options);
            }

            private static int calculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
            {
                // Raw height and width of image
                int height = options.OutHeight;
                int width = options.OutWidth;
                int inSampleSize = 1;

                if ((height > reqHeight) || (width > reqWidth))
                {
                    int halfHeight = height / 2;
                    int halfWidth = width / 2;

                    // Calculate the largest inSampleSize value that is a power of 2 and keeps both
                    // height and width larger than the requested height and width.
                    while (((halfHeight / inSampleSize) > reqHeight) && ((halfWidth / inSampleSize) > reqWidth))
                    {
                        inSampleSize *= 2;
                    }
                }
                return inSampleSize;
            }
        }
    }
}