using UnityEngine;
using System.Collections;

public class MotionMapSensor : MonoBehaviour {
	private float xLeft;
	private float xRight;
	private float yBottom;
	private float yTop;
	private float yMiddle;

	private float currentMouseX;
	private float currentMouseY;

	// Can probably just attach a collider in the current class.
	// Now, I think this help readability.  But consider in the future.
	MotionMapCollider collider;
	GameObject marker;
	private bool markerOnFlag;

	// Use this for initialization
	void Start () {
		collider = (MotionMapCollider)GameObject.Find("MotionMapCollider").GetComponent("MotionMapCollider");
		//markerOnFlag = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(collider.CheckMousePosition()) {
			currentMouseX = collider.GetMouseX();
			currentMouseY = collider.GetMouseY();
			if(markerOnFlag && CheckRange())
				UpdateMarkerPosition();
		}
	}

	private bool CheckRange() {
		if(xLeft <= currentMouseX && currentMouseX <= xRight &&
		   yBottom <= currentMouseY && currentMouseY <= yTop) {
			//Debug.Log("current mouse x:  " + currentMouseX);
			//Debug.Log("current mouse y:  " + currentMouseY);
			return true;
		}
		return false;
	}

	public void SetDimensions(float xLeft, float xRight, float yBottom, float yTop) {
		this.xLeft = xLeft;
		this.xRight = xRight;
		this.yBottom = yBottom;
		this.yTop = yTop;
		yMiddle = yBottom + (yTop - yBottom)/2.0f;

		if(marker == null)
			MakeMotionMapMarker();

		collider.TurnOff();
		collider.SetScale(new Vector3(
			Conversions.PixelsToUnits(xRight - xLeft),
			Conversions.PixelsToUnits(yTop - yBottom),
			1));
		collider.SetPosition(
			Conversions.PositionObject_PixelsToWorld(
				xLeft + (xRight-xLeft)/2.0f, yBottom + (yTop - yBottom)/2.0f, 1));
	}

	public void TurnMarkerOn() {
		markerOnFlag = true;
		marker.renderer.enabled = true;
		//UpdateMarkerPosition();
		collider.TurnOn();
	}

	public void TurnMarkerOff() {
		markerOnFlag = false;
		collider.TurnOff();
	}

	public void HideMarker() {
		marker.renderer.enabled = false;
	}

	private void UpdateMarkerPosition() {
		marker.transform.position = Conversions.PositionObject_PixelsToWorld(currentMouseX, yMiddle, 1);
	}

	private void MakeMotionMapMarker() {
		if(marker == null)
			marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		marker.transform.localScale = new Vector3(.2f, .2f, .2f);
		marker.renderer.material.color = Color.green;
		marker.transform.position = Conversions.PositionObject_PixelsToWorld(xLeft, yMiddle, 1);
	}

	public float GetMarkerXInPixels() {
		return Camera.main.WorldToScreenPoint(new Vector3(
			marker.transform.position.x, 
			marker.transform.position.y, 
			marker.transform.position.z)).x;
	}

	public void SetMarkerPosition(float x) {
		marker.transform.position = Conversions.PositionObject_PixelsToWorld(x, yMiddle, 1);
	}

	public bool CheckPlayerStarted() {
		return collider.CheckPlayerStarted();
	}
}
