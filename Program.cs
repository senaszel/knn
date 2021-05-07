using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiejsceNaTwojNamespace
{
    class Program
    {
        static void Main(params string[] args)
        {
            Dictionary<List<double>, double> dict;
            dict = CreateDictFromFile(args[1]);
            FindK(int.Parse(args[2]), dict);
        }

        private static Dictionary<List<double>, double> CreateDictFromFile(string path)
        {
            Dictionary<List<double>, double> rez = new();

            string[] lines = File.ReadAllLines(path);

            lines.ToList().ForEach(x =>
            {
                string[] temp = x.Split(' ');
                List<double> split = temp.ToList().Select(x => double.Parse(x)).ToList();
                double last = split.Last();
                split.RemoveAt(split.Count - 1);
                rez.TryAdd(split, last);
            });

            return rez;
        }

        private static void FindK(
            int howManyResultsOfEachDecisionClass,
            Dictionary<List<double>, double> dict)
        {
            double longestClassNb =
                dict.Select(x => x.Key[0])
                    .Distinct()
                    .OrderByDescending(x => x)
                    .First();

            int longestClassNb_Length =
                longestClassNb.ToString().Length * -1;

            double longestValueNb =
                dict.SelectMany(x => x.Key)
                    .Select(x => Math.Abs(x))
                    .OrderByDescending(x => x)
                    .First();

            int longestValueNb_Length =
                longestValueNb.ToString().Length + 2;

            List<double> classes =
                dict.Select(x => x.Key[0])
                    .Distinct()
                    .ToList();

            IOrderedEnumerable<KeyValuePair<List<double>, double>> rez =
                dict.OrderBy(x => x.Key[0])
                    .OrderBy(x => x.Value);

            StringBuilder sb = new();
            classes.ForEach(classNb =>
                {
                    sb.AppendLine("Dla klasy decyzyjnej " + classNb);
                    rez.Where(x => x.Key[0] == classNb)
                        .Select(x => x)
                        .Take(howManyResultsOfEachDecisionClass)
                        .ToList()
                        .ForEach(x =>
                        {
                            sb.Append("{");
                            x.Key.ForEach(keyElement =>
                                {
                                    if (keyElement == x.Key.First())
                                    {
                                        var format_s =
                                        $"{{0,{ (longestClassNb_Length > 3 ? longestClassNb_Length : -3) } }}";
                                        sb.Append(string.Format(format_s, keyElement));
                                    }
                                    else
                                    {
                                        var format_l = $"{{0,{longestValueNb_Length}}}";
                                        sb.Append(string.Format(format_l, keyElement));
                                    }
                                });
                            sb.Append("}");
                            sb.AppendLine($"{"| " + x.Value,7}");
                        });
                    sb.AppendLine(string.Empty);
                });

            Console.WriteLine(sb.ToString());
        }
    }
}
