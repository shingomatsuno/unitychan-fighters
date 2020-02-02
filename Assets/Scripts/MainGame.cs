using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
public class MainGame : MonoBehaviour {
	
	RewardBasedVideoAd reward;
	public Light light;
	public GameObject[] classRoomArray;
	GameObject classRoom;
	public GameObject mainCanvas;
	public GameObject endCanvas;
	public GameObject coinPanel;
	public GameObject endPanel;
	public GameObject gameOverPanel;
	public GameObject retryButton;
	public GameObject nextButtonText;
	public GameObject gameOverText;
	public GameObject katinukiKekkaPanel;
	public GameObject katinukiText;
	public GameObject katinukiContinuePanel;
	public GameObject afterContinuePanel;
	public Button rewardButton;
	public Button continueButton;
	public GameObject continueText;
	public Button katinukiContinueButton;
	public GameObject katinukiContinueText;
	public GameObject levelTextPanel;
	public GameObject levelText;

	public GameObject rewardButtonObj;
	public GameObject rewardImage;

	public GameObject r1;
	public GameObject r2;
	public GameObject r3;

	public GameObject talkEvent;

	public GameObject katinukiScore;
	public GameObject katinukiMaxScore;

	public GameObject name1;
	public GameObject name2;

	public GameObject coinText;
	public GameObject videoText;

	public Image shareImage;

	public GameObject controller2;
	public GameObject controllerLeft;
	public GameObject controllerRight;

	public Image p1Win1;
	public Image p2Win1;

	public Image p1Win2;
	public Image p2Win2;

	public GameObject[] playerList;

	public GameObject pausePanel;

	public GameObject enagyBar1;
	public GameObject enagyBar2;
	public GameObject sBar1;
	public GameObject sBar2;

	public GameObject currentCoinPanel;
	public GameObject currentCoinText;

	Vector3 player1LPos = new Vector3(-0.15f,0f,0f);
	Vector3 player2LPos = new Vector3(0.3f,0f,-0.15f);

	Quaternion player1LRotate = Quaternion.Euler(new Vector3(0f,120f,0f));
	Quaternion player2LRotate = Quaternion.Euler(new Vector3(0f,290f,0f));

	Vector3 player1RPos = new Vector3(-0.15f,0f,-0.15f);
	Vector3 player2RPos  = new Vector3(0.3f,0f,0f);

	Quaternion player1RRotate = Quaternion.Euler(new Vector3(0f,-280f,0f));
	Quaternion player2RRotate = Quaternion.Euler(new Vector3(0f,240f,0f));

	GameObject player1;
	GameObject player2;

	int level;

	int p1Life;
	int p2Life;
	string player1Name;
	string player2Name;

	//勝ち抜きモードで一度だけコンテニューするフラグ
	bool isContinue;

	//獲得したコイン
	int gotCoins;
	int chareKatinukiCount;

	// Use this for initialization
	void Start () {
		if (!Advertisement.IsReady ("rewardedVideo")) {
			RequestReward ();
		}
		classRoom = classRoomArray [GameMaster.Instance.stage];
		if (GameMaster.Instance.stage == 1) {
			Color color = new Color();
			color.r = 240f/255f;
			color.g = 231f/255f;
			color.b = 206f/255f;
			light.color = color;
		}
		if (GameMaster.Instance.stage == 2) {
			Color color = new Color();
			color.r = 182f/255f;
			color.g = 182f/255f;
			color.b = 182f/255f;
			light.color = color;
		}
		if (GameMaster.Instance.round == 0) {
			GameMaster.Instance.FadeIn ();
		}
		level = EncryptedPlayerPrefs.LoadInt (Const.KEY_LEVEL, Const.GAME_LEVEL_NOMAL);

     	GameMaster.Instance.state = GameMaster.GameState.Ready;
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode
			||GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			if (GameMaster.Instance.round == 0) {

				Sound.Instans.audioSource.Stop ();

			}
		}
		if (GameMaster.Instance.mode != GameMaster.GameMode.EndlessMode) {
			if (GameMaster.Instance.p1Win == 1) {
				p1Win1.enabled = true;
			}
			if (GameMaster.Instance.p1Win == 2) {
				p1Win1.enabled = true;
				p1Win2.enabled = true;
			}
			if (GameMaster.Instance.p2Win == 1) {
				p2Win1.enabled = true;
			}
			if (GameMaster.Instance.p2Win == 2) {
				p2Win1.enabled = true;
				p2Win2.enabled = true;
			}
		}

