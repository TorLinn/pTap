using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
	private bool isPaused = false;

	public void SetPauseMode() {
		if (isPaused) {
			isPaused = false;
			Time.timeScale = 1f;
		}
		else {
			isPaused = true;
			Time.timeScale = 0f;
		}
	}
}
