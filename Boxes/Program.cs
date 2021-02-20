using System;
using System.Collections.Generic;
using System.Linq;

namespace Boxes
{
    class Program
    {
        private class Box 
        {
            public int Width { get; set; }

            public int Depth { get; set; }

            public int Height { get; set; }

            public override string ToString()
            {
                return $"{this.Width} {this.Depth} {this.Height}";
            }
        }

        static void Main(string[] args)
        {
            int boxesCount = int.Parse(Console.ReadLine());
            List<Box> boxes = ReadBoxes(boxesCount);

            GetLIS(boxes);
        }

        private static void GetLIS(List<Box> boxes)
        {
            int[] lengths = new int[boxes.Count];
            int[] previous = new int[boxes.Count];
            previous[0] = -1;

            int lastIdx = -1;
            int maxLength = 0;

            for (int i = 0; i < boxes.Count; i++)
            {
                Box current = boxes[i];
                int bestLegth = 1;
                int prevIdx = -1;

                for (int j = i - 1; j >= 0; j--)
                {
                    Box prev = boxes[j];

                    if (prev.Width < current.Width &&
                        prev.Depth < current.Depth &&
                        prev.Height < current.Height &&
                        bestLegth <= lengths[j] + 1)
                    {
                        bestLegth = lengths[j] + 1;
                        prevIdx = j;
                    }
                }

                lengths[i] = bestLegth;
                previous[i] = prevIdx;

                if (bestLegth > maxLength)
                {
                    maxLength = bestLegth;
                    lastIdx = i;
                }
            }

            Stack<Box> lis = new Stack<Box>();

            while (lastIdx != -1)
            {
                lis.Push(boxes[lastIdx]);
                lastIdx = previous[lastIdx];
            }

            foreach (Box box in lis)
            {
                Console.WriteLine(box.ToString());
            }
        }

        private static List<Box> ReadBoxes(int boxesCount)
        {
            List<Box> boxes = new List<Box>();

            for (int i = 0; i < boxesCount; i++)
            {
                int[] data = Console.ReadLine().Split().Select(int.Parse).ToArray();
                int width = data[0];
                int depth = data[1];
                int height = data[2];

                boxes.Add(new Box
                {
                    Width = width,
                    Depth = depth,
                    Height = height,
                });
            }

            return boxes;
        }
    }
}
