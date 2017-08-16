using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedButtonHoldCameraCntrl : DelayedButtonHold {


	[SerializeField]
	protected SimpleButtonFunctions _buttons;
	[SerializeField]
	protected Constants.CameraControls _cntrl;


	protected override void activate()
	{
		_buttons.sendPositionCameraDirection(_cntrl);
	}
	

}
