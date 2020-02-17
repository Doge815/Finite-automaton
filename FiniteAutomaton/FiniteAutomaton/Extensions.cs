#pragma warning disable RCS1037
namespace FiniteAuto
{
    using System.Text;
    public static class Extensions
    {
        public static string TableFormat(this string[,] table)
        {
            const char SepX = '║';
            const char SepY = '═';
            const char SeperatorE = '╬';
            const int minBorder = 1;
            int maxSize = 0;
            
            int tableX = table.GetLength(0);
            int tableY = table.GetLength(1);

            for(int i = 0; i < tableX; i++)
            {
                for(int u = 0; u < tableY; u++)
                {
                    if(table[i,u].Length > maxSize) maxSize = table[i,u].Length;
                }
            }
            maxSize += 2*minBorder;
            
            StringBuilder tableString = new StringBuilder();

            for(int i = 0; i < tableY; i++)
            {
                if(i == 1)
                {
                    for(int u = 0; u < tableX; u++)
                    {
                        for(int o = 0; o < maxSize; o++)
                        {
                            tableString.Append(SepY);
                        }
                        if(u != tableX - 1)
                        tableString.Append(SeperatorE);
                    }
                    tableString.AppendLine();
                }
                for(int u = 0; u < tableX; u++)
                {
                    //Console.WriteLine($"{i} {u}: {table[u,i]}");
                    tableString.Append(GetBlock(table[u,i], u == 0));
                }
                tableString.AppendLine();
            }
            return tableString.ToString();

            string GetBlock(string content, bool Left = false)
            {
                string block = string.Empty;
                if(!Left) block += SepX;
                for(int i = 0; i < System.Math.Floor((maxSize-content.Length)/2d); i++)
                {
                    block += " ";
                }
                block += content;
                for(int i = 0; i < System.Math.Ceiling((maxSize-content.Length)/2d); i++)
                {
                    block += " ";
                }
                return block;
            }
        }
    }
}