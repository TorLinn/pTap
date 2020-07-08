using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public SoundEvent soundEventSO;
	public PlayerStorage playerStorageSO;
	public StateStorage stateStorageSO;
	public Transform nextLevelUI;
	public Text levelIDText;
	public Button reloadLevelButton;
	public Button nextLevelButton;
	public Color goodStarColor;
	public Color badStarColor;
	public Image firstStar;
	public Image secondStar;
	public Image thirdStar;

	private void OnEnable() {
		FieldController.LevelCompliteAction += ShowNextLevelUI;
		FieldController.LevelPauseAction += ShowReloarLevelUI;
	}

	private void OnDisable() {
		FieldController.LevelCompliteAction -= ShowNextLevelUI;
		FieldController.LevelPauseAction -= ShowReloarLevelUI;
	}

	private void ShowNextLevelUI() {
		soundEventSO.SomeSoundPlay(SoundType.Win);
		reloadLevelButton.gameObject.SetActive(true);
		nextLevelButton.gameObject.SetActive(true);
		levelIDText.text = $"Level {playerStorageSO.GetPlayerLevel() + 1} Complite!";
		nextLevelUI.gameObject.SetActive(true);
		SetStars();
	}

	private void ShowReloarLevelUI() {
		soundEventSO.SomeSoundPlay(SoundType.Loose);
		reloadLevelButton.gameObject.SetActive(true);
		nextLevelButton.gameObject.SetActive(false);
		levelIDText.text = $"Level {playerStorageSO.GetPlayerLevel() + 1} Loos!";
		nextLevelUI.gameObject.SetActive(true);
	}

	private void SetStars() {
		thirdStar.color = goodStarColor;
		secondStar.color = goodStarColor;
		firstStar.color = goodStarColor;

		if (stateStorageSO.errorsCounter > 0) {
			thirdStar.color = badStarColor;

			if (stateStorageSO.errorsCounter > 1) {
				secondStar.color = badStarColor;

				if (stateStorageSO.errorsCounter > 2) {
					firstStar.color = badStarColor;
				}
			}
		}
	}
}
