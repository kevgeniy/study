using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game_scripts {
	class TShip : IComparable<TShip> {
		public String Name { get; protected set; }
		public String ClassName { get; protected set; }
		public Int32 CreationYear { get; protected set; }
		public TNation CreationNation { get; protected set; }
		public TNation PlayingNation { get; set; }
		public Int32 Level { get; set; }
		public Int32 Experience { get; set; }
		public Int32 RequiredCommandLevel { get; set; }
		public Int32 Width { get; protected set; }
		public Int32 Length { get; protected set; }
		public TStorage Storage { get; protected set; }
		public TBindedParametersController Base { get; protected set; }
		public TBalancingParametersController Current { get; protected set; }
		public TCannon CannonKind { get; set; }
		public TCannonBall CannonBallKind { get; set; }
		public IEnumerable<TCannonBall> CannonBalls() {
			return (IEnumerable<TCannonBall>)Storage.GetObjectsByType<TCannonBall>();
		}
		public IEnumerable<TCannon> Cannons() {
			return (IEnumerable<TCannon>)Storage.GetObjectsByType<TCannon>();
		}
		public Int32 CompareTo(TShip second) {
			if (this.Current.Parameters.Initiative > second.Current.Parameters.Initiative)
				return 1;
			if (this.Current.Parameters.Initiative < second.Current.Parameters.Initiative)
				return -1;
			return 0;
		}
		public override Int32 GetHashCode() {
			return this.ClassName.GetHashCode() 
				^ this.Name.GetHashCode() 
				^ Enum.GetName(typeof(TNation), this.CreationNation).GetHashCode() 
				^ this.CreationYear;
		}
		public override bool Equals(object obj) {
			if (obj is TShip)
				Equals((TShip)obj);
			return false;
		}
		public bool Equals(TShip ship) {
			return this.Name == ship.Name &&
				this.ClassName == ship.ClassName &&
				this.CreationYear == ship.CreationYear &&
				this.CreationNation == ship.CreationNation;
		}
		public static bool operator ==(TShip first, TShip second) {
			return first.Equals(second);
		}
		public static bool operator !=(TShip first, TShip second) {
			return !(first == second);
		}
	}
	public enum ShipClass {
		BattleShip = 1,
		Frigate,
		Carvette,
		Galley,
		Trader,
		Sloop,
		FishingBoat
	}
	public enum TNation {
		England,
		Spain,
		Portugal,
		France,
		Brothers
	}
}
