using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// USEFUL:  var worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
// USEFUL:  var pixelsToWorld = (Camera.main.orthographicSize / (Screen.height / 2.0f));

public class Engine : MonoBehaviour {
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

	private float adjustedPositionGraphYStart;
	private float adjustedPositionGraphYAxisLength;
	private float positionGraphMarkerX;

	private float currentTime;

	// Use this for initialization
	void Start () {
		mms = (MotionMapSensor)GameObject.Find("MotionMapSensorClass").GetComponent("MotionMapSensor");
		positionGraphMarkerOnFlag = false;
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
			if(pg.IsMarkerOnPositionLine())
				pg.MarkerInBounds();
			else
				pg.MarkerOutOfBounds();
		}
	}

	void Test() {
		currentTime = 0;
		pg.Setup(new Rect(5, 100, POSITION_GRAPH_WIDTH, POSITION_GRAPH_HEIGHT), Color.white);
		//PositionMaker.Make(Random.Range (0, MAX_POSITION_VALUE + 1), 
		//                   Random.Range (1, 4), 
		//                    Random.Range (0, MAX_POSITION_VALUE + 1),
		//                    Random.Range (0, MAX_POSITION_VALUE + 1));//3, Random.Range (1, 4), 3, 3);
		PositionMaker.Make(DirectionMaker(), 0, MAX_POSITION_VALUE);
		positions = PositionMaker.GetPositions();
		PositionObjectBuilder.Make(positions, MAX_POSITION_VALUE, pg.GetXAxisStartInPixels(), pg.GetYAxisStartInPixels(), pg.GetXAxisLengthInPixels(), pg.GetYAxisLengthInPixels());
		// for debugging:  showPositions();
		mm.Setup(new Rect(520, 100, MOTION_MAP_WIDTH, MOTION_MAP_HEIGHT), Color.white);
		mms.SetDimensions(mm.GetXAxisStartInPixels(), mm.GetXAxisEndInPixels(), mm.GetBottomInPixels(), mm.GetTopInPixels());
		pg.TurnMarkerOn();
		positionGraphMarkerOnFlag = true;
		SetMarkersPositivePositions(-1, 5, -1, 5);
		//SetMarkers(0, 5, 0, 5);
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


	private List<string> DirectionMaker() {
		int numDirections = Random.Range(1, 4);
		List<string> directions = new List<string>();
		int choice = 0;
		while(directions.Count < numDirections) {
			if(directions.Count > 0 && directions[directions.Count - 1].Equals("constant"))
				choice = Random.Range(0, 2);
			else
				choice = Random.Range(0, 3);
			if(choice == 0)
				directions.Add("left");
			else if(choice == 1)
				directions.Add("right");
			else
				directions.Add("constant");
		}

		//directions.Clear();
		//directions.Add("left");
		//directions.Add("right");

		//string output = "";
		//foreach(string s in directions)
		//	output += s + "  ";
		//Debug.Log("Directions:  " + output);


		return directions;
	}

	private void showPositions() {
		string output = "";
		foreach(int t in positions)
			output += t + ", ";
		Debug.Log("Positions:  " + output);
	}
}
