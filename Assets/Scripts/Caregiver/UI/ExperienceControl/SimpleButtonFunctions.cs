using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SimpleButtonFunctions : MonoBehaviour {


	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private ReceiveView _positionView;
	[SerializeField]
	private UIManager _ui;
	
	public void changePosition()
	{
		_socket.Emit(Constants.CAREGIVER_TOGGLE_ON_POSITION_VIEW);
		toggleOnMainView(false);
	}

	public void selectPosition()
	{
		_socket.Emit(Constants.CAREGIVER_SELECT_POSITION_MESSAGE);
		toggleOnMainView(true);
	}
	public void cancelPositionSelect()
	{
		_socket.Emit(Constants.CAREGIVER_TOGGLE_OFF_POSITION_VIEW);
		toggleOnMainView(true);
	}


	public void sendPositionCameraDirection(int cntrl)
	{
		sendPositionCameraDirection((Constants.CameraControls) cntrl);
	}

	public void sendPositionCameraDirection(Constants.CameraControls cntrl)
	{
		JSONObject data = ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_MOVE_POSITION_VIEW_FIELDS, new string[1]{((int) cntrl).ToString()});
		_socket.Emit(Constants.CAREGIVER_MOVE_POSITION_VIEW_MESSAGE, data);
	}

	void toggleOnMainView(bool toggleOn)
	{
		if(toggleOn)
			_ui.nextUI();
		else
			_ui.previousUI();
		_positionView.enabled = !toggleOn;
	}
}
