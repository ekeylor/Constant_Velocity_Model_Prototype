using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerCollision : MonoBehaviour {
	private List<string> enteredPositionLines;

	// Use this for initialization
	void Start () {
		enteredPositionLines = new List<string>();
	}
	void Update () {}

	public void Reset() {
		if(enteredPositionLines == null)
			enteredPositionLines = new List<string>();
		enteredPositionLines.Clear();
	}
	/*
	void OnCollisionEnter(Collision collision) {
		Debug.Log ("collision");
		Debug.Log("collider:  " + collision.collider.name);
		Debug.Log("game object:  " + collision.gameObject.name);
	}
	*/

	public bool IsMarkerOnPositionLine() {
		//foreach(string s in enteredPositionLines)
		//	Debug.Log(s);
		//Debug.Log("count:  " + enteredPositionLines.Count);
		if(enteredPositionLines.Count == 0)
			return false;
		return true;
	}

	// For some reason, it seems that a line is being registered twice.  Why?
	void OnTriggerEnter(Collider collider) {
		if(collider.name.Contains("Position_Line") && !enteredPositionLines.Contains(collider.name)) {
			enteredPositionLines.Add(collider.name);
			//Debug.Log("enter");
			//Debug.Log("collider:  " + collider.name);
			//Debug.Log(enteredPositionLines.Count);
		}
	}

	void OnTriggerExit(Collider collider) {
		//enteredPositionLines.RemoveAll((name) => { return (collider.name.Equals(name)); });
		enteredPositionLines.Remove(collider.name);
		//if(enteredPositionLines.Remove(collider.name)) {
			//Debug.Log("leave");
			//Debug.Log("collider:  " + collider.name);
			//Debug.Log(enteredPositionLines.Count);
		//}
	}
}