		Init ();
		StartCoroutine(RoundStart());
	}


	public void Init(){

		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode) {
			//アーケードモード
			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				int p1 = GameMaster.Instance.player1Chara;
				int p2 = GameMaster.Instance.stageCharas[GameMaster.Instance.stage];

				if (p1 == p2) {
					//同じなら敵が服違い
					p2 += 1;
				}

				GameMaster.Instance.player2Chara = p2;

				player1 = Instantiate(playerList [p1]) as GameObject;
				player2 = Instantiate(playerList [p2]) as GameObject;
			} else {
				int p1 = GameMaster.Instance.stageCharas[GameMaster.Instance.stage];
				int p2 = GameMaster.Instance.player2Chara;
				if (p1 == p2) {
					//同じなら敵が服違い
					p1 += 1;
				}

				GameMaster.Instance.player1Chara = p1;
				player1 = Instantiate(playerList [p1]) as GameObject;
				player2 = Instantiate(playerList [p2]) as GameObject;
			}

			//キャラ対面アップ
			if (GameMaster.Instance.round == 0) {
				player1.transform.position = new Vector3(-0.4f,0f,0f);
				player1.transform.rotation = Quaternion.Euler(new Vector3(0f,122.4f,0f));

				player2.transform.position = new Vector3(0.4f,0f,0f);
				player2.transform.rotation = Quaternion.Euler(new Vector3(0f,-122.4f,0f));
			}
		} else if(GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode){
			//ランダム
			int random = Random.Range (0, playerList.Length);
			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				int p1 = GameMaster.Instance.player1Chara;
				int p2 = random;
				GameMaster.Instance.player2Chara = p2;
				player1 = Instantiate(playerList [p1]) as GameObject;
				player2 = Instantiate(playerList [p2]) as GameObject;
			} else {
				int p1 = random;
				int p2 = GameMaster.Instance.player2Chara;
				GameMaster.Instance.player1Chara = p1;
				player1 = Instantiate(playerList [p1]) as GameObject;
				player2 = Instantiate(playerList [p2]) as GameObject;
			}

			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				player1.transform.position = player1LPos;
				player2.transform.position = player2LPos;

				player1.transform.rotation = player1LRotate;
				player2.transform.rotation = player2LRotate;
			} else {
				player1.transform.position = player1RPos;
				player2.transform.position = player2RPos;

				player1.transform.rotation = player1RRotate;
				player2.transform.rotation = player2RRotate;
			}

		}else if(GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode){
			
			int p1 = GameMaster.Instance.player1Chara;
			int p2 = GameMaster.Instance.player2Chara;

			player1 = Instantiate(playerList [p1]) as GameObject;
			player2 = Instantiate(playerList [p2]) as GameObject;
			//キャラ対面アップ
			if (GameMaster.Instance.round == 0) {
				player1.transform.position = new Vector3 (-0.4f, 0f, 0f);
				player1.transform.rotation = Quaternion.Euler (new Vector3 (0f, 122.4f, 0f));

				player2.transform.position = new Vector3 (0.4f, 0f, 0f);
				player2.transform.rotation = Quaternion.Euler (new Vector3 (0f, -122.4f, 0f));
			}
		}

		player1.name = "Player1";
		player2.name = "Player2";

		p1Life = Const.MAX_LIFE;
		p2Life = Const.MAX_LIFE;

		player1.tag = Const.TAG_PLAYER;
		player2.tag = Const.TAG_PLAYER;
		player1Name = GameMaster.GetCharaName (GameMaster.Instance.charaType1);
		player2Name = GameMaster.GetCharaName (GameMaster.Instance.charaType2);

		player1.SendMessage ("Init", PlayerInfo.PlayerType.Player1);
		player2.SendMessage ("Init", PlayerInfo.PlayerType.Player2);

		if (GameMaster.Instance.mode != GameMaster.GameMode.EndlessMode) {

			//黒板に名前表示
			GameObject room = Instantiate (classRoom, classRoom.transform.position, classRoom.transform.rotation) as GameObject;
			TextMeshPro tm = room.GetComponentInChildren<TextMeshPro> ();
			if (tm != null) {
				room.GetComponentInChildren<TextMeshPro> ().text = player1Name + "  VS  " + player2Name;
			} else {
				name1.GetComponent<TextMeshProUGUI> ().color = Color.white;
				name2.GetComponent<TextMeshProUGUI> ().color = Color.white;
				room.transform.position = new Vector3 (0f, -0.05f, 0f);
			}
		}
		if (GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {
			
			GameObject room = Instantiate (classRoom, classRoom.transform.position, classRoom.transform.rotation) as GameObject;
			room.GetComponentInChildren<TextMeshPro> ().text = "かちぬきモード";
		}
	}

	IEnumerator RoundStart(){
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode
			|| GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			//キャラ対面アップ
			if (GameMaster.Instance.round == 0) {
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.battleStart);

				yield return new WaitForSeconds (4f);

				if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode) {
					//対面会話
					talkEvent.SendMessage ("OnBegin");
					GameMaster.Instance.isTalk = true;
					while(true){
						if (!GameMaster.Instance.isTalk) {
							break;
						}
						yield return null;
					}
				}
				if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode) {

					if (IsPlayer (PlayerInfo.PlayerType.Player1)) {
						if (GameMaster.Instance.charaType2 == PlayerInfo.BLACKKOHAKU) {
							Sound.Instans.PlayBgm (SoundMainScene.Instans.lastBattle);
						} else {
							Sound.Instans.PlayBgm(GameMaster.Instance.mainBgm);
						}
					} else {
						if (GameMaster.Instance.charaType1 == PlayerInfo.BLACKKOHAKU) {
							Sound.Instans.PlayBgm (SoundMainScene.Instans.lastBattle);
						} else {
							Sound.Instans.PlayBgm(GameMaster.Instance.mainBgm);
						}
					}
				} else {
					Sound.Instans.PlayBgm(GameMaster.Instance.mainBgm);
				}
			} 
		} else {
			if (GameMaster.Instance.round == 0) {
				Sound.Instans.PlayBgm (GameMaster.Instance.mainBgm);
			}
		}
		BattleStart ();
		InitCanvas ();
		yield return new WaitForSeconds (1.5f);
		//よーい
		r1.SetActive (true);
		SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.ready);
		yield return new WaitForSeconds(2.5f);
		SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.go);
		r1.SetActive (false);
		//はじめ！
		r2.SetActive (true);
		yield return new WaitForSeconds(0.5f);
		//スタート
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode || 
			GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode){
			if (!IsPlayer (PlayerInfo.PlayerType.Player1)) {
				player1.SendMessage ("SetPlay");
			} 
			if(!IsPlayer (PlayerInfo.PlayerType.Player2)){
				player2.SendMessage ("SetPlay");
			}
		}
		GameMaster.Instance.state = GameMaster.GameState.Start;
		r2.SetActive (false);
	}
	private void BattleStart(){
		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			player1.transform.position = player1LPos;
			player2.transform.position = player2LPos;

			player1.transform.rotation = player1LRotate;
			player2.transform.rotation = player2LRotate;
		} else {
			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				player1.transform.position = player1LPos;
				player2.transform.position = player2LPos;

				player1.transform.rotation = player1LRotate;
				player2.transform.rotation = player2LRotate;
			} else {
				player1.transform.position = player1RPos;
				player2.transform.position = player2RPos;

				player1.transform.rotation = player1RRotate;
				player2.transform.rotation = player2RRotate;
			}
		}

		player1.SendMessage ("SetOnStart",player1.transform);
		player2.SendMessage ("SetOnStart",player2.transform);

		Camera.main.gameObject.SendMessage ("Init");
	}

	private string GetLevelText(int level){
		if (level == Const.GAME_LEVEL_EASY) {
			return "よわい";
		} else if (level == Const.GAME_LEVEL_NOMAL) {
			return "ふつう";
		}else if(level == Const.GAME_LEVEL_HEARD){
			return "つよい";
		}
		return "";
	}

	private void InitCanvas(){
		mainCanvas.SetActive (true);
		name1.GetComponent<TextMeshProUGUI> ().text = player1Name;
		name2.GetComponent<TextMeshProUGUI> ().text = player2Name;
		if(GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode){
			levelTextPanel.SetActive (true);
			levelText.GetComponent<TextMeshProUGUI> ().text = "つよさ　" + GetLevelText(level);
		}
		//モードによってコントローラー出しわけ
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode
			|| GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {

			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				controllerLeft.SetActive (true);
			}
			if (GameMaster.Instance.ctrPos == GameMaster.CtrRight) {
				controllerRight.SetActive (true);
			}

			if (GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {
				katinukiScore.GetComponent<TextMeshProUGUI> ().text = string.Format("{0:D}にんぬき",GameMaster.Instance.round);
				katinukiMaxScore.GetComponent<TextMeshProUGUI> ().text = string.Format("(さいこう{0:D})",EncryptedPlayerPrefs.LoadInt(Const.KEY_KATINUKI_MAX_SCORE,0));
				katinukiScore.SetActive (true);
				katinukiMaxScore.SetActive (true);
			}
		}
		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			controller2.SetActive(true);
		}

		enagyBar1.SendMessage ("SetBar",p1Life);
		enagyBar2.SendMessage ("SetBar",p2Life);
		if (GameMaster.Instance.round == 0) {
			GameMaster.Instance.p1SGage = 0;
			GameMaster.Instance.p2SGage = 0;
		}
		player1.SendMessage ("InitSSButton", GameMaster.Instance.p1SGage);
		player2.SendMessage ("InitSSButton", GameMaster.Instance.p2SGage);

		player1.SendMessage ("SetSgage", GameMaster.Instance.p1SGage);
		player2.SendMessage ("SetSgage", GameMaster.Instance.p2SGage);

		sBar1.SendMessage ("SetSuperBar", GameMaster.Instance.p1SGage);
		sBar2.SendMessage ("SetSuperBar", GameMaster.Instance.p2SGage);
	}

	public void HitDamage(PlayerInfo playerInfo){
		if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
			enagyBar1.SendMessage ("SetBar",playerInfo.life);
			p1Life = playerInfo.life;
		} else {
			enagyBar2.SendMessage ("SetBar",playerInfo.life);
			p2Life = playerInfo.life;
		}
		if (playerInfo.life <= 0) {
			if (GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {
				if (GameMaster.Instance.state != GameMaster.GameState.End) {
					GameMaster.Instance.state = GameMaster.GameState.End;
					StartCoroutine (Restart ());
				}
			} else {
				if (GameMaster.Instance.state != GameMaster.GameState.End) {
					GameMaster.Instance.state = GameMaster.GameState.End;
					StartCoroutine (Restart ());
				}
			}
		}
	}

	public void AddSuperBar(PlayerInfo playerInfo){
		if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
			sBar1.SendMessage ("SetSuperBar",playerInfo.sGage);
			GameMaster.Instance.p1SGage = playerInfo.sGage;
		} else {
			sBar2.SendMessage ("SetSuperBar",playerInfo.sGage);
			GameMaster.Instance.p2SGage = playerInfo.sGage;
		}
	}

	IEnumerator Restart(){
		yield return new WaitForSeconds (1f);
		//結果判定
		PlayerInfo p1Info = player1.GetComponent<PlayerController>().playerInfo;
		PlayerInfo p2Info = player2.GetComponent<PlayerController>().playerInfo;
		if (p1Info.life <= 0 && p2Info.life <= 0) {
			//DROW 勝ち抜きモードの場合は負け
		} else if (p1Info.life <= 0) {
			//2pWIN
			GameMaster.Instance.p2Win += 1;
		} else if (p2Info.life <= 0) {
			//1pWIN
			GameMaster.Instance.p1Win += 1;
		}
		yield return new WaitForSeconds (2f);
		if (GameMaster.Instance.mode != GameMaster.GameMode.EndlessMode) {
			if (GameMaster.Instance.p1Win == 1) {
				p1Win1.enabled = true;
			}
			if (GameMaster.Instance.p1Win == 2) {
				p1Win1.enabled = true;
				p1Win2.enabled = true;
			}
			if (GameMaster.Instance.p2Win == 1) {
				p2Win1.enabled = true;
			}
			if (GameMaster.Instance.p2Win == 2) {
				p2Win1.enabled = true;
				p2Win2.enabled = true;
			}
		}

		if (GameMaster.Instance.mode != GameMaster.GameMode.EndlessMode) {
			GameMaster.Instance.round += 1;
			if (GameMaster.Instance.p1Win == 2) {
				StartCoroutine ("ShowEndCanvas", PlayerInfo.PlayerType.Player1);
			} else if (GameMaster.Instance.p2Win == 2) {
				StartCoroutine ("ShowEndCanvas", PlayerInfo.PlayerType.Player2);
			} else {
				SceneManager.LoadScene ("Main");
			}
		} else {
			//勝ち抜きモード
			if (IsPlayer (PlayerInfo.PlayerType.Player1) && p1Info.life <= 0) {
				//自分が1pかつ自分のライフが０
				if (!isContinue) {
					//勝ち抜きモードの場合動画広告を見ると1度だけ回復できる
					DOVirtual.DelayedCall (1.5f, () => {
						katinukiContinuePanel.SetActive (true);
					});
				} else {
					StartCoroutine ("ShowEndCanvas", PlayerInfo.PlayerType.Player1);
				}

			} else if (IsPlayer (PlayerInfo.PlayerType.Player2) && p2Info.life <= 0) {
				//自分が2pかつ自分のライフが０
				if (!isContinue) {
					//勝ち抜きモードの場合動画広告を見ると1度だけ回復できる
					DOVirtual.DelayedCall (1.5f, () => {
						katinukiContinuePanel.SetActive (true);
					});
				} else {
					StartCoroutine ("ShowEndCanvas", PlayerInfo.PlayerType.Player2);
				}
			} else {
				GameMaster.Instance.state = GameMaster.GameState.Start;
				GameMaster.Instance.round += 1;
				//スコア更新
				katinukiScore.GetComponent<TextMeshProUGUI> ().text = string.Format("{0:D}にんぬき",GameMaster.Instance.round);
				int maxScore = EncryptedPlayerPrefs.LoadInt (Const.KEY_KATINUKI_MAX_SCORE, 0);
				if (maxScore < GameMaster.Instance.round) {
					EncryptedPlayerPrefs.SaveInt (Const.KEY_KATINUKI_MAX_SCORE, GameMaster.Instance.round);
					katinukiMaxScore.GetComponent<TextMeshProUGUI> ().text = string.Format("(さいこう{0:D})",EncryptedPlayerPrefs.LoadInt(Const.KEY_KATINUKI_MAX_SCORE,0));
				}

				// 次の相手を投入
				if (p1Info.life <= 0) {
					player1.GetComponent<InitPosition> ().enabled = false;
					player1.transform.DOMoveX (-4.5f, 0.2f).OnComplete (() => {
						Destroy(player1);
					});

					yield return new WaitForSeconds (0.5f);
					int random = Random.Range(0,playerList.Length);
					int p1 = random;
					GameMaster.Instance.player1Chara = p1;
					player1 = Instantiate(playerList [p1],new Vector3(-4.5f,0f,0f),player1RRotate) as GameObject;
					player1.SendMessage ("Init", PlayerInfo.PlayerType.Player1);
					enagyBar1.SendMessage ("SetBar",Const.MAX_LIFE);

					player1.transform.DOMove(player1RPos,0.5f).OnComplete(()=>{
						player1.transform.position = player1RPos;
						player1.transform.rotation = player1RRotate;
						player1.SendMessage("OnStart",player1.transform);
						player1.SendMessage ("SetPlay");
						player1.SendMessage ("SetSgage", GameMaster.Instance.p1SGage);
						sBar1.SendMessage ("SetSuperBar", GameMaster.Instance.p1SGage);
						name1.GetComponent<TextMeshProUGUI> ().text = GameMaster.GetCharaName (GameMaster.Instance.charaType1);
					});

				}else if(p2Info.life <= 0){
					player2.GetComponent<InitPosition> ().enabled = false;
					player2.transform.DOMoveX (4.5f, 0.2f).OnComplete (() => {
						Destroy(player2);
					});

					yield return new WaitForSeconds (0.5f);
					int random = Random.Range(0,playerList.Length);
					int p2 = random;
					GameMaster.Instance.player2Chara = p2;
					player2 = Instantiate(playerList [p2],new Vector3(4.5f,0f,0f),player2LRotate) as GameObject;
					player2.SendMessage ("Init", PlayerInfo.PlayerType.Player2);
					enagyBar2.SendMessage ("SetBar",Const.MAX_LIFE);

					player2.transform.DOMove(player2LPos,0.5f).OnComplete(()=>{
						player2.transform.position = player2LPos;
						player2.transform.rotation = player2LRotate;
						player2.SendMessage("OnStart",player2.transform);
						player2.SendMessage ("SetPlay");
						player2.SendMessage ("SetSgage", GameMaster.Instance.p2SGage);
						sBar2.SendMessage ("SetSuperBar", GameMaster.Instance.p2SGage);
						name2.GetComponent<TextMeshProUGUI> ().text = GameMaster.GetCharaName (GameMaster.Instance.charaType2);;
					});
				}
			}
		}
	}

	IEnumerator ShowEndCanvas(PlayerInfo.PlayerType winPlayerType){
		//そこまで！
		r3.SetActive (true);
		SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.sokomade);
		Sound.Instans.Stop ();
		yield return new WaitForSeconds (2);

		mainCanvas.SetActive (false);
		endCanvas.SetActive (true);

		GameMaster.Instance.p1Win = 0;
		GameMaster.Instance.p2Win = 0;

		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode 
			|| GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {

			GameMaster.Instance.round = 0;
			if (IsPlayer (winPlayerType) 
				|| GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {

				if (winPlayerType == PlayerInfo.PlayerType.Player1) {
					OnVictory(GameMaster.Instance.charaType1,player1);
				} else {
					OnVictory (GameMaster.Instance.charaType2,player2);
				}

				// 勝ちキャラアップ
				Camera.main.gameObject.SendMessage ("Win",winPlayerType);

				yield return new WaitForSeconds (3);
				// キャラのセリフ
				if (winPlayerType == PlayerInfo.PlayerType.Player1) {
					talkEvent.SendMessage("OnSerihu",GameMaster.Instance.charaType1);
				} else {
					talkEvent.SendMessage("OnSerihu",GameMaster.Instance.charaType2);
				}
				yield return new WaitForSeconds (0.5f);
			}
		}

		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			//対戦モード
			yield return new WaitForSeconds (1.5f);
			Application.CaptureScreenshot("screenShot.png");
			yield return new WaitForSeconds (1.5f);
			SetShareImage ();
			//二人プレイ
			endPanel.SetActive (true);
			retryButton.SetActive (true);
			nextButtonText.GetComponent<TextMeshProUGUI>().text = "きゃらせんたく";
		} else if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode) {
			//ストーリーモード ゲームオーバー判定
			if (IsPlayer(winPlayerType)) {
				yield return new WaitForSeconds (1.5f);
				Application.CaptureScreenshot("screenShot.png");
				yield return new WaitForSeconds (1.5f);
				//シェア画像生成
				SetShareImage ();
				int currentCoin = EncryptedPlayerPrefs.LoadInt (Const.KEY_COIN, 0);
				currentCoinText.GetComponent<TextMeshProUGUI> ().text = currentCoin.ToString();
				coinPanel.SetActive (true);
				//コイン計算
				gotCoins = level * (GameMaster.Instance.stage + 1) * 10;
				SetCoinText (gotCoins);
				CountUpCoin (currentCoin, gotCoins);
				endPanel.SetActive (true);
				//ステージ繰り上げ
				GameMaster.Instance.stage += 1;

			} else {
				// ゲームオーバーの場合
				levelTextPanel.SetActive (false);
				// カメラ移動
				if(GameMaster.Instance.ctrPos == GameMaster.CtrLeft){
					Destroy (player2);
					// カメラ移動
					Camera.main.gameObject.SendMessage("Lose",PlayerInfo.PlayerType.Player1);
				}
				if (GameMaster.Instance.ctrPos == GameMaster.CtrRight) {
					Destroy (player1);
					Camera.main.gameObject.SendMessage("Lose",PlayerInfo.PlayerType.Player2);
				}
				yield return new WaitForSeconds (1.5f);
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.zannen);
				gameOverPanel.SetActive (true);
				gameOverText.SetActive (true);
			}
		} else {
			//勝ち抜きモード
			if (winPlayerType == PlayerInfo.PlayerType.Player1) {
				Destroy (player2);
				transform.position = player1LPos;
				OnVictoryNonVoice(GameMaster.Instance.charaType1,player1);
				// 負けキャラアップ
				Camera.main.gameObject.SendMessage ("Win",PlayerInfo.PlayerType.Player1);
			} else {
				Destroy (player1);
				OnVictoryNonVoice (GameMaster.Instance.charaType2,player2);
				// 負けキャラアップ
				Camera.main.gameObject.SendMessage ("Win",PlayerInfo.PlayerType.Player2);
			}
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kekka);
			// 結果発表
			yield return new WaitForSeconds (3f);
			katinukiKekkaPanel.SetActive (true);
			yield return new WaitForSeconds (1.5f);
			katinukiText.GetComponent<TextMeshProUGUI>().text = GameMaster.Instance.round + "にんぬき！";
			katinukiText.SetActive (true);
			yield return new WaitForSeconds (0.5f);
			Application.CaptureScreenshot("screenShot.png");
			yield return new WaitForSeconds (1.5f);
			SetShareImage ();

			int currentCoin = EncryptedPlayerPrefs.LoadInt (Const.KEY_COIN, 0);
			currentCoinText.GetComponent<TextMeshProUGUI> ().text = currentCoin.ToString();
			yield return new WaitForSeconds (0.5f);
			//勝ち抜き数で評価ボイス
			if (0 <= GameMaster.Instance.round && GameMaster.Instance.round <= 4) {
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.end04);
			} else if (5 <= GameMaster.Instance.round && GameMaster.Instance.round <= 9) {
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.end59);
			} else if (10 <= GameMaster.Instance.round && GameMaster.Instance.round <= 19) {
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.end1019);
			} else if(20 <= GameMaster.Instance.round){
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.end20);
			}
			yield return new WaitForSeconds (0.5f);
			if (GameMaster.Instance.round > 0) {
				gotCoins = GameMaster.Instance.round * 10;
				SetCoinText (gotCoins);
				coinPanel.SetActive (true);
				CountUpCoin (currentCoin, gotCoins);
			}
			endPanel.SetActive (true);
			retryButton.SetActive (true);
			nextButtonText.GetComponent<TextMeshProUGUI>().text = "きゃらせんたく";
			//シェアするための勝ち抜き数を保持
			chareKatinukiCount = GameMaster.Instance.round;
			GameMaster.Instance.round = 0;
		}
	}

	//コインの保存とカウントアップ
	private void CountUpCoin(int currentCoin,int coin){
		int finalVal = currentCoin + coin;
		if (finalVal > Const.MAX_COIN) {
			finalVal = Const.MAX_COIN;
		}
		EncryptedPlayerPrefs.SaveInt (Const.KEY_COIN, finalVal);
		TextMeshProUGUI text = currentCoinText.GetComponent<TextMeshProUGUI> ();
		DOVirtual.DelayedCall(1f,()=>{
			DOTween.To (
				() => currentCoin,
				it => text.text = it.ToString(),
				finalVal,
				0.3f);
		});
	}

	private void SetCoinText(int coin){
		coinText.GetComponent<TextMeshProUGUI> ().text = string.Format("コイン\n{0:D}まいげっと！",coin);
		videoText.GetComponent<TextMeshProUGUI> ().text = string.Format ("どうがをみてあと\n{0:D}まいげっと！", coin);
	}

	private void SetShareImage(){
		//シェア画像をセット
		string imagePath = Application.persistentDataPath + "/screenShot.png";

		if (File.Exists (imagePath)) {

			byte[] image = File.ReadAllBytes (imagePath);
			Texture2D texture = new Texture2D (0,0);
			texture.LoadImage (image);

			Sprite imageSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
			shareImage.sprite = imageSprite;
		}
	}

	private void OnVictory(int charaType,GameObject player){
		if (charaType == PlayerInfo.KOHAKU) {
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuWin);
			player.GetComponent<Animator> ().SetBool ("Win1",true);
		}else if (charaType == PlayerInfo.YUKO) {
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.yukoWin);
			player.GetComponent<Animator> ().SetBool ("Win2",true);
		}else if (charaType == PlayerInfo.MISAKI) {
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.misakiWin);
			player.GetComponent<Animator> ().SetBool ("Win3",true);
		} 
	}


	private void OnVictoryNonVoice(int charaType,GameObject player){
		if (charaType == PlayerInfo.KOHAKU) {
			player.GetComponent<Animator> ().SetBool ("Win1",true);
		}else if (charaType == PlayerInfo.YUKO) {
			player.GetComponent<Animator> ().SetBool ("Win2",true);
		}else if (charaType == PlayerInfo.MISAKI) {
			player.GetComponent<Animator> ().SetBool ("Win3",true);
		} 
	}

	public void OnClickShareButton(){
		// シェア
		Sound.Instans.PlaySe (Sound.Instans.fixSound);

		string shareText = "";
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode) {
			string charaName = "";
			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				charaName = player2Name;
			} else {
				charaName = player1Name;
			}

			shareText = string.Format (Const.SHARE_TEXT_ARCADE, charaName);
		} else if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			shareText = Const.SHARE_TEXT_VS;
		} else if (GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {
			shareText = string.Format (Const.SHARE_TEXT_ENDRESS, chareKatinukiCount);
		}

		string url = "";
		#if UNITY_ANDROID
		url = Const.APPSTORE_URL;
		#elif UNITY_IPHONE
		url = Const.PLAYSTORE_URL;
		#endif
		shareText += "\n#" + Const.GAME_NAME + "\n" + url;

		string fileName = Application.persistentDataPath + "/screenShot.png";
		SocialConnector.SocialConnector.Share (shareText,"",fileName);
	}

	public void OnClickNextButton(){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode) {

			if (IsPlayer (PlayerInfo.PlayerType.Player1)) {
				if (GameMaster.Instance.charaType2 == PlayerInfo.BLACKKOHAKU) {
					// エンディング
					GameMaster.Instance.state = GameMaster.GameState.Ending;
					LoadScene ("Ending");
					return;
				}
			} else {
				if (GameMaster.Instance.charaType1 == PlayerInfo.BLACKKOHAKU) {
					GameMaster.Instance.state = GameMaster.GameState.Ending;
					LoadScene ("Ending");
					return;
				}
			}
			LoadScene ("Main");
		}
		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode
			||GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {
			LoadScene ("Select");
		}
	}

	public void OnClickRetryButton(){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		LoadScene ("Main");
	}

	private bool IsPlayer(PlayerInfo.PlayerType playerType){

		if ((GameMaster.Instance.ctrPos == GameMaster.CtrLeft && playerType == PlayerInfo.PlayerType.Player1)
			|| (GameMaster.Instance.ctrPos == GameMaster.CtrRight && playerType == PlayerInfo.PlayerType.Player2)) {
			return true;
		}

		return false;
	}

	private void LoadScene(string sceneName){
		//フェードアウト
		GameMaster.Instance.FadeOut ();
		Sequence seq = DOTween.Sequence();
		seq.Append ( DOVirtual.DelayedCall(3f,()=>{
			SceneManager.LoadScene (sceneName);
		}));
		seq.OnUpdate(()=>{
			if(GameMaster.Instance.mode == GameMaster.GameMode.Title){
				seq.Kill();
			}
		});
		seq.Play ();

	}

	public void OnClickPauseButton(){
		GameMaster.Instance.OnClickPauseButton ();
	}


	bool rewardSuccess = false;
	bool continueSuccess = false;
	bool katinukiCtSuccess = false;
	bool admobClose = false;
	//勝ち抜きモードでのコンテニュー
	public void OnKatinukiContinue(){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		katinukiContinueButton.enabled = false;
		TextMeshProUGUI k = katinukiContinueText.GetComponent<TextMeshProUGUI> ();
		DOTween.ToAlpha (() => k.color, color => k.color = color, 0.3f, 0f);
		DOVirtual.DelayedCall (1.5f, () => {
			// 動画リワード コンティニュー
			if (Advertisement.IsReady ("rewardedVideo")) {
				var options = new ShowOptions {
					resultCallback = (ShowResult result) => {
						OnKatinukiReward (true);
					}
				};

				Advertisement.Show ("rewardedVideo", options);
			} else if (reward.IsLoaded ()) {
				reward.OnAdRewarded += (object sender, Reward e) => {
					katinukiCtSuccess = true;
				};
				reward.OnAdClosed += (object sender, System.EventArgs e) => {
					admobClose = true;
					RequestReward ();
				};
				reward.Show ();
			} else {
				OnKatinukiReward (true);
			}
		});
	}

	public void OnKatinukiNotContinue(){
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		katinukiContinuePanel.SetActive (false);
		if (IsPlayer (PlayerInfo.PlayerType.Player1)) {
			StartCoroutine ("ShowEndCanvas", PlayerInfo.PlayerType.Player1);
		} else {
			StartCoroutine ("ShowEndCanvas", PlayerInfo.PlayerType.Player2);
		}
	}

	public void OnClickRewardButton(){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		DOVirtual.DelayedCall (1.5f, () => {
			if (Advertisement.IsReady ("rewardedVideo")) {
				var options = new ShowOptions {
					resultCallback = (ShowResult result) => {
						// 動画リワード　コイン2倍
						OnCoinRewrd (true);
					}
				};

				Advertisement.Show ("rewardedVideo", options);
			} else if (reward.IsLoaded ()) {
				reward.OnAdRewarded += (object sender, Reward e) => {
					//動画リワード　コイン2倍
					rewardSuccess = true;
				};
				reward.OnAdClosed += (object sender, System.EventArgs e) => {
					admobClose = true;
					RequestReward ();
				};
				reward.Show ();
			} else {
				OnCoinRewrd (true);
			}
		});

		rewardButton.enabled = false;
		TextMeshProUGUI v = videoText.GetComponent<TextMeshProUGUI> ();
		DOTween.ToAlpha (() => v.color, color => v.color = color, 0.3f, 0f);
	}

	public void OnClickContinueButton(){
		// 動画リワード コンティニュー
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		continueButton.enabled = false;
		TextMeshProUGUI t = continueText.GetComponent<TextMeshProUGUI> ();
		DOTween.ToAlpha (() => t.color, color => t.color = color, 0.3f, 0f);
		DOVirtual.DelayedCall (1.5f, () => {
			if (Advertisement.IsReady ("rewardedVideo")) {
				var options = new ShowOptions {
					resultCallback = (ShowResult result) => {
						AfterContinue();
					}
				};

				Advertisement.Show ("rewardedVideo", options);
			} else if (reward.IsLoaded ()) {
				reward.OnAdRewarded += (object sender, Reward e) => {
					continueSuccess = true;
				};
				reward.OnAdClosed += (object sender, System.EventArgs e) => {
					admobClose = true;
					RequestReward ();
				};
				reward.Show ();
			} else {
				AfterContinue();
			}
		});
	}

	private void OnCoinRewrd(bool isReward){
		if (isReward) {
			Sound.Instans.PlaySe (Sound.Instans.fixSound);
			int currentCoin = EncryptedPlayerPrefs.LoadInt (Const.KEY_COIN, 0);
			RewardCountUpCoin (currentCoin, gotCoins);
		}
	}

	private void OnKatinukiReward(bool isReward){
		katinukiContinuePanel.SetActive (false);
		if (isReward) {
			// 復活ボイス
			isContinue = true;
			if (IsPlayer (PlayerInfo.PlayerType.Player1)) {
				if (GameMaster.Instance.charaType1 == PlayerInfo.KOHAKU) {
					SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuHukkatsu);
				} else if (GameMaster.Instance.charaType1 == PlayerInfo.YUKO) {
					SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.yukoHukkatsu);
				} else if (GameMaster.Instance.charaType1 == PlayerInfo.MISAKI) {
					SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.misakiHukkatsu);
				}
				player1.transform.position = player1LPos;
				player1.transform.rotation = player1LRotate;
				player1.GetComponent<InitPosition> ().enabled = true;
				player1.GetComponent<Animator> ().SetBool ("Continue", true);
				player1.GetComponent<PlayerController> ().playerInfo.life = Const.MAX_LIFE;
				enagyBar1.SendMessage ("SetBar", Const.MAX_LIFE);
				if (player2.GetComponent<PlayerController> ().playerInfo.life <= 0) {
					//相打ちなら倒したことにする
					StartCoroutine (Restart ());
				} else {
					GameMaster.Instance.state = GameMaster.GameState.Start;
				}
			}
			if (IsPlayer (PlayerInfo.PlayerType.Player2)) {
				if (GameMaster.Instance.charaType2 == PlayerInfo.KOHAKU) {
					SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuHukkatsu);
				} else if (GameMaster.Instance.charaType2 == PlayerInfo.YUKO) {
					SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.yukoHukkatsu);
				} else if (GameMaster.Instance.charaType2 == PlayerInfo.MISAKI) {
					SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.misakiHukkatsu);
				}
				player2.transform.position = player2RPos;
				player2.transform.rotation = player2RRotate;
				player2.GetComponent<InitPosition> ().enabled = true;
				player2.GetComponent<Animator> ().SetBool ("Continue", true);
				player2.GetComponent<PlayerController> ().playerInfo.life = Const.MAX_LIFE;
				enagyBar2.SendMessage ("SetBar", Const.MAX_LIFE);
				if (player1.GetComponent<PlayerController> ().playerInfo.life <= 0) {
					StartCoroutine (Restart ());
				} else {
					GameMaster.Instance.state = GameMaster.GameState.Start;
				}
			}
		}
	}


	// Update is called once per frame
	void Update () {

		if (admobClose) {
			admobClose = false;
			if (rewardSuccess) {
				rewardSuccess = false;
				OnCoinRewrd (true);
			} else if (katinukiCtSuccess) {
				katinukiCtSuccess = false;
				OnKatinukiReward (true);
			} else if (continueSuccess) {
				continueSuccess = false;
				AfterContinue ();
			}
		}
	}

	//コインの保存とカウントアップ
	private void RewardCountUpCoin(int currentCoin,int coin){
		int finalVal = currentCoin + coin;
		if (finalVal > Const.MAX_COIN) {
			finalVal = Const.MAX_COIN;
		}
		EncryptedPlayerPrefs.SaveInt (Const.KEY_COIN, finalVal);
		TextMeshProUGUI text = currentCoinText.GetComponent<TextMeshProUGUI> ();
		DOVirtual.DelayedCall(2.5f,()=>{
			DOTween.To (
				() => currentCoin,
				it => text.text = it.ToString(),
				finalVal,
				0.3f);
		}).OnComplete(()=>{
			rewardButtonObj.GetComponent<RectTransform>().DOScaleY(0,0.5f);
			rewardImage.GetComponent<RectTransform>().DOScaleY(0,0.5f);
		});
	}

	public void OnClickResetButton(){
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		LoadScene ("Title");
	}

	public void AfterContinue(){

		if (IsPlayer (PlayerInfo.PlayerType.Player1)) {
			Camera.main.gameObject.SendMessage ("Win",PlayerInfo.PlayerType.Player1);
			if (GameMaster.Instance.charaType1 == PlayerInfo.KOHAKU) {
				OnVictoryNonVoice (PlayerInfo.KOHAKU, player1);
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuHukkatsu);
			} else if (GameMaster.Instance.charaType1 == PlayerInfo.YUKO) {
				OnVictoryNonVoice (PlayerInfo.YUKO, player1);
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.yukoHukkatsu);
			} else if (GameMaster.Instance.charaType1 == PlayerInfo.MISAKI) {
				OnVictoryNonVoice (PlayerInfo.MISAKI, player1);
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.misakiHukkatsu);
			}
		}

		if (IsPlayer (PlayerInfo.PlayerType.Player2)) {
			Camera.main.gameObject.SendMessage ("Win",PlayerInfo.PlayerType.Player2);
			if (GameMaster.Instance.charaType2 == PlayerInfo.KOHAKU) {
				OnVictoryNonVoice (PlayerInfo.KOHAKU, player2);
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuHukkatsu);
			} else if (GameMaster.Instance.charaType2 == PlayerInfo.YUKO) {
				OnVictoryNonVoice (PlayerInfo.YUKO, player2);
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.yukoHukkatsu);
			} else if (GameMaster.Instance.charaType2 == PlayerInfo.MISAKI) {
				OnVictoryNonVoice (PlayerInfo.MISAKI, player2);
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.misakiHukkatsu);
			}
		}
		gameOverPanel.SetActive (false);
		gameOverText.SetActive (false);
		afterContinuePanel.SetActive (true);
	}

	public void OnClickSelectButton(){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		LoadScene ("Select");
	}


	public void RequestReward()
	{
		// 広告ユニット ID を記述します
		string adUnitId = "";
		#if UNITY_ANDROID
		adUnitId = Const.ADMOB_ANDROID_ID;
		#elif UNITY_IPHONE
		adUnitId = Const.ADMOB_IOS_ID;
		#endif
		reward = RewardBasedVideoAd.Instance;

		AdRequest request = new AdRequest.Builder().AddKeyword("game").AddTestDevice(Const.TEST_DEVICE_ID).Build();
		reward.LoadAd (request, adUnitId);
	}
}
