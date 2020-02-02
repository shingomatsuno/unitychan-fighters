using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSelectScene : MonoBehaviour {

	public static SoundSelectScene Instans;

	public AudioClip bgm;
	public AudioClip selectVoice;
	public AudioClip kohakuVoice;
	public AudioClip yukoVoice;
	public AudioClip misakiVoice;

	void Awake(){
		Instans = this;
	}

	// Use this for initialization
	void Start () {
		//Sound.Instans.PlaySe (selectVoice);
		Sound.Instans.PlayBgm(bgm);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySe(AudioClip se){
		Sound.Instans.PlaySe (se);
	}
}
