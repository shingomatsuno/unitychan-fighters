using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Endng : MonoBehaviour {

	public GameObject endrollPanel;
	public GameObject endRoll;
	public GameObject endText;
	public GameObject talk;
	public AudioClip endingSong;

	public GameObject talkCanvas;
	public GameObject kohakuImageObj;
	public Sprite nomalKohaku;
	public Sprite egaoKohaku;

	Image kohakuImage;

	// Use this for initialization
	void Start () {
		Sound.Instans.audioSource.Stop ();
		kohakuImage = kohakuImageObj.GetComponent<Image> ();
		GameMaster.Instance.FadeIn ();
		Sound.Instans.PlayBgm (endingSong);
		Invoke ("ShowTalkCanvas",3f);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ShowEndPanel(){
		GameMaster.Instance.OnClickPauseButton ();
	}

	public void OnClickOkButton(){
		LoadScene ("Title");
	}

	private void ShowTalkCanvas(){
		EgaoKohaku ();
		talkCanvas.SetActive (true);
		talk.SendMessage ("OnEnding");
	}

	public void EgaoKohaku(){
		kohakuImage.sprite = egaoKohaku;
	}

	public void NomalKohaku(){
		kohakuImage.sprite = nomalKohaku;
	}

	public void End(){
		StartCoroutine (EndRoll ());
	}

	IEnumerator EndRoll(){
		yield return new WaitForSeconds (1f);
		endrollPanel.SetActive (true);
		Image panelImage = endrollPanel.GetComponent<Image> ();
		DOTween.ToAlpha (() => panelImage.color, color => panelImage.color = color, 1f, 1.5f);
		yield return new WaitForSeconds (3f);
		kohakuImage.sprite = null;
		endRoll.transform.DOLocalMoveY (4300f, 60f).SetEase(Ease.Linear).OnComplete(()=>{
			endText.SetActive(true);
		});
	}

	private void LoadScene(string sceneName){
		//フェードアウト
		GameMaster.Instance.FadeOut ();
		DOVirtual.DelayedCall(3f,()=>{
			SceneManager.LoadScene (sceneName);
		});
	}
}
