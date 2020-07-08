using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldPointController : MonoBehaviour {
	private string prefabDir = "Plants/";
	public PlantTypeStorage plantTypeStorageSO;
	public FieldPoint concreteFieldPoint;
	public MeshRenderer thisRenderer;
	private Material material;
	public Transform trigger;
	private FieldPointTriggerController triggerController;
	public static Action ThiefAction;
	public Transform hole;
	public Transform bed;
	private GameObject plant;
	public Transform targetPlane;
	public StateStorage stateStorageSO;
	private void Awake() {
		material = thisRenderer.material;
		triggerController = GetComponentInChildren<FieldPointTriggerController>();
	}

	private void OnEnable() {
		if (triggerController != null) {
			triggerController.PlantOkAction += PlantHasBeenPlanted;
		}
		stateStorageSO.SetNewPlantAction += SetTargetMode;
		targetPlane.gameObject.SetActive(false);
	}

	private void OnDisable() {
		if (triggerController != null) {
			triggerController.PlantOkAction -= PlantHasBeenPlanted;
		}
		stateStorageSO.SetNewPlantAction -= SetTargetMode;
	}

	private void PlantHasBeenPlanted() {
		concreteFieldPoint.pointType = PointType.Planted;
		SelectMode();
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void Show(Vector3 _position, FieldPoint _fieldPoint, float _size) {
		concreteFieldPoint = _fieldPoint;
		gameObject.transform.position = _position;
		trigger.localScale = new Vector3(_size, trigger.localScale.y, _size);
		if (triggerController != null) {
			triggerController.Init(_fieldPoint.plantType);
		}
		gameObject.SetActive(true);
		SelectMode();
	}

	private void SelectMode() {
		switch (concreteFieldPoint.pointType) {
			case PointType.Deffault:
				SetDeffaultMode();
				break;
			case PointType.Free:
				SetFreeMode();
				break;
			case PointType.Bussy:
				SetBussyMode();
				break;
			case PointType.Planted:
				SetPlantedMode();
				break;
			case PointType.Theft:
				SetTheftMode();
				break;
			default:
				break;
		}
	}

	private void SetDeffaultMode() {
		material.SetColor("_Color", plantTypeStorageSO.GetColor(PointType.Deffault));
		hole.gameObject.SetActive(false);
		bed.gameObject.SetActive(false);
		trigger.gameObject.SetActive(false);
		if (plant != null) {
			Destroy(plant);
		}
	}

	private void SetFreeMode() {
		material.SetColor("_Color", plantTypeStorageSO.GetColor(PointType.Deffault));
		if (plant != null) {
			Destroy(plant);
		}
		hole.gameObject.SetActive(true);
		bed.gameObject.SetActive(false);
		trigger.gameObject.SetActive(true);
		SetTargetMode();
	}

	private void SetBussyMode() {
		material.SetColor("_Color", plantTypeStorageSO.GetColor(PointType.Deffault));
		hole.gameObject.SetActive(false);
		if (Random.Range(0, 2) > 0) {
			bed.Rotate(0f, 90f, 0f);
		}
		bed.gameObject.SetActive(true);
		trigger.gameObject.SetActive(false);
		LoadPlantPrefab();
	}

	private void SetPlantedMode() {
		targetPlane.gameObject.SetActive(false);
		material.SetColor("_Color", plantTypeStorageSO.GetColor(PointType.Deffault));
		hole.gameObject.SetActive(false);
		if (Random.Range(0, 2) > 0) {
			bed.Rotate(0f, 90f, 0f);
		}
		bed.gameObject.SetActive(true);
		trigger.gameObject.SetActive(false);
		LoadPlantPrefab();
	}

	private void SetTargetMode() {
		if (concreteFieldPoint.pointType == PointType.Free && stateStorageSO.currentPlantType == concreteFieldPoint.plantType) {
			targetPlane.gameObject.SetActive(true);
		}
		else {
			targetPlane.gameObject.SetActive(false);
		}
	}

	private void SetTheftMode() {
		material.SetColor("_Color", plantTypeStorageSO.GetColor(concreteFieldPoint.pointType));
	}

	private void LoadPlantPrefab() {
		var build = Resources.Load<GameObject>(prefabDir + concreteFieldPoint.plantType);

		if (build != null) {
			if (plant != null) {
				Destroy(plant);
			}

			plant = Instantiate(build, transform.position + build.transform.position, transform.rotation, this.transform);
			plant.transform.Rotate(0f, Random.Range(0f, 90f), 0f);
		}
		else {
			Debug.Log("Cannot Find: " + prefabDir + concreteFieldPoint.plantType);
		}
	}

	public void StartThift() {
		concreteFieldPoint.pointType = PointType.Theft;
		SelectMode();
	}

	public void StopThift() {
		concreteFieldPoint.pointType = PointType.Planted;
		SelectMode();
	}

	public void PlantHasBeenThief() {
		concreteFieldPoint.pointType = PointType.Free;
		SelectMode();
		ThiefAction?.Invoke();
	}
}
