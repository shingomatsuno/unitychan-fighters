using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour {

	private const int ACTIONRATE_NOMAL = 80;
	private const int ACTIONRATE_EASY = 88;
	private const int ACTIONRATE_HEARD = 50;
	//前回のアクション
	PlayerInfo.Action prevAction;
	bool isPlay;
	int actionRate;
	int level;
	// Use this for initialization
	void Start () {
		actionRate = ACTIONRATE_NOMAL;
		level = Const.GAME_LEVEL_NOMAL;
		if (GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {
			level = Const.GAME_LEVEL_NOMAL;
		}
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode) {
			level = EncryptedPlayerPrefs.LoadInt (Const.KEY_LEVEL, Const.GAME_LEVEL_NOMAL);
		}
		prevAction = PlayerInfo.Action.None;
		if (level == Const.GAME_LEVEL_EASY) {
			actionRate = ACTIONRATE_EASY;
		} else if (level == Const.GAME_LEVEL_NOMAL) {
			actionRate = ACTIONRATE_NOMAL;
		} else if (level == Const.GAME_LEVEL_HEARD) {
			actionRate = ACTIONRATE_HEARD;
		}
	}

	public void SetPlay(){
		isPlay = true;
	}

	public PlayerInfo.Action GetAction(PlayerInfo info, string humanTag){

		if (!isPlay) {
			return PlayerInfo.Action.None;
		}

		int rate = 0;
		if (info.charaType == PlayerInfo.BLACKKOHAKU) {
			rate = -15;
		}

		if(PerAction(actionRate + rate)){
			if (prevAction == PlayerInfo.Action.Guard) {
				return prevAction;
			}

			return PlayerInfo.Action.None;
		}

		PlayerInfo.Action action = PlayerInfo.Action.None;


		// キャラごとにパターンを少し変える
		switch (humanTag) {
		case Const.TAG_PLAYER:
			{
				//相手：Idle 
				int rand = Random.Range(0,15);
				if (info.charaType == PlayerInfo.KOHAKU) {
					if (rand == 0 || rand == 1 || rand == 2) {
						action = PlayerInfo.Action.Punch;
					} else if (rand == 3 || rand == 4) {
						action = PlayerInfo.Action.Kick;
					} else if (rand == 5) {
						action = PlayerInfo.Action.Special;
					} else if (rand == 6 || rand == 7) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							action = PlayerInfo.Action.SS;
						}
					} else if (rand == 8 || rand == 9 || rand == 10 || rand == 11 || rand == 12) {
						action = PlayerInfo.Action.Guard;
					}
				} else if (info.charaType == PlayerInfo.YUKO) {
					if (info.life < 30) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 3) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 5 || rand == 4 || rand == 11) {
							action = PlayerInfo.Action.Special;
						} else if (rand == 6 || rand == 7 || rand == 2 || rand == 12) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						} else if (rand == 8 || rand == 9 || rand == 10) {
							action = PlayerInfo.Action.Guard;
						}
					} else {
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 3 || rand == 4) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 5 || rand == 12) {
							action = PlayerInfo.Action.Special;
						} else if (rand == 6 || rand == 7) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						} else if (rand == 8 || rand == 9 || rand == 10 || rand == 11) {
							action = PlayerInfo.Action.Guard;
						}
					}
				} else if (info.charaType == PlayerInfo.MISAKI) {
					if (info.life < 30) {
						if (rand == 0 || rand == 1|| rand == 11 || rand == 12) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 3 || rand == 4) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 5) {
							action = PlayerInfo.Action.Special;
						} else if (rand == 6 || rand == 7) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						} else if (rand == 8 || rand == 9 || rand == 10 ) {
							action = PlayerInfo.Action.Guard;
						}
					} else {
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 3 || rand == 4) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 5) {
							action = PlayerInfo.Action.Special;
						} else if (rand == 6 || rand == 7) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						} else if (rand == 8 || rand == 9 || rand == 10 || rand == 11 || rand == 12) {
							action = PlayerInfo.Action.Guard;
						}
					}
				} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
					if (rand == 0 || rand == 1 || rand == 2) {
						action = PlayerInfo.Action.Punch;
					} else if (rand == 3 || rand == 4) {
						action = PlayerInfo.Action.Kick;
					} else if (rand == 5) {
						action = PlayerInfo.Action.Special;
					} else if (rand == 6 || rand == 7) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							action = PlayerInfo.Action.SS;
						}
					} else if (rand == 8 || rand == 9 || rand == 10) {
						action = PlayerInfo.Action.Guard;
					}
				}
				break;
			}
		case Const.TAG_PUNCH:
			{
				//相手：パンチ
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					int rand = Random.Range(0,15);
					if (info.charaType == PlayerInfo.KOHAKU) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2 ) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
							|| rand == 10 || rand == 11 || rand == 9) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					} else if (info.charaType == PlayerInfo.YUKO) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2 ) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
							|| rand == 10 || rand == 11 || rand == 9) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					} else if (info.charaType == PlayerInfo.MISAKI) {
						if (rand == 0 || rand == 1 || rand == 9) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
							|| rand == 10 || rand == 11 || rand == 9) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
							|| rand == 10 || rand == 11 || rand == 9) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					}
				}
				break;
			}
		case Const.TAG_KICK:
			{
				//相手：キック
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					int rand = Random.Range(0,15);
					if (info.charaType == PlayerInfo.KOHAKU) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2 || rand == 3) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
						           || rand == 10 || rand == 11) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8 || rand == 9) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					} else if (info.charaType == PlayerInfo.YUKO) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2 || rand == 3) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
							|| rand == 10 || rand == 11) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8 || rand == 9) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					} else if (info.charaType == PlayerInfo.MISAKI) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2 || rand == 3) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
							|| rand == 10 || rand == 11) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8 || rand == 9) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
						if (rand == 0 || rand == 1) {
							action = PlayerInfo.Action.Punch;
						} else if (rand == 2 || rand == 3) {
							action = PlayerInfo.Action.Kick;
						} else if (rand == 4 || rand == 5 || rand == 6
							|| rand == 10 || rand == 11) {
							action = PlayerInfo.Action.Guard;
						} else if (rand == 7 || rand == 8 || rand == 9) {
							if (info.sGage >= Const.MAX_S_GAGE) {
								action = PlayerInfo.Action.SS;
							}
						}
					}
				}
				break;
			}
		case Const.TAG_SOLT:
			{
				//相手：回転キック
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					int rand = Random.Range(0,15);
					if (rand == 0 || rand == 1) {
						action = PlayerInfo.Action.Punch;
					} else if (rand == 2 || rand == 3) {
						action = PlayerInfo.Action.Kick;
					} else if (rand == 4 || rand == 5 || rand == 6) {
						action = PlayerInfo.Action.Guard;
					} else if (rand == 7 || rand == 8 || rand == 9) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							action = PlayerInfo.Action.SS;
						}
					}
				}
				break;
			}
		case Const.TAG_SPECIAL_START:
			{
				//相手：しょーゆーけん（開始）
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					if (prevAction == PlayerInfo.Action.Guard) {
						action = PlayerInfo.Action.Guard;
					} else {
						int rand = Random.Range (0, 10);
						if (info.charaType == PlayerInfo.KOHAKU) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else if (rand == 1 || rand == 2) {
								action = RandomAttack ();
							} else if (rand == 7) {
								if (info.sGage >= Const.MAX_S_GAGE) {
									action = PlayerInfo.Action.SS;
								}
							}
						} else if (info.charaType == PlayerInfo.YUKO) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else if (rand == 1 || rand == 2) {
								action = RandomAttack ();
							} else if (rand == 7) {
								if (info.sGage >= Const.MAX_S_GAGE) {
									action = PlayerInfo.Action.SS;
								}
							}
						} else if (info.charaType == PlayerInfo.MISAKI) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else if (rand == 1 || rand == 2) {
								action = RandomAttack ();
							} else if (rand == 7) {
								if (info.sGage >= Const.MAX_S_GAGE) {
									action = PlayerInfo.Action.SS;
								}
							}
						} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else if (rand == 1 || rand == 2) {
								action = RandomAttack ();
							} else if (rand == 7) {
								if (info.sGage >= Const.MAX_S_GAGE) {
									action = PlayerInfo.Action.SS;
								}
							}
						}
					}
				}
				break;
			}
		case Const.TAG_SPECIAL:
			{
				//相手：しょーゆーけん
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					if (info.charaType == PlayerInfo.KOHAKU) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							int rand = Random.Range (0, 10);
							if (rand == 0 || rand == 1 || rand == 2) {
								action = PlayerInfo.Action.SS;
							} else if (rand == 3) {
								action = PlayerInfo.Action.Guard;
							}
						} else {
							action = RandomAttack ();
						}
					} else if (info.charaType == PlayerInfo.YUKO) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							int rand = Random.Range (0, 10);
							if (rand == 0 || rand == 1 || rand == 2) {
								action = PlayerInfo.Action.SS;
							} else if (rand == 3) {
								action = PlayerInfo.Action.Guard;
							}
						} else {
							action = RandomAttack ();
						}
					} else if (info.charaType == PlayerInfo.MISAKI) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							int rand = Random.Range (0, 10);
							if (rand == 0 || rand == 1 || rand == 2) {
								action = PlayerInfo.Action.SS;
							} else if (rand == 3) {
								action = PlayerInfo.Action.Guard;
							}
						} else {
							action = RandomAttack ();
						}
					} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							int rand = Random.Range (0, 10);
							if (rand == 0 || rand == 1 || rand == 2) {
								action = PlayerInfo.Action.SS;
							} else if (rand == 3 || rand == 4) {
								action = PlayerInfo.Action.Guard;
							}
						} else {
							action = RandomAttack ();
						}
					}
				}
				break;
			}
		case Const.TAG_SPECIAL_UPPER:
			{
				//相手：しょーゆーけん（上昇中）
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					if (info.charaType == PlayerInfo.KOHAKU) {
					} else if (info.charaType == PlayerInfo.YUKO) {
					} else if (info.charaType == PlayerInfo.MISAKI) {
					} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							int rand = Random.Range (0, 10);
							if (rand == 0) {
								action = PlayerInfo.Action.SS;
							}
						}
					}
				}
				break;
			}
		case Const.TAG_SPECIAL_FALL:
			{
				if (info.charaType == PlayerInfo.KOHAKU) {
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				} else if (info.charaType == PlayerInfo.YUKO) {
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				} else if (info.charaType == PlayerInfo.MISAKI) {
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				}

				break;
			}
		case Const.TAG_HADOUKEN:
			{
				//相手：はどーけん
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					if (prevAction == PlayerInfo.Action.Guard) {
						action = PlayerInfo.Action.Guard;
					} else {
						int rand = Random.Range (0, 10);
						if (info.charaType == PlayerInfo.KOHAKU) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else {
								action = RandomAttack ();
							}
						} else if (info.charaType == PlayerInfo.YUKO) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else {
								action = RandomAttack ();
							}
						} else if (info.charaType == PlayerInfo.MISAKI) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else {
								action = RandomAttack ();
							}
						} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
							if (rand == 0) {
								action = PlayerInfo.Action.Guard;
							} else {
								action = RandomAttack ();
							}
						}
					}
				}
				break;
			}
		case Const.TAG_LITE_DAMAGE:
			{
				//相手：小ダメージ
				if (info.charaType == PlayerInfo.KOHAKU){
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				} else if (info.charaType == PlayerInfo.YUKO) {
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				} else if (info.charaType == PlayerInfo.MISAKI) {
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
					if (info.sGage >= Const.MAX_S_GAGE) {
						int rand = Random.Range (0, 10);
						if (rand == 0 || rand == 1 || rand == 2) {
							action = PlayerInfo.Action.SS;
						} else {
							action = RandomAttack ();
						}
					} else {
						action = RandomAttack ();
					}
				}
				break;
			}
		case Const.TAG_MIDDLE_DAMAGE:
			{
				//相手：中ダメージ
				if (info.sGage >= Const.MAX_S_GAGE) {
					int rand = Random.Range (0, 10);
					if (rand == 0 || rand == 1 || rand == 2) {
						action = PlayerInfo.Action.SS;
					} else {
						action = RandomAttack ();
					}
				} else {
					action = RandomAttack ();
				}
				break;
			}
		case Const.TAG_DOWN:
			{
				//相手：ダウン
				break;
			}
		case Const.TAG_HEADSPRING:
			{
				//相手：起き上がり中
				if (gameObject.tag == Const.TAG_GUARD) {
					action = PlayerInfo.Action.Guard;
				} else {
					int rand = Random.Range (0, 10);
					if (rand == 0) {
						action = PlayerInfo.Action.SS;
					} else if(rand == 1){
						action = RandomAttack ();
					}
				}
				break;
			}
		case Const.TAG_GUARD:
			{
				//相手：ガード
				int guardRate = 25;
				if (level == Const.GAME_LEVEL_HEARD) {
					guardRate = 55;
				}
				int rand = Random.Range(0,guardRate);
				if (info.charaType == PlayerInfo.KOHAKU) {
					if (rand == 0 || rand == 1 || rand == 2 || rand == 3) {
						action = PlayerInfo.Action.Punch;
					} else if ( rand == 4) {
						action = PlayerInfo.Action.Kick;
					} else if (rand == 6) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							action = PlayerInfo.Action.SS;
						}
					}
				} else if (info.charaType == PlayerInfo.YUKO) {
					if (rand == 0 || rand == 1 || rand == 2 || rand == 3) {
						action = PlayerInfo.Action.Punch;
					} else if ( rand == 4) {
						action = PlayerInfo.Action.Kick;
					} else if (rand == 6) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							action = PlayerInfo.Action.SS;
						}
					}
				} else if (info.charaType == PlayerInfo.MISAKI) {
					if (rand == 0 || rand == 1 || rand == 2 || rand == 3) {
						action = PlayerInfo.Action.Punch;
					} else if ( rand == 4) {
						action = PlayerInfo.Action.Kick;
					} else if (rand == 6) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							action = PlayerInfo.Action.SS;
						}
					}
				} else if (info.charaType == PlayerInfo.BLACKKOHAKU) {
					if (rand == 0 || rand == 1 || rand == 2 || rand == 3) {
						action = PlayerInfo.Action.Punch;
					} else if ( rand == 4) {
						action = PlayerInfo.Action.Kick;
					} else if (rand == 6) {
						if (info.sGage >= Const.MAX_S_GAGE) {
							action = PlayerInfo.Action.SS;
						}
					}
				}
				break;
			}
		}
		prevAction = action;
		return action;

	}

	private PlayerInfo.Action RandomAttack(){
		PlayerInfo.Action action = PlayerInfo.Action.None;
		int rand = Random.Range (0, 10);
		if (rand == 0) {
			//1/10でしょうゆーけん
			action = PlayerInfo.Action.Special;
		} else if (rand == 1 || rand == 2) {
			//2/10でパンチ
			action = PlayerInfo.Action.Punch;
		} else if (rand == 3 || rand == 4) {
			//2/10でキック
			action = PlayerInfo.Action.Kick;
		}
		return action;
	}

	private bool PerAction(int percent){
		int rand = Random.Range (0, 100);
		if (0 <= rand && rand < percent) {
			return true;
		}
		return false;
	}
}
