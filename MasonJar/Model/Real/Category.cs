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

        public int Id { get; private set; }

        private string _Title = "";
        private Color  _Color;

        public event EventHandler TitleUpdated;
        public event EventHandler ColorUpdated;

        public Category(Color c, string title)
        {
            _Color = c;
            _Title = title;
            Id = new Random().Next();
        }

        public Category(string data)
        {
            // Get the category from string data.
            string[] dataTokens = data.Split(null);
            Id = int.Parse(dataTokens[0]);
            _Color = Color.FromArgb(int.Parse(dataTokens[1]));

            _Title = dataTokens[2];
            for (int i = 3; i < dataTokens.Length; ++i)
            {
                _Title += " " + dataTokens[i];
            }
        }

        public override string ToString()
        {
            return Id + " " + _Color.ToArgb() + " " + _Title;
        }
    }
}