using System;

[Serializable]
public class FieldPoint 
{
	public PointType pointType;
	public PlantType plantType;
	public int xCoord;
	public int yCoord;
	public int order;

	public FieldPoint(int _x, int _y) {
		pointType = PointType.Deffault;
		plantType = PlantType.None;
		xCoord = _x;
		yCoord = _y;
		order = 0;
	}
}
