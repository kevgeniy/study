using System;
using System.Collections.Generic;
using System.Linq;


namespace clock_3 {
	partial class Program {
		public static void Main() {
			//string a = "abcabcabcabc";
			//string b = "abc*ab";
			string a = "abababab";
			string b = "z*z";

			Regex<Char> regex = new Regex<Char>(b, '*');
			var ans = RegexEnumerable.Where(a.ToArray<char>(), (obj) => regex);
			foreach (var item in ans) {
				foreach (var i in item)
					Console.Write(i);
				Console.WriteLine();
			}
			Console.ReadLine();
		}
	}

	/// <summary>
	/// interface that defines necessary members for object which
	/// want to interact with RegexEnumerable
	/// </summary>
	/// <typeparam name="T">
	/// must implement IComparable of <typeparamref name="T"/> interface
	/// </typeparam>
	public interface IRegex<T> where T : IComparable<T> {
		/// <summary>
		/// indicate if first element of the regex may be any
		/// </summary>
		bool IsFirstAny { get; }

		/// <summary>
		/// indicate if last element of the regex may be any
		/// </summary>
		bool IsLastAny { get; }

		/// <summary>
		/// indicate number of subsequences without "any" elementls in every of them
		/// </summary>
		int CountOfSubsequences { get; }

		/// <summary>
		/// for receiving subsequences without "any" elements
		/// </summary>
		/// <param name="index">
		/// indicate number of the subsequence
		/// </param>
		/// <returns>
		///  return subsequence number <paramref name="index"/> without "any" elements
		/// </returns>
		IList<T> GetSubsequence(int index);
	}

	/// <summary>
	/// one possible implementation of IRegex for work
	/// </summary>
	/// <typeparam name="T">
	/// must implements IComparable of <typeparamref name="T"/> interface
	/// </typeparam>
	public class Regex<T> : IRegex<T> where T : IComparable<T> {
		private bool _isFirstAny = false;
		private bool _isLastAny = false;
		private T[] _anySimbols;
		private T _anySimbol;
		private List<List<T>> _subsequence = new List<List<T>>();

		/// <summary>
		/// indicate number of subsequences without "any" elements in every of them
		/// </summary>
		public int CountOfSubsequences {
			get { return _subsequence.Count; }
		}

		/// <summary>
		/// indicate if first element of the regex may be any
		/// </summary>
		public bool IsFirstAny {
			get { return _isFirstAny; }
		}

		/// <summary>
		/// indicate if last element of the regex may be any
		/// </summary>
		public bool IsLastAny {
			get { return _isLastAny; }
		}

		/// <summary>
		/// constructor, involve method for finding all subsequences without "any" elements
		/// </summary>
		/// <param name="sequence">current sequence of <typeparamref name="T"/> type elements</param>
		/// <param name="anySimbols"> array of "any" elements</param>
		public Regex(IEnumerable<T> sequence, T[] anySimbols) {
			if (sequence == null)
				throw new ArgumentNullException("sequence");
			_anySimbols = anySimbols;
			FindSubsequences(sequence);
		}

		/// <summary>
		/// constructor, involve method for finding all subsequences without "any" elements
		/// overloading for one "any" element for speeding up
		/// </summary>
		/// <param name="sequence">current sequence of <typeparamref name="T"/> type elements</param>
		/// <param name="anySimbol"> one "any" element</param>
		public Regex(IEnumerable<T> sequence, T anySimbol) {
			if (sequence == null)
				throw new ArgumentNullException("sequence");
			_anySimbol = anySimbol;
			FindSubsequences(sequence);
		}

		/// <summary>
		/// for receiving subsequences without "any" simbols
		/// </summary>
		/// <param name="index">
		/// indicate number of the subsequence
		/// </param>
		/// <returns>
		///  return subsequence number <paramref name="index"/> without "any" simbols
		/// </returns>
		public IList<T> GetSubsequence(int index) {
			if (index > CountOfSubsequences - 1)
				throw new IndexOutOfRangeException("index");
			return _subsequence[index];
		}

		/// <summary>
		/// find all subsequences without "any" elements
		/// </summary>
		/// <param name="sequence">initial sequence for searching</param>
		protected virtual void FindSubsequences(IEnumerable<T> sequence) {
			if (IsAny(sequence.ElementAt(0)))
				_isFirstAny = true;
			List<T> subSeq = new List<T>();
			_isLastAny = true;
			using (IEnumerator<T> enumerator = sequence.GetEnumerator()) {
				while (enumerator.MoveNext()) {
					T element = enumerator.Current;
					if (!IsAny(element)) {
						//_isLastAny = false;
						subSeq.Add(element);
					}
					else {
						//_isLastAny = true;
						if (subSeq.Count != 0) {
							_subsequence.Add(subSeq);
							subSeq = new List<T>();
						}
					}
				}
				if (subSeq.Count != 0) {
					_subsequence.Add(subSeq);
					_isLastAny = false;
				}
			}
		}

		private bool IsAny(T simbol) {
			if (simbol == null)
				throw new ArgumentNullException();
			if (_anySimbols == null)
				return _anySimbol == null ? true : _anySimbol.CompareTo(simbol) == 0;
			return _anySimbols.Contains(simbol);
		}
	}

	/// <summary>
	/// simple "pair" structure with == and != overloads than to store
	/// indexes of the first and last subsequences in the sequence
	/// </summary> 
	public struct Pair {
		private int _begin, _end;

		/// <summary>
		/// stores and let to get the index of the first element of first subsequence in the sequence
		/// </summary>
		public int Begin {
			get { return _begin; }
		}

