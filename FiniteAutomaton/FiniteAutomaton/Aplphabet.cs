using System.Collections.Generic;
namespace FiniteAuto
{
    using System;
    using System.Linq;
    public class Aplphabet <SymbolType>
    where SymbolType : IComparable<SymbolType>, IEquatable<SymbolType>
    {
        private List<SymbolType> symbols;
        public List<SymbolType> Symbols {get => symbols.ToList(); }

        public Aplphabet (IEnumerable<SymbolType> symbol)
        {
            symbols = symbol.ToList();
        }
    }
}