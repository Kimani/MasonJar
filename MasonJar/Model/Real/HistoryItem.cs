// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;
using System.Text;

namespace MasonJar.Model.Real
{
    public class HistoryItem : IHistoryItem
    {
        public string   CategoryTitle { get; private set; }
        public Color?   Color         { get; private set; }
        public string   Content       { get; private set; }
        public DateTime Timestamp     { get; private set; }

        private static string CATEGORY_START_STRING = "category_start_string";
        private static string CATEGORY_END_STRING   = "category_end_string";

        public HistoryItem(string category, Color? color, string content)
        {
            CategoryTitle = category;
            Color         = color;
            Content       = content;
            Timestamp     = DateTime.Now;
        }

        public HistoryItem(string data)
        {
            string[] dataTokens = data.Split(null);
            int nextToken = 0;
            if (dataTokens[0].Equals(CATEGORY_START_STRING))
            {
                Color = System.Drawing.Color.FromArgb(int.Parse(dataTokens[1]));
                CategoryTitle = dataTokens[2];
                for (nextToken = 3; !nextToken.Equals(CATEGORY_END_STRING); ++nextToken)
                {
                    CategoryTitle += " " + dataTokens[nextToken];
                }
            }

            Timestamp = new DateTime(long.Parse(dataTokens[nextToken++]));

            Content = dataTokens[nextToken++];
            for (; nextToken < dataTokens.Length; ++nextToken)
            {
                Content += " " + dataTokens[nextToken];
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Color != null)
            {
                sb.Append(CATEGORY_START_STRING);
                sb.Append(" ");
                sb.Append(Color.Value.ToArgb());
                sb.Append(" ");
                sb.Append(CategoryTitle.Trim());
                sb.Append(" ");
                sb.Append(CATEGORY_END_STRING);
                sb.Append(" ");
            }

            sb.Append(Timestamp.Ticks);
            sb.Append(" ");
            sb.Append(Content);
            return sb.ToString();
        }
    }
}