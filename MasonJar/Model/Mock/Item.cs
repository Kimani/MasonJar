// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Collections.Generic;

namespace MasonJar.Model.Mock
{
    public class Item : IItem
    {
        public ICategory Category { get; }
        public string    Content  { get; }

        public Item(List<ICategory> categories)
        {
            Random r = new Random(GetHashCode());
            Category = ((r.Next() % 2) == 0) ? categories[r.Next() % categories.Count] : null;
            Content = "Item #" + (1 + (r.Next() % 99));
        }
    }
}