using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupTextController : MonoBehaviour
{
	public SoundEvent soundEventSO;
	public Text popupText;
	public PopupTextStorage popupTextStorageSO;
	private PopupText currentPopupText;
	private Coroutine showTextCoroutine = null;
	public int maxFontSize = 96;
	public int minFontSize = 24;
	public int fontDeltaSize = 1;

	private void OnEnable() {
		popupTextStorageSO.NewPopuTextAction += ShowPopup;
		HideText();
	}

	private void OnDisable() {
		popupTextStorageSO.NewPopuTextAction -= ShowPopup;
	}

	private void ShowPopup() {
		currentPopupText = popupTextStorageSO.CurrentPopupText;

		if (showTextCoroutine != null) {
			StopCoroutine(showTextCoroutine);
		}

		showTextCoroutine = StartCoroutine(ShowTextCoroutine());
	}

	private IEnumerator ShowTextCoroutine() {
		popupText.text = currentPopupText.popupText[Random.Range(0, currentPopupText.popupText.Count)];
		popupText.color = currentPopupText.textColor;
		popupText.fontSize = minFontSize;
		ShowText();

		while (popupText.fontSize < maxFontSize) {
			yield return new WaitForFixedUpdate();
			popupText.fontSize += fontDeltaSize;
		}

		yield return new WaitForSeconds(currentPopupText.lifeTime);

		while (popupText.fontSize > minFontSize) {
			yield return new WaitForFixedUpdate();
			popupText.fontSize -= fontDeltaSize;
		}

		HideText();
	}

	private void ShowText() {
		popupText.gameObject.SetActive(true);
	}

	private void HideText() {
		popupText.gameObject.SetActive(false);
	}
}
