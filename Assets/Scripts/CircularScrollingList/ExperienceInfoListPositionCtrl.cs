﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceInfoListPositionCtrl : MonoBehaviour {

	public enum Direction
	{
		VERTICAL,
		HORIZONTAL
	};

	public static ExperienceInfoListPositionCtrl Instance;
	/* Initial settings.
	 *   Mode            controlByButton  alignToCenter
	 * --------------------------------------------------
	 *   Free moving          false           false
	 *   Align to center      false           true
	 *   Control by btn       true          Don't care
	 */
	public bool controlByButton = false;
	public bool alignToCenter = false;
	[HideInInspector]
	public bool needToAlignToCenter = false;

	public ExperienceInfoListBox[] listBoxes;

	public Button[] buttons;

	public Direction direction = Direction.VERTICAL;
	// For 3D camera, the distance between canvas plane and camera.
	public float canvasDistance = 100.0f;
	// Set the distance between each ExperienceInfoListBox. The larger, the closer.
	public float divideFactor = 2.0f;
	// Set the sliding duration. The larger, the longer.
	public int slidingFrames = 35;
	// Set the sliding speed. The larger, the quicker.
	[Range(0.0f, 1.0f)]
	public float slidingFactor = 0.2f;
	// Set the scrolling list curving to left/right, or up/down in HORIZONTAL mode.
	// Positive: Curve to right (up); Negative: Curve to left (down).
	[Range(-1.0f, 1.0f)]
	public float angularity = 0.3f;
	// Set the scale amount of the center listBox.
	public float scaleFactor = 0.32f;

	protected bool _isTouchingDevice;

	// The constrains of position in the local space of the canvas plane.
	protected Vector2 _canvasMaxPos_L;
	protected Vector2 _unitPos_L;
	protected Vector2 _lowerBoundPos_L;
	protected Vector2 _upperBoundPos_L;
	protected Vector2 _shiftBoundPos_L;
	// The gets of the above variables
	public Vector2 canvasMaxPos_L { get { return _canvasMaxPos_L; } }
	public Vector2 unitPos_L { get { return _unitPos_L; } }
	public Vector2 lowerBoundPos_L { get { return _lowerBoundPos_L; } }
	public Vector2 upperBoundPos_L { get { return _upperBoundPos_L; } }
	public Vector2 shiftBoundPos_L { get { return _shiftBoundPos_L; } }

	// Input mouse/finger position in the local space of the list.
	protected Vector3 _startInputPos_L;
	protected Vector3 _lastInputPos_L;
	protected Vector3 _currentInputPos_L;
	protected Vector3 _deltaInputPos_L;
	protected int _numofSlideFrames;

	void Awake()
	{
		Instance = this;

		switch (Application.platform) {
		case RuntimePlatform.WindowsEditor:
			_isTouchingDevice = false;
			break;
		case RuntimePlatform.Android:
			_isTouchingDevice = true;
			break;
		}
	}

	/* Notice: ExperienceInfoListBox will initialize its variables from here, so ExperienceInfoListPositionCtrl
	 * must be executed before ExperienceInfoListBox. You have to set the execution order in the inspector.
	 */
	void Start()
	{
		/* Convert the coordination space from the screen space to the world space.
		 * Then, substract the coorination at the left-bottom corner of the screen
		 * from the one at the right-top corner to get the size of the screen in world space.
		 * For perspective view,
		 * we have to take the distance between canvas plane and camera into account. */
		_canvasMaxPos_L = Camera.main.ScreenToWorldPoint(
			new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, canvasDistance)) -
			Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, canvasDistance));
		/* The result above is the length of sides of boundary of the canvas plane in the world space,
		 * so we need to convert it to the local space of the list. The lossyScale will return
		 * the scale vector which is the scaling amount from its local space to the world
		 * space. Finally, by dividing the result by two we get the max coordination
		 * of the canvas plane in the canvas plane space (Assuming the origin of the
		 * canvas plane is at the center of the canvas plane).*/
		_canvasMaxPos_L = new Vector2(
			_canvasMaxPos_L.x / (2.0f * transform.parent.lossyScale.x),
			_canvasMaxPos_L.y / (2.0f * transform.parent.lossyScale.y));
		// Use the lossy scale of the canvas plane here, so we can scale the whole list
		// by scaling the gameObject ExperienceInfoListPositionCtrl.

		_unitPos_L = _canvasMaxPos_L / divideFactor;
		_lowerBoundPos_L = _unitPos_L * (-1 * listBoxes.Length / 2 - 1);
		_upperBoundPos_L = _unitPos_L * (listBoxes.Length / 2 + 1);
		_shiftBoundPos_L = _unitPos_L * 0.3f;

		// If there are even number of ListBoxes, narrow the boundary for 1 unitPos.
		if ((listBoxes.Length & 0x1) == 0) {
			_lowerBoundPos_L += _unitPos_L / 2;
			_upperBoundPos_L -= _unitPos_L / 2;
		}

		if (!controlByButton)
			foreach (Button button in buttons)
				button.gameObject.SetActive(false);
	}

	void Update()
	{
		if (!controlByButton) {
			if (!_isTouchingDevice)
				storeMousePosition();
			else
				storeFingerPosition();
		}
	}

	/* Store the position of mouse when the player clicks the left mouse button.
	 */
	void storeMousePosition()
	{
		if (Input.GetMouseButtonDown(0)) {
			_lastInputPos_L = Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x, Input.mousePosition.y, canvasDistance));
			_lastInputPos_L = divideComponent(_lastInputPos_L, transform.lossyScale);
			_startInputPos_L = _lastInputPos_L;
			_numofSlideFrames = 0;
			// When the user starts to drag the list, all listBoxes stop free sliding.
			foreach (ExperienceInfoListBox listBox in listBoxes)
				listBox.keepSliding = false;
		} else if (Input.GetMouseButton(0)) {
			_currentInputPos_L = Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x, Input.mousePosition.y, canvasDistance));
			_currentInputPos_L = divideComponent(_currentInputPos_L, transform.lossyScale);
			_deltaInputPos_L = _currentInputPos_L - _lastInputPos_L;
			foreach (ExperienceInfoListBox listbox in listBoxes)
				listbox.updatePosition(_deltaInputPos_L);

			_lastInputPos_L = _currentInputPos_L;
			++_numofSlideFrames;
		} else if (Input.GetMouseButtonUp(0))
			setSlidingEffect();
	}

	/* Store the position of touching on the mobile.
	 */
	void storeFingerPosition()
	{
		if (Input.GetTouch(0).phase == TouchPhase.Began) {
			_lastInputPos_L = Camera.main.ScreenToWorldPoint(
				new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, canvasDistance));
			_lastInputPos_L = divideComponent(_lastInputPos_L, transform.lossyScale);
			_startInputPos_L = _lastInputPos_L;
			_numofSlideFrames = 0;
			// When the user starts to drag the list, all listBoxes stop free sliding.
			foreach (ExperienceInfoListBox listBox in listBoxes)
				listBox.keepSliding = false;
		} else if (Input.GetTouch(0).phase == TouchPhase.Moved) {
			_currentInputPos_L = Camera.main.ScreenToWorldPoint(
				new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, canvasDistance));
			_currentInputPos_L = divideComponent(_currentInputPos_L, transform.lossyScale);
			_deltaInputPos_L = _currentInputPos_L - _lastInputPos_L;
			foreach (ExperienceInfoListBox listbox in listBoxes)
				listbox.updatePosition(_deltaInputPos_L);

			_lastInputPos_L = _currentInputPos_L;
			++_numofSlideFrames;
		} else if (Input.GetTouch(0).phase == TouchPhase.Ended)
			setSlidingEffect();
	}

	/* If the touching is ended, calculate the distance to slide and
	 * assign to the listBoxes.
	 */
	void setSlidingEffect()
	{
		Vector3 deltaPos = _deltaInputPos_L;
		Vector3 slideDistance = _lastInputPos_L - _startInputPos_L;
		bool fastSliding = isFastSliding(_numofSlideFrames, slideDistance);

		if (fastSliding)
			deltaPos *= 5.0f;   // Slide more longer!

		if (alignToCenter) {
			foreach (ExperienceInfoListBox listbox in listBoxes)
				listbox.setSlidingDistance(deltaPos, fastSliding ? slidingFrames : slidingFrames / 2);
			needToAlignToCenter = true;
		} else {
			foreach (ExperienceInfoListBox listbox in listBoxes)
				listbox.setSlidingDistance(deltaPos, fastSliding ? slidingFrames * 2 : slidingFrames);
		}
	}

	/* Judge if this cursor or finger slide is the fast sliding.
	 * If the duration of a slide is within 15 frames and the distance is
	 * longer than the 1/3 of the distance of the list, the slide is the fast sliding.
	 */
	bool isFastSliding(int frames, Vector3 distance)
	{
		if (frames < 15) {
			switch (direction) {
			case Direction.HORIZONTAL:
				if (Mathf.Abs(distance.x) > _canvasMaxPos_L.x * 2.0f / 3.0f)
					return true;
				else
					return false;
			case Direction.VERTICAL:
				if (Mathf.Abs(distance.y) > _canvasMaxPos_L.y * 2.0f / 3.0f)
					return true;
				else
					return false;
			}
		}
		return false;
	}

	/* Move all ListBoxes for a distance which equals to the smallest distance
	 * between ExperienceInfoListBox and the center.
	 * This method will be called from ListBox0 when needToAlignToCenter flag is set, and
	 * the flag will be cleared in here.
	 */
	public void alignToCenterSlide()
	{
		Vector3 deltaPos = findDeltaPositionToCenter();

		foreach (ExperienceInfoListBox listbox in listBoxes)
			listbox.setSlidingDistance(deltaPos, slidingFrames / 2);

		needToAlignToCenter = false;
	}

	/* Find the listBox which is the closest to the center position,
	 * And calculate the delta position of x or y between them.
	 */
	Vector3 findDeltaPositionToCenter()
	{
		float minDeltaPos = Mathf.Infinity;
		float deltaPos;

		switch (direction) {
		case Direction.VERTICAL:
			foreach (ExperienceInfoListBox listBox in listBoxes) {
				deltaPos = -listBox.transform.localPosition.y;
				if (Mathf.Abs(deltaPos) < Mathf.Abs(minDeltaPos))
					minDeltaPos = deltaPos;
			}

			return new Vector3(0.0f, minDeltaPos, 0.0f);

		case Direction.HORIZONTAL:
			foreach (ExperienceInfoListBox listBox in listBoxes) {
				deltaPos = -listBox.transform.localPosition.x;
				if (Mathf.Abs(deltaPos) < Mathf.Abs(minDeltaPos))
					minDeltaPos = deltaPos;
			}

			return new Vector3(minDeltaPos, 0.0f, 0.0f);

		default:
			return Vector3.zero;
		}
	}

	/* Divide each component of vector a by vector b.
	 */
	Vector3 divideComponent(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
	}

	/* controlByButton is enabled!
	 * When the next content button is pressed,
	 * move all listBoxes 1 unit up.
	 */
	public void nextContent()
	{
		foreach (ExperienceInfoListBox listbox in listBoxes)
			listbox.unitMove(1, true);
	}

	/* controlByButton is enabled!
	 * When the last content button is pressed,
	 * move all listBoxes 1 unit down.
	 */
	public void lastContent()
	{
		foreach (ExperienceInfoListBox listbox in listBoxes)
			listbox.unitMove(1, false);
	}
}
