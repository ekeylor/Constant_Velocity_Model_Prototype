using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EngineManager : MonoBehaviour {

	//private float currentTime;
	List<int> positions = new List<int>();
	PositionGraph pg = new PositionGraph();
	MotionMap mm = new MotionMap();
	MotionMapSensor mms;

	//private GraphAxisLabel positionGraphCaptionText;
	//private GraphAxisLabel motionMapCaptionText;
	private GraphAxisLabel messageText;

	//private float adjustedPositionGraphYStart;
	//private float adjustedPositionGraphYAxisLength;
	//private float positionGraphMarkerX;

	MotionDetectorLabGUI motionDetectorLabGUI;
	MotionDetectorLabBehavior motionDetectorLabBehavior;

	private LerpStopWatch stopWatch;

	// Use this for initialization
	void Start () {
		stopWatch = (LerpStopWatch)GameObject.Find("LerpStopWatchClass").GetComponent("LerpStopWatch");
		mms = (MotionMapSensor)GameObject.Find("MotionMapSensorClass").GetComponent("MotionMapSensor");
		//positionGraphCaptionText = (GraphAxisLabel)GameObject.Find("PositionGraphCaptionText").GetComponent("GraphAxisLabel");
		//motionMapCaptionText = (GraphAxisLabel)GameObject.Find("MotionMapCaptionText").GetComponent("GraphAxisLabel");
		messageText = (GraphAxisLabel)GameObject.Find("MessageText").GetComponent("GraphAxisLabel");
		motionDetectorLabGUI = new MotionDetectorLabGUI();
		motionDetectorLabBehavior = (MotionDetectorLabBehavior)GameObject.Find("MotionDetectorLabBehaviorClass").GetComponent("MotionDetectorLabBehavior");
	}
	
	// Update is called once per frame
	void Update () {
		if(mms.CheckPlayerStarted()) {
			if(stopWatch.GetCurrentTime() == 0)
				stopWatch.TurnOn();
			motionDetectorLabGUI.UpdateAndCheckMarkers();
			/*
			 * AdjustPositionGraphMarker();

			//Debug.Log(pg.GetMarkerYInPixels());
			//currentTime += Time.deltaTime / 15.0f; // 15 seconds to cross position graph x axis
			positionGraphMarkerX = Mathf.Lerp(pg.GetXAxisStartInPixels(), pg.GetXAxisEndInPixels(), stopWatch.GetCurrentTime());//currentTime);// 
			pg.UpdateMarkerPosition(positionGraphMarkerX, pg.GetMarkerYInPixels());
			//Debug.Log(pg.IsMarkerOnPositionLine());
			if(pg.IsMarkerOnPositionLine())
				pg.MarkerInBounds();
			else
				pg.MarkerOutOfBounds();
				*/
		}
		/*
		if(!stopWatch.IsWatchRunning() && stopWatch.GetCurrentTime() != 0) {//currentTime >= 1.0f) { // 
			mms.TurnMarkerOff();
			pg.MarkerReset();
		}
		*/
	}

	public void MotionDetectorLab(List<string> directions) {
		//currentTime = 0;
		stopWatch.TurnOff();
		stopWatch.Set(0, 15f);

		positions = PositionMaker.GetPositions();
		motionDetectorLabGUI.Setup(pg, mm, mms, directions, positions);
		motionDetectorLabBehavior.Setup(pg, mms, directions, positions);
		motionDetectorLabGUI.AdjustMarkers();
		//SetMarkersPositivePositions(-1, 5, -1, 5);
		//AdjustPositionGraphMarker();

		messageText.SetLabel ("1.  GOAL:  Keep the circle on the line in the Position Graph!\n\n" +
			"The Position Graph shows change in position (vertical / up-down axis) over time (horizontal / left-right axis).\n\n" +
		    "2.  Find the Motion Map number line under the Position Graph.\n\n" +
		    "3.  Drag the circle on the Motion Map to graph the circle on the position graph.\n\n" +
			"REMEMBER!  Keep the position graph circle on the line!");
	}




}
