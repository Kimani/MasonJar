// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Model
{
    public interface ICategory
    {
        event EventHandler TitleUpdated;
        event EventHandler ColorUpdated;

        string Title { get; set; }
        Color  Color { get; set; }
        int    Id    { get; }
    }
}