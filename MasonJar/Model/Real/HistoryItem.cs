// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Model.Real
{
    public class HistoryItem : IHistoryItem
    {
        public string   CategoryTitle { get; private set; }
        public Color?   Color         { get; private set; }
        public string   Content       { get; private set; }
        public DateTime Timestamp     { get; private set; }

        public HistoryItem(string category, Color? color, string content)
        {
            CategoryTitle = category;
            Color         = color;
            Content       = content;
            Timestamp     = DateTime.Now;
        }
    }
}