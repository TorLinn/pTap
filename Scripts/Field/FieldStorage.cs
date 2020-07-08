using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "ScriptableObjects/FieldStorage", fileName = "FieldStorage")]
public class FieldStorage : ScriptableObject
{
	public FieldController fieldController;
	public GameFieldsList gameFieldsList = new GameFieldsList();
	
	
	public bool Init(string _json) {
		gameFieldsList.gameFields = new List<GameField>();
		try {
			gameFieldsList = JsonUtility.FromJson<GameFieldsList>(_json);
			return true;
		}
		catch (Exception _ex) {
			Debug.Log(_ex);
			return false;
		}
	}

	public GameField GetConcreteField(int _ID) {
		if (gameFieldsList.gameFields.Find(someField => someField.fieldID == _ID) == null) {
			gameFieldsList.gameFields.Add(GenerateNewField(_ID));
		}

		var tempField = gameFieldsList.gameFields.Find(someField => someField.fieldID == _ID);
		SetTrajectory(tempField);

		return (GameField)tempField.Clone();

	}

	private GameField GenerateNewField(int _ID) {
		GameField result = new GameField();
		result.fieldID = _ID;
		var tempPower = Random.Range(4, 5);
		result.fieldXPower = tempPower;
		result.fieldYPower = tempPower;
		result.harvesterSpeed = Random.Range(5, 10);
		result.additionalPlantCount = Random.Range(0, 3);
		result.triggerSize = Random.Range(0.5f, 1f);
		result.timeToSpownEnemy = Random.Range(4f, 7f);
		result.enemySpeed = Random.Range(4f, 7f);
		result.fieldPoints = new List<FieldPoint>();

		for (int i = 0; i < result.fieldYPower; i++) {
			for (int j = 0; j < result.fieldXPower; j++) {
				var tempPoint = new FieldPoint(j, i);
				result.fieldPoints.Add(tempPoint);
				tempPoint.xCoord = j;
				tempPoint.yCoord = i;

				var temRand = Random.Range(0f, 1f);

				if (temRand < 0.6f) {
					tempPoint.pointType = PointType.Bussy;
					tempPoint.plantType = (PlantType)Random.Range(1, Enum.GetValues(typeof(PlantType)).Length);
				}
				else {
					tempPoint.pointType = PointType.Free;
					tempPoint.plantType = (PlantType)Random.Range(1, Enum.GetValues(typeof(PlantType)).Length);
				}
			}
		}

		if (Random.Range(0f, 1f) < 0.6f) {
			result.fieldTrajectory = FieldTrajectory.Consistent;
		}
		else {
			result.fieldTrajectory = FieldTrajectory.Spiral;
		}

		return result;
	}

	private void SetTrajectory(GameField _gameField) {
		switch (_gameField.fieldTrajectory) {
			case FieldTrajectory.Consistent:
				SetConsistent(_gameField);
				break;
			//case FieldTrajectory.BackConsistent:
			//	break;
			case FieldTrajectory.Spiral:
				SetSpiral(_gameField);
				break;
			//case FieldTrajectory.BackSpiral:
			//	break;
			//case FieldTrajectory.Oriental:
			//	break;
			//case FieldTrajectory.BackOriental:
			//	break;
			default:
				break;
		}
	}

	private void SetConsistent(GameField _gameField) {
		for (int i = 0; i < _gameField.fieldYPower; i++) {
			for (int j = 0; j < _gameField.fieldYPower; j++) {
				if (i % 2 == 0) {
					_gameField.fieldPoints[i * _gameField.fieldXPower + j].order = i * _gameField.fieldXPower + j;
				}
				else {
					_gameField.fieldPoints[i * _gameField.fieldXPower + j].order = (i + 1) * _gameField.fieldXPower - j - 1;
				}
			}
		}
	}

	private void SetSpiral(GameField _gameField) {
		int row = 0;
		int col = 0;
		int dx = 1;
		int dy = 0;
		int dirChanges = 0;
		int visits = _gameField.fieldXPower;

		for (int i = 0; i < _gameField.fieldPoints.Count; i++) {
			_gameField.fieldPoints.Find(somePoint => somePoint.yCoord == row && somePoint.xCoord == col).order = i;	

			if (--visits == 0) {
				visits = _gameField.fieldXPower * (dirChanges % 2) + _gameField.fieldYPower * ((dirChanges + 1) % 2) - (dirChanges / 2 - 1) - 2;
				int temp = dx;
				dx = -dy;
				dy = temp;
				dirChanges++;
			}

			col += dx;
			row += dy;
		}
	}
}

[Serializable]
public class GameFieldsList {
	public List<GameField> gameFields = new List<GameField>();
}