// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using MasonJar.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MasonJar.Model.Real
{
    public class Jar : IJar
    {
        public List<ICategory>    Categories { get { return _Categories; } }
        public List<IItem>        Items      { get { return _Items; } }
        public List<IHistoryItem> History    { get { return _History; } }

        public event EventHandler CategoryCollectionChanged;
        public event EventHandler ItemCollectionChanged;
        public event EventHandler HistoryCollectionChanged;

        private List<ICategory>    _Categories     = new List<ICategory>();
        private List<IItem>        _Items          = new List<IItem>();
        private List<IHistoryItem> _History        = new List<IHistoryItem>();
        private int                _NextColorIndex = 0;

        public void AddNewCategory()
        {
            // Get the index of the next color to pre-populate the category with.
            // We don't want to have a duplicate color if we can help it. Look through all
            // categories and if this color is already represented, goto the next index.
            Array swatchValues = Enum.GetValues(typeof(CategorySwatch));
            int swatchCount = swatchValues.Length;
            int nextColorIndex = _NextColorIndex;
            for (int i = 0; i < swatchCount; i++, nextColorIndex = ((nextColorIndex + 1) % swatchCount))
            {
                Color currColor = ((CategorySwatch)swatchValues.GetValue(nextColorIndex)).GetColor();

                // Go through all the categories and see if any of them use the same color.
                bool found = false;
                foreach (Category c in _Categories)
                {
                    if ((c.Color.R == currColor.R) &&
                        (c.Color.G == currColor.G) &&
                        (c.Color.B == currColor.B))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    break;
                }
            }

            // Get the color we'll use for the new category and advance the index for the next time.
            _NextColorIndex = (_NextColorIndex + 1) % swatchCount;
            Color nextColor = ((CategorySwatch)swatchValues.GetValue(nextColorIndex)).GetColor();

            // Create the new category.
            _Categories.Add(new Category(nextColor));
            CategoryCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddNewItem()
        {
            _Items.Add(new Item());
            ItemCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(IItem i)
        {
            if (_Items.Remove(i))
            {
                ItemCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RemoveCategory(ICategory c)
        {
            if (_Categories.Remove(c))
            {
                CategoryCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void MoveItemToHistory(IItem i)
        {
            if (_Items.Contains(i))
            {
                HistoryItem historyItem = (i.Category != null) ? new HistoryItem(i.Category.Title, i.Category.Color, i.Content) :
                                                                 new HistoryItem(null, null, i.Content);
                _Items.Remove(i);
                _History.Add(historyItem);

                ItemCollectionChanged?.Invoke(this, EventArgs.Empty);
                HistoryCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}