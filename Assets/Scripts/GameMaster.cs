using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Advertisements;
public class GameMaster : MonoBehaviour {

	public static GameMaster Instance;

	public GameObject fadeCanvas;
	public AudioClip mainBgm;

	public GameMode mode;

	public int player1Chara;
	public int player2Chara;
	public bool isTalk;

	public int p1SGage;
	public int p2SGage;

	// キャラタイプ
	public int charaType1{
		get{
			if (PlayerInfo.KOHAKU <= player1Chara && player1Chara <= PlayerInfo.KOHAKU5) {
				return PlayerInfo.KOHAKU;
			} else if (PlayerInfo.YUKO <= player1Chara && player1Chara <= PlayerInfo.YUKO5) {
				return PlayerInfo.YUKO;
			} else if (PlayerInfo.MISAKI <= player1Chara && player1Chara <= PlayerInfo.MISAKI5) {
				return PlayerInfo.MISAKI;
			} else {
				return player1Chara;
			}
		}
	}
	public int charaType2{
		get{
			if (PlayerInfo.KOHAKU <= player2Chara && player2Chara <= PlayerInfo.KOHAKU5) {
				return PlayerInfo.KOHAKU;
			} else if (PlayerInfo.YUKO <= player2Chara && player2Chara <= PlayerInfo.YUKO5) {
				return PlayerInfo.YUKO;
			} else if (PlayerInfo.MISAKI <= player2Chara && player2Chara <= PlayerInfo.MISAKI5) {
				return PlayerInfo.MISAKI;
			} else {
				return player2Chara;
			}
		}
	}

	public int stage;
	public List<int> stageCharas;

	public int ctrPos;

	public static int CtrLeft = 1;
	public static int CtrRight = 2;

	public int round;
	public int p1Win;
	public int p2Win;
	public string GameModeName{
		get{
			return GetModeName (mode);
		}
	}

	public GameState state;

	public enum GameState{
		Ready,
		Start,
		End,
		Ending
	}

	void Awake(){

		if (Instance) {
			DestroyImmediate (gameObject);
		} else {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		// 設定を反映
		bool bgm = EncryptedPlayerPrefs.LoadBool (Const.KEY_BGM_ON, true);
		Sound.Instans.audioSource.mute = !bgm;
		bool se = EncryptedPlayerPrefs.LoadBool (Const.KEY_SE_ON, true);
		Sound.Instans.seAudioSource.mute = !se;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			OnClickPauseButton ();
		}
	}

	public enum GameMode{
		Title,
		OnePlayerMode,
		TwoPlayerMode,
		EndlessMode
	}


	public static string GetCharaName(int chara){

		if (chara == PlayerInfo.KOHAKU) {
			return "こはく";
		} else if (chara == PlayerInfo.YUKO) {
			return "ゆうこ";
		} else if (chara == PlayerInfo.MISAKI) {
			return "みさき";
		} else {
			return "やみこはく";
		}
	}

	public static string GetSystemCharaName(int chara){

		if (chara == PlayerInfo.KOHAKU) {
			return "Kohaku";
		} else if (chara == PlayerInfo.YUKO) {
			return "Yuko";
		} else if (chara == PlayerInfo.MISAKI) {
			return "Misaki";
		} else {
			return "BlackKohaku";
		}
	}

	public void FadeIn(){
		fadeCanvas.SetActive (true);
		fadeCanvas.SendMessage ("FadeIn");
	}

	public void FadePanelOff(){
		fadeCanvas.SetActive (false);
	}

	public void FadeOut(){
		fadeCanvas.SetActive (true);
		fadeCanvas.SendMessage ("FadeOut");
	}

	public string GetModeName(GameMode mode){
		if (mode == GameMode.OnePlayerMode) {
			return "アーケードモード";
		} else if (mode == GameMode.TwoPlayerMode) {
			return "たいせんモード";
		} else if (mode == GameMode.EndlessMode) {
			return "かちぬきモード";
		}
		return "";
	}

	public void OnClickPauseButton(){
		fadeCanvas.SetActive (true);
		fadeCanvas.SendMessage ("OpenPausePanel");
	}

	public List<int> InitStageCharas(){
		//ランダムでアーケードモードの各ステージのキャラを選択
		int[] charas = new int[]{PlayerInfo.KOHAKU,PlayerInfo.YUKO,PlayerInfo.MISAKI};
		charas = charas.OrderBy (x => Guid.NewGuid ()).ToArray();
		List<int> list = new List<int> ();
		list.AddRange (charas);
		list.Add (PlayerInfo.BLACKKOHAKU);
		return list;
	}
}
