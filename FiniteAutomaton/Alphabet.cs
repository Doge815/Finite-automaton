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
            _symbols = values.ToList();
        }
    }
}