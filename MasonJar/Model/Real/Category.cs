// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Drawing;

namespace MasonJar.Model.Real
{
    public class Category : ICategory
    {
        public string Title  { get; set; }
        public Color? Color  { get; set; }

        public Category()
        {
            Title = "";
            Color = null;
        }

        public Category(Color c)
        {
            Color = c;
            Title = "";
        }

        public Category(Color c, string title)
        {
            Color = c;
            Title = title;
        }
    }
}