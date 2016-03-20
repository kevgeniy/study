using System;

namespace game_scripts {
	class TMapController {
		private TMap _map;
		private Double[,] _residualLength;
		// TODO calibrate
		private static Double roundTime = 7;
		private static Double cellHeight = 5;
		public TMapController(TMap map) {
			this._map = map;
			this._residualLength = new Double[map.Width, map.Height];
		}
		public void CalculateAvailableArea(TShip ship, TCell cell) {
			for (int i = 0; i < _residualLength.GetLength(0); i++)
				for (int j = 0; j < _residualLength.GetLength(1); j++)
					_residualLength[i, j] = 0;
			_residualLength[cell.X, cell.Y] = ship.Current.Parameters.Speed * roundTime;
			Int32 x = cell.X;
			Int32 y = cell.Y;
			while (_residualLength[x, y] != 0) {
				_map[x, y].IsAvailableRouteCell = true;
				var enumerator = _map.GetNeighbours(x, y).GetEnumerator();
				while (enumerator.MoveNext()) {
					Int32 curX = enumerator.Current.X;
					Int32 curY = enumerator.Current.Y;
					if (_map[curX, curY].IsFree)
						_residualLength[curX, curY] = Math.Max(_residualLength[curX, curY], _residualLength[x, y] - cellHeight / (1 - 0.5 * (_map[x, y].Bonus.Speed + _map[curX, curY].Bonus.Speed) / ship.Current.Parameters.Speed));
				}
				
				for(int i = 0; i < _residualLength.GetLength(0); i++)
					for (int j = 0; j < _residualLength.GetLength(1); j++)
						if (_map[i, j].IsFree && !_map[i, j].IsAvailableRouteCell && (x < 0 || _residualLength[i, j] > _residualLength[x, y])) {
							x = i;
							y = j;
						}	
			}
		}
	}
}