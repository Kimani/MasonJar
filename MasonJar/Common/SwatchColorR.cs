// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Common
{
    public class SwatchColorR : Attribute, IAttribute<byte>
    {
        public byte Value { get; set; }

        public SwatchColorR(byte r)
        {
            Value = r;
        }
    }
}