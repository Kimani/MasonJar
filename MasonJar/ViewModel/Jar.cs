// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MasonJar.ViewModel
{
    public class Jar
    {
        private static bool _UseMockData = false;
        private static Jar  _Instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Jar GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new Jar();
            }
            return _Instance;
        }

        public List<Item>        Items      { get { return _ActiveItems; } }
        public List<Category>    Categories { get { return _ActiveCategories; } }
        public List<HistoryItem> History    { get { return _HistoryItems; } }

        public event EventHandler CategoryCollectionChanged;
        public event EventHandler ItemCollectionChanged;
        public event EventHandler HistoryCollectionChanged;

        private Model.IJar        _Model;
        private List<Item>        _ActiveItems      = new List<Item>();
        private List<Category>    _ActiveCategories = new List<Category>();
        private List<HistoryItem> _HistoryItems     = new List<HistoryItem>();

        private Jar()
        {
            _Model = Model.JarFactory.GetInstance(_UseMockData);
            _Model.CategoryCollectionChanged += CategoryModelCollectionChanged;
            _Model.HistoryCollectionChanged += HistoryModelCollectionChanged;
            _Model.ItemCollectionChanged += ItemModelCollectionChanged;

            // Create categories from the model.
            foreach (Model.ICategory categoryModel in _Model.Categories)
            {
                CreateCategory(categoryModel);
            }

            // Create items from the model, and reference our view model categories.
            foreach (Model.IItem itemModel in _Model.Items)
            {
                Category categoryViewModel = null;
                if (itemModel.Category != null)
                {
                    foreach (Category c in _ActiveCategories)
                    {
                        if (c.CategoryModel == itemModel.Category)
                        {
                            categoryViewModel = c;
                            break;
                        }
                    }
                }
                CreateItem(itemModel, categoryViewModel);
            }

            // Create history items.
            foreach (Model.IHistoryItem historyModel in _Model.History)
            {
                CreateHistory(historyModel);
            }
        }

        public void DeleteItem(Item item)                               { _Model.RemoveItem(item.ItemModel); }
        public void DeleteCategory(Category category)                   { _Model.RemoveCategory(category.CategoryModel); }
        public void AddNewCategory()                                    { _Model.AddNewCategory(); }
        public void AddNewItem()                                        { _Model.AddNewItem(); }
        private void CategoryDataChanged(object sender, EventArgs args) { CategoryCollectionChanged?.Invoke(sender, args); }
        private void ItemDataChanged(object sender, EventArgs args)     { ItemCollectionChanged?.Invoke(sender, args); }
        private void CreateHistory(Model.IHistoryItem historyModel)     { History.Add(new HistoryItem(historyModel)); }
        public void MoveItemToHistory(Item item)                        { _Model.MoveItemToHistory(item.ItemModel); }

        public Item GetItemFromStick(Stick stick)
        {
            Random r = new Random();

            Item result;
            if (stick.Random)
            {
                result = Items[r.Next(Items.Count)];
            }
            else
            {
                List<Item> matchingItems = new List<Item>();
                foreach (Item i in Items)
                {
                    if (i.Category == stick.Category)
                    {
                        matchingItems.Add(i);
                    }
                }
                result = matchingItems[r.Next(matchingItems.Count)];
            }
            return result;
        }

        public List<Stick> GetSticks(int maxStickCount)
        {
            List<Stick> sticks = new List<Stick>();
            if (Items.Count > 0)
            {
                // Alright. First, get a list of all the categories, without duplicates, that are present the items, in
                // the form of sticks. This will also capture items that have no categories (Stick.Category will be null).
                // As we do these, keep a count of how many items are corresponding to that stick.
                List<Stick> possibleSticks = new List<Stick>();
                foreach (Item i in Items)
                {
                    bool found = false;
                    foreach (Stick p in possibleSticks)
                    {
                        if (p.Category == i.Category)
                        {
                            p.Count++;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        possibleSticks.Add(new Stick(i.Category));
                    }
                }

                // If we have more than one kind of stick, we need to have a random option. Put that at the front of sticks.
                if (possibleSticks.Count > 1)
                {
                    sticks.Add(new Stick());
                }
                int sticksRemaining = maxStickCount - sticks.Count;

                // Make a copy of possibleSticks and start seeding sticks with items from the copy at random. Decrement their count
                // as this is done, and remove the stick from possibleSticks if the count hits zero. If we end up with extra remaining
                // slots, go through possibleSticks at random so long as it has items still, and add them again to sticks.
                Random r = new Random();
                List<Stick> uniqueSticks = new List<Stick>(possibleSticks);
                while ((sticksRemaining > 0) && (uniqueSticks.Count > 0))
                {
                    int nextSlot = r.Next(uniqueSticks.Count);
                    Stick uStick = uniqueSticks[nextSlot];
                    sticks.Add(uStick);
                    uniqueSticks.RemoveAt(nextSlot);
                    sticksRemaining--;

                    uStick.Count--;
                    if (uStick.Count == 0)
                    {
                        possibleSticks.Remove(uStick);
                    }
                }

                while ((sticksRemaining > 0) && (possibleSticks.Count > 0))
                {
                    int nextSlot = r.Next(possibleSticks.Count);
                    Stick pStick = possibleSticks[nextSlot];
                    sticks.Add(pStick);
                    sticksRemaining--;

                    pStick.Count--;
                    if (pStick.Count == 0)
                    {
                        possibleSticks.Remove(pStick);
                    }
                }
            }
            return sticks;
        }

        private void CreateItem(Model.IItem itemModel, Category categoryViewModel)
        {
            Item itemViewModel = new Item(categoryViewModel, itemModel);
            itemViewModel.ItemChanged += ItemDataChanged;
            Items.Add(itemViewModel);
        }

        private void CreateCategory(Model.ICategory categoryModel)
        {
            Category categoryViewModel = new Category(categoryModel);
            categoryViewModel.CategoryUpdated += CategoryDataChanged;
            Categories.Add(categoryViewModel);
        }

        private void CategoryModelCollectionChanged(object sender, EventArgs args)
        {
            bool updated = false;

            // Go through all the categories. If a category from the model doesn't have a corresponding entry in the view model, make one.
            // If a category from the view model doesn't have a corresponding entry in the model, remove it and remove it from all items.
            foreach (Model.ICategory modelCategory in _Model.Categories)
            {
                bool found = false;
                foreach (Category viewModelCategory in Categories)
                {
                    if (viewModelCategory.CategoryModel == modelCategory)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    CreateCategory(modelCategory);
                    updated = true;
                }
            }

            for (int i = Categories.Count - 1; i >= 0; --i)
            {
                Category viewModelCategory = Categories[i];

                bool found = false;
                foreach (Model.ICategory modelCategory in _Model.Categories)
                {
                    if (viewModelCategory.CategoryModel == modelCategory)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Categories.Remove(viewModelCategory);
                    updated = true;

                    foreach (Item viewModelItem in Items)
                    {
                        if (viewModelItem.Category == viewModelCategory)
                        {
                            viewModelItem.SetCategory(null, false);
                        }
                    }
                }
            }

            if (updated)
            {
                CategoryCollectionChanged?.Invoke(sender, args);
            }
        }

        private void ItemModelCollectionChanged(object sender, EventArgs args)
        {
            // Go through all the items. If an item from the model doesn't have a corresponding viewmodel entry, create the
            // viewmodel entry. If an item from the viewmodel doesn't have a corresponding model entry, delete the model entry.
            bool updated = false;

            foreach (Model.IItem modelItem in _Model.Items)
            {
                bool found = false;
                foreach (Item viewModelItem in Items)
                {
                    if (viewModelItem.HasModel(modelItem))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Category categoryViewModel = null;
                    if (modelItem.Category != null)
                    {
                        foreach (Category c in _ActiveCategories)
                        {
                            if (c.CategoryModel == modelItem.Category)
                            {
                                categoryViewModel = c;
                                break;
                            }
                        }
                    }

                    CreateItem(modelItem, categoryViewModel);
                    updated = true;
                }
            }

            for (int i = Items.Count - 1; i >= 0; --i)
            {
                Item viewModelItem = Items[i];

                bool found = false;
                foreach (Model.IItem modelItem in _Model.Items)
                {
                    if (viewModelItem.HasModel(modelItem))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Items.Remove(viewModelItem);
                    updated = true;
                }
            }

            if (updated)
            {
                ItemCollectionChanged?.Invoke(sender, args);
            }
        }

        private void HistoryModelCollectionChanged(object sender, EventArgs args)
        {
            bool updated = false;
            foreach (Model.IHistoryItem modelHistory in _Model.History)
            {
                bool found = false;
                foreach (HistoryItem viewModelHistory in History)
                {
                    if (viewModelHistory.HasHistoryItem(modelHistory))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    CreateHistory(modelHistory);
                    updated = true;
                }
            }

            for (int i = History.Count - 1; i >= 0; --i)
            {
                HistoryItem viewModelHistory = History[i];

                bool found = false;
                foreach (Model.IHistoryItem modelHistory in _Model.History)
                {
                    if (viewModelHistory.HasHistoryItem(modelHistory))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    History.Remove(viewModelHistory);
                    updated = true;
                }
            }

            if (updated)
            {
                HistoryCollectionChanged?.Invoke(sender, args);
            }
        }
    }
}