using UnityEngine;
using System.Collections;

public class YAxis : GraphAxis {
	GameObject yAxis;
	GameObject yAxisArrowLeft;
	GameObject yAxisArrowRight;
	
	public void Make(float xStart, float xLength, float yStart, float yLength, int AXIS_WIDTH, int ARROW_SIZE, int OFFSET, string name) {
		if(yAxis != null) {
			Object.Destroy(yAxis);
			Object.Destroy(yAxisArrowLeft);
			Object.Destroy(yAxisArrowRight);
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

	protected override void Initialize() {
		Init(ref yAxis);
		Init(ref yAxisArrowLeft);
		Init(ref yAxisArrowRight);
	}

	protected override void NameParts(string name) {
		yAxis.name = name + "_yAxis";
		yAxisArrowLeft.name = name + "_yAxisArrowLeft";
		yAxisArrowRight.name = name + "_yAxisArrowRight";
	}
}
