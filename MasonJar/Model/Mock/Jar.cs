// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Collections.Generic;

namespace MasonJar.Model.Mock
{
    public class Jar : IJar
    {
        private List<ICategory>    _Categories = new List<ICategory>();
        private List<IItem>        _Items      = new List<IItem>();
        private List<IHistoryItem> _History    = new List<IHistoryItem>();

        public List<ICategory>    Categories { get { return _Categories; } }
        public List<IItem>        Items      { get { return _Items; } }
        public List<IHistoryItem> History    { get { return _History; } }

        private static int MOCK_CATEGORY_COUNT = 5;
        private static int MOCK_ITEM_COUNT     = 30;
        private static int MOCK_HISTORY_COUNT  = 300;

        public Jar()
        {
            for (int i = 0; i < MOCK_CATEGORY_COUNT; ++i) { Categories.Add(new Category()); }
            for (int i = 0; i < MOCK_ITEM_COUNT; ++i)     { Items.Add(new Item(Categories)); }
            for (int i = 0; i < MOCK_HISTORY_COUNT; ++i)  { History.Add(new HistoryItem()); }
        }

        public void AddNewCategory()           {}
        public void AddNewItem()               {}
        public void RemoveItem(IItem i)        {}
        public void MoveItemToHistory(IItem i) {}
    }
}