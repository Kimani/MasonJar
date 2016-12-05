// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.ViewModel
{
    public class Item
    {
        public Category Category { get { return _ItemCategory; }      set { SetCategory(value, false); } }
        public string   Content  { get { return _ItemModel.Content; } set { _ItemModel.Content = value; } }
        public Color?   Color    { get { return (Category != null) ? Category.Color : null; } }

        public event EventHandler ItemChanged;

        private Model.IItem _ItemModel;
        private Category    _ItemCategory;

        public Item(Category category, Model.IItem itemModel)
        {
            _ItemModel = itemModel;
            _ItemModel.CategoryChanged += ItemOrCategoryChanged;

            SetCategory(category, true);
        }

        public void SetCategory(Category category, bool initial)
        {
            if (_ItemCategory != category)
            {
                // Unregister current category events.
                _ItemCategory.CategoryUpdated -= ItemOrCategoryChanged;

                // Update the item model with the new category.
                _ItemModel.Category = (category != null) ? category.CategoryModel : null;

                // Set the category and register for events if it's not null.
                _ItemCategory = category;
                if (category != null)
                {
                    _ItemCategory.CategoryUpdated += ItemOrCategoryChanged;
                }
            }
        }

        private void ItemOrCategoryChanged(object sender, EventArgs args)
        {
            ItemChanged(sender, args);
        }
    }
}