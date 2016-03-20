using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clock_2
{
    class Program
    {
        static readonly double eps = 0.000001;

        /// <summary>
        /// simple pair structure for points representation 
        /// </summary>
        public struct point
        {
            public point (double x, double y)
                :this()
            {
                X = x;
                Y = y;
            }
            public double X { get; private set; }
            public double Y { get; private set; }
        }
        public static void Main ()
        {
            var reader = new StreamReader("input.txt");
            var points = new List<point>();
            while(!reader.EndOfStream)
                points.Add(GetPoint(reader.ReadLine(), ' ', ',', ';'));

            bool isExit = false;
            while (!isExit)
            {
                Console.WriteLine("Please, enter your coordinates in one line:");
                point currentPoint;
                if (TryGetPoint(Console.ReadLine(), out currentPoint, ' ', ',', ';'))
                {
                    int count = 0;
                    for (int i = 0; i < points.Count; i++)
                    {
                        count += IntersectThisNext(points, i, currentPoint);
                    }
                    if (count % 2 == 0)
                        Console.WriteLine("Doesn't belong");
                    else
                        Console.WriteLine("Belong");
                }
                else
                {
                    Console.WriteLine("Invalid point!");
                }

                isExit = IsExit("Do you want to test yet one point?", "Y", "N");
            }
        }

        /// <summary>
        /// answer if you want to continue and wait for correct answer
        /// </summary>
        /// <param name="message"> output message</param>
        /// <param name="positiveAnswer"> your "Yes" answer</param>
        /// <param name="negativeAnswer"> your "No" answer</param>
        /// <returns>true if your answer is negative ("No")</returns>
        public static bool IsExit(string message, string positiveAnswer, string negativeAnswer)
        {
            while (true)
            {
                Console.WriteLine(message + "(" + positiveAnswer + "/" + negativeAnswer + ")");
                string line = Console.ReadLine();
                if (line == positiveAnswer)
                    return false;
                if (line == negativeAnswer)
                    return true;
            }
        }

        /// <summary>
        /// try to get point from string
        /// </summary>
        /// <param name="str">string for splitting</param>
        /// <param name="splitSimbols"> split simbols for splitting</param>
        /// <returns> new point if all is good or throw InvalidCastException</returns>
        public static point GetPoint(string str, params char[] splitSimbols)
        {
            string[] line = str.Split(splitSimbols);
            double x,y;
            if (line.Length != 2 || !(double.TryParse(line[0], out x) && double.TryParse(line[1], out y)))
                throw new InvalidCastException();
            return new point(x, y);
        }

        /// <summary>
        /// try to get point from string
        /// </summary>
        /// <param name="str">string for splitting</param>
        /// <param name="result">point from string, default(point) if all is bad</param>
        /// <param name="splitSimbols">simbols for splitting</param>
        /// <returns>true if all is good and false if all is bad and exception was caught during conversion</returns>
        public static bool TryGetPoint(string str, out point result, params char[] splitSimbols)
        {
            try
            {
                point a = GetPoint(str, splitSimbols);
                result = a;
                return true;
            }
            catch(InvalidCastException e)
            {
                result = default(point);
                return false;
            }
        }

        /// <summary>
        /// check if current point's line intersect current arc from this point to next point
        /// </summary>
        /// <param name="points">array of points of our shape</param>
        /// <param name="index">index of current point in points</param>
        /// <param name="pt"> point for checking</param>
        /// <returns>0 if 0 or 2 points of intersecting and 1 otherwise</returns>
        public static int IntersectThisNext(List<point> points, int index, point pt)
        {
            point currentPoint = points[index];
            point previousPoint = index == 0 ? points[points.Count - 1] : points[index - 1];
            point nextPoint = index == points.Count - 1 ? points[0] : points[index + 1];
            if (IsEqual(currentPoint.X, nextPoint.X))
                return 0;
            if (IsEqual(currentPoint.X, pt.X) && currentPoint.Y > pt.Y && (previousPoint.X < pt.X && nextPoint.X > pt.X || previousPoint.X > pt.X && nextPoint.X < pt.X))
                return 1;
            if ((currentPoint.X < pt.X && pt.X < nextPoint.X || currentPoint.X > pt.X && pt.X > nextPoint.X) && (nextPoint.Y - currentPoint.Y) * (pt.X - currentPoint.X) / (nextPoint.X - currentPoint.X) > (pt.Y - currentPoint.Y))
                return 1;
            return 0;
        }

        /// <summary>
        /// check if two double is equal (with epsilon)
        /// </summary>
        /// <param name="a">first double</param>
        /// <param name="b">second double</param>
        /// <returns>true if they are equal (with epsilon)</returns>
        public static bool IsEqual(double a, double b)
        {
            if (Math.Abs(a - b) < eps)
                return true;
            return false;
        }
    }   
}