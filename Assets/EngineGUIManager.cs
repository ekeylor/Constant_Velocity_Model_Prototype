using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EngineGUIManager : MonoBehaviour {

	private float currentTime;
	List<int> positions = new List<int>();
	PositionGraph pg = new PositionGraph();
	MotionMap mm = new MotionMap();
	MotionMapSensor mms;

	private const int POSITION_GRAPH_WIDTH = 500;
	private const int POSITION_GRAPH_HEIGHT = 250;
	private const int MAX_POSITION_VALUE = 5;
	private const int MOTION_MAP_WIDTH = 500;
	private const int MOTION_MAP_HEIGHT = 100;

	private bool positionGraphMarkerOnFlag;

	private GraphAxisLabel positionGraphCaptionText;
	private GraphAxisLabel motionMapCaptionText;
	private GraphAxisLabel messageText;

	private float adjustedPositionGraphYStart;
	private float adjustedPositionGraphYAxisLength;
	private float positionGraphMarkerX;

	MotionDetectorLabGUI motionDetectorLabGUI;

	// Use this for initialization
	void Start () {
		mms = (MotionMapSensor)GameObject.Find("MotionMapSensorClass").GetComponent("MotionMapSensor");
		positionGraphMarkerOnFlag = false;
		positionGraphCaptionText = (GraphAxisLabel)GameObject.Find("PositionGraphCaptionText").GetComponent("GraphAxisLabel");
		motionMapCaptionText = (GraphAxisLabel)GameObject.Find("MotionMapCaptionText").GetComponent("GraphAxisLabel");
		messageText = (GraphAxisLabel)GameObject.Find("MessageText").GetComponent("GraphAxisLabel");
		motionDetectorLabGUI = new MotionDetectorLabGUI();
	}
	
	// Update is called once per frame
	void Update () {
		if(positionGraphMarkerOnFlag)
			AdjustPositionGraphMarker();
		if(mms.CheckPlayerStarted()) {
			//Debug.Log(pg.GetMarkerYInPixels());
			currentTime += Time.deltaTime / 15.0f; // 15 seconds to cross position graph x axis
			positionGraphMarkerX = Mathf.Lerp(pg.GetXAxisStartInPixels(), pg.GetXAxisEndInPixels(), currentTime);
			pg.UpdateMarkerPosition(positionGraphMarkerX, pg.GetMarkerYInPixels());
			//Debug.Log(pg.IsMarkerOnPositionLine());
			if(pg.IsMarkerOnPositionLine())
				pg.MarkerInBounds();
			else
				pg.MarkerOutOfBounds();
		}
		if(currentTime >= 1.0f) {
			positionGraphMarkerOnFlag = false;
			//pg.TurnMarkerOff();
			mms.TurnMarkerOff();
			pg.MarkerReset();
		}
	}

	public void MotionDetectorLab(List<string> directions) {
		currentTime = 0;
		motionDetectorLabGUI.Setup(pg, mm, mms, directions);
		positions = PositionMaker.GetPositions();
		PositionMaker.Make(directions, 0, InterfaceConstants.Get("max position value"));
		PositionObjectBuilder.Make(positions, InterfaceConstants.Get("max position value"), pg.GetXAxisStartInPixels(), pg.GetYAxisStartInPixels(), pg.GetXAxisLengthInPixels(), pg.GetYAxisLengthInPixels());
		pg.TurnMarkerOn();
		mms.TurnMarkerOn();
		/*
		pg.Setup(new Rect(520, 100, POSITION_GRAPH_WIDTH, POSITION_GRAPH_HEIGHT), Color.white);
		pg.MakeXAxisNumbers(0, 15);
		pg.MakeYAxisNumbers(0, 5);
		//PositionMaker.Make(Random.Range (0, MAX_POSITION_VALUE + 1), 
		//                   Random.Range (1, 4), 
		//                    Random.Range (0, MAX_POSITION_VALUE + 1),
		//                    Random.Range (0, MAX_POSITION_VALUE + 1));//3, Random.Range (1, 4), 3, 3);
		PositionMaker.Make(directions, 0, MAX_POSITION_VALUE);
		positions = PositionMaker.GetPositions();
		PositionObjectBuilder.Make(positions, MAX_POSITION_VALUE, pg.GetXAxisStartInPixels(), pg.GetYAxisStartInPixels(), pg.GetXAxisLengthInPixels(), pg.GetYAxisLengthInPixels());
		// for debugging:  PositionMaker.ShowPositions();
		mm.Setup(new Rect(520, 400, MOTION_MAP_WIDTH, MOTION_MAP_HEIGHT), Color.white);
		mm.MakeAxisNumbers(-1, 5, 3, 15);
		mms.SetDimensions(mm.GetXAxisStartInPixels(), mm.GetXAxisEndInPixels(), mm.GetBottomInPixels(), mm.GetTopInPixels());
		pg.TurnMarkerOn();
		*/
		positionGraphMarkerOnFlag = true;
		SetMarkersPositivePositions(-1, 5, -1, 5);
		//SetMarkers(0, 5, 0, 5);
		positionGraphCaptionText.SetLabel("Position Graph:");
		motionMapCaptionText.SetLabel("Motion Map:");
		messageText.SetLabel ("Hold down and move the big dot on the MOTION MAP line so the dot on the position chart stays over the line.");
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
