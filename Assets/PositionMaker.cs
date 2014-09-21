using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Need to test:
// left, right, right, positions:  5, 4, 5, 6
// left, right, right, positions:  ? argument out of bounds exception (line 174)
// left, right, right, ditto

public class PositionMaker {
	private static List<int> positions = new List<int>();
	private static List<int> maxPositionLimits = new List<int>();
	private static List<int> minPositionLimits = new List<int>();
	// For constant position:  Set numberNewPositions = 1 and make min and max equal to startPosition

	public static void Make(int startPosition, int numberNewPositions, int minPositionValue, int maxPositionValue) {
		positions.Clear();
		positions.Add(startPosition);
		while(positions.Count  - 1 < numberNewPositions)
			positions.Add(Random.Range(minPositionValue, maxPositionValue + 1));
	}

	public static void Make(List<string> directions, int minPositionValue, int maxPositionValue) {
		CreatePositionLimits(directions, minPositionValue, maxPositionValue);
		CreatePositions(directions);
	}

	public static int GetNumberPositions() {
		return positions.Count;
	}

	public static List<int> GetPositions() {
		return positions;
	}

	public static void Intelligence() {
		// Constraints:
		//	don't repeat the same position sequence from problem to problem
		//	don't have two adjacent equal changes:  up-up should not produce 1-2-3, but 1-2-4, 1-3-4 are OK
		//	
		// keep track of previous position(s)
		// can input left, constant, right
		// constant is easiest
		// 
		// algorithm:
		//		create a list of minPositionValue and maxPositionValue
		//		scan the direction sequence
		//			for right
		//				if numRight = 1, add 1
		//				else
		//					use maxPositionValue = numRight + numRight/2 (int div)
		//			for left
		//				if numLeft = 1, add 1
		//				else
		//					use minPositionValue = numLeft + numLeft/2 (int div)

	}

	// can refactor this, especially the else statements
	private static void CreatePositions(List<string> directions) {
		positions.Clear();
		positions.Add(ChoosePosition(minPositionLimits[positions.Count], maxPositionLimits[positions.Count]));
		while(positions.Count - 1 < directions.Count) {
			//if(directions[positions.Count -1 ].Equals("constant") || directions[positions.Count -1 ].Equals("c"))
			//	positions.Add(positions[positions.Count - 1]);
			//else if(directions[positions.Count -1 ].Equals("left") || directions[positions.Count -1 ].Equals("l"))
			//	positions.Add(ChoosePosition(minPositionLimits[positions.Count], positions[positions.Count - 1] - 1));
			//else
			//	positions.Add(ChoosePosition(positions[positions.Count - 1] + 1, maxPositionLimits[positions.Count]));

			if(directions[positions.Count -1 ].Equals("constant") || directions[positions.Count -1 ].Equals("c"))
				positions.Add(positions[positions.Count - 1]);
			else if(directions[positions.Count -1 ].Equals("left") || directions[positions.Count -1 ].Equals("l"))
				if(positions.Count == 1) 
					positions.Add(ChoosePosition(
						minPositionLimits[positions.Count], 
						ChooseTheLeastPosition(
							positions[positions.Count - 1] - 1, 
							maxPositionLimits[positions.Count])));
				else
					positions.Add(ChoosePosition(
						positions[positions.Count - 2],
						positions[positions.Count - 1],
						minPositionLimits[positions.Count], 
						positions[positions.Count - 1] - 1));
			else
				if(positions.Count == 1) 
					positions.Add(ChoosePosition(
						ChooseTheGreatestPosition(
							positions[positions.Count - 1] + 1, 
							minPositionLimits[positions.Count]),
						maxPositionLimits[positions.Count]));
				else
					positions.Add(ChoosePosition(
						positions[positions.Count - 2],
						positions[positions.Count - 1],
						positions[positions.Count - 1] + 1, 
						maxPositionLimits[positions.Count]));
		}
	}

	// this is OK for random, but may contain repetitions
	private static void CreatePositions() {
		positions.Clear();
		while(positions.Count < maxPositionLimits.Count)
			positions.Add(ChoosePosition(minPositionLimits[positions.Count], maxPositionLimits[positions.Count]));
	}

