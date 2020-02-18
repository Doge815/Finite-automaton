namespace CLI
{
    using FiniteAuto;
    using System;
    using System.Collections.Generic;

    public static class Program
    {
        private static void Main()
        {
            Alphabet a = new Alphabet(new List<object> { 'a', 'b', 'c', 3 });
            FiniteAutomaton automaton = new FiniteAutomaton(a);
            State two = automaton.AddState();
            State one = automaton.AddState();
            State three = automaton.AddState();
            three.AddFollow(one, 'a');
            three.AddFollow(two, 'b');
            one.AddFollow(two, 'a');
            one.AddFollow(one, 'a');
            one.AddFollow(one, 'b');
            two.AddFollow(three, 'a');
            two.AddFollow(two, 'a');

            Console.WriteLine(automaton.GetTable() + "\n");
        }
    }
}