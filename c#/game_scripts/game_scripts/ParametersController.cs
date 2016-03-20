using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game_scripts {
	class TParametersController {
		protected TParameters _parameters;
		public TParameters Parameters {
			get {
				return _parameters;
			}
			set {
				SetParameters(value);
			}
		}
		/// <summary>
		/// Is virtual needed?
		/// </summary>
		/// <param name="parameters"></param>
		protected virtual void SetParameters(TParameters parameters) {
			AddArmour(parameters.Armour - this.Parameters.Armour);
			AddDraft(parameters.Draft - this.Parameters.Draft);
			AddHitPoints(parameters.HitPoints - this.Parameters.HitPoints);
			AddInitiative(parameters.Initiative - this.Parameters.Initiative);
			AddLuck(parameters.Luck - this.Parameters.Luck);
			AddMoral(parameters.Moral - this.Parameters.Moral);
			AddNumberOfGuns(parameters.NumberOfGuns - this.Parameters.NumberOfGuns);
			AddObservation(parameters.Observation - this.Parameters.Observation);
			AddSharpshooting(parameters.Sharpshooting - this.Parameters.Sharpshooting);
			AddSpeed(parameters.Speed - this.Parameters.Speed);
			AddSubtlety(parameters.Subtlety - this.Parameters.Subtlety);
			AddWeight(parameters.Weight - this.Parameters.Weight);
		}

		public virtual void AddObservation(Int32 addition) {
			this._parameters.Observation += addition;
		}
		public virtual void AddSubtlety(Int32 addition) {
			this._parameters.Subtlety += addition;
		}
		public virtual void AddArmour(TShipParts addition) {
			this._parameters.Armour += addition;
		}
		public virtual void AddHitPoints(TShipParts addition) {
			this._parameters.HitPoints += addition;
		}
		public virtual void AddNumberOfGuns(TShipParts addition) {
			this._parameters.NumberOfGuns += addition;
		}
		public virtual void AddSharpshooting(int addition) {
			this._parameters.Sharpshooting += addition;
		}
		public virtual void AddSpeed(int addition) {
			this._parameters.Speed += addition;
		}
		public virtual void AddDraft(int addition) {
			this._parameters.Draft += addition;
		}
		public virtual void AddWeight(int addition) {
			this._parameters.Weight += addition;
		}
		public virtual void AddLuck(int addition) {
			this._parameters.Luck += addition;
		}
		public virtual void AddMoral(int addition) {
			this._parameters.Moral += addition;
		}
		public virtual void AddInitiative(int addition) {
			this._parameters.Initiative += addition;
		}
	}
	class TBalancingParametersController : TParametersController {
		public void UpdateParameters(TParameters oldParameters) {
			throw new NotImplementedException();
			///////////// TO DO /////////////
		}
		public override void AddArmour(TShipParts addition) {
			TParameters oldParameters = _parameters;
			base.AddArmour(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddDraft(int addition) {
			TParameters oldParameters = _parameters;
			base.AddDraft(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddHitPoints(TShipParts addition) {
			TParameters oldParameters = _parameters;
			base.AddHitPoints(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddInitiative(int addition) {
			TParameters oldParameters = _parameters;
			base.AddInitiative(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddLuck(int addition) {
			TParameters oldParameters = _parameters;
			base.AddLuck(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddMoral(int addition) {
			TParameters oldParameters = _parameters;
			base.AddMoral(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddNumberOfGuns(TShipParts addition) {
			TParameters oldParameters = _parameters;
			base.AddNumberOfGuns(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddObservation(int addition) {
			TParameters oldParameters = _parameters;
			base.AddObservation(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddSharpshooting(int addition) {
			TParameters oldParameters = _parameters;
			base.AddSharpshooting(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddSpeed(int addition) {
			TParameters oldParameters = _parameters;
			base.AddSpeed(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddSubtlety(int addition) {
			TParameters oldParameters = _parameters;
			base.AddSubtlety(addition);
			UpdateParameters(oldParameters);
		}
		public override void AddWeight(int addition) {
			TParameters oldParameters = _parameters;
			base.AddWeight(addition);
			UpdateParameters(oldParameters);
		}
	}
	class TBindedParametersController : TParametersController {
		private TParametersController targetController;
		public TBindedParametersController(TParametersController paramController) {
			this.targetController = paramController;
		}
		public override void AddArmour(TShipParts addition) {
			base.AddArmour(addition);
			targetController.AddArmour(addition);
		}
		public override void AddDraft(int addition) {
			base.AddDraft(addition);
			targetController.AddDraft(addition);
		}
		public override void AddHitPoints(TShipParts addition) {
			base.AddHitPoints(addition);
			targetController.AddHitPoints(addition);
		}
		public override void AddInitiative(int addition) {
			base.AddInitiative(addition);
			targetController.AddInitiative(addition);
		}
		public override void AddLuck(int addition) {
			base.AddLuck(addition);
			targetController.AddLuck(addition);
		}
		public override void AddMoral(int addition) {
			base.AddMoral(addition);
			targetController.AddMoral(addition);
		}
		public override void AddNumberOfGuns(TShipParts addition) {
			base.AddNumberOfGuns(addition);
			targetController.AddNumberOfGuns(addition);
		}
		public override void AddObservation(int addition) {
			base.AddObservation(addition);
			targetController.AddObservation(addition);
		}
		public override void AddSharpshooting(int addition) {
			base.AddSharpshooting(addition);
			targetController.AddSharpshooting(addition);
		}
		public override void AddSpeed(int addition) {
			base.AddSpeed(addition);
			targetController.AddSpeed(addition);
		}
		public override void AddSubtlety(int addition) {
			base.AddSubtlety(addition);
			targetController.AddSubtlety(addition);
		}
		public override void AddWeight(int addition) {
			base.AddWeight(addition);
			targetController.AddWeight(addition);
		}
	}
}

//#region hit poInt32s
//public Int32 HullLeftHitpoInt32s {
//	get { return currentSpecs.HullLeftHitpoInt32s; }
//	set {
//		TSpecificators oldSpecs = currentSpecs;
//		currentSpecs.HullLeftHitpoInt32s = value;
//		UpdateNumberOfGunsLeft(oldSpecs);
//		UpdateMoral(oldSpecs);
//	}
//}
//public Int32 HullRightHitpoInt32s {
//	get { return currentSpecs.HullRightHitpoInt32s; }
//	set {
//		TSpecificators oldSpecs = currentSpecs;
//		currentSpecs.HullRightHitpoInt32s = value;
//		UpdateNumberOfGunsRight(oldSpecs);
//		UpdateMoral(oldSpecs);
//	}
//}
//public Int32 HullHeadHitpoInt32s {
//	get { return currentSpecs.HullHeadHitpoInt32s; }
//	set {
//		TSpecificators oldSpecs = currentSpecs;
//		currentSpecs.HullHeadHitpoInt32s = value;
//		UpdateNumberOfGunsHead(oldSpecs);
//		UpdateMoral(oldSpecs);

//	}
//}
//public Int32 HullTailHitpoInt32s {
//	get { return currentSpecs.HullTailHitpoInt32s; }
//	set {
//		TSpecificators oldSpecs = currentSpecs;
//		currentSpecs.HullTailHitpoInt32s = value;
//		UpdateNumberOfGunsTail(oldSpecs);
//		UpdateMoral(oldSpecs);
//	}
//}
//public Int32 MastHitpoInt32s {
//	get { return currentSpecs.MastHitpoInt32s; }
//	set {
//		TSpecificators oldSpecs = currentSpecs;
//		currentSpecs.MastHitpoInt32s = value;
//		UpdateNumberOfGunsAir(oldSpecs);
//		UpdateMoral(oldSpecs);
//	}
//}
//}
//public Int32 BearingCapacity {
//	get { return currentSpecs.BearingCapacity; }
//	set {
//		if (value != this.BearingCapacity) {
//			TSpecificators oldSpecs = currentSpecs;
//			currentSpecs.BearingCapacity = value;
//			UpdateDraft(oldSpecs);
//			UpdateSpeed(oldSpecs);
//		}
//	}
//}
//public Int32 Speed {
//	get { return currentSpecs.Speed; }
//	set {
//		if (value != this.Speed) {
//			TSpecificators oldSpecs = currentSpecs;
//			currentSpecs.Speed = value;
//			UpdateInitiative(oldSpecs);
//		}
//	}
//}
//public Int32 Moral {
//	get { return currentSpecs.Moral; }
//	set {
//		if (value != this.Moral) {
//			TSpecificators oldSpecs = currentSpecs;
//			currentSpecs.Moral = value;
//			UpdateSpeed(oldSpecs);
//			UpdateSharpshooting(oldSpecs);
//			UpdateSubtlety(oldSpecs);
//			UpdateObservation(oldSpecs);
//			//UpdateInitiative(oldSpecs);
//		}
//	}
//}

//#endregion

//#endregion
//#region update methods
//#region dependent
//private void UpdateSpeed(TSpecificators oldSpecs) {
//	UpdateInitiative(oldSpecs);
//	throw new NotImplementedException();
//}
//private void UpdateMoral(TSpecificators oldSpecs) {
//	UpdateSpeed(new TSpecificators());
//	UpdateSharpshooting(oldSpecs);
//	UpdateSubtlety(oldSpecs);
//	UpdateObservation(oldSpecs);
//	//UpdateInitiative(oldSpecs);
//	throw new NotImplementedException();
//}
//#endregion
