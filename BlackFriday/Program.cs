using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackFriday
{
    class Program
    {
        private class Edge
        {
            public int First { get; set; }

            public int Second { get; set; }

            public int Weight { get; set; }

            public override string ToString()
            {
                return $"{this.First}-{this.Second}";
            }
        }

        static void Main(string[] args)
        {
            int nodesCount = int.Parse(Console.ReadLine());
            int edgesCount = int.Parse(Console.ReadLine());
            Queue<Edge> edges = ReadEdges(edgesCount);

            GetMST(nodesCount, edges);
        }

        private static void GetMST(int nodesCount, Queue<Edge> edges)
        {
            int[] roots = new int[nodesCount];
            for (int i = 0; i < roots.Length; i++)
            {
                roots[i] = i;
            }

            int totalTime = 0;

            while (edges.Count > 0)
            {
                Edge edge = edges.Dequeue();

                int firstNodeRoot = GetRoot(roots, edge.First);
                int secondNodeRoot = GetRoot(roots, edge.Second);

                if (firstNodeRoot != secondNodeRoot)
                {
                    roots[secondNodeRoot] = firstNodeRoot;

                    totalTime += edge.Weight;
                }
            }

            Console.WriteLine(totalTime);
        }

        private static int GetRoot(int[] roots, int node)
        {
            while (node != roots[node])
            {
                node = roots[node];
            }

            return node;
        }

        private static Queue<Edge> ReadEdges(int edgesCount)
        {
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < edgesCount; i++)
            {
                int[] data = Console.ReadLine()
                    .Split()
                    .Select(int.Parse)
                    .ToArray();
                int first = data[0];
                int second = data[1];
                int weight = data[2];

                edges.Add(new Edge
                {
                    First = first,
                    Second = second,
                    Weight = weight,
                });
            }

            return new Queue<Edge>(edges.OrderBy(e => e.Weight));
        }
    }
}
