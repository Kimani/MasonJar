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
using System.Runtime.CompilerServices;

namespace MasonJar.Model
{
    public class MasonJarModel
    {
        private static MasonJarModel _Instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void EnsureInstance()
        {
            if (_Instance == null)
            {
                _Instance = new MasonJarModel();
            }
        }

        private List<Category> _Categories;
        private List<Stick>    _Sticks;
        private int            _HistoryCount;


    }
}