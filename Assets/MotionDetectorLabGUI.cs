using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionDetectorLabGUI  {

	// Use this for initialization
	public void Setup (PositionGraph pg, MotionMap mm, MotionMapSensor mms, List<string> directions) {
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
		//pg.TurnMarkerOn();
	}
}
