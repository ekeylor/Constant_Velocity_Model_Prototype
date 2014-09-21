using UnityEngine;
using System.Collections;

public class DrawProjectGraph : MonoBehaviour {
	bool guiOff = true;
	Rect dimensions;
	Texture2D bgRectangle;

	void Start () {
		guiOff = true;
	}
	void Update () {}

	void Draw(object[] rectData) {//Rect dimensions, Texture2D rectangle, xAxisStart, xAxisLength) {
		dimensions = (Rect)rectData[0];
		bgRectangle = (Texture2D)rectData[1];
		guiOff = false;
	}

	void OnGUI() {
		if(guiOff)
			return;

		GUI.DrawTexture(dimensions, bgRectangle);

	}
}
