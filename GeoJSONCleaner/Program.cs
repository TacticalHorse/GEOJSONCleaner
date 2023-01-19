using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace GeoJSONCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filepath = "";
            if (args.Length > 0) filepath = args[0];
            else return; 
            string resltFilePath =
                Path.GetFullPath(filepath).Replace(Path.GetFileName(filepath), "") +
                Path.GetFileNameWithoutExtension(filepath) +
                "CLEANED" + Path.GetExtension(filepath);

            Regex find = new Regex(@"""properties"" : \{[""\da-zA-Zа-яА-Я_ \.\-:,°C/()\*%Ёё№`;\+]+\},", RegexOptions.Compiled);
            StringBuilder NEWGEOJSONDATA = new StringBuilder();
            long counter = 0;
            foreach (var item in File.ReadLines(filepath))
            {
                NEWGEOJSONDATA.AppendLine(find.Replace(item, ""));
                counter++;

                if (counter % 10 == 0) Console.WriteLine($"Очитстка - линия {counter}...");
            }
            Console.Clear();
            Console.WriteLine("Очищено. Сохраняем.");
            File.WriteAllText(resltFilePath, NEWGEOJSONDATA.ToString());
            Console.WriteLine($"Сохранено по пути {resltFilePath}");
            if (!string.IsNullOrEmpty(Directory.GetParent(resltFilePath)?.FullName))
            {
                Console.WriteLine("Открыть папку? Y/N");
                if (Console.ReadKey().Key == ConsoleKey.Y) Process.Start("explorer.exe", Directory.GetParent(resltFilePath).FullName);
            }
        }
    }
}