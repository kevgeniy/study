using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensibility;

namespace Geomety {
    public class Pythagoras : ICalculation {
		public int Calculate(int a, int b) { 
			return (int)Math.Sqrt(a * a + b * b);
		}
    }
}
