using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventStorage", fileName = "EventStorage")]
public class EventStorage : ScriptableObject
{
	public Action PlayAction;
	public Action ResumeLevelAction;

	public void StartPlay() {
		PlayAction?.Invoke();
	}

	public void ResumeLevel() {
		ResumeLevelAction?.Invoke();
	}
}
