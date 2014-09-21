using UnityEngine;
using System.Collections;

public class Conversions : MonoBehaviour {
	private static float pixelsToWorld;
	private static float worldToPixels;
	private static Camera mainCamera;

	public static Vector3 ScaleObject_PixelsToWorld(float x, float y, float z) {
		if(mainCamera == null)
			makePixelUnitConversionFactors();
		return new Vector3(x*pixelsToWorld, y*pixelsToWorld, z*pixelsToWorld);
	}
	
	public static Vector3 PositionObject_PixelsToWorld(float x, float y, float z) {
		if(mainCamera == null)
			makePixelUnitConversionFactors();
		return mainCamera.ScreenToWorldPoint (new Vector3(x, y, z));
	}
	
	public static float UnitsToPixels(float number) {
		if(mainCamera == null)
			makePixelUnitConversionFactors();
		return number * worldToPixels;
	}
	
	public static float PixelsToUnits(float number) {
		if(mainCamera == null)
			makePixelUnitConversionFactors();
		return number * pixelsToWorld;
	}
	
	private static void makePixelUnitConversionFactors() {
		mainCamera = (Camera)GameObject.Find("Main Camera").GetComponent("Camera");
		pixelsToWorld = (mainCamera.orthographicSize / (Screen.height / 2.0f));
		worldToPixels = ((Screen.height / 2.0f) / mainCamera.orthographicSize);
	}
}
