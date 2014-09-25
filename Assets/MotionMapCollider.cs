using UnityEngine;
using System.Collections;

public class MotionMapCollider : MonoBehaviour {
	private float currentMouseX;
	private float currentMouseY;
	private bool checkMouseFlag;
	private bool startFlag;
	private bool offFlag;

	void Start () {
		//gameObject.renderer.material.color = Color.red;
		gameObject.renderer.enabled = false;
	}
	
	void Update () {}

	void OnMouseDrag() {
		if(offFlag)
			return;

		currentMouseX = Input.mousePosition.x;
		currentMouseY = Input.mousePosition.y;
		checkMouseFlag = true;
		startFlag = true;
	}
	
	//void OnMouseOver() {
	//	//Debug.Log("over");
	//	currentMouseX = Input.mousePosition.x;
	//	currentMouseY = Input.mousePosition.y;
	//}

	void OnMouseExit() {
		if(offFlag)
			return;

		currentMouseX = Input.mousePosition.x;
		currentMouseY = Input.mousePosition.y;
		checkMouseFlag = false;
	}

	void OnMouseUp() {
		if(offFlag)
			return;

		checkMouseFlag = false;
	}

	public void SetScale(Vector3 scale) {
		gameObject.transform.localScale = scale;
	}

	public void SetPosition(Vector3 position) {
		gameObject.transform.position = position;
	}

	public float GetMouseX() {
		return currentMouseX;
	}

	public float GetMouseY() {
		return currentMouseY;
	}

	public bool CheckMousePosition() {
		return checkMouseFlag;
	}

	public bool CheckPlayerStarted() {
		return startFlag;
	}

	public void TurnOn() {
		offFlag = false;
	}

	public void TurnOff() {
		offFlag = true;
		checkMouseFlag = false;
		startFlag = false;
	}
}
