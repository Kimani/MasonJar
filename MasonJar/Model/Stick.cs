// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasonJar.Model
{
    public class Stick
    {
        public Category Category { get; set; }
        public string   Content  { get; set; }

        public Stick()
        {
            Category = null;
            Content = "";
        }
    }
}