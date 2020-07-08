using System;
using UnityEngine;

public class FieldPointTriggerController : MonoBehaviour
{
	[SerializeField]
	private string plantTag = "Plant";
	public Action PlantOkAction;
	public static Action PlantOk;
	private PlantType plantType;
	public PlantTypeStorage plantTypeStorageSO;
    public Transform plantHole;

	private void OnTriggerEnter(Collider other) {
		if (plantType != PlantType.None && other.gameObject.tag == plantTag) {
			var temp = other.gameObject.GetComponent<PlantController>();
			if (temp != null && temp.concretePlantType == plantType) {
				temp.hitTheTarget = true;
				PlantOkAction?.Invoke();
				PlantOk?.Invoke();
			}
		}
	}

	public void Init(PlantType _plantType) {
		plantType = _plantType;
		if (_plantType != PlantType.None) {
            plantHole.GetComponent<MeshRenderer>().enabled = true;
            plantHole.GetComponent<MeshRenderer>().material.color = plantTypeStorageSO.GetColor(plantType);
		}
		else {
            plantHole.GetComponent<MeshRenderer>().enabled = false;

        }
    }
}
