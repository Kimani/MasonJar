// [Ready Design Corps] - [Mason Jar] - Copyright 2016

namespace MasonJar.Model.Real
{
    public class Item : IItem
    {
        public ICategory Category { get; set; }
        public string    Content  { get; set; }

        public Item()
        {
            Category = null;
            Content = "";
        }

        public Item(Category category, string content)
        {
            Category = category;
            Content = content;
        }
    }
}