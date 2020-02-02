using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SelectPlayer : MonoBehaviour {

	public int charaType;
	public int charaSeq;
	public int price;
	public bool enableFlg{
		get{
			return IsEnable();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySe(AudioClip se){

	}

	private bool IsEnable(){
		string enabeledCharas = EncryptedPlayerPrefs.LoadString (Const.KEY_ENABLE_CHARAS, Const.ENABLE_CHARAS_DEFAULT);
		List<string> enableList = new List<string> ();
		enableList.AddRange (enabeledCharas.Split (','));
		int charaNo = charaType + charaSeq;
		bool enable = enableList.Contains (charaNo.ToString());
		return enable;
	}
}
