using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XAxis : GraphAxis {
	GameObject xAxis;
	GameObject xAxisArrowTop;
	GameObject xAxisArrowBottom;
	List<GameObject> axisTicks;
	float axisLength;
	float axisStart;
	float yStart;

	GameObject zeroText;
	GameObject maxText;
	GraphAxisLabel label;

	public void Make(float xStart, float xLength, float yStart, int AXIS_WIDTH, int ARROW_SIZE, int OFFSET, string name) {
		axisLength = xLength;
		axisStart = xStart;
		this.yStart = yStart;

		if(xAxis != null) {
			Object.Destroy(xAxis);
			Object.Destroy(xAxisArrowTop);
			Object.Destroy(xAxisArrowBottom);
		}
		while(axisTicks != null && axisTicks.Count > 0) {
			Object.Destroy(axisTicks[0]);
			axisTicks.RemoveAt(0);
		}
		Initialize();
		NameParts(name);


		xAxis.transform.localScale = Conversions.ScaleObject_PixelsToWorld(xLength, AXIS_WIDTH, 1);
		xAxisArrowTop.transform.localScale = Conversions.ScaleObject_PixelsToWorld(ARROW_SIZE, AXIS_WIDTH, 1);
		xAxisArrowBottom.transform.localScale = Conversions.ScaleObject_PixelsToWorld(ARROW_SIZE, AXIS_WIDTH, 1);
		
		xAxisArrowTop.transform.RotateAround(xAxisArrowTop.transform.right, Vector3.back, 45);//renderer.bounds.center, Vector3.back, 45);
		xAxisArrowBottom.transform.RotateAround(xAxisArrowBottom.transform.right, Vector3.back, -45);
		
		xAxis.transform.position = Conversions.PositionObject_PixelsToWorld(
			xStart + Conversions.UnitsToPixels(xAxis.renderer.bounds.size.x/2), 
			yStart,
			0.9f);
		
		xAxisArrowTop.transform.position = Conversions.PositionObject_PixelsToWorld(
			xStart + xLength - Conversions.UnitsToPixels(xAxisArrowTop.renderer.bounds.size.x/2) + AXIS_WIDTH, 
			yStart + Conversions.UnitsToPixels(xAxisArrowTop.renderer.bounds.size.y/2) - AXIS_WIDTH/2.0f,
			0.9f);
		xAxisArrowBottom.transform.position = Conversions.PositionObject_PixelsToWorld(
			xStart + xLength - Conversions.UnitsToPixels(xAxisArrowBottom.renderer.bounds.size.x/2) + AXIS_WIDTH, 
			yStart - Conversions.UnitsToPixels(xAxisArrowBottom.renderer.bounds.size.y/2) + AXIS_WIDTH/2.0f,
			0.9f);
	}

	public void MakeAxisNumbers(int minNumber, int maxNumber, int AXIS_WIDTH, int ARROW_SIZE) {
		float xZero = 0;
		float xMaxText = 0;

		int count = maxNumber - minNumber;
		while(axisTicks.Count < count) {
			GameObject tick = GameObject.CreatePrimitive(PrimitiveType.Cube);
			tick.renderer.material.color = Color.black;
			tick.transform.localScale = Conversions.ScaleObject_PixelsToWorld(AXIS_WIDTH, ARROW_SIZE, 1);
			tick.transform.position = Conversions.PositionObject_PixelsToWorld(
				axisStart + .5f*AXIS_WIDTH + (float)axisTicks.Count/(float)count * axisLength, 
				yStart,
				0.9f);
			axisTicks.Add(tick);

			if(axisTicks.Count - 1 + minNumber == 0)
				xZero = axisStart + (float)(axisTicks.Count - 1)/(float)count * axisLength;
			if(count == axisTicks.Count)
				xMaxText = axisStart + (float)axisTicks.Count/(float)count * axisLength;
		}

		// REFACTOR THIS!!!
		zeroText = GameObject.Find("PositionGraphXAxisZeroText");
		label = (GraphAxisLabel)zeroText.GetComponent("GraphAxisLabel");
		label.SetLabel("0");
		
		maxText = GameObject.Find("PositionGraphXAxisMaxText");
		label = (GraphAxisLabel)maxText.GetComponent("GraphAxisLabel");
		label.SetLabel("" + maxNumber);
		
		zeroText.transform.position = new Vector3(
			xZero, 
			yStart - .5f*ARROW_SIZE,
			0);
		maxText.transform.position = new Vector3(
			xMaxText, 
			yStart - .5f*ARROW_SIZE,
			0);
	}

	protected override void Initialize() {
		Init(ref xAxis);
		Init(ref xAxisArrowTop);
		Init(ref xAxisArrowBottom);
		if(axisTicks == null)
			axisTicks = new List<GameObject>();
	}

	protected override void NameParts(string name) {
		xAxis.name = name + "_xAxis";
		xAxisArrowTop.name = name + "_xAxisArrowTop";
		xAxisArrowBottom.name = name + "_xAxisArrowBottom";
	}
}
