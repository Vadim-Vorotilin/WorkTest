using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2 {
	
	class Program {

		private const string InFilename = "in.txt";
		private const string OutFilename = "out.txt";

		private const char Separator = ' ';

		static void Main () {

			Sokoban sokoban = new Sokoban();
			sokoban.Load(InFilename, Separator);

		}

	}

}
