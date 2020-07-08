using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PopupTextStorage", fileName = "PopupTextStorage")]
public class PopupTextStorage : ScriptableObject
{
	public List<PopupText> popupTexts;
	public Action NewPopuTextAction;
	public PopupText CurrentPopupText { get; private set; }

	public void ShowText(PopupTextType _textType) {
		var temp = popupTexts.Find(someText => someText.popupTextType == _textType);
		if (temp != null) {
			CurrentPopupText = temp;
			NewPopuTextAction?.Invoke();
		}
	}
}

[Serializable]
public class PopupText {
	public PopupTextType popupTextType;
	public Color textColor;
	public List<string> popupText = new List<string>();
	public float lifeTime;
}
