using UnityEngine;
using System.Collections;

public class LerpStopWatch : MonoBehaviour {
	private float currentTime;
	private float duration;
	private bool watchOnFlag;

	// Use this for initialization
	void Start () {
		currentTime = 0;
		watchOnFlag = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!watchOnFlag)
			return;

		currentTime += Time.deltaTime / duration;

		if(currentTime >= 1.0f)
			watchOnFlag = false;	
	}

	public float GetCurrentTime() {
		return currentTime;
	}

	public void Set(float startTime, float totalDuration) {
		currentTime = startTime;
		duration = totalDuration;
	}

	public void TurnOn() {
		watchOnFlag = true;
	}

	public void TurnOff() {
		watchOnFlag = false;
	}

	public bool IsWatchRunning() {
		return watchOnFlag;
	}
}
