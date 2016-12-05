// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Collections.Generic;

namespace MasonJar.Model.Mock
{
    public class Item : IItem
    {
        public ICategory Category
        {
            get
            {
                return _Category;
            }

            set
            {
                if (_Category != value)
                {
                    // Unregister the old category.
                    if (_Category != null)
                    {
                        _Category.ColorUpdated -= CategoryUpdated;
                        _Category.TitleUpdated -= CategoryUpdated;
                    }

                    // Register the new category.
                    _Category = value;
                    if (_Category != null)
                    {
                        _Category.ColorUpdated += CategoryUpdated;
                        _Category.TitleUpdated += CategoryUpdated;
                    }

                    // Signal for the new category.
                    CategoryChanged(this, EventArgs.Empty);
                }

            }
        }

        public string Content
        {
            get
            {
                return _Content;
            }

            set
            {
                if (_Content != value)
                {
                    _Content = value;
                    ContentChanged(this, EventArgs.Empty);
                }
            }
        }

        private ICategory _Category = null;
        private string _Content = "";

        public event EventHandler CategoryChanged;
        public event EventHandler ContentChanged;

        public Item(List<ICategory> categories)
        {
            Random r = new Random(GetHashCode());
            Category = ((r.Next() % 2) == 0) ? categories[r.Next() % categories.Count] : null;
            Content = "Item #" + (1 + (r.Next() % 99));

            if (_Category != null)
            {
                _Category.ColorUpdated += CategoryUpdated;
                _Category.TitleUpdated += CategoryUpdated;
            }
        }

        private void CategoryUpdated(object source, EventArgs args)
        {
            CategoryChanged(source, args);
        }
    }
}