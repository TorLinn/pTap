using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapController : MonoBehaviour
{
	public StateStorage stateStorageSO;
	public GameObject counterPrefab;
	public Button mainButton;
	public static Action TapAction;
	private List<PlantCounterDisplayer> plantCounters = new List<PlantCounterDisplayer>();
	public Transform counterParrent;
	private void Awake() {
		mainButton.onClick.AddListener(Tap);
	}

	private void OnEnable() {
		stateStorageSO.SetPlantCounterAction += SetPlantsCounters;
	}

	private void OnDisable() {
		stateStorageSO.SetPlantCounterAction -= SetPlantsCounters;
	}

	private void SetPlantsCounters() {
		var temp = stateStorageSO.plantCounters;
		
		foreach (var item in temp) {
		var tempCounter = plantCounters.Find(someCounter => someCounter.GetCounterType() == item.plantType);
			if (tempCounter == null) {
				var tempNewCounter = Instantiate(counterPrefab, counterParrent).GetComponent<PlantCounterDisplayer>();
				if (tempNewCounter != null) {
					plantCounters.Add(tempNewCounter);
					tempNewCounter.Init(item);
				}
			}
		}

		foreach (var counter in plantCounters) {
			var tempCount = temp.Find(someCounter => someCounter.plantType == counter.GetCounterType());
			if (tempCount != null) {
				counter.gameObject.SetActive(true);
				counter.SetCounter(tempCount.plantCount);
			}
			else {
				counter.gameObject.SetActive(false);
				counter.SetCounter(0);
			}
		}
	}

	private void Tap() {
		TapAction?.Invoke();
	}
}
