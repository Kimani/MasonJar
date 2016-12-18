// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.ViewModel
{
    public class Category : Java.Lang.Object
    {
        public Model.ICategory CategoryModel { get; private set; }
        public Color  Color                  { get { return CategoryModel.Color; } set { CategoryModel.Color = value; } }
        public string Title                  { get { return CategoryModel.Title; } set { CategoryModel.Title = value; } }

        public event EventHandler CategoryUpdated;

        public Category()
        {
            CategoryModel = null;
        }

        public Category(Model.ICategory categoryModel)
        {
            CategoryModel = categoryModel;
            categoryModel.TitleUpdated += CategoryModelUpdated;
            categoryModel.ColorUpdated += CategoryModelUpdated;
        }

        private void CategoryModelUpdated(object sender, EventArgs args)
        {
            CategoryUpdated?.Invoke(sender, args);
        }
    }
}