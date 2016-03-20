namespace WpfApplication3
{
    public struct cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Price { get; set; }

        public static bool operator ==(cell a, cell b)
        {
            return (a.X == b.X && a.Y == b.Y && a.Price == b.Price);
        }

        public static bool operator !=(cell a, cell b)
        {
            return !(a == b);
        }
        public cell(int x = -1, int y = -1, int price = 0)
            : this()
        {
            X = x;
            Y = y;
            Price = price;
        }

        public override bool Equals(object obj)
        {
            if (obj is cell)
                return true;
            return false;
        }
    }

    public struct pair
    {
        public int Key { get; set; }
        public int Value { get; set; }

        public pair(int a = -1, int b = -1)
            : this()
        {
            Key = a;
            Value = b;
        }

        public static bool operator <(pair a, pair b)
        {
            return (a.Key < b.Key || a.Key == b.Key && a.Value < b.Value);
        }

        public static bool operator >(pair a, pair b)
        {
            return (b < a);
        }

        public static pair plus(pair a, int b, int life)
        {
            if (a.Value + b > life)
                return (new pair(a.Key + 1, b));
            else
                if (a.Value + b == life)
                {
                    a.Key++;
                    a.Value = 0;
                    return (a);
                }
                else
                {
                    a.Value += b;
                    return (a);
                }
        }
    }
}
