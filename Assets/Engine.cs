using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// USEFUL:  var worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
// USEFUL:  var pixelsToWorld = (Camera.main.orthographicSize / (Screen.height / 2.0f));

public class Engine : MonoBehaviour {
	//List<int> positions = new List<int>();
	//PositionGraph pg = new PositionGraph();
	//MotionMap mm = new MotionMap();
	MotionMapSensor mms;

	private bool positionGraphMarkerOnFlag;

	private float adjustedPositionGraphYStart;
	private float adjustedPositionGraphYAxisLength;
	private float positionGraphMarkerX;

	private float currentTime;

	private GraphAxisLabel positionGraphCaptionText;
	private GraphAxisLabel motionMapCaptionText;
	private GraphAxisLabel messageText;

	private EngineManager engineManager;
	TutorialManager tutorialManager;
	MotionDetectorLabBehavior motionDetectorLabBehavior;

	// Use this for initialization
	void Start () {
		engineManager = (EngineManager)GameObject.Find("EngineManagerClass").GetComponent("EngineManager");
		tutorialManager = new TutorialManager();
	}
	
	// Update is called once per frame
	void Update () {}

	void Test() {
		engineManager.MotionDetectorLab(tutorialManager.DirectionMaker());
	}
}
