using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using VIDE_Data; //<--- Import to use easily call VD class
using DG.Tweening;
public class TalkUI : MonoBehaviour {

    private List<Text> currentOptions = new List<Text>();
    
    private Text talkText;
    private GameObject uiContainer;

    //We'll use these later
    bool dialoguePaused = false;
    bool animatingText = false;

	//アップにするキャラ
	GameObject humanPlayer;
	GameObject cpuPlayer;

	Vector3 humanInitPosision;
	Vector3 cpuInitPosition;

    IEnumerator npcTextAnimator;

    void Start() {
        VD.LoadDialogues(); //Load all dialogues to memory so that we dont spend time doing so later
		if (GameMaster.Instance.state != GameMaster.GameState.Ending) {
			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				humanPlayer = GameObject.Find ("Player1");
				cpuPlayer = GameObject.Find ("Player2");
			} else {
				humanPlayer = GameObject.Find ("Player2");
				cpuPlayer = GameObject.Find ("Player1");
			}
			humanInitPosision = humanPlayer.transform.position;
			cpuInitPosition = cpuPlayer.transform.position;
		}
    }

    void OnDisable() {
        //If the script gets destroyed, let's make sure we force-end the dialogue to prevent errors
        EndDialogue(null);
    }

    //This begins the conversation (Called by examplePlayer script)
	public void Begin(VIDE_Assign diagToLoad,GameObject textArea,bool isPlayerUp) {
        //Let's clean the NPC text variables
		uiContainer = textArea;
        talkText = uiContainer.transform.Find("Talk Text").GetComponent<Text>();

        talkText.text = "";

        VD.OnActionNode += ActionHandler;
        VD.OnNodeChange += NodeChangeAction;
        VD.OnEnd += EndDialogue;

        VD.BeginDialogue(diagToLoad);
		if (isPlayerUp) {
			PlayerUp ();
		}
    }

    public void CallNext() {
       
        if (animatingText) {
			CutTextAnim();
			return;
		}

        if (!dialoguePaused) {
            VD.Next();
            return;
        }
    }

    void ActionHandler(int actionNodeID) {
        Debug.Log("ACTION TRIGGERED: " + actionNodeID.ToString());
    }

    void NodeChangeAction(VD.NodeData data) {
        talkText.text = "";
        talkText.transform.parent.gameObject.SetActive(false);

        if (data.isPlayer) {
        }
        else {
            if (data.sprite != null) {
                
                if (data.extraVars.ContainsKey("side")) {
                    int side = (int)data.extraVars["side"];

                }
            }
            else {

            }

            npcTextAnimator = AnimateText(data);
            StartCoroutine(npcTextAnimator);
            talkText.transform.parent.gameObject.SetActive(true);
        }
    }

    //Very simple text animation usin StringBuilder
    public IEnumerator AnimateText(VD.NodeData data) {
        animatingText = true;
        string text = data.comments[data.commentIndex];

        if (!data.isPlayer) {
            StringBuilder builder = new StringBuilder();
            int charIndex = 0;
            while (talkText.text != text) {
                builder.Append(text[charIndex]);
                charIndex++;
                talkText.text = builder.ToString();
                yield return new WaitForSeconds(0.02f);
            }
        }

        talkText.text = data.comments[data.commentIndex]; //Now just copy full text		
        animatingText = false;
    }

    void CutTextAnim() {
        StopCoroutine(npcTextAnimator);
        talkText.text = VD.nodeData.comments[VD.nodeData.commentIndex]; //Now just copy full text		
        animatingText = false;
    }

    //Unsuscribe from everything, disable UI, and end dialogue
    void EndDialogue(VD.NodeData data) {
        VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= NodeChangeAction;
        VD.OnEnd -= EndDialogue;
		if (uiContainer != null) {
			uiContainer.SetActive (false);
		}
        VD.EndDialogue();
		GameMaster.Instance.isTalk = false;
    }

	public void PlayerUp(){
		humanPlayer.transform.DOMoveZ (-0.24f, 1f);
		cpuPlayer.transform.DOMoveZ (cpuInitPosition.z, 1f);
	}

	public void CpuUp(){
		cpuPlayer.transform.DOMoveZ (-0.24f, 1f);
		humanPlayer.transform.DOMoveZ (humanInitPosision.z, 1f);
	}
}
