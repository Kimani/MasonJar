// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Model
{
    public class Category
    {
        public string Title  { get; set; }
        public Color  Color  { get; set; } // maybe make set assert if random is set.
        public bool   Random { get; private set; }

        public Category()
        {
            Random = true;
            Title = "";
            Color = Color.Black;
        }

        public Category(Color c)
        {
            Random = false;
            Color = c;
            Title = "";
        }
    }
}