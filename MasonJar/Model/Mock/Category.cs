// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using MasonJar.Common;
using System;
using System.Drawing;

namespace MasonJar.Model.Mock
{
    class Category : ICategory
    {
        public string Title  { get; set; }
        public Color? Color  { get; set; }

        public Category()
        {
            Random r = new Random(GetHashCode());
            Title = "Random Item #" + (1 + (r.Next() % 999));
            Color = ((CategorySwatch)System.Enum.GetValues(typeof(CategorySwatch)).GetValue(r.Next() % 16)).GetColor();
        }
    }
}