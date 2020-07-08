using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
	public PlayerStorage playerStorageSO;
	public string prefKey = "_playerSave";
	public Player player;
	private void OnEnable() {
		playerStorageSO.playerStateController = this;
		LoadPlayer();	
	}

	private void OnDisable() {
		SavePlayer();
	}

	private void LoadPlayer() {
		var tempString = PlayerPrefs.GetString(prefKey, "");
		if (tempString != "") {
			player = JsonUtility.FromJson<Player>(tempString);
		}
		else {
			player = new Player();
		}
	}

	private void SavePlayer() {
		PlayerPrefs.SetString(prefKey, JsonUtility.ToJson(player));
	}
}
