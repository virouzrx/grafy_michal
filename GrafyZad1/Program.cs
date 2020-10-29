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
        public static List<int> rejectedNodes = new List<int>();
        public static List<int> allNodes = new List<int>();
        public static Dictionary<int, List<int>> removedNodes = new Dictionary<int, List<int>>();
        public static bool warunek = false;
        public static bool finalCondition = false;

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

            foreach (var item in GrafParsed.graf) //szukaj elementow wiszacych lub takich, ktore maja tylko wejscia - taki element napewno bedzie w zbiorze rozwiazan
            {
                allNodes.Add(item.Key);
                if (!item.Value.Any())//jesli element nie ma zadnych wyjsc, sprawdz czy ktorykolwiek element ma do niego wejscia
                {
                    wynik.Add(item.Key);
                    allNodes.Remove(item.Key);
                }
            }
            Console.WriteLine(GrafParsed.graf.Count);
            while (!finalCondition || !Enumerable.SequenceEqual(allNodes.OrderBy(e => e), rejectedNodes.OrderBy(e => e)))
            {
                Console.WriteLine("pierwsza faza");//pierwszy element 
                for (int i = 0; i < GrafParsed.graf.Count; i++)
                {
                    if (!wynik.Any()) //jesli nie ma zadnych elementow wiszacych dodaj 0
                    {
                        var key = GrafParsed.graf.ElementAt(i).Key;
                        wynik.Add(key);
                    }
                    else if (wynik.Contains(GrafParsed.graf.ElementAt(i).Key) || rejectedNodes.Contains(GrafParsed.graf.ElementAt(i).Key))
                    {
                        continue; //jesli ten element jest juz w wyniku lub elemenet zostal odrzucony, pomin
                    }
                    else
                    {
                        Console.WriteLine("druga faza");
                        warunek = false;
                        foreach (var item in wynik)
                        {
                            if (!GrafParsed.graf[i].Contains(item))
                            {
                                if (!GrafParsed.graf[item].Contains(i))
                                {
                                    warunek = true;
                                }
                                else
                                {
                                    warunek = false;
                                    break;
                                }
                            }
                            else
                            {
                                warunek = false;
                                break;
                            }
                        }
                        if (warunek)
                        {
                            var key = GrafParsed.graf.ElementAt(i).Key;
                            wynik.Add(key);
                            temp.Add(key);
                            Console.WriteLine("added element: " + key);
                        }
                        else continue;
                    }
                    Console.ReadKey();
                }
                foreach (var item in wynik)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("trzecia faza");
                foreach (var item in GrafParsed.graf) // sprawdz wynik
                {
                    finalCondition = true;
                    if (wynik.Contains(item.Key))
                    {
                        continue;
                    }
                    else if (item.Value.Intersect(wynik).Any()) //wartosci z grafu musza miec w sobie wynik
                    {
                        continue;
                    }
                    else
                    {
                        finalCondition = false;
                        foreach (var item2 in temp)
                        {
                            wynik.Remove(item2);
                            rejectedNodes.Add(item2); //dodaj je do listy wykluczonych wierzcholkow
                        }
                        temp.Clear();//wyczysc liste tymczasowych wierzcholkow
                        break;
                    }

                }
                Console.ReadKey();
            }
            if (finalCondition == true)
            {
                foreach (var item in wynik)
                {
                    Console.Write(item);
                }
            }
            else
            {
                Console.WriteLine("Brak rozwiązań dla danego zbioru");
                foreach (var item in wynik)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}




