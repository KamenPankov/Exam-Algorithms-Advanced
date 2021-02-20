using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Wintellect.PowerCollections;

namespace EmergencyPlan
{
    class Program
    {
        private class Edge
        {
            public int First { get; set; }

            public int Second { get; set; }

            public TimeSpan Time { get; set; }

            public override string ToString()
            {
                return $"{this.First}-{this.Second}-{this.Time}";
            }
        }

        static void Main(string[] args)
        {
            int nodesCount = int.Parse(Console.ReadLine());
            int[] exitRooms = Console.ReadLine().Split().Select(int.Parse).ToArray();
            HashSet<int> exits = new HashSet<int>(exitRooms);
            int edgesCount = int.Parse(Console.ReadLine());
            Dictionary<int, List<Edge>> graph = ReadGraph(nodesCount, edgesCount);
            int[] data = Console.ReadLine()
                .Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            int minutes = data[0];
            int seconds = data[1];
            TimeSpan evacuateTime = new TimeSpan(0, minutes, seconds);

            for (int node = 0; node < graph.Count; node++)
            {
                if (exits.Contains(node))
                {
                    continue;
                }

                GetShortestPath(graph, node, exits, evacuateTime);
            }
        }

        private static void GetShortestPath(Dictionary<int, List<Edge>> graph, int start, HashSet<int> exits, TimeSpan evacuateTime)
        {
            TimeSpan[] distances = new TimeSpan[graph.Count];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = TimeSpan.MaxValue;
            }
            distances[start] = TimeSpan.Zero;

            int[] previous = new int[graph.Count];
            previous[start] = -1;

            OrderedBag<int> queue = new OrderedBag<int>(
                Comparer<int>.Create((a, b) => distances[a].CompareTo(distances[b])));

            queue.Add(start);

            bool isReachable = false;

            while (queue.Count > 0)
            {
                int node = queue.RemoveFirst();

                if (exits.Contains(node))
                {
                    TimeSpan neededTime = distances[node];
                    string saveUnsave = neededTime <= evacuateTime ? "Safe" : "Unsafe";

                    Console.WriteLine($"{saveUnsave} {start} ({neededTime})");

                    isReachable = true;

                    break;
                }

                foreach (Edge edge in graph[node])
                {
                    int childNode = edge.Second;

                    if (distances[childNode] == TimeSpan.MaxValue)
                    {
                        queue.Add(childNode);
                    }

                    TimeSpan newDistance = edge.Time + distances[node];

                    if (newDistance < distances[childNode])
                    {
                        distances[childNode] = newDistance;
                        previous[childNode] = node;

                        queue = new OrderedBag<int>(
                            queue,
                            Comparer<int>.Create((a, b) => distances[a].CompareTo(distances[b])));
                    }
                }
            }

            if (!isReachable)
            {
                Console.WriteLine($"Unreachable {start} (N/A)");
            }
        }

        private static Dictionary<int, List<Edge>> ReadGraph(int nodesCount, int edgesCount)
        {
            Dictionary<int, List<Edge>> graph = new Dictionary<int, List<Edge>>();

            for (int i = 0; i < nodesCount; i++)
            {
                graph[i] = new List<Edge>();
            }

            for (int i = 0; i < edgesCount; i++)
            {
                string[] data = Console.ReadLine().Split();
                int first = int.Parse(data[0]);
                int second = int.Parse(data[1]);
                int minutes = int.Parse(data[2].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                int seconds = int.Parse(data[2].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                TimeSpan time = new TimeSpan(0, minutes, seconds);

                //if (!graph.ContainsKey(first))
                //{
                //    graph[first] = new List<Edge>();
                //}

                //if (!graph.ContainsKey(second))
                //{
                //    graph[second] = new List<Edge>();
                //}

                graph[first].Add(new Edge
                {
                    First = first,
                    Second = second,
                    Time = time,
                });

                graph[second].Add(new Edge
                {
                    First = second,
                    Second = first,
                    Time = time,
                });
            }

            return graph;
        }
    }
}
