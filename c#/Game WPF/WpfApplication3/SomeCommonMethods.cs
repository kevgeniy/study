using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication3
{
    static class SCM
    {     
        //TextBox's Treating
        public static void TBTreating(TextBox currentTextBox, int MaxValue)
        {
            int x;
            if (System.Int32.TryParse(currentTextBox.Text, out x) && x > 0)
            {
                while (x > MaxValue)
                    x /= 10;
                if (x == 0)
                    x++;
                currentTextBox.Text = x.ToString();
            }
            else
            {
                if (!(System.Int32.TryParse(currentTextBox.Text, out x) && x > 0))
                    currentTextBox.Clear();
            }
        }

        public static void TBTreating(TextBox currentTextBox, TextBox MaxValueTextBox)
        {
            if (MaxValueTextBox.Text != "")
                TBTreating(currentTextBox, Convert.ToInt32(MaxValueTextBox.Text));
            else
                currentTextBox.Clear();
        }

        public static int X(TextBox thisX)
        {
            return Int32(thisX) - 1;
        }

        public static int Y(TextBox thisY)
        {
            return Int32(thisY) - 1;
        }

        public static int Int32(TextBox thisTextBox)
        {
            return Convert.ToInt32(thisTextBox.Text);
        }

        public static int InvX(int x)
        {
            return x + 1;
        }

        public static int InvY(int y)
        {
            return y + 1;
        }

    }
}
