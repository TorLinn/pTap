using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	public SoundEvent soundEventSO;
	[HideInInspector]
	public AudioSource audioSource;

	private void Awake() {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		soundEventSO.SetMusicManager(this);
	}

	private void FixedUpdate() {
		if (soundEventSO.playMusic) {
			if (!audioSource.isPlaying) {
				soundEventSO.PlayMusic();
			}
		}
		else {
			audioSource.Stop();
		}
	}

	public void PlaySound(SomeSound _someSound) {
		if (_someSound != null && _someSound.audioClip != null && _someSound.audioClip.Count > 0) {
			audioSource.clip = _someSound.audioClip[Random.Range(0, _someSound.audioClip.Count)];
			audioSource.volume = _someSound.soundVolume;
			audioSource.Play();
		}
	}
}
