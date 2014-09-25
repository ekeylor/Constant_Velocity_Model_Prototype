using UnityEngine;
using System.Collections;

public class MotionDetectorLabGUI  {

	// Use this for initialization
	public void Setup (PositionGraph pg, MotionMap mm, MotionMapSensor mms) {
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
	}
}
