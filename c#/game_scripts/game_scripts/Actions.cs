using System;

namespace game_scripts {
	abstract class TAction {
		public abstract void Execute(TBaseShipController shipController, params Object[] objects);
	}

	abstract class TBaseShipAction : TAction {
		public override void Execute(TBaseShipController shipController, params Object[] objects) {
			if (objects.Length != 2)
				throw new ArgumentException();
			Execute(shipController, (TShip)objects[0], objects[1]);
		}
		public abstract void Execute(TBaseShipController shipController, TShip ship, Object obj);
	}

	abstract class TBaseCellAction : TAction {
		public override void Execute(TBaseShipController shipController, Object[] objects) {
			if (objects.Length != 1)
				throw new ArgumentException();
			Execute(shipController, (TCell)objects[0]);
		}
		public abstract void Execute(TBaseShipController shipController, TCell cell);
	}

	class TDamageAction : TBaseShipAction {
		private TBaseDamageController _damageController;
		public TDamageAction(TBaseDamageController damageController) {
			this._damageController = damageController;
		}
		public override void Execute(TBaseShipController shipController, TShip ship, object obj) {
			Execute(shipController, ship, (TDirection)obj);
		}
		public void Execute(TBaseShipController shipController, TShip ship, TDirection direction) {
			TParameters damage = _damageController.CalculateDamage(shipController.CurrentShip, ship, direction);
			TShipParts oldHitPoints = ship.Current.Parameters.HitPoints;
			ship.Current.Parameters -= damage;
			ship.Storage.OnDamage(oldHitPoints, ship.Current.Parameters.HitPoints);
		}
	}
	class TGoAction : TBaseCellAction {
		public override void Execute(TBaseShipController shipController, TCell cell) {
			if (!cell.IsAvailableRouteCell)
				throw new ArgumentOutOfRangeException();
			TShip ship = shipController.CurrentShip;
			shipController.SubShip(ship);
			ship.Current.Parameters -= cell.Bonus;
			shipController.AddShip(ship, cell);
		}
	}
	class TWaitAction : TAction {
		public override void Execute(TBaseShipController shipController, params object[] objects) {
			// TODO
			throw new NotImplementedException();
		}
	}
	class TDefenseAction : TAction {
		public override void Execute(TBaseShipController shipController, params object[] objects) {
			// TODO
			throw new NotImplementedException();
		}
	}
}
