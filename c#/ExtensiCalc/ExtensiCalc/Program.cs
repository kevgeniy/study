using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Extensibility;
using System.Reflection;
using System.Reflection.Emit;

namespace ExtensiCalc {
	class Addition : ICalculation {
		public int Calculate(int a, int b) {
			return a + b;
		}
	}
	class Substruction : ICalculation {
		public int Calculate(int a, int b) {
			return a - b;
		}
	}

	class Multiplication : ICalculation {
		public int Calculate(int a, int b) {
			return a * b;
		}
	}

	class Division : ICalculation {
		public int Calculate(int a, int b) {
			if (b == 0)
				throw new DivideByZeroException();
			return a / b;
		}
	}
	class Program {
		static void Main(string[] args) {
			AssemblyBuilder asm = AppDomain.CurrentDomain.DefineDynamicAssembly(
				new AssemblyName("LCGDemo"),
				AssemblyBuilderAccess.Run);
			ModuleBuilder mod = asm.DefineDynamicModule("MyModule");
			TypeBuilder tpe = mod.DefineType("Sample");
			MethodBuilder mtd = tpe.DefineMethod("SayHello", MethodAttributes.Public | MethodAttributes.Static);
			ILGenerator gen = mtd.GetILGenerator();
			gen.Emit(OpCodes.Ldstr, "Hello LCG");
			gen.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }, null));
			gen.Emit(OpCodes.Ret);

			Type res = tpe.CreateType();
			res.GetMethod("SayHello").Invoke(null, null);
			}

		public static void Display(string salutation) {
			Console.WriteLine(salutation);
			var list = GetOperations().ToList();

			while (true) {
				Console.WriteLine("You can choose these operations:");

				int i = 0;
				 while (i++ < list.Count) {
					Console.WriteLine(i + "." + list[i - 1].GetType().Name);
				}

				int operation = ReadInt("Please, enter number of the operation or 'q' to exit: ", 1, list.Count, "q");

				if (operation == -1)
					break;

				int a = ReadInt("a: ");
				int b = ReadInt("b: ");
				Console.WriteLine(list[operation - 1].Calculate(a, b));
				Console.WriteLine();
				Console.WriteLine();
			}
		}
		public static IEnumerable<ICalculation> GetOperations() {
			return GetBuiltinOperations().Concat(GetExtensions("Ext"));
		}
		public static IEnumerable<ICalculation> GetBuiltinOperations() {
			yield return new Addition();
			yield return new Substruction();
			yield return new Multiplication();
			yield return new Division();
		}
		public static IEnumerable<ICalculation> GetExtensions(string path) {
			if (!Directory.Exists(path)) {
				yield break;
			}
			Type iCalc = typeof(ICalculation);

			foreach (string file in Directory.GetFiles(path, "*.dll")) {
				Assembly asm = Assembly.LoadFrom(file);
				IEnumerable<Type> extTypes = from type in asm.GetTypes()
							   where type.GetInterfaces().Contains(iCalc)
							   select type;
				foreach (Type type in extTypes) {
					yield return (ICalculation)Activator.CreateInstance(type);
				}
			}
		}
		public static int ReadInt(string prompt, int lowerBound, int upperBound, string exitStr) {
			int ans = 0;
			bool valid = false;
			while (!valid) {
				Console.Write(prompt);
				string str = Console.ReadLine();
				if (exitStr != null && str == exitStr)
					return -1;
				valid = int.TryParse(str, out ans);
				if (lowerBound < upperBound && (ans < lowerBound || ans > upperBound))
					valid = false;
				if (!valid)
					Console.WriteLine("\nUncorrect!");
			}
			return ans;
		}
		public static int ReadInt(string prompt) {
			return ReadInt(prompt, 1, 0, null);
		}
	}
}
