// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MasonJar.ViewModel
{
    public class Jar
    {
        private static bool _UseMockData = true;
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

        private void CreateHistory(Model.IHistoryItem historyModel)
        {
            History.Add(new HistoryItem(historyModel));
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