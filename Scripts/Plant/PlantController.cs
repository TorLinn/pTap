using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlantController : MonoBehaviour {
    public SoundEvent soundEventSO;
    public StateStorage stateStorageSO;
    public PlantTypeStorage plantTypeStorageSO;    
    public float plantSpeed = 1f;
    public bool isActive = false;
    private Coroutine actionCoroutine;
    public Transform sphere;
    public PlantType concretePlantType;
    public static Action HidePlantAction;
    public bool hitTheTarget = false;

    private void HidePlant() {
        isActive = false;
        hitTheTarget = false;
        gameObject.SetActive(false);
    }

    public void InitPlant(PlantType _plantType) {
        concretePlantType = _plantType;
        sphere.GetComponent<MeshRenderer>().material.color = plantTypeStorageSO.GetColor(concretePlantType);
        HidePlant();
    }

    public void Activate(Vector3 _startPoint) {
        isActive = true;
        transform.position = _startPoint;
        gameObject.SetActive(true);
        if (actionCoroutine != null) {
            StopCoroutine(actionCoroutine);
        }
        actionCoroutine = StartCoroutine(ActionCoroutine());
    }

    private IEnumerator ActionCoroutine() {
        Tween myTween = transform.DOMove(new Vector3(transform.position.x, transform.position.y - 6f, transform.position.z), plantSpeed);
        yield return myTween.WaitForCompletion();
        if (!hitTheTarget) {
            stateStorageSO.errorsCounter++;
            soundEventSO.SomeSoundPlay(SoundType.Miss);
        }
        else {
            soundEventSO.SomeSoundPlay(SoundType.Planted);
        }
        HidePlant();
        HidePlantAction?.Invoke();
    }
}