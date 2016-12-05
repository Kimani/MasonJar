// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using MasonJar.Common;
using System;
using System.Drawing;

namespace MasonJar.Model.Mock
{
    public class HistoryItem : IHistoryItem
    {
        public string   CategoryTitle { get; private set; }
        public Color?   Color         { get; private set; }
        public string   Content       { get; private set; }
        public DateTime Timestamp     { get; private set; }

        public HistoryItem()
        {
            Random r = new Random(GetHashCode());

            if (r.Next() % 3 == 0)
            {
                // Uncategorized item.
                CategoryTitle = null;
                Color = null;
            }
            else
            {
                // Categorized item.
                CategoryTitle = "Old Category #" + (1 + (r.Next() % 99));
                Color = ((CategorySwatch)System.Enum.GetValues(typeof(CategorySwatch)).GetValue(r.Next() % 16)).GetColor();
            }

            Content = "Old Content #" + (1 + (r.Next() % 999));
            Timestamp = DateTime.Now.AddMinutes(-(r.Next() % (60 * 24 * 3)));
        }
    }
}