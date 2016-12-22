// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using Android.Content;
using Android.Preferences;
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
        private ISharedPreferences _SharedPreferences;

        private static string CATEGORIES_KEY = "categories";
        private static string ITEMS_KEY      = "items";
        private static string HISTORY_KEY    = "history";

        public Jar(Context c)
        {
            // Load data from preferences.
            _SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(c);

            ICollection<string> categoryData = _SharedPreferences.GetStringSet(CATEGORIES_KEY, null);
            if (categoryData != null)
            {
                foreach (string data in categoryData)
                {
                    AddNewCategory(new Category(data), true);
                }
            }

            ICollection<string> itemsData = _SharedPreferences.GetStringSet(ITEMS_KEY, null);
            if (itemsData != null)
            {
                foreach (string data in itemsData)
                {
                    AddNewItem(new Item(data, _Categories), true);
                }
            }

            ICollection<string> historyData = _SharedPreferences.GetStringSet(HISTORY_KEY, null);
            if (historyData != null)
            {
                foreach (string data in historyData)
                {
                    AddNewHistoryItem(new HistoryItem(data), true);
                }
            }
        }

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
            AddNewCategory(new Category(nextColor, "New Category"));
        }

        private void AddNewCategory(Category category, bool init = false)
        {
            category.ColorUpdated += CategoryOrItemValueChanged;
            category.TitleUpdated += CategoryOrItemValueChanged;

            Categories.Add(category);
            if (!init)
            {
                CategoryListChanged();
            }
        }

        public void AddNewItem()
        {
            AddNewItem(new Item(null, "New Stick"));

            ItemCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void AddNewItem(Item item, bool init = false)
        {
            item.CategoryChanged += CategoryOrItemValueChanged;
            item.ContentChanged += CategoryOrItemValueChanged;

            Items.Add(item);
            if (!init)
            {
                ItemListChanged();
            }
        }

        public void RemoveItem(IItem i)
        {
            if (_Items.Remove(i))
            {
                ItemListChanged();
            }
        }

        public void RemoveCategory(ICategory c)
        {
            if (_Categories.Remove(c))
            {
                CategoryListChanged();
            }
        }

        public void MoveItemToHistory(IItem i)
        {
            if (_Items.Contains(i))
            {
                // Add the new history item, which doesn't save the data to disk. Removing the item from Items will save data to disk.
                AddNewHistoryItem((i.Category != null) ? new HistoryItem(i.Category.Title, i.Category.Color, i.Content) :
                                                         new HistoryItem(null, null, i.Content));
                RemoveItem(i);
            }
        }

        private void AddNewHistoryItem(HistoryItem item, bool init = false)
        {
            _History.Add(item);

            if (!init)
            {
                // Dont need to save data at this juncture. This gets called on init or when moving item to history.
                // Data will get saved when the move the item to history.
                HistoryCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CategoryOrItemValueChanged(object sender, EventArgs args)
        {
            // Category color/title or item content/category changed. Resave content.
            SaveDataToPreferences();
        }

        private void ItemListChanged()
        {
            SaveDataToPreferences();
            ItemCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CategoryListChanged()
        {
            SaveDataToPreferences();
            CategoryCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SaveDataToPreferences()
        {
            // Marshall all the data to strings.
            List<string> categories = new List<string>();
            foreach (Category c in Categories)
            {
                categories.Add(c.ToString());
            }

            List<string> items = new List<string>();
            foreach (Item i in Items)
            {
                items.Add(i.ToString());
            }

            List<string> historyItems = new List<string>();
            foreach (HistoryItem hi in History)
            {
                historyItems.Add(hi.ToString());
            }

            // Get the shared preferences editor and save all the data.
            ISharedPreferencesEditor editor = _SharedPreferences.Edit();
            editor.PutStringSet(CATEGORIES_KEY, categories);
            editor.PutStringSet(ITEMS_KEY, items);
            editor.PutStringSet(HISTORY_KEY, historyItems);
            editor.Commit();
        }
    }
}