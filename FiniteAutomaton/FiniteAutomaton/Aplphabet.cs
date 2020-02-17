using System.Collections.Generic;
namespace FiniteAuto
{
    using System;
    using System.Linq;
    public class Aplphabet
    {
        public Type t;
        private List<t> symbols;
        public List<t> Symbols {get => symbols.ToList(); }

        public Aplphabet (IEnumerable<t> symbol)
        {
            symbols = symbol.ToList();
        }
    }
}