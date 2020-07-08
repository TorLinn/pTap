using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public FieldStorage fieldStorageSO;
	public List<Transform> spownPoints;
	public GameObject enemyPrefab;
	private List<EnemyController> enemyControllers = new List<EnemyController>();
	private float timeToPullEnemy = 1000f;
	private float timer = 0f;
	private Coroutine workCoroutine;

	private void OnEnable() {
		FieldController.LevelStartAction += StartWork;
		FieldController.LevelPauseAction += EndWork;
		FieldController.LevelCompliteAction += EndWork;
	}

	private void OnDisable() {
		FieldController.LevelStartAction -= StartWork;
		FieldController.LevelPauseAction -= EndWork;
		FieldController.LevelCompliteAction -= EndWork;
	}

	private void StartWork() {
		timeToPullEnemy = fieldStorageSO.fieldController.ConcreteGameField.timeToSpownEnemy;

		if (workCoroutine != null) {
			StopCoroutine(workCoroutine);
			workCoroutine = null;
		}

		workCoroutine = StartCoroutine(WorkCoroutine());
	}

	private void EndWork() {
		if (workCoroutine != null) {
			StopCoroutine(workCoroutine);
			workCoroutine = null;
		}
		ClearLevel();
	}

	private IEnumerator WorkCoroutine() {
		timer = 0f;
		while (true) {
			if (timer >= timeToPullEnemy) {
				timer = 0f;
				if (enemyControllers.Count == 0 || enemyControllers.Find(someEnemy => someEnemy.enemyState == EnemyState.Free) == null) {
					enemyControllers.Add(Instantiate(enemyPrefab, transform).GetComponent<EnemyController>());
				}

				enemyControllers.Find(someEnemy => someEnemy.enemyState == EnemyState.Free).StartThift(spownPoints[Random.Range(0, spownPoints.Count)]);
			}

			timer += Time.deltaTime;
			yield return null;
		}
	}

	private void ClearLevel() {
		foreach (var item in enemyControllers) {
			item.Hide();
		}
	}
}