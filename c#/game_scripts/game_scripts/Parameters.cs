using System;

namespace game_scripts {
	struct TParameters {
		public Int32 Observation { get; set; }
		public Int32 Subtlety { get; set; }
		public TShipParts Armour { get; set; }
		public TShipParts HitPoints { get; set; }
		public TShipParts NumberOfGuns { get; set; }
		public Int32 Sharpshooting { get; set; }
		public Int32 Speed { get; set; }
		public Int32 Draft { get; set; }
		public Int32 Weight { get; set; }
		public Int32 Luck { get; set; }
		public Int32 Moral { get; set; }
		public Int32 Initiative { get; set; }
		public static TParameters operator -(TParameters first, TParameters second) {
			TParameters result = new TParameters();
			result.Observation = first.Observation - second.Observation;
			result.Subtlety = first.Subtlety - second.Subtlety;
			result.Armour = first.Armour - second.Armour;
			result.HitPoints = first.HitPoints - second.HitPoints;
			result.NumberOfGuns = first.NumberOfGuns - second.NumberOfGuns;
			result.Sharpshooting = first.Sharpshooting - second.Sharpshooting;
			result.Speed = first.Speed - second.Speed;
			result.Draft = first.Draft - second.Draft;
			result.Weight = first.Weight - second.Weight;
			result.Luck = first.Luck - second.Luck;
			result.Moral = first.Moral - second.Moral;
			result.Initiative = first.Initiative - second.Initiative;
			return result;
		}
	}
	struct TShipParts {
		public Int32 HullLeft { get; set; }
		public Int32 HullRight { get; set; }
		public Int32 HullTail { get; set; }
		public Int32 HullHead { get; set; }
		public Int32 Deck { get; set; }
		public Int32 Mast { get; set; }
		public static TShipParts operator +(TShipParts first, TShipParts second) {
			TShipParts result = new TShipParts();
			result.Deck = first.Deck + second.Deck;
			result.HullHead = first.Deck + second.Deck;
			result.HullLeft = first.HullLeft + second.HullLeft;
			result.HullRight = first.HullRight + second.HullRight;
			result.HullTail = first.HullTail + second.HullTail;
			result.Mast = first.Mast + second.Mast;
			return result;
		}
		public static TShipParts operator -(TShipParts first, TShipParts second) {
			second.Deck *= -1;
			second.HullHead *= -1;
			second.HullLeft *= -1;
			second.HullRight *= -1;
			second.HullTail *= -1;
			second.Mast *= -1;
			return first + second;
		}
		public static Boolean operator ==(TShipParts first, TShipParts second) {
			return first.Equals(second);	
		}
		public static Boolean operator !=(TShipParts first, TShipParts second) {
			return !(first == second);
		}
		public static Double operator /(TShipParts first, TShipParts second) {
			return Sum(first) / Sum(second);
		}
		private static Int32 Sum(TShipParts first) {
			return first.Deck + first.HullHead + first.HullLeft + first.HullRight + first.HullTail + first.Mast;
		}
		public override bool Equals(object obj) {
			if(obj is TShipParts)
				return Equals((TShipParts)obj);
			return false;
		}
		public bool Equals(TShipParts second) {
			return Deck == second.Deck &&
				HullHead == second.HullHead &&
				HullLeft == second.HullLeft &&
				HullRight == second.HullRight &&
				HullTail == second.HullTail &&
				Mast == second.Mast;
		}
		public override int GetHashCode() {
			return this.HullHead ^
				this.HullLeft ^
				this.HullRight ^
				this.HullTail ^
				this.Mast ^
				this.Deck;
		}
	}
}
