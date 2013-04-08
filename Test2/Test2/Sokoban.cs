using System;
using System.IO;
using System.Linq;

namespace Test2 {

	class Sokoban {

		private class Point {

			public int x;
			public int y;

			public Point () { }

			public Point (int x, int y) {
				this.x = x;
				this.y = y;
			}

		}

		private bool[,] walls;

		private Point playerPos;
		private Point boxPos;
		private Point targetPos;

		private int width;
		private int height;

		public void Load (string filename, char separator) {
			string[] lines = File.ReadAllLines(filename);

			height = lines.Length;
			width = lines[0].Split(separator).Length;

			walls = new bool[height, width];

			for (int i = 0; i != height; i++) {
				int[] row = lines[i].Split(separator).Select(s => Convert.ToInt32(s)).ToArray();

				for (int j = 0; j != width; j++) {
					if (row[j] == 3) {
						walls[i, j] = true;
						continue;
					}

					walls[i, j] = false;

					switch (row[j]) {
						case 1 :
							boxPos = new Point(i, j);
							break;
						case 2 :
							targetPos = new Point(i, j);
							break;
						case 4 :
							playerPos = new Point(i, j);
							break;
					}
				}
			}
		}

		public int[] Solve () {
			//TODO: implement logic
			return null;
		}

	}

}
