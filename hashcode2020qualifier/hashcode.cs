using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Qualifier
{
    internal class hashcode
    {
        private static void Main(string[] args)
        {
            string[] lines;
            Thread th = new Thread(() =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                lines = ReadInput("a_example.in");
                Pizzafy(int.Parse(lines[0].Split(' ')[0]), Array.ConvertAll(lines[1].Split(' '), int.Parse), "a_output.txt");

                lines = ReadInput("b_small.in");
                Pizzafy(int.Parse(lines[0].Split(' ')[0]), Array.ConvertAll(lines[1].Split(' '), int.Parse), "b_output.txt");

                lines = ReadInput("c_medium.in");
                Pizzafy(int.Parse(lines[0].Split(' ')[0]), Array.ConvertAll(lines[1].Split(' '), int.Parse), "c_output.txt");

                lines = ReadInput("d_quite_big.in");
                Pizzafy(int.Parse(lines[0].Split(' ')[0]), Array.ConvertAll(lines[1].Split(' '), int.Parse), "d_output.txt");

                lines = ReadInput("e_also_big.in");
                Pizzafy(int.Parse(lines[0].Split(' ')[0]), Array.ConvertAll(lines[1].Split(' '), int.Parse), "e_output.txt");

                watch.Stop();
                Console.WriteLine("Time: " + watch.ElapsedMilliseconds.ToString());
            },
            1024 * 1024 * 64);

            th.Start();
            th.Join();
        }

        /// <summary>
        /// Helper method to reduce clutter
        /// </summary>
        /// <param name="input"> The path to the file </param>
        /// <returns></returns>
        private static string[] ReadInput(string input)
        {
            return System.IO.File.ReadAllLines(Path.Combine("../../../input/", input));
        }

        /// <summary>
        /// Method to call the recursion and format the file
        /// </summary>
        /// <param name="n"> Slices to hit </param>
        /// <param name="s"> Int Array of pizzas with their slices </param>
        /// <param name="output"> Output file name </param>
        private static void Pizzafy(int n, int[] s, string output)
        {
            Tuple<int, Stack<string>> pizzas = Repizzafy(s.Length - 1, n, 0, 0, s, new Stack<string>(), new Stack<string>());

            string order = string.Empty;
            pizzas.Item2.ToList().ForEach(x => order += x + " ");
            Console.WriteLine("Slices: " + pizzas.Item1);
            Console.WriteLine(pizzas.Item2.Count);
            Console.WriteLine();

            string[] lines = { pizzas.Item2.Count.ToString(), order };
            System.IO.File.WriteAllLines(Path.Combine(Directory.GetCurrentDirectory(), "../../../output/" + output), lines);
        }


        /// <summary>
        /// Recursive method to create the result tree
        /// </summary>
        /// <param name="i"> Current Index </param>
        /// <param name="n"> Maximum number of slices </param>
        /// <param name="se"> Slices eaten </param>
        /// <param name="he"> Highest eaten slices </param>
        /// <param name="s"> Int Array of pizzas with their slices </param>
        /// <param name="pe"> Order of Pizzas eaten for current iteration </param>
        /// <param name="hep"> Order of Pizzas eaten for the highest iteration </param>
        /// <returns></returns>
        private static Tuple<int, Stack<string>> Repizzafy(int i, int n, int se, int he, int[] s, Stack<string> pe, Stack<string> hep)
        {
            // Base conditions
            // Index reaches below 0
            if (i < 0)
            {
                // If slices is greater than highest
                if (se > he)
                {
                    he = se;
                    hep = new Stack<string>(pe.Reverse());
                    return new Tuple<int, Stack<string>>(he, hep);
                }

                return new Tuple<int, Stack<string>>(se, hep);
            }
            // Slices eaten is the exact number
            if (se + s[i] == n)
            {
                pe.Push(i.ToString());
                return new Tuple<int, Stack<string>>(se + s[i], pe);
            }
            // Slices is greater than the exact number
            else if (se + s[i] > n)
                return Repizzafy(i - 1, n, se, he, s, pe, hep);
            // Normal condition, less than n
            else
            {
                // Traverse the right side of the tree first
                pe.Push(i.ToString());
                Tuple<int, Stack<string>> temp1 = Repizzafy(i - 1, n, se + s[i], he, s, pe, hep);

                // Fall out for the base condition
                if (temp1.Item1 == n)
                    return new Tuple<int, Stack<string>>(n, temp1.Item2);

                // Traverse the left
                pe.Pop();
                Tuple<int, Stack<string>> temp2 = Repizzafy(i - 1, n, se, he, s, pe, hep);

                // Fall out for the base condition
                if (temp2.Item1 == n)
                    return new Tuple<int, Stack<string>>(n, temp2.Item2);

                // Comparison for majority of iterations
                if (temp1.Item1 > temp2.Item1)
                    return temp1;
                else
                    return temp2;
            }
        }
    }
}