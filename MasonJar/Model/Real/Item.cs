// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Collections.Generic;
using System.Text;

namespace MasonJar.Model.Real
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
                    CategoryChanged?.Invoke(this, EventArgs.Empty);
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
                    ContentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private ICategory _Category = null;
        private string    _Content = "";

        private static string NO_CATEGORY_TOKEN = "no_category";

        public event EventHandler CategoryChanged;
        public event EventHandler ContentChanged;

        public Item(Category category, string content)
        {
            _Category = category;
            _Content = content.Trim();

            if (_Category != null)
            {
                _Category.ColorUpdated += CategoryUpdated;
                _Category.TitleUpdated += CategoryUpdated;
            }
        }

        public Item(string data, List<ICategory> categories)
        {
            string[] dataTokens = data.Split(null);
            if (!dataTokens[0].Equals(NO_CATEGORY_TOKEN))
            {
                int id = int.Parse(dataTokens[0]);
                foreach (ICategory category in categories)
                {
                    if (category.Id == id)
                    {
                        _Category = category;
                        break;
                    }
                }
            }

            for (int i = 1; i < dataTokens.Length; ++i)
            {
                _Content += " " + dataTokens[i];
            }
            _Content = _Content.Trim();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_Category == null)
            {
                sb.Append(NO_CATEGORY_TOKEN);
            }
            else
            {
                sb.Append(_Category.Id);
            }
            sb.Append(" ");
            sb.Append(_Content);
            return sb.ToString();
        }

        private void CategoryUpdated(object source, EventArgs args)
        {
            CategoryChanged?.Invoke(source, args);
        }
    }
}