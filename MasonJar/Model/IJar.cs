// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Collections.Generic;

namespace MasonJar.Model
{
    public interface IJar
    {
        List<ICategory>    Categories { get; }
        List<IItem>        Items      { get; }
        List<IHistoryItem> History    { get; }

        void AddNewCategory();
        void AddNewItem();
        void RemoveItem(IItem i);
        void MoveItemToHistory(IItem i);
    }
}