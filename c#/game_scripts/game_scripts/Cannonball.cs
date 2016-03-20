using System;

namespace game_scripts {
	class TCannonBall : IStorable {
		protected String _name;
		public TCannonBall(String name, TCapacity capacity, Int32 airResistence, TShipParts damage, TShipParts sharpshooting) {
			this._name = name;
			this.Capacity = capacity;
			this.AirResistence = airResistence;
			this.Damage = damage;
			this.Sharpshooting = sharpshooting;
		}
		public String Name { get { return _name; } }
		public TCapacity Capacity { get; protected set; }
		public Int32 AirResistence { get; protected set; }
		public TShipParts Damage { get; protected set; }
		public TShipParts Sharpshooting { get; protected set; }
	}
}
