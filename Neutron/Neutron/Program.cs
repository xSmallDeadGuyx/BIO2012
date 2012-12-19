using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neutron {
	class Piece {
		public int x;
		public int y;

		public int player;
		public int n;

		public Piece(int x, int y, int p, int n) {
			this.x = x;
			this.y = y;
			this.player = p;
			this.n = n;
		}

		public override bool Equals(object obj) {
			if(obj == null || obj.GetType() != GetType()) return false;
			Piece p2 = (Piece) obj;
			return p2.player == player && p2.n == n;
		}
	}

	class Program {

		static List<Piece> pieces;

		enum Board { empty, neutron, p1, p2 };

		static Piece getPieceAt(int x, int y) {
			foreach(Piece p in pieces)
				if(p.x == x && p.y == y)
					return p;
			return null;
		}

		static Piece getPiece(int pl, int n) {
			foreach(Piece p in pieces)
				if(p.player == pl && p.n == n)
					return p;
			return null;
		}

		static int dirX(int dir) {
			switch(dir) {
				case 2:
				case 3:
				case 4:
					return 1;
				case 6:
				case 7:
				case 8:
					return -1;
			}
			return 0;
		}

		static int dirY(int dir) {
			switch(dir) {
				case 8:
				case 1:
				case 2:
					return -1;
				case 4:
				case 5:
				case 6:
					return 1;
			}
			return 0;
		}

		static List<int> getMovableDirs(Piece p) {
			List<int> dirs = new List<int>();
			for(int dir = 1; dir < 9; dir++) {
				int px = p.x;
				int py = p.y;

				do {
					px += dirX(dir);
					py += dirY(dir);
				}
				while(getPieceAt(px, py) == null && px > -1 && py > -1 && px < 5 && py < 5);

				px -= dirX(dir);
				py -= dirY(dir);
				if(px != p.x || py != p.y)
					dirs.Add(dir);
			}
			return dirs;
		}

		static void printBoard(Board[,] b) {
			Console.WriteLine();
			for(int j = 0; j < 5; j++) {
				string s = "";
				for(int i = 0; i < 5; i++) {
					switch(b[i, j]) {
						case Board.empty:
							s += ".";
							break;
						case Board.neutron:
							s += "*";
							break;
						case Board.p1:
							s += "F";
							break;
						case Board.p2:
							s += "S";
							break;
					}
				}
				Console.WriteLine(s);
			}
		}

		static void Main(string[] args) {
			Board[,] board = {
				{Board.p2, Board.empty, Board.empty, Board.empty, Board.p1},
				{Board.p2, Board.empty, Board.empty, Board.empty, Board.p1},
				{Board.p2, Board.empty, Board.neutron, Board.empty, Board.p1},
				{Board.p2, Board.empty, Board.empty, Board.empty, Board.p1},
				{Board.p2, Board.empty, Board.empty, Board.empty, Board.p1}
			};
			pieces = new List<Piece>();
			for(int x = 0; x < 5; x++) {
				pieces.Add(new Piece(x, 0, 2, x + 1));
				pieces.Add(new Piece(x, 4, 1, x + 1));
			}
			int[] p1Turn = new int[5];
			int[] p2Turn = new int[5];
			string s = Console.ReadLine();
			string[] ss = s.Split(' ');
			string s2 = Console.ReadLine();
			string[] ss2 = s.Split(' ');
			for(int i = 0; i < 5; i++) {
				p1Turn[i] = Int32.Parse(ss[i]);
				p2Turn[i] = Int32.Parse(ss2[i]);
			}

			int turn = 0;
			bool won = false;
			while(!won) {
				int n = turn % 2 == 0 ? p1Turn[(turn / 2) % 5] : p2Turn[(turn / 2) % 5];
				Piece p = getPiece(turn % 2 + 1, n);
				int nx = -1;
				int ny = -1;
				for(int x = 0; x < 5; x++) for(int y = 0; y < 5; y++)
					if(board[x, y] == Board.neutron) {
						nx = x;
						ny = y;
					}

				List<int> losingDirs = new List<int>();
				List<int> losingX = new List<int>();
				List<int> losingY = new List<int>();
				List<int> movableDirs = new List<int>();
				List<int> movableX = new List<int>();
				List<int> movableY = new List<int>();

				bool moved = false;
				for(int dir = 1; dir < 9; dir++) {
					int xx = nx;
					int yy = ny;
					do {
						xx += dirX(dir);
						yy += dirY(dir);
					}
					while(getPieceAt(xx, yy) == null && xx > -1 && yy > -1 && xx < 5 && yy < 5);
					xx -= dirX(dir);
					yy -= dirY(dir);
					if(xx != nx || yy != ny) {
						if(yy == 0) {
							if(turn % 2 == 1) {
								board[nx, ny] = Board.empty;
								board[xx, yy] = Board.neutron;
								nx = xx;
								ny = yy;
								won = true;
								break;
							}
							else {
								losingDirs.Add(dir);
								losingX.Add(xx);
								losingY.Add(yy);
								continue;
							}
						}
						else if(yy == 4) {
							if(turn % 2 == 0) {
								board[nx, ny] = Board.empty;
								board[xx, yy] = Board.neutron;
								nx = xx;
								ny = yy;
								won = true;
								break;
							}
							else {
								losingDirs.Add(dir);
								losingX.Add(xx);
								losingY.Add(yy);
								continue;
							}
						}
						else {
							List<int> dirs = getMovableDirs(p);
							if(dirs.Count > 0) {
								movableDirs.Add(dir);
								movableX.Add(xx);
								movableY.Add(yy);
							}
						}
					}
				}
				if(!won && !moved) {
					if(movableDirs.Count > 0) {
						int xx = movableX[0];
						int yy = movableY[0];
						board[nx, ny] = Board.empty;
						board[xx, yy] = Board.neutron;
						nx = xx;
						ny = yy;
					}
					else if(losingDirs.Count > 0) {
						int xx = losingX[0];
						int yy = losingY[0];
						board[nx, ny] = Board.empty;
						board[xx, yy] = Board.neutron;
						nx = xx;
						ny = yy;
						won = true;
					}
				}
				if(won) break;
				int px = p.x;
				int py = p.y;
				int d = getMovableDirs(p)[0];
				do {
					px += dirX(d);
					py += dirY(d);
				}
				while(getPieceAt(px, py) == null && px > -1 && py > -1 && px < 5 && py < 5);
				px -= dirX(d);
				py -= dirY(d);
				board[p.x, p.y] = Board.empty;
				board[px, py] = turn % 2 == 0 ? Board.p1 : Board.p2;
				p.x = px;
				p.y = py;
				if(turn < 2)
					printBoard(board);
				turn++;
			}
			printBoard(board);
			Console.ReadLine();
		}
	}
}
