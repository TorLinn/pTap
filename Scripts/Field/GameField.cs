using System;
using System.Collections.Generic;

[Serializable]
public class GameField : ICloneable
{
	public int fieldID;
	public int fieldXPower;
	public int fieldYPower;
	public float harvesterSpeed;
	public int additionalPlantCount;
	public float triggerSize;
	public float timeToSpownEnemy;
	public float enemySpeed;
	public FieldTrajectory fieldTrajectory;
	public List<FieldPoint> fieldPoints = new List<FieldPoint>();

	public GameField() {
		fieldID = 0;
		harvesterSpeed = 0f;
		fieldXPower = 5;
		fieldYPower = 5;
		additionalPlantCount = 0;
		triggerSize = 1f;
		timeToSpownEnemy = 5f;
		enemySpeed = 5f;
		fieldTrajectory = FieldTrajectory.Consistent;
		for (int i = 0; i < fieldYPower; i++) {
			for (int j = 0; j < fieldXPower; j++) {
				fieldPoints.Add(new FieldPoint(j, i));
			}
		}
	}

	public bool GameFieldComplite() {
		foreach (var item in fieldPoints) {
			if (item.pointType == PointType.Free) {
				return false;
			}
		}
		return true;
	}

	public bool CanPlantByType(PlantType _plantType) {
		var temp = fieldPoints.Find(somePoint => somePoint.plantType == _plantType && somePoint.pointType == PointType.Free);
		if (temp == null) {
			return false;
		}
		else {
			return true;
		}
	}

	public bool GameFieldLost(List<PlantCounter> _plantCounters) {
		foreach (PlantType plant in Enum.GetValues(typeof(PlantType))) {
			var temp1 = fieldPoints.FindAll(somePoint => somePoint.plantType == plant && somePoint.pointType == PointType.Free);
			if (temp1 != null && temp1.Count > 0) {
				var temp2 = _plantCounters.Find(someCounter => someCounter.plantType == plant);
				if (temp2 == null || temp2.plantCount < temp1.Count) {
					return true;
				}
			}
		}
		return false;
	}

	public void ResumeCurrentGameField(List<PlantCounter> _plantCounters) {
		foreach (PlantType plant in Enum.GetValues(typeof(PlantType))) {
			var temp1 = fieldPoints.FindAll(somePoint => somePoint.plantType == plant && somePoint.pointType == PointType.Free);
			if (temp1 != null && temp1.Count > 0) {
				var temp2 = _plantCounters.Find(someCounter => someCounter.plantType == plant);
				if (temp2 == null) {
					_plantCounters.Add(new PlantCounter(plant, temp1.Count));
				}
				else {
					temp2.plantCount = temp1.Count;
				}
			}
		}
	}

	public object Clone() {
		GameField newGameField = new GameField();
		newGameField.fieldID = this.fieldID;
		newGameField.fieldXPower = this.fieldXPower;
		newGameField.fieldYPower = this.fieldYPower;
		newGameField.harvesterSpeed = this.harvesterSpeed;
		newGameField.additionalPlantCount = this.additionalPlantCount;
		newGameField.triggerSize = this.triggerSize;
		newGameField.timeToSpownEnemy = this.timeToSpownEnemy;
		newGameField.fieldPoints = new List<FieldPoint>();
		for (int i = 0; i < newGameField.fieldYPower; i++) {
			for (int j = 0; j < newGameField.fieldXPower; j++) {
				var temp = new FieldPoint(i, j);
				newGameField.fieldPoints.Add(temp);
				temp.order = this.fieldPoints[i * newGameField.fieldXPower + j].order;
				temp.plantType = this.fieldPoints[i * newGameField.fieldXPower + j].plantType;
				temp.pointType = this.fieldPoints[i * newGameField.fieldXPower + j].pointType;
				temp.xCoord = this.fieldPoints[i * newGameField.fieldXPower + j].xCoord;
				temp.yCoord = this.fieldPoints[i * newGameField.fieldXPower + j].yCoord;

			}
		}
		return newGameField;
	}
}
