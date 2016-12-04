// [Ready Design Corps] - [Mason Jar] - Copyright 2016

namespace MasonJar.ViewModel
{
    // Represents the end of a stick to be pulled from the jar.
    public class Stick
    {
        public Category Category { get; private set; }
        public bool     Random   { get; private set; }

        public Stick(Category c)
        {
            Category = c;
            Random = false;
        }

        public Stick()
        {
            Category = null;
            Random = true;
        }
    }
}