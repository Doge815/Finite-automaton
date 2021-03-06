namespace FiniteAuto
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Extensions
    {
        public const char SeparatorX = '║';
        public const char SeparatorY = '═';
        public const char SeperatorCross = '╬';
        public const int Border = 1;

        public static string TableFormat(this string[,] table)
        {
            int maxSize = 0;

            int width = table.GetLength(0);
            int height = table.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int u = 0; u < height; u++)
                {
                    if (table[i, u].Length > maxSize) maxSize = table[i, u].Length;
                }
            }

            maxSize += 2 * Border;

            StringBuilder tableString = new StringBuilder();

            for (int i = 0; i < height; i++)
            {
                if (i == 1) // Print header separator
                {
                    for (int u = 0; u < width; u++)
                    {
                        if (u != 0)
                        {
                            tableString.Append(SeperatorCross);
                        }
                        for (int o = 0; o < maxSize; o++)
                        {
                            tableString.Append(SeparatorY);
                        }
                    }

                    tableString.AppendLine();
                }

                for (int u = 0; u < width; u++) // Print elements
                {
                    tableString.Append(GetBlock(table[u, i], u == 0));
                }

                tableString.AppendLine(); // Flush line
            }

            return tableString.ToString();

            string GetBlock(string content, bool isLeft = false)
            {
                string separator = isLeft ? string.Empty : SeparatorX.ToString();

                return separator + new string(' ', (int)Math.Ceiling((maxSize - content.Length) / 2d)) + content + new string(' ', (int)Math.Floor((maxSize - content.Length) / 2d));
            }
        }

        //public static Predicate<T> Not<T>(this Predicate<T> predicate) => x => !predicate(x);

        //public static void AddToList<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> dict, TKey key, TValue value)
        //{
        //    if (!dict.TryGetValue(key, out var list)) list = dict[key] = new List<TValue>();
        //    list.Add(value);
        //}

        public static T[] FindPartition<T>(this IEnumerable<T[]> partitions, T element) where T : notnull =>
            partitions.FirstOrDefault(x => x.Contains(element))
            ?? throw new KeyNotFoundException(
                "Could not find partition for " + element
                + " in " + partitions.ToStringPartition());

        public static string ToStringPartition<T>(this IEnumerable<IEnumerable<T>> partitions) =>
            "{ " + string.Join(", ", partitions.Select(x => "{ " + string.Join(", ", x) + " }")) + " }";

        public static IEnumerable<IEnumerable<T>> Subsets<T>(this IEnumerable<T> elements)
        {
            var enumerator = elements.GetEnumerator();

            if (!enumerator.MoveNext()) yield break;

            var head = enumerator.Current;
            // TODO: Optimize
            var remainder = new List<T>();

            while(enumerator.MoveNext())
            {
                remainder.Add(enumerator.Current);
            }

            // TODO: Optimize
            foreach (var subset in remainder.Subsets()) {
                yield return new[]{head}.Concat(subset);
                yield return subset;
            }
        }
    }
}