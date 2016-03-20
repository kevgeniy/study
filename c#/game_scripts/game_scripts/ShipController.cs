using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace game_scripts {
	class TBaseShipController {
		private SortedList<TShip, TCell> _ships;
		private SortedList<TShip, TCell> _nextStepShips;
		private bool IsRoundPlay;
		public TShip CurrentShip { get; protected set; }
		public TMap Map { get; protected set; }
		public TMapController MapController { get; protected set; }
		public TBaseShipController(TAction wait, TAction defense, TAction rotate, TAction damage, TAction go) {
			this._ships = new SortedList<TShip, TCell>();
			this.Wait = wait;
			this.Defense = defense;
			//this.Rotate = rotate;
			this.Damage = damage;
			this.Go = go;
		}
		// obviously can be grouped in one array/List of actions if it will be needed
		public TAction Wait { get; protected set; }
		public TAction Defense { get; protected set; }
		//public TAction Rotate { get; protected set; }
		public TAction Damage { get; protected set; }
		public TAction Go { get; protected set; }
		public void AddShip(TShip ship, TCell cell) {
			if (!IsRoundPlay)
				_ships.Add(ship, cell);
			else
				_nextStepShips.Add(ship, cell);
			Map[cell.X, cell.Y].IsFree = false;
		}
		public void SubShip(TShip ship) {
			int index = 0;
			for (int i = 0; i < _ships.Count; i++)
				if (_ships.Keys[i].Equals(ship)) {
					index = i;
					break;
				}
			Map[_ships.Values[index].X, _ships.Values[index].Y].IsFree = true;
			_ships.RemoveAt(index);
		}
	}
}
