using System;

namespace game_scripts {
	class TCannon : IStorable {
		protected String _name;
		public TCannon(String name, TCapacity capacity, Int32 maxCannonballWeight, TShipParts sharpshooting) {
			this._name = name;
			this.Capacity = capacity;
			this.MaxCannonballWeight = maxCannonballWeight;
			this.Sharpshooting = sharpshooting;
		}
		public String Name { get { return _name; } }
		public TCapacity Capacity { get; protected set; }
		public Int32 MaxCannonballWeight { get; protected set; }
		public TShipParts Sharpshooting { get; protected set; }
	}
}
