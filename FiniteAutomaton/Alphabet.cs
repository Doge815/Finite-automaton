using System.Collections.Generic;

namespace FiniteAuto
{
    using System;
    using System.Collections;
    using System.Linq;

    public class Alphabet
        where TSymbol : notnull
    {
        private readonly List<object> _symbols;
        public IReadOnlyList<object> Symbols => _symbols;

        public Alphabet(IEnumerable values)
        {
            _symbols = values.ToList();
        }
    }
}