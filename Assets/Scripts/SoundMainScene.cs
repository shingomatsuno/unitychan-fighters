using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMainScene : MonoBehaviour {

	public static SoundMainScene Instans;

	public AudioClip battleStart;
	public AudioClip sokomade;
	public AudioClip lastBattle;

	public AudioClip ready;
	public AudioClip go;

	public AudioClip guard;

	public AudioClip kohakuSolt;
	public AudioClip yukoSolt;
	public AudioClip misakiSolt;

	public AudioClip KohakuSpecial;
	public AudioClip yukoSpecial;
	public AudioClip misakiSpecial;

	public AudioClip kohakuDead;
	public AudioClip yukoDead;
	public AudioClip misakiDead;

	public AudioClip kohakuWin;
	public AudioClip yukoWin;
	public AudioClip misakiWin;

	public AudioClip kohakuSS;
	public AudioClip yukoSS;
	public AudioClip misakiSS;

	public AudioClip kohakuHukkatsu;
	public AudioClip yukoHukkatsu;
	public AudioClip misakiHukkatsu;

	public AudioClip yamikoSolt;
	public AudioClip yamikoSpecial;
	public AudioClip yamikoSS;
	public AudioClip yamikoDead;

	public AudioClip kekka;

	public AudioClip zannen;

	//勝ち抜き数に応じて評価のボイスを変える
	public AudioClip end04;
	public AudioClip end59;
	public AudioClip end1019;
	public AudioClip end20;


	void Awake(){
		Instans = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySe(AudioClip se){
		Sound.Instans.PlaySe (se);
	}
}
