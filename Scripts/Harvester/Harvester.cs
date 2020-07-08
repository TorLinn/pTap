using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Harvester : MonoBehaviour {
	public SoundEvent soundEventSO;
	public PopupTextStorage popupTextStorageSO;
	public PlantTypeStorage plantTypeStorageSO;
	public StateStorage stateStorageSO;
	public NavMeshAgent navMeshAgent;
	public GameObject plantPrefab;
	private List<PlantController> plantControllers = new List<PlantController>();
	[SerializeField]
	private List<PlantType> plants = new List<PlantType>();
	[SerializeField]
	private PlantType currentPlant = PlantType.None;
	private Material material;
	public TrailRenderer trailRenderer;

	private bool forwardRun = true;
	public GameField concreteGameField;
	private int currentPosition = 0;
	private List<FieldPointController> fieldPoints = new List<FieldPointController>();

	private bool trailIsShow = false;
	private bool levelStarted = false;
    public Transform plantTransform;

	private void Awake() {
		PreparePlants();
		material = plantTransform.GetComponent<MeshRenderer>().material;
		trailRenderer.enabled = false;
	}

	private void OnEnable() {
		TapController.TapAction += Tap;
		FieldController.LevelCompliteAction += DisableRun;
		FieldController.LevelPauseAction += DisableRun;
	}

	private void OnDisable() {
		TapController.TapAction -= Tap;
		FieldController.LevelCompliteAction -= DisableRun;
		FieldController.LevelPauseAction -= DisableRun;
	}

	private void DisableRun() {
		levelStarted = false;
	}

	private void PreparePlants() {
		foreach (PlantType plant in Enum.GetValues(typeof(PlantType))) {
			plantControllers.Add(Instantiate(plantPrefab, transform.parent).GetComponent<PlantController>());
			plantControllers[plantControllers.Count - 1].InitPlant(PlantType.None);
		}
	}

	private void Tap() {
		if (!trailIsShow) {
			if (plants.Count > 0) {
				var temp2 = plantControllers.Find(somePlant => !somePlant.isActive);

				if (temp2 == null) {
					plantControllers.Add(Instantiate(plantPrefab, transform.parent).GetComponent<PlantController>());
					temp2 = plantControllers.Find(somePlant => !somePlant.isActive);
				}
				temp2.InitPlant(currentPlant);
				if (temp2 != null) {
					soundEventSO.SomeSoundPlay(SoundType.ScreenTap);
					temp2.Activate(transform.position);
				}

				ReloadPlant();
			}
		}
		else {
			SetHatvesterToStartPoint();
		}
	}

	public void AddPlants(PlantType _plantType, int _plantCount) {
		for (int i = 0; i < _plantCount; i++) {
			plants.Add(_plantType);
		}
		stateStorageSO.SetPlantCounter(plants);
	}

	public void CorrectPlants() {
		LoadPlant();
		stateStorageSO.SetPlantCounter(plants);
	}

	public void ResumeLevel() {
		AddResumedLevelPlants();
		stateStorageSO.SetPlantCounter(plants);
		levelStarted = true;
	}

	public void PrepareHarvester(GameField _gameField, List<FieldPointController> _fieldPoints) {
		concreteGameField = _gameField;
		fieldPoints = _fieldPoints;
		navMeshAgent.speed = _gameField.harvesterSpeed * 2;
		currentPosition = 0;
		forwardRun = true;
		var temp1 = fieldPoints.Find(somePoint => somePoint.concreteFieldPoint.order == currentPosition);
		if (temp1 != null) {
			transform.position = new Vector3(temp1.transform.position.x, transform.position.y, temp1.transform.position.z);
		}
		SetDestination(temp1.transform.position);

		foreach (var item in plantControllers) {
			item.InitPlant(PlantType.None);
		}
		
		ClearPlants();
		foreach (PlantType plant in Enum.GetValues(typeof(PlantType))) {
			var temp2 = concreteGameField.fieldPoints.FindAll(somePoint => somePoint.pointType == PointType.Free && somePoint.plantType == plant);
			if (temp2 != null && temp2.Count != 0 && plant != PlantType.None) {
				AddPlants(plant, temp2.Count + concreteGameField.additionalPlantCount);
			}
		}
		LoadPlant();
		trailRenderer.Clear();
		trailIsShow = true;
		trailRenderer.enabled = true;
		soundEventSO.SomeSoundPlay(SoundType.Prepare);
		popupTextStorageSO.ShowText(PopupTextType.Prepare);



		levelStarted = true;
	}

	private void Update() {
		if (levelStarted && navMeshAgent.remainingDistance <= 0.15f) {
			SetHarvesterDestination();
		}
	}
	public void SetDestination(Vector3 _position) {
		navMeshAgent.SetDestination(_position);
	}

	public void ReloadPlant() {

		var temp1 = plants.Find(somePlant => somePlant == currentPlant);
		if (temp1 != PlantType.None) {
			plants.Remove(temp1);
		}
		stateStorageSO.SetPlantCounter(plants);
		if (plants.Count > 0) {
			var temp = GetNextPlant();
			if (temp != PlantType.None) {
				currentPlant = temp;
				stateStorageSO.SetNewPlant(temp);
				material.color = plantTypeStorageSO.GetColor(currentPlant);
			}
		}
		else {
			material.color = plantTypeStorageSO.GetColor(PlantType.None);
		}
	}

	private void AddResumedLevelPlants() {
		ClearPlants();
		foreach (PlantType plant in Enum.GetValues(typeof(PlantType))) {
			var temp2 = concreteGameField.fieldPoints.FindAll(somePoint => somePoint.pointType == PointType.Free && somePoint.plantType == plant);
			if (temp2 != null && temp2.Count != 0 && plant != PlantType.None) {
				AddPlants(plant, temp2.Count);
			}
		}
	}

	public void LoadPlant() {
		if (plants.Count > 0) {
			if (!concreteGameField.CanPlantByType(currentPlant)) {
				var temp = GetNextPlant();
				if (temp != PlantType.None) {
					currentPlant = temp;
					stateStorageSO.SetNewPlant(temp);
					material.color = plantTypeStorageSO.GetColor(currentPlant);
				}
			}
			else {
				material.color = plantTypeStorageSO.GetColor(currentPlant);
			}
		}
		else {
			material.color = plantTypeStorageSO.GetColor(PlantType.None);
		}
	}

	private PlantType GetNextPlant() {
		ShufflePlants();
		for (int i = 0; i < plants.Count; i++) {
			if (concreteGameField.CanPlantByType(plants[i])) {
				return plants[i];
			}
		}
		return PlantType.None;
	}

	private void ShufflePlants() {
		System.Random rnd = new System.Random();

		for (int i = 0; i < plants.Count; i++) {
			var tmp = plants[0];
			plants.RemoveAt(0);
			plants.Insert(rnd.Next(plants.Count), tmp);
		}
	}

	public void ClearPlants() {
		plants?.Clear();
	}

	private void SetHarvesterDestination() {
		if (concreteGameField != null) {
			if (forwardRun) {
				if (currentPosition < ((concreteGameField.fieldXPower * concreteGameField.fieldYPower) - 1)) {
					++currentPosition;
				}
				else {
					forwardRun = false;
					if (trailIsShow) {
						trailRenderer.Clear();
					}
				}
			}

			if (!forwardRun) {
				if (currentPosition > 0) {
					--currentPosition;
				}
				else {
					trailRenderer.Clear();
					if (trailIsShow) {
						soundEventSO.SomeSoundPlay(SoundType.Start);
						popupTextStorageSO.ShowText(PopupTextType.Start);
					}
					trailIsShow = false;
					trailRenderer.enabled = false;
					navMeshAgent.speed = concreteGameField.harvesterSpeed;
					forwardRun = true;
				}
			}

			var temp = fieldPoints.Find(somePoint => somePoint.concreteFieldPoint.order == currentPosition);
			if (temp != null) {
				SetDestination(temp.transform.position);
			}
		}
	}

	private void SetHatvesterToStartPoint() {
		if (trailIsShow) {
			soundEventSO.SomeSoundPlay(SoundType.Start);
			popupTextStorageSO.ShowText(PopupTextType.Start);
		}
		trailIsShow = false;
		trailRenderer.enabled = false;
		navMeshAgent.speed = concreteGameField.harvesterSpeed;
		forwardRun = true;
		currentPosition = 0;
		var temp1 = fieldPoints.Find(somePoint => somePoint.concreteFieldPoint.order == currentPosition);
		if (temp1 != null) {
			transform.position = new Vector3(temp1.transform.position.x, transform.position.y, temp1.transform.position.z);
		}
		trailRenderer.Clear();
		SetHarvesterDestination();
	}
}
