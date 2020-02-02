using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	public static Sound Instans;

	public AudioClip pushSound;
	public AudioClip fixSound;
	public AudioClip cancelSound;
	public AudioClip endVoice;

	public AudioSource audioSource;
	public AudioSource seAudioSource;
	void Awake(){

		if (Instans) {
			DestroyImmediate (gameObject);
		} else {
			Instans = this;
			DontDestroyOnLoad (gameObject);
			AudioSource[] adArray = GetComponents<AudioSource> ();
			audioSource = adArray [0];
			seAudioSource = adArray [1];
		}
	}
	// Use this for initialization
	void Start () {

	}

	public void PlayBgm(AudioClip bgm){
		audioSource.clip = bgm;
		audioSource.loop = true;
		audioSource.Play ();
	}

	public void PlaySe(AudioClip se){
		seAudioSource.PlayOneShot (se);
	}

	public void Stop(){
		audioSource.Stop ();
	}

	public void SetBgmMute(bool mute){
		audioSource.mute = mute;
	}

	public void SetSeMute(bool mute){
		seAudioSource.mute = mute;
	}
}
