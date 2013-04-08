using System;
using System.Collections.Generic;
using System.IO;

namespace Test2 {

	class Sokoban {

		private class Point {

			public readonly int x;
			public readonly int y;

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
			public Point Moved (int direction) {
				switch (direction) {
					case 1 : 
						return new Point(x, y - 1);
					case 2 :
						return new Point(x - 1, y);
					case 3 : 
						return new Point(x, y + 1);
					case 4 : 
						return new Point(x + 1, y);
				}

				throw new Exception("Illegal direction!");
			}

			public override string ToString () {
				return string.Format("Point(x = {0}, y = {1})", x, y);
			}

			public static bool operator == (Point p1, Point p2) {
				if ((object) p1 == null || (object) p2 == null)
					return false;

				return p1.x == p2.x && p1.y == p2.y;
			}

			public static bool operator != (Point p1, Point p2) {
				return !(p1 == p2);
			}
		}

		private class State {

			public Point playerPos;
			public Point boxPos;
			public Point targetPos;

			public readonly List<int> path;

			public State () {
				path = new List<int>();
			}

			public State (State state, int moveDir) {
				path = new List<int>(state.path) { moveDir };

				targetPos = state.targetPos;
				boxPos = state.boxPos;

				playerPos = state.playerPos.Moved(moveDir);

				if (boxPos == playerPos)
					boxPos = boxPos.Moved(moveDir);
			}

			public bool IsLegal {
				get {
					return IsPointLegal(playerPos) &&
					       IsPointLegal(boxPos) &&
					       !walls[playerPos.x, playerPos.y] &&
					       !walls[boxPos.x, boxPos.y];
				}
			}

			public bool IsSuccess {
				get { return boxPos == targetPos; }
			}

		}

		private static bool[,] walls;

		private State initState;

		private static int width;
		private static int height;

		public void Load (string filename) {
			initState = new State();

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
							initState.boxPos = new Point(i, j);
							break;
						case '2' :
							initState.targetPos = new Point(i, j);
							break;
						case '4' :
							initState.playerPos = new Point(i, j);
							break;
					}
				}
			}
		}
		
		private static bool IsPointLegal (Point point) {
			return point.x >= 0 && point.y >= 0 && point.x < width && point.y < height;
		}

		public int[] Solve () {
			List<State> states = new List<State> { initState };

			while (true) {
				List<State> newStates = new List<State>();

				foreach (State state in states) {
					for (int i = 1; i != 5; i++) {
						State newState = new State(state, i);

						if (newState.IsSuccess)
							return newState.path.ToArray();

						if (!newState.IsLegal)
							continue;

						newStates.Add(newState);
					}
				}

				states = newStates;
			}
		}
		
	}

}
