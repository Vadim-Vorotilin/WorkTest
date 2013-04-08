using System;

namespace Test2 {
	
	class Program {

		private const string InFilename = "in.txt";

		static void Main () {

			Sokoban sokoban = new Sokoban();
			sokoban.Load(InFilename);

			Console.ReadKey();

		}

	}

}
