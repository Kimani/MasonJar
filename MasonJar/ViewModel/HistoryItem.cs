// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.ViewModel
{
    public class HistoryItem : Java.Lang.Object
    {
        public string   CategoryTitle { get { return _HistoryModel.CategoryTitle; } }
        public Color?   Color         { get { return _HistoryModel.Color; } }
        public string   Content       { get { return _HistoryModel.Content; } }
        public DateTime Timestamp     { get { return _HistoryModel.Timestamp; } }
        public bool     HasCategory   { get { return Color != null; } }

        private Model.IHistoryItem _HistoryModel;

        public HistoryItem(Model.IHistoryItem historyModel)
        {
            _HistoryModel = historyModel;
        }
    }
}