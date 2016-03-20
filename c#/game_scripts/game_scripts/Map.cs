using System;
using System.Collections.Generic;

namespace game_scripts {
	class TCell {
		public Int32 X { get; set; }
		public Int32 Y { get; set; }
		public Int32 Depth { get; set; }
		public TParameters Bonus { get; set; }
		public TWeather Weather { get; set; }
		public Boolean IsAvailableRouteCell { get; set; }
		public Boolean IsFree { get; set; }
	}
	class TMap {
		private TCell[,] Cells;
		public TMap(Int32 width, Int32 height) {
			this.Width = width;
			this.Height = height;
			this.Cells = new TCell[width, height];
		}
		public Int32 Width { get; protected set; }
		public Int32 Height { get; protected set; }
		public TCell this[Int32 x, Int32 y] {
			get {
				if (x >= 0 && x < Width && y >= 0 && y < Height)
					return Cells[x, y];
				else
					throw new IndexOutOfRangeException();
			}
			set {
				if (x >= 0 && x < Width && y >= 0 && y < Height)
					Cells[x, y] = value;
				else
					throw new IndexOutOfRangeException();
			}
		}
		public IEnumerable<TCell> GetNeighbours(TCell current) {
			return GetNeighbours(current.X, current.Y);
		}
		public IEnumerable<TCell> GetNeighbours(Int32 x, Int32 y) {
			if (x - 1 >= 0) {
				yield return this[x - 1, y];
				if (y + 1 < this.Height)
					yield return this[x - 1, y + 1];
			}
			if (x + 1 < this.Width) {
				yield return this[x + 1, y];
				if (y + 1 < this.Height)
					yield return this[x + 1, y + 1];
			}
			if (y - 1 >= 0)
				yield return this[x, y - 1];
			if (y + 1 < this.Height)
				yield return this[x, y + 1];
		}
	}
}