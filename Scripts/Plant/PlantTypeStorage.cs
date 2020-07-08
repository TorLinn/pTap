using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/PlantTypeStorage", fileName = "PlantTypeStorage")]
public class PlantTypeStorage : ScriptableObject
{
    public List<PlantObject> plantObjects;
    public List<FieldObject> fieldObjects;

    public Color GetColor(PlantType _plantType) {
        return plantObjects.Find(somePlant => somePlant.plantType == _plantType).plantColor;
    }

    public Sprite GetSprite(PlantType _plantType) {
        return plantObjects.Find(somePlant => somePlant.plantType == _plantType).plantSprite;
    }

    public Color GetColor(PointType _pointType) {
        return fieldObjects.Find(someObjec => someObjec.pointType == _pointType).pointColor;
    }
}

[Serializable]
public class PlantObject {
    public PlantType plantType;
    public Color plantColor;
    public Sprite plantSprite;
}


[Serializable]
public class FieldObject {
    public PointType pointType;
    public Color pointColor;
}