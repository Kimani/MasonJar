// [Ready Design Corps] - [Mason Jar] - Copyright 2016

namespace MasonJar.Model
{
    public interface IItem
    {
        ICategory Category { get; }
        string    Content  { get; }
    }
}