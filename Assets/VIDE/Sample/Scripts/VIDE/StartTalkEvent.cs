using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using DG.Tweening;
public class StartTalkEvent : MonoBehaviour {
    public TalkUI diagUI;
	public GameObject textArea;
    private VIDE_Assign assigned;

	// Use this for initialization
	void Start () {
        assigned = GetComponent<VIDE_Assign>();
	}

	public void OnBegin(){
		string playerName = "";
		string cpuName = "";
		if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
			playerName = GameMaster.GetSystemCharaName (GameMaster.Instance.charaType1);
			cpuName = GameMaster.GetSystemCharaName (GameMaster.Instance.charaType2);
		} else {
			cpuName = GameMaster.GetSystemCharaName (GameMaster.Instance.charaType1);
			playerName = GameMaster.GetSystemCharaName (GameMaster.Instance.charaType2);
		}
		assigned.assignedDialogue = playerName + "_" + cpuName;
		textArea.SetActive (true);
		DOVirtual.DelayedCall (1f, () => {
			diagUI.Begin (assigned,textArea,true);
		});
	}

	public void OnSerihu(int charaType){
		//キャラによって出しわけ
		string charaName = GameMaster.GetSystemCharaName (charaType);
		int random = Random.Range (0, 10);
		assigned.assignedDialogue = charaName + random.ToString();
		textArea.SetActive (true);
		DOVirtual.DelayedCall (1f, () => {
			diagUI.Begin (assigned,textArea,false);
		});
	}

	// Update is called once per frame
	void Update () {
		
		if (!textArea.activeSelf || GameMaster.Instance.state == GameMaster.GameState.End) {
			return;
		}

		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) {
			if (VD.isActive) {
				if (assigned.alias != "NonDialogue") {
					diagUI.CallNext();
				}
			}
		}
		#else 
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            if (VD.isActive) {
                if (assigned.alias != "NonDialogue") {
                    diagUI.CallNext();
                }
            }
        }
		#endif

    }

	public void OnEnding(){
		int level = EncryptedPlayerPrefs.LoadInt (Const.KEY_LEVEL, 2);
		assigned.assignedDialogue = "Ending" + level;
		textArea.SetActive (true);
		DOVirtual.DelayedCall (1f, () => {
			diagUI.Begin (assigned,textArea,true);
		});
	}
}
