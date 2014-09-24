using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YAxis : GraphAxis {
	GameObject yAxis;
	GameObject yAxisArrowLeft;
	GameObject yAxisArrowRight;

	List<GameObject> axisTicks;
	float axisLength;
	float axisStart;
	float xStart;

	GameObject zeroText;
	GameObject maxText;
	GraphAxisLabel label;


	public void Make(float xStart, float xLength, float yStart, float yLength, int AXIS_WIDTH, int ARROW_SIZE, int OFFSET, string name) {
		axisLength = yLength;
		axisStart = yStart;
		this.xStart = xStart;

		if(yAxis != null) {
			Object.Destroy(yAxis);
			Object.Destroy(yAxisArrowLeft);
			Object.Destroy(yAxisArrowRight);
		}
		while(axisTicks != null && axisTicks.Count > 0) {
			Object.Destroy(axisTicks[0]);
			axisTicks.RemoveAt(0);
		}

		Initialize();
		NameParts(name);

		yAxis.transform.localScale = Conversions.ScaleObject_PixelsToWorld(AXIS_WIDTH, yLength, 1);
		yAxisArrowLeft.transform.localScale = Conversions.ScaleObject_PixelsToWorld(ARROW_SIZE, AXIS_WIDTH, 1);
		yAxisArrowRight.transform.localScale = Conversions.ScaleObject_PixelsToWorld(ARROW_SIZE, AXIS_WIDTH, 1);
		
		yAxisArrowLeft.transform.RotateAround(yAxisArrowLeft.transform.right, Vector3.back, -45);//renderer.bounds.center, Vector3.back, 45);
		yAxisArrowRight.transform.RotateAround(yAxisArrowRight.transform.right, Vector3.back, 45);
		
		yAxis.transform.position = Conversions.PositionObject_PixelsToWorld(
			xStart + Conversions.UnitsToPixels(yAxis.renderer.bounds.size.x/2), 
			yStart + yLength/2, 
			0.9f);
		yAxisArrowLeft.transform.position = Conversions.PositionObject_PixelsToWorld(
			xStart - AXIS_WIDTH, 
			yStart + yLength - AXIS_WIDTH,
			0.9f);
		yAxisArrowRight.transform.position = Conversions.PositionObject_PixelsToWorld(
			xStart + Conversions.UnitsToPixels(yAxisArrowRight.renderer.bounds.size.x/2), 
			yStart + yLength - AXIS_WIDTH,
			0.9f);
	}

	public void MakeAxisNumbers(int minNumber, int maxNumber, int AXIS_WIDTH, int ARROW_SIZE) {
		float yZero = 0;
		float yMaxText = 0;

		int count = maxNumber - minNumber;
		while(axisTicks.Count < count) {
			GameObject tick = GameObject.CreatePrimitive(PrimitiveType.Cube);
			tick.renderer.material.color = Color.black;
			tick.transform.localScale = Conversions.ScaleObject_PixelsToWorld(ARROW_SIZE, AXIS_WIDTH, 1);
			tick.transform.position = Conversions.PositionObject_PixelsToWorld(
				xStart + .5f*AXIS_WIDTH,
				axisStart + (float)axisTicks.Count/(float)count * axisLength, 
				0.9f);
			axisTicks.Add(tick);

			if(axisTicks.Count - 1 + minNumber == 0)
				yZero = axisStart + (float)(axisTicks.Count - 1)/(float)count * axisLength;
			if(count == axisTicks.Count)
				yMaxText = axisStart + (float)axisTicks.Count/(float)count * axisLength;
		}
		// REFACTOR THIS!!!
		zeroText = GameObject.Find("PositionGraphYAxisZeroText");
		label = (GraphAxisLabel)zeroText.GetComponent("GraphAxisLabel");
		label.SetLabel("0");

		maxText = GameObject.Find("PositionGraphYAxisMaxText");
		label = (GraphAxisLabel)maxText.GetComponent("GraphAxisLabel");
		label.SetLabel("" + maxNumber);

		zeroText.transform.position = new Vector3(
			xStart - .25f*ARROW_SIZE,
			yZero, 
			0);
		maxText.transform.position = new Vector3(
			xStart - .25f*ARROW_SIZE,
			yMaxText, 
			0);
		//Debug.Log(GameObject.Find("PositionGraphYAxisMaxText").GetType());

		//(GraphAxisLabel)GameObject.Find("PositionGraphYAxisZeroText").GetComponent("GraphAxisLabel").SetLabel("0");
		//(GraphAxisLabel)GameObject.Find("PositionGraphYAxisMaxText").GetComponent("GraphAxisLabel").SetLabel("" + maxNumber);

			
	}

	protected override void Initialize() {
		Init(ref yAxis);
		Init(ref yAxisArrowLeft);
		Init(ref yAxisArrowRight);
		if(axisTicks == null)
			axisTicks = new List<GameObject>();
	}

	protected override void NameParts(string name) {
		yAxis.name = name + "_yAxis";
		yAxisArrowLeft.name = name + "_yAxisArrowLeft";
		yAxisArrowRight.name = name + "_yAxisArrowRight";
	}
}
