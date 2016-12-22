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
            set { if (_Title != value) { _Title = value; TitleUpdated?.Invoke(this, EventArgs.Empty); } }
        }

        public Color Color
        {
            get { return _Color; }
            set { if (_Color != value) { _Color = value; ColorUpdated?.Invoke(this, EventArgs.Empty); } }
        }

        public int Id { get; private set; }

        private string _Title = "";
        private Color _Color;

        public event EventHandler TitleUpdated;
        public event EventHandler ColorUpdated;

        public Category()
        {
            Random r = new Random(GetHashCode());
            _Title = "Random Category #" + (1 + (r.Next() % 999));
            _Color = ((CategorySwatch)System.Enum.GetValues(typeof(CategorySwatch)).GetValue(r.Next() % 16)).GetColor();
            Id = r.Next();
        }
    }
}