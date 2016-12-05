// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;

namespace MasonJar.Model
{
    public interface IItem
    {
        event EventHandler CategoryChanged;
        event EventHandler ContentChanged;

        ICategory Category { get; set; }
        string    Content  { get; set; }
    }
}