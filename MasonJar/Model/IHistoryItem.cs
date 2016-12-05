// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Model
{
    public interface IHistoryItem
    {
        string   CategoryTitle { get; } // Name of the category. Can be null if the item was uncategorized.
        Color?   Color         { get; } // Color of the category. Should be null if Category is null.
        string   Content       { get; } // Content of the item.
        DateTime Timestamp     { get; } // Time the item was picked by the user.
    }
}