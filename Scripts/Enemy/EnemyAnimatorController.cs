using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
	private int currentSpeedFloat = Animator.StringToHash("_speed");
	public Animator animator;

	public void SetSpeed(float speed) {
		animator.SetFloat(currentSpeedFloat, speed);
	}
}
