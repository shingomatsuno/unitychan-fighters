using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class SettingController : MonoBehaviour {

	public GameObject settingPanel;

	public GameObject easyButtonText;
	public GameObject nomalButtonText;
	public GameObject heardButtonText;

	public GameObject bgmOnButtonText;
	public GameObject bgmOffButtonText;

	public GameObject seOnButtonText;
	public GameObject seOffButtonText;

	public Image easyButtonImage;
	public Image nomalButtonImage;
	public Image heardButtonImage;

	public Image bgmOnButtonImage;
	public Image bgmOffButtonImage;

	public Image seOnButtonImage;
	public Image seOffButtonImage;

	public Image rightPosButtonImage;
	public Image leftPosButtonImage;

	float alpha = 0.3f;

	// Use this for initialization
	void Start () {
		int playerPos = EncryptedPlayerPrefs.LoadInt (Const.KEY_PLAYERPOS, GameMaster.CtrRight);
		SetPlayerPosImage (playerPos);
		int gameLevel = EncryptedPlayerPrefs.LoadInt (Const.KEY_LEVEL, Const.GAME_LEVEL_NOMAL);
		SetLevelImage (gameLevel);

		bool bgm = EncryptedPlayerPrefs.LoadBool (Const.KEY_BGM_ON, true);
		SetBgmImage (bgm);
		bool se = EncryptedPlayerPrefs.LoadBool (Const.KEY_SE_ON, true);
		SetSeImage (se);
	}

	private void SetBgmImage(bool isOn){
		if (isOn) {
			OffImage (bgmOffButtonImage);
			OnImage (bgmOnButtonImage);
			OffText (bgmOffButtonText);
			OnText (bgmOnButtonText);
		} else {
			OffImage (bgmOnButtonImage);
			OnImage (bgmOffButtonImage);
			OnText (bgmOffButtonText);
			OffText (bgmOnButtonText);
		}

		Sound.Instans.audioSource.mute = !isOn;
	}

	private void SetSeImage(bool isOn){
		if (isOn) {
			OffImage (seOffButtonImage);
			OnImage (seOnButtonImage);
			OnText (seOnButtonText);
			OffText (seOffButtonText);
		} else {
			OffImage (seOnButtonImage);
			OnImage (seOffButtonImage);
			OffText (seOnButtonText);
			OnText (seOffButtonText);
		}

		Sound.Instans.seAudioSource.mute = !isOn;
	}

	private void SetLevelImage(int level){

		if (level == Const.GAME_LEVEL_EASY) {
			OffImage (heardButtonImage);
			OffImage (nomalButtonImage);
			OnImage (easyButtonImage);
			OffText (heardButtonText);
			OffText (nomalButtonText);
			OnText (easyButtonText);
		}
		if (level == Const.GAME_LEVEL_NOMAL) {
			OffImage (heardButtonImage);
			OnImage (nomalButtonImage);
			OffImage (easyButtonImage);
			OffText (heardButtonText);
			OnText (nomalButtonText);
			OffText (easyButtonText);
		}
		if (level == Const.GAME_LEVEL_HEARD) {
			OnImage (heardButtonImage);
			OffImage (nomalButtonImage);
			OffImage (easyButtonImage);
			OnText (heardButtonText);
			OffText (nomalButtonText);
			OffText (easyButtonText);
		}
	}

	private void SetPlayerPosImage(int pos){
		
		if (pos == GameMaster.CtrLeft) {
			OffImage (rightPosButtonImage);
			OnImage (leftPosButtonImage);
		}
		if (pos == GameMaster.CtrRight) {
			OffImage (leftPosButtonImage);
			OnImage (rightPosButtonImage);
		}
	}

	private void OffImage(Image image){
		DOTween.ToAlpha (() => image.color, color => image.color = color, alpha, 0f);
	}
	private void OnImage(Image image){
		DOTween.ToAlpha (() => image.color, color => image.color = color, 1f, 0f);
	}

	private void OffText(GameObject obj){
		TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI> ();
		DOTween.ToAlpha (() => textMesh.color, color => textMesh.color = color, alpha, 0f);
	}
	private void OnText(GameObject obj){
		TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI> ();
		DOTween.ToAlpha (() => textMesh.color, color => textMesh.color = color, 1f, 0f);
	}

	public void OnClickLevelButton(int level){
		EncryptedPlayerPrefs.SaveInt (Const.KEY_LEVEL, level);
		SetLevelImage (level);
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
	}

	public void OnClickBgmButton(bool isOn){
		EncryptedPlayerPrefs.SaveBool (Const.KEY_BGM_ON, isOn);
		SetBgmImage (isOn);
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
	}

	public void OnClickSeButton(bool isOn){
		EncryptedPlayerPrefs.SaveBool (Const.KEY_SE_ON, isOn);
		SetSeImage (isOn);
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
	}

	public void OnClickPosButton(int pos){
		EncryptedPlayerPrefs.SaveInt (Const.KEY_PLAYERPOS, pos);
		SetPlayerPosImage (pos);
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
	}

	public void OnClickSettingSubmitButton(){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		settingPanel.SetActive (false);
	}
}
