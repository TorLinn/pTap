using UnityEngine;

public class EditorPoint : MonoBehaviour
{
	public bool canDraw = true;
	public Color color = Color.grey;
	public float scale = 1f;

#if UNITY_EDITOR
	void OnDrawGizmos() {
		if (canDraw) {
			Gizmos.color = color;
			Gizmos.DrawCube(transform.position, Vector3.one * scale);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, scale / 5f);
		}
	}
#endif
}
