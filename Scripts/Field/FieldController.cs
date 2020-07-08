using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldController : MonoBehaviour
{
	public PopupTextStorage popupTextStorageSO;
	public EventStorage eventStorageSO;
	public FieldStorage fieldStorageSO;
	public PlayerStorage playerStorageSO;
	public StateStorage stateStorageSO;
	public GameObject fieldPointPrefab;
	public int pointsCount = 100;
	public float pointOffset = 1f;
	public Transform fieldTransform;
	public GameField ConcreteGameField { get; private set; }
	public int fieldID = 0;

	private List<FieldPointController> fieldPoints = new List<FieldPointController>();

	public GameObject harvesterPrefab;
	private Harvester harvester;
	public Transform startPoint;

	public static Action LevelCompliteAction;	
	public static Action LevelPauseAction;
	public static Action LevelResumeAction;
	public static Action LevelStartAction;

	private void Awake() {
		fieldStorageSO.fieldController = this;
	}
	private void OnEnable() {
		InitField();
		eventStorageSO.PlayAction += PrepareField;
		eventStorageSO.ResumeLevelAction += ResumeCurrentLevel;
	}

	private void OnDisable() {
		eventStorageSO.PlayAction -= PrepareField;
		eventStorageSO.ResumeLevelAction -= ResumeCurrentLevel;
		FieldPointTriggerController.PlantOk -= CorrectPlants;
		PlantController.HidePlantAction -= LevelEstimation;
		FieldPointController.ThiefAction -= LevelEstimation;
	}

	public void InitField() {
		StartInit();
	}

	private void StartInit() {
		if (fieldPoints.Count == 0) {
			for (int i = 0; i < pointsCount; i++) {
				fieldPoints.Add(Instantiate(fieldPointPrefab, fieldTransform.position, fieldTransform.rotation, fieldTransform).GetComponent<FieldPointController>());
				fieldPoints[i].Hide();
			}
		}

		harvester = Instantiate(harvesterPrefab, startPoint.position, startPoint.rotation, transform).GetComponent<Harvester>();
		FieldPointTriggerController.PlantOk += CorrectPlants;
		PlantController.HidePlantAction += LevelEstimation;
		FieldPointController.ThiefAction += LevelEstimation;
	}

	private void LevelEstimation() {
		if (ConcreteGameField.GameFieldComplite()) {
			CompliteLevel();
		}
		else if (ConcreteGameField.GameFieldLost(stateStorageSO.plantCounters)) {
			LossLevel();
		}
	}

	private void ResumeCurrentLevel() {
		harvester.ResumeLevel();
	}

	private void CompliteLevel() {
		AdsManager.ShowNextLevelVideo();
		LevelCompliteAction?.Invoke();
	}

	private void LossLevel() {
		LevelPauseAction?.Invoke();
	}

	private void CorrectPlants() {
		popupTextStorageSO.ShowText(PopupTextType.Good);
		foreach (PlantType plant in Enum.GetValues(typeof(PlantType))) {
			var temp = ConcreteGameField.fieldPoints.FindAll(somePoint => somePoint.pointType == PointType.Free && somePoint.plantType == plant);
			if ((temp == null || temp.Count == 0) && plant != PlantType.None) {
				harvester.CorrectPlants();
			}
		}
	}

	private void PrepareField() {
		for (int i = 0; i < pointsCount; i++) {
			fieldPoints[i].Hide();
		}
		ConcreteGameField = fieldStorageSO.GetConcreteField(playerStorageSO.GetPlayerLevel());
		for (int i = 0; i < ConcreteGameField.fieldYPower; i++) {
			for (int j = 0; j < ConcreteGameField.fieldXPower; j++) {
				fieldPoints[j + (i * ConcreteGameField.fieldXPower)].Show(new Vector3(fieldTransform.position.x + j * pointOffset, fieldTransform.position.y, fieldTransform.position.z + i * pointOffset), ConcreteGameField.fieldPoints.Find(somePoint => somePoint.yCoord == i && somePoint.xCoord == j), ConcreteGameField.triggerSize);
			}
		}
		fieldTransform.Rotate(new Vector3(0f, 90f, 0f));
		harvester.PrepareHarvester(ConcreteGameField, fieldPoints);
		stateStorageSO.errorsCounter = 0;
		LevelStartAction?.Invoke();
	}

	public FieldPointController GetEnemyTarget() {
		var plantedCells = ConcreteGameField.fieldPoints.FindAll(somePoint => somePoint.pointType == PointType.Planted);
		if (plantedCells != null && plantedCells.Count > 0) {
			var randomCell = plantedCells[Random.Range(0, plantedCells.Count)];
			return fieldPoints.Find(somePoint => somePoint.concreteFieldPoint == randomCell);
		}
		else {
			return null;
		}
	}
}
