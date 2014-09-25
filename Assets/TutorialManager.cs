using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager {

	public List<string> DirectionMaker() {
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
}
