using System.Collections.Generic;
namespace FiniteAuto
{
    using System;
    using System.Linq;
    public class Alphabet
    {
        public Type T;
        private List<object> symbols;
        public List<object> Symbols {get => symbols.ToList(); }

        public Alphabet (IEnumerable<object> values)
        {
            Type t = null;
            symbols = new List<object>();
            foreach(object o in values)
            {
                symbols.Add(o);
                if(t == null) t = o.GetType();
                if(t != o.GetType()) throw new ArgumentException();
            }
            T = t;
        }
    }
}