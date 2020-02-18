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
                string whiteSpace = new string(' ', (int)Math.Floor((maxSize - content.Length) / 2d));
                string separator = isLeft ? string.Empty : SeparatorX.ToString();

                return separator + whiteSpace + content + whiteSpace;
            }
        }

        //public static Predicate<T> Not<T>(this Predicate<T> predicate) => x => !predicate(x);

        //public static void AddToList<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> dict, TKey key, TValue value)
        //{
        //    if (!dict.TryGetValue(key, out var list)) list = dict[key] = new List<TValue>();
        //    list.Add(value);
        //}

        public static T[]? FindPartition<T>(this IEnumerable<T[]?> partitions, T element) =>
            partitions.FirstOrDefault(x => x?.Contains(element) == true);
    }
}