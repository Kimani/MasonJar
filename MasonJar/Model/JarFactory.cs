// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Runtime.CompilerServices;

namespace MasonJar.Model
{
    public class JarFactory
    {
        private static IJar _RealInstance;
        private static IJar _MockInstance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IJar GetInstance(bool mock)
        {
            if (mock && (_MockInstance == null))
            {
                _MockInstance = new Mock.Jar();
            }
            else if (!mock && (_RealInstance == null))
            {
                _RealInstance = new Real.Jar();
            }

            return mock ? _MockInstance : _RealInstance;
        }
    }
}