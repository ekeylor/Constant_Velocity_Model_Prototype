using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionObjectBuilder {
	private static List<GameObject> positionObjects = new List<GameObject>();
	private static List<GameObject> positionLines = new List<GameObject>();
	private static List<float> xPositions = new List<float>();
	private static List<float> yPositions = new List<float>();
	private static List<float> lineLengths = new List<float>();
	private static List<float> lineAngles = new List<float>();

	private const int LINE_WIDTH = 3;

	// assertions:  maxPositionValue > 1
	// assertions:  positions.Count > 1
	public static void Make(List<int> positions, int maxPositionValue, float xAxisStart, float yAxisStart, float xAxisLength, float yAxisLength) {
		CalculateXPositionsEqually(positions.Count, xAxisStart, xAxisLength);
		CalculateYPositions(positions, maxPositionValue, yAxisStart, yAxisLength);
		CalculateLineLengths(positions.Count);
		CalculateLineAngles(positions.Count);
		MakePositionObjects(positions);
		MakePositionLines(positions);
	}

	// Euclidean distance (pythagorean theorem)
	private static float CalculateLength(float x1, float x2, float y1, float y2) {
		return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
	}

	//private static float CalculateAngle(float adjacent, float hypotenuse) {
	//	return Mathf.Acos(adjacent/hypotenuse);
	//}

	private static float CalculateAngle(float opposite, float adjacent) {
		return -Mathf.Atan2(opposite, adjacent);
	}
	
	private static void CalculateXPositionsEqually(int numPositions, float xStart, float xAxisLength) {
		xPositions.Clear();
		//Debug.Log(xStart + "   " + xAxisLength);
		while(xPositions.Count < numPositions)
			xPositions.Add(xPositions.Count/((numPositions - 1) * 1.0f) * xAxisLength + xStart);
		//PrintList(xPositions, "PositionObjectBuilder.xPositions:  ");
	}

	private static void CalculateYPositions(List<int> positions, int maxPositionValue, float yStart, float yAxisLength) {
		yPositions.Clear();
		//Debug.Log(maxPositionValue + "   " + yStart + "   " + yAxisLength);
		while(yPositions.Count < positions.Count)
			yPositions.Add(positions[yPositions.Count]/(maxPositionValue*1.0f)*yAxisLength + yStart);
		//PrintList(yPositions, "PositionObjectBuilder.yPositions:  ");
	}

	private static void CalculateLineLengths(int numPositions) {
		lineLengths.Clear();
		while(lineLengths.Count < numPositions - 1) {
			lineLengths.Add(CalculateLength(
				xPositions[lineLengths.Count], xPositions[lineLengths.Count + 1], 
				yPositions[lineLengths.Count], yPositions[lineLengths.Count + 1])); 
		}
		//PrintList(lineLengths, "PositionObjectBuilder.lineLengths:  ");
	}

	private static void CalculateLineAngles(int numPositions) {
		lineAngles.Clear();
		while(lineAngles.Count < numPositions - 1) {
			lineAngles.Add(CalculateAngle(
				yPositions[lineAngles.Count + 1] - yPositions[lineAngles.Count],
				xPositions[lineAngles.Count + 1] - xPositions[lineAngles.Count]) * Mathf.Rad2Deg);
		}
		//PrintList(lineAngles, "PositionObjectBuilder.lineAngles:  ");
	}

	private static void MakePositionLines(List<int> positions) {
		foreach(GameObject g in positionLines)
			GameObject.Destroy(g);
		positionLines.Clear();
		while(positionLines.Count < positions.Count - 1) {
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.renderer.material.color = Color.cyan;
			obj.name = "Position_Line_" + positionObjects.Count;
			obj.transform.localScale = Conversions.ScaleObject_PixelsToWorld(
				lineLengths[positionLines.Count],
				LINE_WIDTH, 
				1);
			obj.transform.RotateAround(obj.renderer.bounds.center,
				Vector3.back,
				lineAngles[positionLines.Count]);
			obj.transform.position = Conversions.PositionObject_PixelsToWorld(
				xPositions[positionLines.Count] + (xPositions[positionLines.Count + 1] - xPositions[positionLines.Count])/2f,
				yPositions[positionLines.Count] + (yPositions[positionLines.Count + 1] - yPositions[positionLines.Count])/2f,
				.85f);
			positionLines.Add(obj);
		}
	}

	// Maybe use this.  Shows precisely where the positions are.
	private static void MakePositionObjects(List<int> positions) {
		foreach(GameObject g in positionObjects)
			GameObject.Destroy(g);
		positionObjects.Clear();
		while(positionObjects.Count < positions.Count) {
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			obj.renderer.material.color = Color.red;
			obj.name = "Position_" + positionObjects.Count;
			obj.transform.localScale = Conversions.ScaleObject_PixelsToWorld(5, 5, 1);
			//obj.transform.position = new Vector3(positionObjects.Count - 2, 0, .8f);//Conversions.PositionObject_PixelsToWorld(0, (float)positions[i], .8f);
			obj.transform.position = Conversions.PositionObject_PixelsToWorld(
				xPositions[positionObjects.Count], yPositions[positionObjects.Count], .8f);
			positionObjects.Add(obj);
		}
	}

	private static void PrintList(List<float> list, string message) {
		string output = "";
		foreach(float f in list)
			output += f + ", ";
		Debug.Log (message + output);
	}
}
