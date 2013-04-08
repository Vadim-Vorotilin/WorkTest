using System;
using System.IO;

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

			/// <summary>
			/// Calculates moved point.
			/// </summary>
			/// <param name="direction">1 - left, 2 - up, 3 - right, 4 - down</param>
			/// <returns>Moved point.</returns>
			public Point Move (int direction) {
				switch (direction) {
					case 1 : 
						return new Point(x - 1, y);
					case 2 :
						return new Point(x, y - 1);
					case 3 : 
						return new Point(x + 1, y);
					case 4 : 
						return new Point(x, y + 1);
				}

				throw new Exception("Illegal direction!");
			}

			public override string ToString () {
				return string.Format("Point(x = {0}, y = {1})", x, y);
			}

		}

		private bool[,] walls;

		private Point playerPos;
		private Point boxPos;
		private Point targetPos;

		private int width;
		private int height;

		public void Load (string filename) {
			string[] lines = File.ReadAllLines(filename);

			height = lines.Length;
			width = lines[0].Length;

			walls = new bool[height, width];

			for (int i = 0; i != height; i++) {
				for (int j = 0; j != width; j++) {
					if (lines[i][j] == '3') {
						walls[i, j] = true;
						continue;
					}

					walls[i, j] = false;

					switch (lines[i][j]) {
						case '1' :
							boxPos = new Point(i, j);
							break;
						case '2' :
							targetPos = new Point(i, j);
							break;
						case '4' :
							playerPos = new Point(i, j);
							break;
					}
				}
			}

			Console.WriteLine("Width: {0}, Height: {1}\n" +
			                  "Target: {2}, Box: {3}, Player: {4}",
			                  width,
			                  height,
			                  targetPos,
			                  boxPos,
			                  playerPos);
		}
		
		private bool IsPointLegal (Point point) {
			return point.x >= 0 && point.y >= 0 && point.x < width && point.y < height;
		}

		public int[] Solve () {
			//TODO: implement logic
			return null;
		}

	}

}
