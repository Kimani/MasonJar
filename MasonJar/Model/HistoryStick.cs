// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Drawing;

namespace MasonJar.Model
{
    public class HistoryStick
    {
        public Color? Color   { get; private set; }
        public string Content { get; private set; }
        public string Title   { get; private set; }

        public HistoryStick(Color? color, string content, string title)
        {
            Color = color;
            Content = content;
            Title = title;
        }
    }
}