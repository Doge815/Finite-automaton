using System.Collections.Generic;

namespace FiniteAuto
{
    using System;
    using System.Collections;
    using System.Linq;

    public class Alphabet<TSymbol>
        where TSymbol : notnull
    {
        private readonly List<TSymbol> _symbols;
        public IReadOnlyList<TSymbol> Symbols => _symbols;

        public Alphabet(IEnumerable<TSymbol> values)
        {
            _symbols = values.ToList();
        }
    }
}