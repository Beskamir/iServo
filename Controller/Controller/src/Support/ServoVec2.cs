using System.Collections.Generic;

namespace Controller
{
    public class ServoVec2<Type>
    {
        public Type X { get; set; }
        public Type Y { get; set; }

        public ServoVec2(Type value)
        {
            X = Y = value;
        }
        
        public ServoVec2(Type x, Type y)
        {
            X = x;
            Y = y;
        }
    }
}