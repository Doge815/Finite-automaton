using System.Collections.Generic;

namespace FiniteAuto
{
    using System;
    using System.Collections;
    using System.Linq;

    public class Alphabet
    {
        private readonly List<object> _symbols;
        public IReadOnlyList<object> Symbols => _symbols;

        public Alphabet(IEnumerable<object> values)
        {
            _symbols = new List<object>();
            Type t = null;
            foreach(object o in values)
            {
                if(t == null) t = o.GetType();
                if(t != o.GetType()) throw new ArgumentException(nameof(values));
                _symbols.Add(o);
            }
        }
    }
}