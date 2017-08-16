using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class testAvatarAndLoadScene : MonoBehaviour {

	public SocketIOComponent _socket;
	public bool _testAvatar;
	public bool _testScene;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_testAvatar)
		{
			_testAvatar = false;
			_socket.Emit("avatarTest");
		}

		if(_testScene)
		{
			_testScene = false;
			_socket.Emit("stageTest");
		}
	}
}
