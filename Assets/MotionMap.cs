using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//refactor with PositionGraph
public class MotionMap {
	private const int AXIS_WIDTH = 3;
	private const int OFFSET = 35;
	private const int ARROW_SIZE = 15;
	
	GameObject background;
	XAxis xAxis;

	//string xLabel;
	//string yLabel;
	float xStart;
	float xLength;
	float yMiddle;
	float yTop;
	float yBottom;
	//float yStart;
	//float yLength;

	List<GameObject> axisTicks;

	GameObject zeroText;
	GameObject maxText;
	GameObject minText;
	GraphAxisLabel label;

	//public void setup(int xAxisStart, int xAxisLength, int yAxisStart, int yAxisLength, Color bgColor, Rect bgDimensions) {
	public void Setup(Rect bgDimensions, Color bgColor) {//int xAxisStart, int yAxisStart, int xAxisLength, int yAxisLength, ) {
		MakeBackgroundRectangle(bgDimensions, bgColor);
		MakeGraphDimensions(bgDimensions);
		MakeXAxis();
		//MakeYAxis();
	}
	
	private void MakeGraphDimensions(Rect bgDimensions) {
		xStart = (int)bgDimensions.x + OFFSET;
		xLength = (int)bgDimensions.width - OFFSET*2;
		yMiddle = Screen.height - (int)bgDimensions.y - ((int)bgDimensions.yMax - (int)bgDimensions.y)/2.0f;
		yTop = Screen.height - (int)bgDimensions.y;
		yBottom = Screen.height - (int)bgDimensions.yMax;
		//yStart = Screen.height - (int)bgDimensions.yMax + OFFSET;//(int)bgDimensions.y + OFFSET*2;
		//yLength = (int)bgDimensions.height - OFFSET*2;
	}

	// refactor MakeXAxis and MakeYAxis
	private void MakeXAxis() {
		if(xAxis == null)
			xAxis = new XAxis();
		if(axisTicks == null)
			axisTicks = new List<GameObject>();
		while(axisTicks.Count > 0) {
			Object.Destroy(axisTicks[0]);
			axisTicks.RemoveAt(0);
		}
		xAxis.Make(xStart, xLength, yMiddle, AXIS_WIDTH, ARROW_SIZE, OFFSET, "MotionMap");
		//xAxis.SetLabel("PositionGraphXAxisLabel", "Time", new Vector3(xStart + xLength/2.0f, yStart - OFFSET/2, .9f));
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
		background.name = "MotionMap_background";
	}

	public void MakeAxisNumbers(int minNumber, int maxNumber, int AXIS_WIDTH, int ARROW_SIZE) {
		float xZero = 0;
		float xMaxText = 0;
		float xMinText = 0;

		int count = maxNumber - minNumber;
		while(axisTicks.Count < count) {
			GameObject tick = GameObject.CreatePrimitive(PrimitiveType.Cube);
			tick.renderer.material.color = Color.black;
			tick.transform.localScale = Conversions.ScaleObject_PixelsToWorld(AXIS_WIDTH, ARROW_SIZE, 1);
			tick.transform.position = Conversions.PositionObject_PixelsToWorld(
				xStart + (float)axisTicks.Count/(float)count * xLength, 
				yMiddle,
				0.9f);
			axisTicks.Add(tick);

			if(axisTicks.Count - 1 == 0 && minNumber != 0)
				xMinText = xStart + (float)(axisTicks.Count - 1)/(float)count * xLength;
			if(axisTicks.Count - 1 + minNumber == 0)
				xZero = xStart + (float)(axisTicks.Count - 1)/(float)count * xLength;
			if(count == axisTicks.Count)
				xMaxText = xStart + (float)axisTicks.Count/(float)count * xLength;
		}
		
		// REFACTOR THIS!!!
		minText = GameObject.Find("MotionMapMinText");
		label = (GraphAxisLabel)minText.GetComponent("GraphAxisLabel");
		if(minNumber != 0)
			label.SetLabel("" + minNumber);
		else
			label.SetLabel("");

		zeroText = GameObject.Find("MotionMapZeroText");
		label = (GraphAxisLabel)zeroText.GetComponent("GraphAxisLabel");
		label.SetLabel("0");
		
		maxText = GameObject.Find("MotionMapMaxText");
		label = (GraphAxisLabel)maxText.GetComponent("GraphAxisLabel");
		label.SetLabel("" + maxNumber);

		minText.transform.position = new Vector3(
			xMinText, 
			yMiddle - ARROW_SIZE,
			0);
		zeroText.transform.position = new Vector3(
			xZero, 
			yMiddle - ARROW_SIZE,
			0);
		maxText.transform.position = new Vector3(
			xMaxText, 
			yMiddle - ARROW_SIZE,
			0);
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

	public float GetTopInPixels() {
		return yTop;
	}

	public float GetBottomInPixels() {
		return yBottom;
	}
	/*
	public float GetYAxisStart() {
		return yStart;
	}
	
	public float GetYAxisEnd() {
		return yStart + yLength;
	}
	
	public float GetYAxisLength() {
		return yLength;
	}
	*/
}
