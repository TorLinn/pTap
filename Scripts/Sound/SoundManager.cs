using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
	public int deffaultAudioSourcesCount = 10;
	public SoundEvent soundEventSO;
    [HideInInspector]
    public AudioSource audioSource;
	public GameObject audioSourcePrefab;
	[SerializeField]
	private List<AudioSourceObject> audioSourceObjects = new List<AudioSourceObject>();
	public Transform audioParent;
	
	void Awake() {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		soundEventSO.SetManager(this);
	}

	private void OnEnable() {
		soundEventSO.SomeSoundPlay(SoundType.MainMelody);

		for (int i = 0; i < deffaultAudioSourcesCount; i++) {
			AddAudioSource();
		}
	}

	private void AddAudioSource() {
		audioSourceObjects.Add(Instantiate(audioSourcePrefab, audioParent).GetComponent<AudioSourceObject>());
	}

	public void PlaySound(SomeSound _someSound) {
		GetFreeAudioSource().PlaySound(_someSound);
	}

	public AudioSourceObject GetFreeAudioSource() {
		var temp = audioSourceObjects.Find(someAudioSource => !someAudioSource.isBusy);
		if (temp != null) {
			return temp;
		}
		else {
			AddAudioSource();
			return GetFreeAudioSource();
		}
	}
}