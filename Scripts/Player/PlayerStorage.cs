using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStorage", fileName = "PlayerStorage")]
public class PlayerStorage : ScriptableObject
{
	public PlayerStateController playerStateController;

	public int GetPlayerLevel() {
		return playerStateController.player.currentLevel;
	}

	public void NextLevel() {
		playerStateController.player.currentLevel++;
	}

	public void ReloadPlayer() {
		playerStateController.player.currentLevel = 0;
	}
}
