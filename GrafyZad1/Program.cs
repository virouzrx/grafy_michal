using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrafyZad1
{
    public class GrafParsed
    {
        public static Dictionary<int, List<int>> graf = new Dictionary<int, List<int>>();
    }
    class Program
    {
        public static Stopwatch stopwatch = new Stopwatch();
        public static Regex nodeRegex = new Regex("^[0-9]*;$");
        public static Regex edgeRegex = new Regex("^[0-9]*\x20->\x20[0-9]*;$");
        public static List<int> wynik = new List<int>();
        public static List<int> temp = new List<int>();
        public static List<int> floatingNodes = new List<int>();
        public static Dictionary<int, List<int>> removedNodes = new Dictionary<int, List<int>>();

        public static void ParseGrafu(string source)
        {
            List<int> wynik = new List<int>();
            List<int> temp = new List<int>();
            var tmp = GrafParsed.graf;
            if (File.Exists(source))
            {
                string[] tekst = File.ReadAllLines(source);
                foreach (var item in tekst)
                {
                    if (nodeRegex.IsMatch(item.ToString()))
                    {
                        tmp.Add(int.Parse(item.Remove(1)), new List<int>());
                    }
                    else if (edgeRegex.IsMatch(item.ToString()))
                    {
                        var numbers = item.Split(" -> ");
                        numbers[1] = numbers[1].Remove(1);
                        _ = Int32.TryParse(numbers[0], out int vertice);
                        _ = Int32.TryParse(numbers[1], out int edges);
                        tmp[vertice].Add(edges);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono pliku. Upewnij się, że podałeś właściwą ściężkę do pliku");
            }
        }
        public static void Main(string[] args)
        {
            stopwatch.Start();
            ParseGrafu(args[0]);

            foreach (var item in GrafParsed.graf) //szukaj elementow wiszacych
            {
                if (!item.Value.Any())//jesli element nie ma zadnych wyjsc, sprawdz czy ktorykolwiek element ma do niego wejscia
                {
                    var tmp = item.Key; //wierzcholek ktory nie ma wyjsc
                    var condition = true;
                    foreach (var item2 in GrafParsed.graf)
                    {
                        if (item2.Value.Contains(tmp)) //sprawdzanie, czy wierzcholek ma wejscia
                        {
                            condition = false;
                        }
                    }
                    if (condition)
                    {
                        floatingNodes.Add(tmp);
                        GrafParsed.graf.Remove(tmp);
                    }
                }
            }
            foreach (var item in GrafParsed.graf.Keys)
            {
                Console.WriteLine(item);
            }
            //pierwszy element 
            try
            {
                for (int i = 0; i < GrafParsed.graf.Count; i++)
                {
                    if (!temp.Any())
                    {
                        var key = GrafParsed.graf.ElementAt(i).Key;
                        temp.Add(key);
                    }
                    else
                    {

                        for (int j = 0; j < temp.Count; j++)
                        {
                            if (!GrafParsed.graf[j].Contains(i))
                            {
                                if (!GrafParsed.graf[i].Contains(j))
                                {
                                    if (!temp.Contains(i))
                                    {
                                        var key = GrafParsed.graf.ElementAt(i).Key;
                                        temp.Add(key);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
            }
            foreach (var item in temp)
            {
                Console.WriteLine(item);
            }
        }
    }
}


