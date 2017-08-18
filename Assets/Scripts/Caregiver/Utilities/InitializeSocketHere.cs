using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SocketIO;

///<summary>
/// sets up the socket connection to the server
/// calls functions once connection is established
///</summary>
public class InitializeSocketHere : MonoBehaviour {

	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private UnityEvent _events;
	private bool _needsInitialize;
	private bool _hasConnectionAttempt;
	void Start() {
		_socket.On(Constants.CONNECTED_MESSAGE, initializationEvents);
	}

	void Update()
	{
		if(!_hasConnectionAttempt)
		{
			_socket.Connect();
			_hasConnectionAttempt = true;
		}
		if(_needsInitialize)
		{
			_events.Invoke();
			_needsInitialize = false;
		}
	}
	
	void initializationEvents(SocketIOEvent ev)
	{
		_needsInitialize = true;
	}

}
