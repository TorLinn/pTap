using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StateStorage", fileName = "StateStorage")]
public class StateStorage : ScriptableObject
{
	public Action SetPlantCounterAction;
	public Action SetNewPlantAction;
	public List<PlantCounter> plantCounters = new List<PlantCounter>();
	public int errorsCounter = 0;
	public PlantType currentPlantType = PlantType.None;

	public void SetPlantCounter(List<PlantType> _plants) {
		plantCounters.Clear();

		foreach (PlantType plant in Enum.GetValues(typeof(PlantType))) {
			var temp = _plants.FindAll(somePlant => somePlant == plant);
			if (temp.Count > 0) {
				plantCounters.Add(new PlantCounter(plant, temp.Count));
			}
		}

		SetPlantCounterAction?.Invoke();
	}

	public void SetNewPlant(PlantType _plantType) {
		currentPlantType = _plantType;
		SetNewPlantAction?.Invoke();
	}
}

[Serializable]
public class PlantCounter 
{
	public PlantType plantType;
	public int plantCount;

	public PlantCounter(PlantType _plantType, int _plantCount) {
		plantType = _plantType;
		plantCount = _plantCount;
	}
}
