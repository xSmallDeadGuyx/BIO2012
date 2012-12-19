using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchingTheClock {
	class Clock {
		public int mins = 0;
		public int hours = 0;

		public override bool Equals(object obj) {
			if(obj == null || obj.GetType() != GetType()) return false;
			return ((Clock) obj).mins == mins && ((Clock) obj).hours == hours;
		}
	}

	class Program {
		static void Main(string[] args) {
			string input = Console.ReadLine();
			int n1 = Int32.Parse(input.Split(' ')[0]);
			int n2 = Int32.Parse(input.Split(' ')[1]);


			Clock c1 = new Clock();
			Clock c2 = new Clock();

			int n = 0;
			do {
				n++;
				c1.hours++;
				c2.hours++;
				c1.mins += n1;
				c2.mins += n2;
				c1.hours += c1.mins / 60;
				c2.hours += c2.mins / 60;
				c1.mins %= 60;
				c2.mins %= 60;
				c1.hours %= 24;
				c2.hours %= 24;
			}
			while(!c1.Equals(c2));

			Console.WriteLine((c2.hours > 10 ? "" + c2.hours : "0" + c2.hours) + ":" + (c2.mins > 10 ? "" + c2.mins : "0" + c2.mins));
			Console.WriteLine(n);

			Console.ReadLine();
		}
	}
}