		/// <summary>
		/// stores and let to get the index of the first element of last subsequence in the sequence
		/// </summary>
		public int End {
			get { return _end; }
		}

		/// <summary>
		/// constructor for creating pair with current <paramref name="begin"/> and <paramref name="end"/> value
		/// </summary>
		/// <param name="begin">index of the first element of the first subsequence int the sequence</param>
		/// <param name="end">index of the first element of the last subsequence int the sequence</param>
		public Pair(int begin, int end) {
			_begin = begin;
			_end = end;
		}

		/// <summary>
		/// compare two pairs through their properties
		/// </summary>
		/// <param name="first">first pair</param>
		/// <param name="second">second pair</param>
		/// <returns>
		/// true if they are equal
		/// </returns>
		public static bool operator ==(Pair first, Pair second) {
			return first.Begin == second.Begin && first.End == second.End;
		}

		/// <summary>
		/// compare two pairs through their properties
		/// </summary>
		/// <param name="first">first pair</param>
		/// <param name="second">second pair</param>
		/// <returns>
		/// true if they are equal
		/// </returns>
		public static bool operator !=(Pair first, Pair second) {
			return !(first == second);
		}

		/// <summary>
		/// standart equal method
		/// </summary>
		/// <param name="obj">object for comparison</param>
		/// <returns></returns>
		public override bool Equals(object obj) {
			if (obj is Pair && (Pair) obj == this)
				return true;
			return false;
		}

		/// <summary>
		/// returns hash code of current pair
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}

	/// <summary>
	/// static class with some methods for IRegex objects a la LINQ
	/// </summary>
	public static class RegexEnumerable {
		/// <summary>
		/// where method a la LINQ for IRegex objects, return all subsequences 
		/// that satisfy the IRegex object
		/// </summary>
		/// <typeparam name="T"> must implement IComparable of <typeparamref name="T"/></typeparam>
		/// <param name="sequence">initial sequence</param>
		/// <param name="function">function that must return regex object for any first (object) argument</param>
		/// <returns>
		/// all subsequences a la regex through IEnumerable interface (lazy)
		/// </returns>
		public static IEnumerable<IList<T>> Where<T>(this IList<T> sequence, Func<object, IRegex<T>> function)
			where T : IComparable<T> {
			//get input regex and check of the correctness
			IRegex<T> regex = function(null);
			int count = regex.CountOfSubsequences;
			if (count == 0)
				throw new Exception("Uncorrect regex!");
			if (regex.IsFirstAny)
				throw new Exception("FirstAny!");
			if (regex.IsLastAny)
				throw new Exception("LastAny!");

			//initialization for the algorithm
			int[] positions = new int[count];
			for (int i = 0; i < count; i++) {
				int next = sequence.Find(i == 0 ? 0 : positions[i - 1] + regex.GetSubsequence(i - 1).Count, regex.GetSubsequence(i));
				if (next == -1)
					yield break;
				positions[i] = next;
			}
			// pair with begin and end of the previous answer
			Pair previous = new Pair(positions[0], positions[count - 1]);

			while (true) {
				bool isFind = false;
				for (int i = count - 1; i >= 0; i--) {
					// move first which can
					int next = sequence.Find(positions[i] + 1, regex.GetSubsequence(i));
					if (next < 0)
						continue;
					positions[i] = next;
					isFind = true;
					//try to move others
					for (int j = i + 1; j < count; j++) {
						next = sequence.Find(positions[j - 1] + regex.GetSubsequence(j - 1).Count, regex.GetSubsequence(j));
						if (next == -1) {
							isFind = false;
							break;
						}
						positions[j] = next;
					}
					//if all is good, then return the result
					if (isFind) {
						Pair current = new Pair(positions[0], positions[count - 1]);
						if (current == previous)
							break;
						T[] ans = DoArray(previous, regex, sequence, count);
						//change previous result for future
						previous = current;
						yield return ans;
						break;
					}
				}
				if (!isFind) {
					yield return DoArray(previous, regex, sequence, count);
					;
					yield break;
				}
			}
		}

		private static T[] DoArray<T>(Pair previous, IRegex<T> regex, IList<T> sequence, int count) where T : IComparable<T> {
			int length = previous.End - previous.Begin + regex.GetSubsequence(count - 1).Count;
			T[] ans = new T[length];
			for (int j = 0; j < length; j++)
				ans[j] = sequence[j + previous.Begin];
			return ans;
		}

		/// <summary>
		/// find <paramref name="subSequence"/> in <paramref name="sequence"/> from the <paramref name="index"/> position
		/// </summary>
		/// <typeparam name="T">
		/// must implement IComparable of <typeparamref name="T"/>
		/// </typeparam>
		/// <param name="sequence"> sequence for searching</param>
		/// <param name="index"> begining index</param>
		/// <param name="subSequence"> subsequence for searching</param>
		/// <returns>begining index of the first found <paramref name="subSequence"/> in <paramref name="sequence"/></returns>
		public static int Find<T>(this IList<T> sequence, int index, IList<T> subSequence) where T : IComparable<T> {
			for (int i = index; i <= sequence.Count - subSequence.Count; i++)
				if (sequence[i].CompareTo(subSequence[0]) == 0) {
					bool isAns = true;
					for (int j = 0; j < subSequence.Count; j++)
						if (sequence[i + j].CompareTo(subSequence[j]) != 0) {
							isAns = false;
							break;
						}
					if (isAns)
						return i;
				}
			return -1;
		}
	}
}