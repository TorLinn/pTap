using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoundEvent", fileName = "SoundEvent")]
public class SoundEvent : ScriptableObject
{
	private SoundManager soundManager;
	private MusicManager musicManager;
	public List<SomeSound> someSounds;
	public bool playMusic = true;
	public bool playSound = true;

	public void SetManager(SoundManager _soundManager) {
		soundManager = _soundManager;
	}

	public void SetMusicManager(MusicManager _musicManager) {
		musicManager = _musicManager;
	}

	public void ButtonClick() {
		SomeSoundPlay(SoundType.ButtonClick);
	}

	public void PlayMusic() {
		musicManager.PlaySound(someSounds.Find(someSound => someSound.soundType == SoundType.MainMelody));
	}

	public void SomeSoundPlay(SoundType _soundType) {
		soundManager.PlaySound(someSounds.Find(someSound => someSound.soundType == _soundType));
	}
}

[Serializable]
public class SomeSound {
	public SoundType soundType;
	[SoundBar]
	public float soundVolume = 1f;
	public List<AudioClip> audioClip;
}