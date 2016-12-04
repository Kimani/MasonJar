// [Ready Design Corps] - [Mason Jar] - Copyright 2016

namespace MasonJar.Common
{
    // From http://joelforman.blogspot.com/2007/12/enums-and-custom-attributes.html
    public interface IAttribute<T>
    {
        T Value { get; }
    }
}
