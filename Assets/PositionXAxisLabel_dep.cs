using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PositionXAxisLabel : MonoBehaviour {
	Text axisLabel;
	string text = "";

	// Use this for initialization
	void Start () {
		axisLabel = GetComponent<Text>();
		axisLabel.text = text;
	}
	
	// Update is called once per frame
	void Update () {
		axisLabel.text = text;
	}

	public void SetLabel(string label) {
		text = label;
	}

	public void SetPosition(Vector3 position) {
		axisLabel.transform.position = position;
	}
}