using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleController : MonoBehaviour {

	public GameObject splashPanel;
	public GameObject mainMenuPanel;
	public GameObject panel;
	public GameObject settingPanel;

	public AudioClip titleBgm;

	int playerPos;

	// Use this for initialization
	void Start () {
		GameMaster.Instance.FadeIn ();
		GameMaster.Instance.stage = 0;
		GameMaster.Instance.round = 0;
		GameMaster.Instance.p1Win = 0;
		GameMaster.Instance.p2Win = 0;
		GameMaster.Instance.mode = GameMaster.GameMode.Title;

		Sound.Instans.PlayBgm (titleBgm);
	}

	public void OnClickCancelButton(){
		Sound.Instans.PlaySe (Sound.Instans.cancelSound);
		panel.SetActive (false);
	}

	public void OnClickOnePlayButton(){
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		panel.SetActive (true);
	}

	public void OnTouchSplash(){
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		splashPanel.SetActive (false);
		mainMenuPanel.SetActive (true);
	}

	public void OnClickStoryButton(){
		GameMaster.Instance.mode = GameMaster.GameMode.OnePlayerMode;
		GameMaster.Instance.stageCharas = GameMaster.Instance.InitStageCharas ();
		StartCoroutine(SceneChange("Select"));
	}

	public void OnClickEndlessButton(){
		GameMaster.Instance.mode = GameMaster.GameMode.EndlessMode;
		StartCoroutine(SceneChange("Select"));
	}

	public void OnClickTwoPlayButton(){
		GameMaster.Instance.mode = GameMaster.GameMode.TwoPlayerMode;
		StartCoroutine(SceneChange("Select"));
	}

	public void OnClickOpenSettingButton(){
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		settingPanel.SetActive (true);
	}

	IEnumerator SceneChange(string sceneName){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		GameMaster.Instance.FadeOut ();
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene ("Select");
	}
}
