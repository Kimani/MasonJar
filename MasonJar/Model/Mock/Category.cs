// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using MasonJar.Common;
using System;
using System.Drawing;

namespace MasonJar.Model.Mock
{
    class Category : ICategory
    {
        public string Title
        {
            get { return _Title; }
            set { if (_Title != value) { _Title = value; TitleUpdated(this, EventArgs.Empty); } }
        }

        public Color Color
        {
            get { return _Color; }
            set { if (_Color != value) { _Color = value; ColorUpdated(this, EventArgs.Empty); } }
        }

        private string _Title = "";
        private Color _Color;

        public event EventHandler TitleUpdated;
        public event EventHandler ColorUpdated;

        public Category()
        {
            Random r = new Random(GetHashCode());
            Title = "Random Item #" + (1 + (r.Next() % 999));
            Color = ((CategorySwatch)System.Enum.GetValues(typeof(CategorySwatch)).GetValue(r.Next() % 16)).GetColor();
        }
    }
}