	// This helps prevent duplicating the same distance, e.g. if given r-r-r
	//		1-2-1 is OK but 1-1-2 and 2-1-1 are not 
	private static void CreatePositionLimits(List<string> directions, int minPositionValue, int maxPositionValue) {
		minPositionLimits.Clear();
		maxPositionLimits.Clear();
		minPositionLimits.Add(0);
		maxPositionLimits.Add(maxPositionValue);
		int rightCounter = 0;
		int leftCounter = 0;
		int currentMax = maxPositionValue;
		int currentMin = minPositionValue;
		List<string> directionsReversed = new List<string>(directions);
		directionsReversed.Reverse();
		foreach(string direction in directionsReversed) {
			if(direction.Equals("left") || direction.Equals("l")) {
				rightCounter = 0;
				currentMax = maxPositionValue;
				//maxPositionLimits.Add(currentMax);

				currentMin += (++leftCounter % 2 == 1) ? 1 : 2;
				//minPositionLimits.Add(currentMin);
			}
			else if(direction.Equals("right") || direction.Equals("r")) {
				currentMax -= (++rightCounter % 2 == 1) ? 1 : 2;
				//maxPositionLimits.Add(currentMax);

				leftCounter = 0;
				currentMin = minPositionValue;
				//minPositionLimits.Add(currentMin);
			} else {
				if(rightCounter > 0)
					rightCounter--;
				if(leftCounter > 0)
					leftCounter--;
			}
			maxPositionLimits.Add(currentMax);
			minPositionLimits.Add(currentMin);
		}
		maxPositionLimits.Reverse();
		minPositionLimits.Reverse();
		rightCounter = 0;
		leftCounter = 0;
		for(int i = 0; i < directions.Count; i++) {
			if(directions[i].Equals("left") || directions[i].Equals("l")) {
				maxPositionLimits[i + 1] -= (++leftCounter % 2 == 1) ? 1 : 2;

				rightCounter = 0;
			}
			else if(directions[i].Equals("right") || directions[i].Equals("r")) {
				leftCounter = 0;

				minPositionLimits[i + 1] += (++rightCounter % 2 == 1) ? 1 : 2;
			}
			else {
				if(rightCounter > 0)
					rightCounter--;
				if(leftCounter > 0)
					leftCounter--;
			}
		}
	}


	private static int ChoosePosition(int minPositionValue, int maxPositionValue) {
		return Random.Range(minPositionValue, maxPositionValue + 1);
	}

	private static int ChooseTheLeastPosition(int pos1, int pos2) {
		if(pos1 < pos2)
			return pos1;
		return pos2;
	}

	private static int ChooseTheGreatestPosition(int pos1, int pos2) {
		if(pos1 > pos2)
			return pos1;
		return pos2;
	}

	private static int ChoosePosition(int previousPosition, int currentPosition, int minPositionValue, int maxPositionValue) {
		List<int> possiblePositions = new List<int>();
		for(int i = minPositionValue; i <= maxPositionValue; i++)
			possiblePositions.Add(i);
		possiblePositions.Remove(currentPosition - (previousPosition - currentPosition)); // *** this is were the exception is thrown
		//Debug.Log("previous position:  " + previousPosition);
		//Debug.Log("current position:  " + currentPosition);
		//Debug.Log("c - (p - c):  " + (currentPosition - (previousPosition - currentPosition)));

		string output = "";
		foreach(int s in possiblePositions)
			output += s + "  ";
		//Debug.Log("Possible positions:  " + output);
		//output = "";
		//foreach(int s in minPositionLimits)
		//	output += s + "  ";
		//Debug.Log("Min limits:  " + output);
		//output = "";
		//foreach(int s in maxPositionLimits)
		//	output += s + "  ";
		//Debug.Log("Max limits:  " + output);
		return possiblePositions[Random.Range(0, possiblePositions.Count)];
	}
}

/*
	// max and min can be refactored
	private static void CreateMaxPositionLimits(List<string> directions, int maxPositionValue) {
		maxPositionLimits.Clear();
		int max = maxPositionValue;
		int counter = 0;
		foreach(string s in directions) {
			if(s.Equals("right") || s.Equals("r"))
				max -= (++counter % 2 == 1) ? 1 : 2;
			maxPositionLimits.Add(max);
		}
		maxPositionLimits.Reverse();
	}

	private static void CreateMinPositionLimits(List<string> directions, int minPositionValue) {
		minPositionLimits.Clear();
		int min = minPositionValue;
		int counter = 0;
		foreach(string s in directions) {
			if(s.Equals("left") || s.Equals("l"))
				min += (++counter % 2 == 1) ? 1 : 2;
			minPositionLimits.Add(min);
		}
		minPositionLimits.Reverse();
	}
*/
