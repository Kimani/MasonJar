// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.ViewModel
{
    public class Item : Java.Lang.Object
    {
        public Category    Category  { get { return _ItemCategory; }      set { SetCategory(value, false); } }
        public string      Content   { get { return _ItemModel.Content; } set { _ItemModel.Content = value; } }
        public Color?      Color     { get { return (Category != null) ? Category.Color : (Color?)null; } }
        public Model.IItem ItemModel { get { return _ItemModel; } }

        public event EventHandler ItemChanged;

        private Model.IItem _ItemModel;
        private Category    _ItemCategory;

        public Item(Category category, Model.IItem itemModel)
        {
            _ItemModel = itemModel;
            itemModel.CategoryChanged += ItemOrCategoryChanged;
            itemModel.ContentChanged += ItemOrCategoryChanged;

            SetCategory(category, true);
        }

        public bool HasModel(Model.IItem item)
        {
            return _ItemModel == item;
        }

        public void SetCategory(Category category, bool initial)
        {
            if (_ItemCategory != category)
            {
                // Unregister current category events.
                if (_ItemCategory != null)
                {
                    _ItemCategory.CategoryUpdated -= ItemOrCategoryChanged;
                }

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
            ItemChanged?.Invoke(sender, args);
        }
    }
}