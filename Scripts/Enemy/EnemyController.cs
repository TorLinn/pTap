using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
	public SoundEvent soundEventSO;
	public PopupTextStorage popupTextStorageSO;
	public FieldStorage fieldStorageSO;
	public EnemyState enemyState = EnemyState.Free;
	private Transform startPosition;
	private FieldPointController targetPosition;
	public Transform body;
	private Coroutine runForwardCoroutine;
	private Coroutine thiftCoroutine;
	private Coroutine runBackCoroutine;
	private NavMeshAgent navMeshAgent;
	public float toPlantDistance = 1f;
	public float thiftTime = 5f;
	private EnemyAnimatorController enemyAnimatorController;
	public Image progressBar;

	private void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.speed = fieldStorageSO.fieldController.ConcreteGameField.enemySpeed;
		enemyAnimatorController = GetComponent<EnemyAnimatorController>();
	}

	private void OnEnable() {
		PlantController.HidePlantAction += StopThief;
		Hide();
	}

	private void OnDisable() {
		PlantController.HidePlantAction -= StopThief;
	}

	private void Update() {
		enemyAnimatorController.SetSpeed(navMeshAgent.velocity.magnitude);
	}

	public void StartThift(Transform _startPoint) {
		startPosition = _startPoint;
		transform.position = startPosition.position;
		targetPosition = fieldStorageSO.fieldController.GetEnemyTarget();

		if (targetPosition != null) {
			if (runForwardCoroutine != null) {
				StopCoroutine(runForwardCoroutine);
			}
			runForwardCoroutine = StartCoroutine(RunForwardCoroutine());
		}
		else {
			Hide();
		}
	}

	public void Hide() {
		enemyState = EnemyState.Free;
		body.gameObject.SetActive(false);
		navMeshAgent.isStopped = true;
		targetPosition = null;
		progressBar.gameObject.SetActive(false);
		StopAllCoroutines();
	}

	private void Show() {
		soundEventSO.SomeSoundPlay(SoundType.Enemy);
		popupTextStorageSO.ShowText(PopupTextType.Warning);
		progressBar.gameObject.SetActive(true);
		progressBar.fillAmount = 0f;
		body.gameObject.SetActive(true);
	}

	private IEnumerator RunForwardCoroutine() {
		targetPosition.StartThift();
		Show();
		navMeshAgent.isStopped = false;
		enemyState = EnemyState.RunForward;
		navMeshAgent.SetDestination(targetPosition.transform.position);
			
		while (Vector3.Distance(transform.position, targetPosition.transform.position) > toPlantDistance) {
			yield return null;
		}

		navMeshAgent.isStopped = true;
			
		if (thiftCoroutine != null) {
			StopCoroutine(thiftCoroutine);
		}
		thiftCoroutine = StartCoroutine(ThiftCoroutine());
	}

	private IEnumerator ThiftCoroutine() {
		enemyState = EnemyState.Theft;
		float timer = 0f;
		while (timer < thiftTime) {
			timer += Time.deltaTime;
			progressBar.gameObject.transform.LookAt(Camera.main.transform.position);
			progressBar.fillAmount = timer / thiftTime;
			yield return null;
		}

		targetPosition.PlantHasBeenThief();

		if (runBackCoroutine != null) {
			StopCoroutine(runBackCoroutine);
		}
		runBackCoroutine = StartCoroutine(RunBackCoroutine());
	}

	private IEnumerator RunBackCoroutine() {
		progressBar.gameObject.SetActive(false);

		enemyState = EnemyState.RunBack;
		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(startPosition.position);
		
		while (Vector3.Distance(transform.position, startPosition.position) > 0.5f) {
			yield return null;
		}

		Hide();
	}

	private void StopThief() {
		if (enemyState == EnemyState.Theft) {
			StopAllCoroutines();
			targetPosition.StopThift();

			if (runBackCoroutine != null) {
				StopCoroutine(runBackCoroutine);
			}
			runBackCoroutine = StartCoroutine(RunBackCoroutine());
		}
	}
}
