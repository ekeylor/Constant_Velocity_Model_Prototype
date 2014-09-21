using UnityEngine;
using System.Collections;


public class XAxis : GraphAxis {
	GameObject xAxis;
	GameObject xAxisArrowTop;
	GameObject xAxisArrowBottom;
	
	public void Make(float xStart, float xLength, float yStart, int AXIS_WIDTH, int ARROW_SIZE, int OFFSET, string name) {
		if(xAxis != null) {
			Object.Destroy(xAxis);
			Object.Destroy(xAxisArrowTop);
			Object.Destroy(xAxisArrowBottom);
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

	protected override void Initialize() {
		Init(ref xAxis);
		Init(ref xAxisArrowTop);
		Init(ref xAxisArrowBottom);
	}

	protected override void NameParts(string name) {
		xAxis.name = name + "_xAxis";
		xAxisArrowTop.name = name + "_xAxisArrowTop";
		xAxisArrowBottom.name = name + "_xAxisArrowBottom";
	}
}
