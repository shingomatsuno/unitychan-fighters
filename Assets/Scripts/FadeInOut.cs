using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
public class FadeInOut : MonoBehaviour {

	public static FadeInOut Instance;

	public Image fadePanelImage; 
	public GameObject pausePanel;

	public GameObject pauseText;
	public GameObject okButton;
	public GameObject ngButton;

	void Awake(){

		if (Instance) {
			DestroyImmediate (gameObject);
		} else {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	public void FadeIn(){
		Sequence seq = DOTween.Sequence ();
		seq.Append(DOTween.ToAlpha (() => fadePanelImage.color, color => fadePanelImage.color = color, 1f, 0f));
		seq.Append(DOTween.ToAlpha (() => fadePanelImage.color, color => fadePanelImage.color = color, 0f, 1.5f));
		seq.OnComplete (() => {
			gameObject.SetActive(false);
		});
		seq.Play ();
	}

	public void FadeOut(){
		Sequence seq = DOTween.Sequence ();
		seq.Append(DOTween.ToAlpha (() => fadePanelImage.color, color => fadePanelImage.color = color, 0f, 0f));
		seq.Append(DOTween.ToAlpha (() => fadePanelImage.color, color => fadePanelImage.color = color, 1f, 1.5f));
		seq.Play ();
	}

	public void OpenPausePanel(){
		Sound.Instans.audioSource.Pause ();
		DOTween.ToAlpha (() => fadePanelImage.color, color => fadePanelImage.color = color, 100.0f/255.0f, 0f);
		if (SceneManager.GetActiveScene ().name == "Title") {
			pauseText.GetComponent<TextMeshProUGUI> ().text = "ゲームをやめる？";
			okButton.GetComponent<TextMeshProUGUI> ().text = "やめる";
			ngButton.GetComponent<TextMeshProUGUI> ().text = "やめない";
		} else {
			pauseText.GetComponent<TextMeshProUGUI> ().text = "たいとるにもどる？";
			okButton.GetComponent<TextMeshProUGUI> ().text = "もどる";
			ngButton.GetComponent<TextMeshProUGUI> ().text = "もどらない";
		}
		pausePanel.SetActive (true);
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		Time.timeScale = 0;
	}

	public void OnClickResetButton(){
		GameMaster.Instance.mode = GameMaster.GameMode.Title;
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		Time.timeScale = 1;
		//フェードアウト
		Sequence seq = DOTween.Sequence ();
		seq.Append(DOTween.ToAlpha (() => fadePanelImage.color, color => fadePanelImage.color = color, 100.0f/255.0f, 0f));
		seq.Append(DOTween.ToAlpha (() => fadePanelImage.color, color => fadePanelImage.color = color, 1f, 1.5f));
		seq.Play ();
		pausePanel.SetActive(false);
		if (SceneManager.GetActiveScene ().name == "Title") {
			Sound.Instans.PlaySe (Sound.Instans.endVoice);
			DOVirtual.DelayedCall(1.5f,()=>{
				pausePanel.SetActive(false);
				gameObject.SetActive(false);
				Application.Quit();
			});
		} else {
			DOVirtual.DelayedCall(2f,()=>{
				SceneManager.LoadScene ("Title");
			});
		}
	}
	public void OnClickCanselButton(){
		if (GameMaster.Instance.state != GameMaster.GameState.End){
			Sound.Instans.audioSource.Play ();
		}
		Sound.Instans.PlaySe (Sound.Instans.cancelSound);
		Time.timeScale = 1;
		pausePanel.SetActive(false);
		gameObject.SetActive(false);
	}
}
