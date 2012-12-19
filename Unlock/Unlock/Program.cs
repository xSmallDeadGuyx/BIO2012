using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unlock {
	class Program {
		static char[,] keypad = {
			{'A', 'B', 'C', 'D', 'E'},
			{'F', 'G', 'H', 'I', 'J'},
			{'K', 'L', 'M', 'N', 'O'},
			{'P', 'Q', 'R', 'S', 'T'},
			{'U', 'V', 'W', 'X', 'Y'}
		};

		static List<char> getAffectedLetters(char letter) {
			letter = char.ToUpper(letter);
			int x = 0;
			int y = 0;
			for(int i = 0; i < 5; i++)
				for(int j = 0; j < 5; j++)
					if(keypad[i, j] == letter) {
						x = i;
						y = j;
					}
			List<char> chars = new List<char>();
			for(int i = 0; i < 5; i++)
				for(int j = 0; j < 5; j++)
					if((Math.Abs(i - x) < 2 && j - y == 0) || (Math.Abs(j - y) < 2 && i - x == 0))
						chars.Add(keypad[i, j]);
			return chars;
		}

		static void Main(string[] args) {
			string s = Console.ReadLine();
			int[] chars = solve(s, new int[25]);
			if(chars != null)
				Console.WriteLine(toPressString(chars));
			else
				Console.WriteLine("IMPOSSIBLE");
			Console.ReadLine();
		}

		static string sort(string s) {
			char[] chars = s.ToCharArray();
			for(int i = 1; i < s.Length - 1; i++)
				for(int j = 0; j < s.Length - i; j++)
					if(char.ToLower(chars[i]) > char.ToLower(chars[i + 1])) {
						char t = chars[i];
						chars[i] = chars[i + 1];
						chars[i + 1] = t;
					}
			string sorted = "";
			for(int i = 0; i < chars.Length; i++)
				sorted += chars[i];
			return sorted;
		}

		static string toPressString(int[] presses) {
			string str = "";
			for(int i = 0; i < presses.Length; i++) {
				char c = (char) ((int) 'A' + i);
				if(presses[i] == 1)
					str += char.ToLower(c);
				else if(presses[i] == 2)
					str += char.ToUpper(c);
			}
			return str;
		}

		static int[] solve(string l, int[] ps) {
			for(char i = 'A'; i < 'Z'; i++) {
				int[] presses = (int[]) ps.Clone();
				if(presses[(int) (i - 'A')] > 1) continue;
				presses[(int) (i - 'A')]++;
				string s = "";
				for(int n = 0; n < presses.Length; n++)
					for(int x = 0; x < presses[n]; x++)
						foreach(char c in getAffectedLetters((char) ((int) 'A' + n))) {
							if(s.Contains(char.ToLower(c)))
								s = s.Replace(char.ToLower(c), char.ToUpper(c));
							else if(s.Contains(char.ToUpper(c)))
								s = s.Remove(s.IndexOf(char.ToUpper(c)));
							else
								s += char.ToLower(c);
						}
				s = sort(s);

				if(s == l) {
					return presses;
				}
				int[] p = solve(l, presses);
				if(p != null)
					return p;
			}
			return null;
		}
	}
}
