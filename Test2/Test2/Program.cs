using System;
using System.Linq;

namespace Test2 {
	
	class Program {

		private const string InFilename = "in.txt";

		static void Main () {

			Sokoban sokoban = new Sokoban();
			sokoban.Load(InFilename);

			DateTime sTime = DateTime.Now;

			int[] result = sokoban.Solve();

			DateTime eTime = DateTime.Now;

			Console.WriteLine(result.Aggregate("", (s, i) => s + i + ", "));
			Console.WriteLine("Time spent: {0} ms", (eTime - sTime).TotalMilliseconds);

			Console.ReadKey();

		}

	}

}
