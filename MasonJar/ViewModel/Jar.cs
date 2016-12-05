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
                _ActiveCategories.Add(new Category(categoryModel));
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
                _ActiveItems.Add(new Item(categoryViewModel, itemModel));
            }

            // Create history items.
            foreach (Model.IHistoryItem historyModel in _Model.History)
            {
                _HistoryItems.Add(new HistoryItem(historyModel));
            }
        }

        private void CategoryModelCollectionChanged(object sender, EventArgs args)
        {
            CategoryCollectionChanged?.Invoke(sender, args);
        }

        private void ItemModelCollectionChanged(object sender, EventArgs args)
        {
            ItemCollectionChanged?.Invoke(sender, args);
        }

        private void HistoryModelCollectionChanged(object sender, EventArgs args)
        {
            HistoryCollectionChanged?.Invoke(sender, args);
        }
    }
}