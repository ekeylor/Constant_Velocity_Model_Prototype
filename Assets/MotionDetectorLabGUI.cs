using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionDetectorLabGUI  {
	private GraphAxisLabel positionGraphCaptionText;
	private GraphAxisLabel motionMapCaptionText;

	PositionGraph pg;
	MotionMapSensor mms;
	MotionMap mm;
	List<int> positions;

	private LerpStopWatch stopWatch;

	private float adjustedPositionGraphYStart;
	private float adjustedPositionGraphYAxisLength;
	private float positionGraphMarkerX;

	// Use this for initialization
	public void Setup (PositionGraph pg, MotionMap mm, MotionMapSensor mms, List<string> directions, List<int> positions) {
		this.pg = pg;
		this.mms = mms;
		this.mm = mm;
		this.positions = positions;
		stopWatch = (LerpStopWatch)GameObject.Find("LerpStopWatchClass").GetComponent("LerpStopWatch");

		positionGraphCaptionText = (GraphAxisLabel)GameObject.Find("PositionGraphCaptionText").GetComponent("GraphAxisLabel");
		motionMapCaptionText = (GraphAxisLabel)GameObject.Find("MotionMapCaptionText").GetComponent("GraphAxisLabel");

		pg.Setup(new Rect(520, 100, InterfaceConstants.Get("position graph width"), InterfaceConstants.Get("position graph height")), Color.white);
		pg.MakeXAxisNumbers(0, 15);
		pg.MakeYAxisNumbers(0, 5);
		//PositionMaker.Make(Random.Range (0, MAX_POSITION_VALUE + 1), 
		//                   Random.Range (1, 4), 
		//                    Random.Range (0, MAX_POSITION_VALUE + 1),
		//                    Random.Range (0, MAX_POSITION_VALUE + 1));//3, Random.Range (1, 4), 3, 3);
		//PositionMaker.Make(directions, 0, InterfaceConstants.Get("max position value"));
		//positions = PositionMaker.GetPositions();
		//PositionObjectBuilder.Make(positions, InterfaceConstants.Get("max position value"), pg.GetXAxisStartInPixels(), pg.GetYAxisStartInPixels(), pg.GetXAxisLengthInPixels(), pg.GetYAxisLengthInPixels());
		// for debugging:  PositionMaker.ShowPositions();
		mm.Setup(new Rect(520, 400, InterfaceConstants.Get("motion map width"), InterfaceConstants.Get("motion map height")), Color.white);
		mm.MakeAxisNumbers(-1, 5, 3, 15);
		mms.SetDimensions(mm.GetXAxisStartInPixels(), mm.GetXAxisEndInPixels(), mm.GetBottomInPixels(), mm.GetTopInPixels());

		positionGraphCaptionText.SetLabel("Position Graph:");
		motionMapCaptionText.SetLabel("Motion Map:");
	}

	public void AdjustMarkers() {
		SetMarkersPositivePositions(-1, 5, -1, 5);
		AdjustPositionGraphMarker();
	}

	public void UpdateAndCheckMarkers() {
		AdjustPositionGraphMarker();
		
		//Debug.Log(pg.GetMarkerYInPixels());
		//currentTime += Time.deltaTime / 15.0f; // 15 seconds to cross position graph x axis
		positionGraphMarkerX = Mathf.Lerp(pg.GetXAxisStartInPixels(), pg.GetXAxisEndInPixels(), stopWatch.GetCurrentTime());//currentTime);// 
		pg.UpdateMarkerPosition(positionGraphMarkerX, pg.GetMarkerYInPixels());
		//Debug.Log(pg.IsMarkerOnPositionLine());
		if(pg.IsMarkerOnPositionLine())
			pg.MarkerInBounds();
		else
			pg.MarkerOutOfBounds();
	}
		
		/*  // Keep just in case
	private void AdjustPositionGraphMarker() {
		float mapMarkerRatio = (mms.GetMarkerXInPixels() - mm.GetXAxisStartInPixels())/mm.GetXAxisLengthInPixels();
		//Debug.Log(mapMarkerRatio * pg.GetYAxisLengthInPixels());
		//Debug.Log(pg.GetYAxisLengthInPixels());
		//Debug.Log(mms.GetMarkerXInPixels());
		float newYPosition = pg.GetYAxisStartInPixels() + mapMarkerRatio * pg.GetYAxisLengthInPixels();
		pg.UpdateMarkerPosition(pg.GetXAxisStartInPixels(), newYPosition);
	}
	*/
	
	private void AdjustPositionGraphMarker() {
		float mapMarkerRatio = (mms.GetMarkerXInPixels() - mm.GetXAxisStartInPixels())/mm.GetXAxisLengthInPixels();
		//Debug.Log(mapMarkerRatio * pg.GetYAxisLengthInPixels());
		//Debug.Log(pg.GetYAxisLengthInPixels());
		//Debug.Log(mms.GetMarkerXInPixels());
		float newYPosition = adjustedPositionGraphYStart + mapMarkerRatio * adjustedPositionGraphYAxisLength;
		pg.UpdateMarkerPosition(positionGraphMarkerX, newYPosition);//pg.GetXAxisStartInPixels(), newYPosition);
	}
	
	// Keep:  This might work for negative positions
	/*	private void SetMarkers(int minPositionGraphValue, int maxPositionGraphValue, int minMotionMapValue, int maxMotionMapValue) {
		float ratioForPositionGraphMarker = positions[0]/((float)maxPositionGraphValue - minPositionGraphValue);
		float rationForMotionMapMarker = positions[0]/((float)maxMotionMapValue - minMotionMapValue);
		float newPositionGraphMarkerY = pg.GetYAxisStartInPixels() + ratioForPositionGraphMarker * pg.GetYAxisLengthInPixels();
		float newMotionMapMarkerX = mm.GetXAxisStartInPixels() + ratioForPositionGraphMarker * mm.GetXAxisLengthInPixels();
		mms.SetMarkerPosition(newMotionMapMarkerX);
		pg.SetMarkerPosition(newPositionGraphMarkerY);
	}
*/
	// This should probably stay in the EngineGUIManager rather than a subclass.
	// This version allows the markers to go into negative values, but positions are always non-negative (include 0)
	private void SetMarkersPositivePositions(int minPositionGraphValue, int maxPositionGraphValue, int minMotionMapValue, int maxMotionMapValue) {
		// refactor
		float negativeDistance = 0;
		if(minPositionGraphValue < 0)
			negativeDistance = -minPositionGraphValue/(float)maxPositionGraphValue*pg.GetYAxisLengthInPixels();
		//Debug.Log (negativeDistance);
		adjustedPositionGraphYStart = pg.GetYAxisStartInPixels() - negativeDistance;
		adjustedPositionGraphYAxisLength = negativeDistance + pg.GetYAxisLengthInPixels();
		
		negativeDistance = 0;
		if(minMotionMapValue < 0)
			negativeDistance = -minMotionMapValue/(1.0f*maxMotionMapValue - minMotionMapValue)*mm.GetXAxisLengthInPixels();
		float adjustedMotionMapXStart = mm.GetXAxisStartInPixels() + negativeDistance;
		float adjustedMotionMapXAxisLength = mm.GetXAxisLengthInPixels() - negativeDistance;
		
		float localRatioMotionMapMarker = positions[0]/(1.0f*maxMotionMapValue);
		float newMotionMapMarkerX = adjustedMotionMapXStart + localRatioMotionMapMarker * adjustedMotionMapXAxisLength;
		
		
		float ratioForMotionMapMarker = (newMotionMapMarkerX - mm.GetXAxisStartInPixels())/mm.GetXAxisLengthInPixels();
		//Debug.Log(ratioForMotionMapMarker);
		float newPositionGraphMarkerY = adjustedPositionGraphYStart + ratioForMotionMapMarker * adjustedPositionGraphYAxisLength;
		
		mms.SetMarkerPosition(newMotionMapMarkerX);
		positionGraphMarkerX = pg.GetXAxisStartInPixels();
		pg.UpdateMarkerPosition(positionGraphMarkerX, newPositionGraphMarkerY);
		//pg.SetMarkerPosition(adjustedPositionGraphYStart);
	}
}
