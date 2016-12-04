// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Collections.Generic;

namespace MasonJar.Model.Real
{
    public class Jar : IJar
    {
        private List<Category>    _Categories = new List<Category>();
        private List<Item>        _Items      = new List<Item>();
        private List<HistoryItem> _History    = new List<HistoryItem>();

        public List<ICategory>    Categories { get; }
        public List<IItem>        Items      { get; }
        public List<IHistoryItem> History    { get; }

        public void AddNewCategory()
        {
            
        }

        public void AddNewItem()
        {

        }

        public void RemoveItem(IItem i)
        {

        }

        public void MoveItemToHistory(IItem i)
        {

        }
    }
}