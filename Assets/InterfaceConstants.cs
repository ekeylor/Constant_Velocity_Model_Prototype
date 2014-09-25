using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InterfaceConstants {
	private static Dictionary<string, int> constants;
	// might need a dictionary for floats

	private const int POSITION_GRAPH_WIDTH = 500;
	private const int POSITION_GRAPH_HEIGHT = 250;
	private const int MAX_POSITION_VALUE = 5;
	private const int MOTION_MAP_WIDTH = 500;
	private const int MOTION_MAP_HEIGHT = 100;

	public static void Initialize() {
		constants = new Dictionary<string, int>(){
			{"position graph width", POSITION_GRAPH_WIDTH},
			{"position graph height", POSITION_GRAPH_HEIGHT},
			{"max position value", MAX_POSITION_VALUE},
			{"motion map width", MOTION_MAP_WIDTH},
			{"motion map height", MOTION_MAP_HEIGHT}
		};
	}

	public static int Get(string variable) {
		if(constants == null)
			Initialize();
		if(constants.ContainsKey(variable))
			return constants[variable];
		Debug.Log("InterfaceConstants:  Get:  The following variable, " + variable + ", is not in the dictionary.");
		return constants[variable];
	}
}
