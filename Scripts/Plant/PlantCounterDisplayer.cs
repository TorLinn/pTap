using UnityEngine;
using UnityEngine.UI;

public class PlantCounterDisplayer : MonoBehaviour
{
    public PlantTypeStorage plantTypeStorageSO;
    private PlantType plantType;
    private int plantCounter;
    public Text counterText;
    public Image counterImage;

    public void Init (PlantCounter _plantCounter) {
        plantType = _plantCounter.plantType;
        plantCounter = _plantCounter.plantCount;

        counterText.text = plantCounter.ToString();
        counterText.color = plantTypeStorageSO.GetColor(plantType);
        counterImage.color = plantTypeStorageSO.GetColor(plantType);
        counterImage.sprite = plantTypeStorageSO.GetSprite(plantType);
    }

    public void SetCounter(int _counter) {
        plantCounter = _counter;
        counterText.text = plantCounter.ToString();
    }

    public PlantType GetCounterType() {
        return plantType;
    }
}
