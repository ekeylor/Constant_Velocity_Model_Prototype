using UnityEngine;
using System.Collections;

public class PositionGraph {
	private const int AXIS_WIDTH = 3;
	private const int OFFSET = 35;
	private const int ARROW_SIZE = 15;

	GameObject background;
	XAxis xAxis;
	YAxis yAxis;

	//string xLabel;
	//string yLabel;
	float xStart;
	float xLength;
	float yStart;
	float yLength;

	GameObject marker;
	MarkerCollision markerCollisionTracker;

	//public void setup(int xAxisStart, int xAxisLength, int yAxisStart, int yAxisLength, Color bgColor, Rect bgDimensions) {
	public void Setup(Rect bgDimensions, Color bgColor) {//int xAxisStart, int yAxisStart, int xAxisLength, int yAxisLength, ) {
		MakeBackgroundRectangle(bgDimensions, bgColor);
		MakeGraphDimensions(bgDimensions);
		MakeXAxis();
		MakeYAxis();
	}

	private void MakeGraphDimensions(Rect bgDimensions) {
		xStart = (int)bgDimensions.x + OFFSET;
		xLength = (int)bgDimensions.width - OFFSET*2;
		yStart = Screen.height - (int)bgDimensions.yMax + OFFSET;//(int)bgDimensions.y + OFFSET*2;
		yLength = (int)bgDimensions.height - OFFSET*2;
	}

	// refactor MakeXAxis and MakeYAxis
	private void MakeXAxis() {
		if(xAxis == null)
			xAxis = new XAxis();
		//xAxis.Make(xStart, xLength, yStart, yLength, AXIS_WIDTH, ARROW_SIZE, OFFSET);
		xAxis.Make(xStart, xLength, yStart, AXIS_WIDTH, ARROW_SIZE, OFFSET, "PositionGraph");
		xAxis.SetLabel("PositionGraphXAxisLabel", "Time", new Vector3(xStart + xLength/2.0f, yStart - OFFSET/2, .9f));
	}

	// refactor:  can probably get rid of xLength parameter for yAxis, compare to xAxis
	private void MakeYAxis() {
		if(yAxis == null)
			yAxis = new YAxis();
		yAxis.Make(xStart, xLength, yStart, yLength, AXIS_WIDTH, ARROW_SIZE, OFFSET, "PositionGraph");
		yAxis.SetLabel("PositionGraphYAxisLabel", "Position", new Vector3(xStart - OFFSET/2, yStart + yLength/2.0f, .9f));
	}

	private void MakeBackgroundRectangle(Rect bgDimensions, Color bgColor) {
		if(background == null)
			background = GameObject.CreatePrimitive(PrimitiveType.Cube);
		background.renderer.material.color = bgColor;
		background.transform.localScale = Conversions.ScaleObject_PixelsToWorld(bgDimensions.width, bgDimensions.height, 3);
		background.transform.position = Conversions.PositionObject_PixelsToWorld(
			bgDimensions.x + Conversions.UnitsToPixels(background.renderer.bounds.size.x/2), 
			Screen.height - bgDimensions.y - Conversions.UnitsToPixels(background.renderer.bounds.size.y/2), 
		    1);
		background.name = "PositionGraph_background";
	}

	public float GetXAxisStartInPixels() {
		return xStart;
	}

	public float GetXAxisEndInPixels() {
		return xStart + xLength;
	}

	public float GetXAxisLengthInPixels() {
		return xLength;
	}

	public float GetYAxisStartInPixels() {
		return yStart;
	}

	public float GetYAxisEndInPixels() {
		return yStart + yLength;
	}

	public float GetYAxisLengthInPixels() {
		return yLength;
	}

	public void TurnMarkerOn() {
		//markerOnFlag = true;
		if(marker == null)
			MakeMarker();
		marker.renderer.enabled = true;
		//UpdateMarkerPosition();
	}
	
	public void TurnMarkerOff() {
		//markerOnFlag = false;
		marker.renderer.enabled = false;
	}
	
	public void UpdateMarkerPosition(float x, float y) {
		marker.transform.position = Conversions.PositionObject_PixelsToWorld(x, y, 1);
	}
	
	private void MakeMarker() {
		if(marker == null) {
			marker = (GameObject)MonoBehaviour.Instantiate(Resources.Load("MarkerPrefab"));//GameObject.CreatePrimitive(PrimitiveType.Sphere);
			markerCollisionTracker = (MarkerCollision)marker.GetComponent("MarkerCollision");
		}
		marker.transform.localScale = new Vector3(.2f, .2f, .2f);
		marker.renderer.material.color = Color.green;
		marker.transform.position = Conversions.PositionObject_PixelsToWorld(xStart, yStart, 1);
	}

	public float GetMarkerYInPixels() {
		return Camera.main.WorldToScreenPoint(marker.transform.position).y;//Conversions.UnitsToPixels(marker.transform.position.y);
	}

	public void MarkerOutOfBounds() {
		marker.renderer.material.color = Color.red;
	}

	public void MarkerInBounds() {
		marker.renderer.material.color = Color.green;
	}

	public bool IsMarkerOnPositionLine() {
		return markerCollisionTracker.IsMarkerOnPositionLine();
	}

	public void MakeXAxisNumbers(int minNumber, int maxNumber) {
		xAxis.MakeAxisNumbers(minNumber, maxNumber, AXIS_WIDTH, ARROW_SIZE);
	}

	public void MakeYAxisNumbers(int minNumber, int maxNumber) {
		yAxis.MakeAxisNumbers(minNumber, maxNumber, AXIS_WIDTH, ARROW_SIZE);
	}
}
