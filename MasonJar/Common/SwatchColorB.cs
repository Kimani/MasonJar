// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Common
{
    public class SwatchColorB : Attribute, IAttribute<byte>
    {
        public byte Value { get; set; }

        public SwatchColorB(byte r)
        {
            Value = r;
        }
    }
}