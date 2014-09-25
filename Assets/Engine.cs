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

	private const int POSITION_GRAPH_WIDTH = 500;
	private const int POSITION_GRAPH_HEIGHT = 250;
	private const int MAX_POSITION_VALUE = 5;
	private const int MOTION_MAP_WIDTH = 500;
	private const int MOTION_MAP_HEIGHT = 100;

	private bool positionGraphMarkerOnFlag;

	private float adjustedPositionGraphYStart;
	private float adjustedPositionGraphYAxisLength;
	private float positionGraphMarkerX;

	private float currentTime;

	private GraphAxisLabel positionGraphCaptionText;
	private GraphAxisLabel motionMapCaptionText;
	private GraphAxisLabel messageText;

	private EngineGUIManager guiManager;
	TutorialManager tutorialManager;

	// Use this for initialization
	void Start () {
		guiManager = (EngineGUIManager)GameObject.Find("EngineGUIManagerClass").GetComponent("EngineGUIManager");
		tutorialManager = new TutorialManager();
	}
	
	// Update is called once per frame
	void Update () {}

	void Test() {
		guiManager.MotionDetectorLab(tutorialManager.DirectionMaker());
	}
}
