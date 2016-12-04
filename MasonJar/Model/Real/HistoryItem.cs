// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Model.Real
{
    public class HistoryItem : IHistoryItem
    {
        public string Category { get; private set; }
        public Color? Color    { get; private set; }
        public string Content  { get; private set; }
        public string Title    { get; private set; }
        public DateTime Timestamp { get; private set; }

        public HistoryItem(string category, Color? color, string content, string title)
        {
            Category = category;
            Color = color;
            Content = content;
            Title = title;
            Timestamp = DateTime.Now;
        }
    }
}