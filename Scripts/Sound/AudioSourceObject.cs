using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceObject : MonoBehaviour
{
	private AudioSource audioSource;
	public SoundEvent soundEventSO;
	public bool isBusy = false;

	private void Awake() {
		audioSource = this.gameObject.GetComponent<AudioSource>();
	}

	private void FixedUpdate() {
		if ( !audioSource.isPlaying) {
			isBusy = false;		
		}
	}

	public void PlaySound(SomeSound _someSound) {
		if (_someSound != null && _someSound.audioClip != null && _someSound.audioClip.Count > 0) {
			isBusy = true;
			audioSource.clip = _someSound.audioClip[Random.Range(0, _someSound.audioClip.Count)];
			audioSource.volume = _someSound.soundVolume;
			audioSource.Play();
		}
	}
}
