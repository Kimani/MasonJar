// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Model.Real
{
    public class Category : ICategory
    {
        public string Title
        {
            get { return _Title; }
            set { if (_Title != value) { _Title = value; TitleUpdated?.Invoke(this, EventArgs.Empty); } }
        }

        public Color Color
        {
            get { return _Color; }
            set { if (_Color != value) { _Color = value; ColorUpdated?.Invoke(this, EventArgs.Empty); } }
        }

        private string _Title = "";
        private Color  _Color;

        public event EventHandler TitleUpdated;
        public event EventHandler ColorUpdated;

        public Category(Color c)               { _Color = c; }
        public Category(Color c, string title) { _Color = c; _Title = title; }
    }
}