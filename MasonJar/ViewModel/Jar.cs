// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Runtime.CompilerServices;
using MasonJar.Model;

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

        private IJar _Model;

        public Jar()
        {
            _Model = JarFactory.GetInstance(_UseMockData);
        }
    }
}