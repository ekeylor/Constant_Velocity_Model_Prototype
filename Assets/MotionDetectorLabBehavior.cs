using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionDetectorLabBehavior : MonoBehaviour {
	private float currentTime;
	private GraphAxisLabel messageText;
	//List<int> positions = new List<int>();
	PositionGraph pg;
	MotionMapSensor mms;

	private LerpStopWatch stopWatch;

	// Use this for initialization
	void Start () {
		stopWatch = (LerpStopWatch)GameObject.Find("LerpStopWatchClass").GetComponent("LerpStopWatch");

	}
	
	// Update is called once per frame
	void Update () {
		if(!stopWatch.IsWatchRunning() && stopWatch.GetCurrentTime() != 0) {//currentTime >= 1.0f) { // 
			mms.TurnMarkerOff();
			pg.MarkerReset();
		}
	}

	public void Setup(PositionGraph pg, MotionMapSensor mms, List<string> directions, List<int> positions) {
		this.pg = pg;
		this.mms = mms;

		MakePositions(pg, directions, positions);
		TurnOn(pg, mms);
	}

	private void MakePositions(PositionGraph pg, List<string> directions, List<int> positions) {
		//positions = PositionMaker.GetPositions();
		PositionMaker.Make(directions, 0, InterfaceConstants.Get("max position value"));
		PositionObjectBuilder.Make(positions, InterfaceConstants.Get("max position value"), pg.GetXAxisStartInPixels(), pg.GetYAxisStartInPixels(), pg.GetXAxisLengthInPixels(), pg.GetYAxisLengthInPixels());
	}
	
	private void TurnOn(PositionGraph pg, MotionMapSensor mms) {
		pg.TurnMarkerOn();
		mms.TurnMarkerOn();
	}
}
