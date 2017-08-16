using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExperienceInfoListBox : MonoBehaviour {

	public int listBoxID;   // Must be unique, and count from 0
	public ExperienceInfoBundle content;        // The content of the list box
	public Text contentDisplay;

	public ExperienceInfoListBox lastListBox;
	public ExperienceInfoListBox nextListBox;

	protected int _contentID;

	// All position calculations here are in the local space of the list
	protected Vector2 _canvasMaxPos;
	protected Vector2 _unitPos;
	protected Vector2 _lowerBoundPos;
	protected Vector2 _upperBoundPos;
	protected Vector2 _shiftBoundPos;

	protected Vector3 _slidingDistance;   // The sliding distance at each frame
	protected Vector3 _slidingDistanceLeft;

	protected Vector3 _originalLocalScale;

	protected bool _keepSliding = false;
	protected int _slidingFramesLeft;

	public bool keepSliding { set { _keepSliding = value; } }

	/* Notice: ExperienceInfoListBox will initialize its variables from ExperienceInfoListPositionCtrl.
	 * Make sure that the execution order of script ExperienceInfoListPositionCtrl is prior to
	 * ExperienceInfoListBox.
	 */
	void Start()
	{
		_canvasMaxPos = ExperienceInfoListPositionCtrl.Instance.canvasMaxPos_L;
		_unitPos = ExperienceInfoListPositionCtrl.Instance.unitPos_L;
		_lowerBoundPos = ExperienceInfoListPositionCtrl.Instance.lowerBoundPos_L;
		_upperBoundPos = ExperienceInfoListPositionCtrl.Instance.upperBoundPos_L;
		_shiftBoundPos = ExperienceInfoListPositionCtrl.Instance.shiftBoundPos_L;

		_originalLocalScale = transform.localScale;

		initialPosition(listBoxID);
		initialContent();
	}

	/* Initialize the content of ExperienceInfoListBox.
	 */
	void initialContent()
	{
		if (listBoxID == ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2)
			_contentID = 0;
		else if (listBoxID < ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2)
			_contentID = ExperienceInfoListBank.Instance.getListLength() - (ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2 - listBoxID);
		else
			_contentID = listBoxID - ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2;

		while (_contentID < 0)
			_contentID += ExperienceInfoListBank.Instance.getListLength();
		_contentID = _contentID % ExperienceInfoListBank.Instance.getListLength();

		updateContent(ExperienceInfoListBank.Instance.getListContent(_contentID));
	}

	protected virtual void updateContent(ExperienceInfoBundle content)
	{
		this.content = content;
		this.contentDisplay.text = content.getName();
	}

	/* Make the list box slide for delta x or y position.
	 */
	public void setSlidingDistance(Vector3 distance, int slidingFrames)
	{
		_keepSliding = true;
		_slidingFramesLeft = slidingFrames;

		_slidingDistanceLeft = distance;
		_slidingDistance = Vector3.Lerp(Vector3.zero, distance, ExperienceInfoListPositionCtrl.Instance.slidingFactor);
	}

	/* Move the listBox for world position unit.
	 * Move up when "up" is true, or else, move down.
	 */
	public void unitMove(int unit, bool up_right)
	{
		Vector2 deltaPos;

		if (up_right)
			deltaPos = _unitPos * (float)unit;
		else
			deltaPos = _unitPos * (float)unit * -1;

		switch (ExperienceInfoListPositionCtrl.Instance.direction) {
		case ExperienceInfoListPositionCtrl.Direction.VERTICAL:
			setSlidingDistance(new Vector3(0.0f, deltaPos.y, 0.0f), ExperienceInfoListPositionCtrl.Instance.slidingFrames);
			break;
		case ExperienceInfoListPositionCtrl.Direction.HORIZONTAL:
			setSlidingDistance(new Vector3(deltaPos.x, 0.0f, 0.0f), ExperienceInfoListPositionCtrl.Instance.slidingFrames);
			break;
		}
	}

	void Update()
	{
		if (_keepSliding) {
			--_slidingFramesLeft;
			if (_slidingFramesLeft == 0) {
				_keepSliding = false;
				// At the last sliding frame, move to that position.
				// At free moving mode, this function is disabled.
				if (ExperienceInfoListPositionCtrl.Instance.alignToCenter ||
					ExperienceInfoListPositionCtrl.Instance.controlByButton) {
					updatePosition(_slidingDistanceLeft);
				}
				// FIXME: Due to compiler optimization?
				// When using condition listBoxID == 0, some boxes don't execute
				// the above code. (For other condition, like 1, 3, or 4, also has the same
				// problem. Only using 2 will work normally.)
				if (listBoxID == 2 &&
					ExperienceInfoListPositionCtrl.Instance.needToAlignToCenter)
					ExperienceInfoListPositionCtrl.Instance.alignToCenterSlide();
				return;
			}

			updatePosition(_slidingDistance);
			_slidingDistanceLeft -= _slidingDistance;
			_slidingDistance = Vector3.Lerp(Vector3.zero, _slidingDistanceLeft, ExperienceInfoListPositionCtrl.Instance.slidingFactor);
		}
	}

	/* Initialize the local position of the list box accroding to its ID.
	 */
	void initialPosition(int listBoxID)
	{
		// If there are even number of ListBoxes, adjust the initial position by an half unitPos.
		if ((ExperienceInfoListPositionCtrl.Instance.listBoxes.Length & 0x1) == 0) {
			switch (ExperienceInfoListPositionCtrl.Instance.direction) {
			case ExperienceInfoListPositionCtrl.Direction.VERTICAL:
				transform.localPosition = new Vector3(0.0f,
					_unitPos.y * (listBoxID * -1 + ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2) - _unitPos.y / 2,
					0.0f);
				updateXPosition();
				break;
			case ExperienceInfoListPositionCtrl.Direction.HORIZONTAL:
				transform.localPosition = new Vector3(_unitPos.x * (listBoxID - ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2) - _unitPos.x / 2,
				0.0f, 0.0f);
				updateYPosition();
				break;
			}
		} else {
			switch (ExperienceInfoListPositionCtrl.Instance.direction) {
			case ExperienceInfoListPositionCtrl.Direction.VERTICAL:
				transform.localPosition = new Vector3(0.0f,
					_unitPos.y * (listBoxID * -1 + ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2),
					0.0f);
				updateXPosition();
				break;
			case ExperienceInfoListPositionCtrl.Direction.HORIZONTAL:
				transform.localPosition = new Vector3(_unitPos.x * (listBoxID - ExperienceInfoListPositionCtrl.Instance.listBoxes.Length / 2),
					0.0f, 0.0f);
				updateYPosition();
				break;
			}
		}
	}

	/* Update the local position of ExperienceInfoListBox accroding to the delta position at each frame.
	 * Note that the deltaPosition must be in local space.
	 */
	public void updatePosition(Vector3 deltaPosition_L)
	{
		switch (ExperienceInfoListPositionCtrl.Instance.direction) {
		case ExperienceInfoListPositionCtrl.Direction.VERTICAL:
			transform.localPosition += new Vector3(0.0f, deltaPosition_L.y, 0.0f);
			updateXPosition();
			checkBoundaryY();
			break;
		case ExperienceInfoListPositionCtrl.Direction.HORIZONTAL:
			transform.localPosition += new Vector3(deltaPosition_L.x, 0.0f, 0.0f);
			updateYPosition();
			checkBoundaryX();
			break;
		}
	}

	/* Calculate the x position accroding to the y position.
	 * Formula: x = max_x * angularity * cos( radian controlled by y )
	 * radian = (y / upper_y) * pi / 2, so the range of radian is from pi/2 to 0 to -pi/2,
	 * and corresponding cosine value is from 0 to 1 to 0.
	 */
	void updateXPosition()
	{
		transform.localPosition = new Vector3(
			_canvasMaxPos.x * ExperienceInfoListPositionCtrl.Instance.angularity
			* Mathf.Cos(transform.localPosition.y / _upperBoundPos.y * Mathf.PI / 2.0f),
			transform.localPosition.y, transform.localPosition.z);
		updateSize(_upperBoundPos.y, transform.localPosition.y);
	}

	/* Calculate the y position accroding to the x position.
	 */
	void updateYPosition()
	{
		transform.localPosition = new Vector3(
			transform.localPosition.x,
			_canvasMaxPos.y * ExperienceInfoListPositionCtrl.Instance.angularity
			* Mathf.Cos(transform.localPosition.x / _upperBoundPos.x * Mathf.PI / 2.0f),
			transform.localPosition.z);
		updateSize(_upperBoundPos.x, transform.localPosition.x);
	}

	/* Check if the ExperienceInfoListBox is beyond the upper or lower bound or not.
	 * If does, move the ExperienceInfoListBox to the other side and update the content.
	 */
	void checkBoundaryY()
	{
		float beyondPosY_L = 0.0f;

		// Narrow the checking boundary in order to avoid the list swaying to one side
		if (transform.localPosition.y < _lowerBoundPos.y + _shiftBoundPos.y) {
			beyondPosY_L = (_lowerBoundPos.y + _shiftBoundPos.y - transform.localPosition.y);
			transform.localPosition = new Vector3(
				transform.localPosition.x,
				_upperBoundPos.y - _unitPos.y + _shiftBoundPos.y - beyondPosY_L,
				transform.localPosition.z);
			updateToLastContent();
		} else if (transform.localPosition.y > _upperBoundPos.y - _shiftBoundPos.y) {
			beyondPosY_L = (transform.localPosition.y - _upperBoundPos.y + _shiftBoundPos.y);
			transform.localPosition = new Vector3(
				transform.localPosition.x,
				_lowerBoundPos.y + _unitPos.y - _shiftBoundPos.y + beyondPosY_L,
				transform.localPosition.z);
			updateToNextContent();
		}

		updateXPosition();
	}

	void checkBoundaryX()
	{
		float beyondPosX_L = 0.0f;

		// Narrow the checking boundary in order to avoid the list swaying to one side
		if (transform.localPosition.x < _lowerBoundPos.x + _shiftBoundPos.x) {
			beyondPosX_L = (_lowerBoundPos.x + _shiftBoundPos.x - transform.localPosition.x);
			transform.localPosition = new Vector3(
				_upperBoundPos.x - _unitPos.x + _shiftBoundPos.x - beyondPosX_L,
				transform.localPosition.y,
				transform.localPosition.z);
			updateToNextContent();
		} else if (transform.localPosition.x > _upperBoundPos.x - _shiftBoundPos.x) {
			beyondPosX_L = (transform.localPosition.x - _upperBoundPos.x + _shiftBoundPos.x);
			transform.localPosition = new Vector3(
				_lowerBoundPos.x + _unitPos.x - _shiftBoundPos.x + beyondPosX_L,
				transform.localPosition.y,
				transform.localPosition.z);
			updateToLastContent();
		}

		updateYPosition();
	}

	/* Scale the size of listBox accroding to the position.
	 */
	void updateSize(float smallest_at, float target_value)
	{
		transform.localScale = _originalLocalScale *
			(1.0f + ExperienceInfoListPositionCtrl.Instance.scaleFactor * Mathf.InverseLerp(smallest_at, 0.0f, Mathf.Abs(target_value)));
	}

	public int getCurrentContentID()
	{
		return _contentID;
	}

	/* Update to the last content of the next ExperienceInfoListBox
	 * when the ExperienceInfoListBox appears at the top of camera.
	 */
	void updateToLastContent()
	{
		_contentID = nextListBox.getCurrentContentID() - 1;
		_contentID = (_contentID < 0) ? ExperienceInfoListBank.Instance.getListLength() - 1 : _contentID;

		updateContent(ExperienceInfoListBank.Instance.getListContent(_contentID));
	}

	/* Update to the next content of the last ExperienceInfoListBox
	 * when the ExperienceInfoListBox appears at the bottom of camera.
	 */
	void updateToNextContent()
	{
		_contentID = lastListBox.getCurrentContentID() + 1;
		_contentID = (_contentID == ExperienceInfoListBank.Instance.getListLength()) ? 0 : _contentID;

		updateContent(ExperienceInfoListBank.Instance.getListContent(_contentID));
	}
}
