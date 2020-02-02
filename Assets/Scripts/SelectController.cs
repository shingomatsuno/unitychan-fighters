using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class SelectController : MonoBehaviour {

	public GameObject onePlayerPanel;
	public GameObject onePArrow;

	public GameObject twoPlayerPanel;
	public GameObject twoPArrow;

	public GameObject buyPanelContainer;
	public GameObject buyPanel;
	public GameObject buyText;

	//決定ボタン
	public GameObject p1Button;
	public GameObject p2Button;
	//購入ボタン
	public GameObject p1BuyButton;
	public GameObject p2BuyButton;
	//値段
	public GameObject p1PriceText;
	public GameObject p2PriceText;

	public GameObject p1LeftArrow;
	public GameObject p1RightArrow;

	public GameObject p2LeftArrow;
	public GameObject p2RightArrow;

	public GameObject onePText;
	public GameObject twoPText;

	public GameObject kohaku;
	public GameObject yuko;
	public GameObject misaki;

	public GameObject kohaku2;
	public GameObject yuko2;
	public GameObject misaki2;

	//買うときの背景
	public GameObject backPanel;
	//買うときに手前に出てくるキャラ
	GameObject buySelectChara;

	public GameObject coinText;

	GameObject[] kohaku1Array;
	GameObject[] yuko1Array;
	GameObject[] misaki1Array;

	GameObject[] kohaku2Array;
	GameObject[] yuko2Array;
	GameObject[] misaki2Array;
	//購入するキャラ
	GameObject buyChara1;
	GameObject buyChara2;
	GameObject buyChara;

	int p1Index = 0;
	int p2Index = 0;

	int p1Chara = 0;
	int p2Chara = 0;

	int kohakuLen;
	int yukoLen;
	int misakiLen;

	bool p1Ok;
	bool p2Ok;

	// Use this for initialization
	void Start () {
		GameMaster.Instance.FadeIn ();
		GameMaster.Instance.ctrPos = EncryptedPlayerPrefs.LoadInt (Const.KEY_PLAYERPOS, GameMaster.CtrRight);
		int coins = EncryptedPlayerPrefs.LoadInt (Const.KEY_COIN, 0);
		coinText.GetComponent<TextMeshProUGUI>().text = coins.ToString();

		kohaku1Array = GameObject.FindGameObjectsWithTag ("Kohaku").OrderBy(x=>x.name).ToArray();
		yuko1Array = GameObject.FindGameObjectsWithTag ("Yuko").OrderBy(x=>x.name).ToArray();
		misaki1Array = GameObject.FindGameObjectsWithTag ("Misaki").OrderBy(x=>x.name).ToArray();

		kohaku2Array = GameObject.FindGameObjectsWithTag ("Kohaku2").OrderBy(x=>x.name).ToArray();
		yuko2Array = GameObject.FindGameObjectsWithTag ("Yuko2").OrderBy(x=>x.name).ToArray();
		misaki2Array = GameObject.FindGameObjectsWithTag ("Misaki2").OrderBy(x=>x.name).ToArray();

		//使用可能キャラのみアニメーションさせる
		EnableChara (ref kohaku1Array);
		EnableChara (ref yuko1Array);
		EnableChara (ref misaki1Array);
		EnableChara (ref kohaku2Array);
		EnableChara (ref yuko2Array);
		EnableChara (ref misaki2Array);

		kohakuLen = kohaku1Array.Length;
		yukoLen = yuko1Array.Length;
		misakiLen = misaki1Array.Length;

		GameObject p1 = SwitchChara (kohaku1Array, p1Index);
		GameObject p2 = SwitchChara (kohaku2Array, p2Index);

		yuko.SetActive (false);
		misaki.SetActive (false);

		yuko2.SetActive (false);
		misaki2.SetActive (false);

		string p1Name = GameMaster.GetCharaName (p1Chara);
		string p2Name = GameMaster.GetCharaName (p2Chara);

		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode
			|| GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {

			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				kohaku2.SetActive (false);
				yuko2.SetActive (false);
				misaki2.SetActive (false);
				twoPlayerPanel.SetActive (false);
				twoPArrow.SetActive (false);
				onePText.GetComponent<TextMeshProUGUI>().text = p1Name;
			} else {
				kohaku.SetActive (false);
				yuko.SetActive (false);
				misaki.SetActive (false);
				onePlayerPanel.SetActive (false);
				onePArrow.SetActive (false);
				twoPText.GetComponent<TextMeshProUGUI>().text = p2Name;
			}
		}
		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			twoPlayerPanel.SetActive (true);
			twoPArrow.SetActive (true);
			onePText.GetComponent<TextMeshProUGUI>().text = p1Name;
			twoPText.GetComponent<TextMeshProUGUI>().text = p2Name;
		}

		GameMaster.Instance.player1Chara = PlayerInfo.KOHAKU;
		//1プレイモードの時はステージごとに代わる(MainGameControllerで調整)
		GameMaster.Instance.player2Chara = PlayerInfo.KOHAKU;

		buyChara1 = p1;
		buyChara2 = p2;
	}

	private void EnableChara(ref GameObject[] charaArray){
		for (int i = 0; i < charaArray.Length; i++) {
			if (charaArray [i].GetComponent<SelectPlayer> ().enableFlg) {
				charaArray [i].GetComponent<Animator> ().enabled = true;
			}
		}
	}

	public void OnClickStartButton(int num){
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		//買い物できるようにする
		float alpha = 0.5f;
		if (num == 1 && !p1Ok) {
			//左の決定ボタン
			GameMaster.Instance.player1Chara = p1Chara + p1Index;
			GameObject chara1 = GetCharactor (PlayerInfo.PlayerType.Player1, p1Chara, p1Index);
			if (p1Chara == PlayerInfo.KOHAKU) {
				SoundSelectScene.Instans.PlaySe (SoundSelectScene.Instans.kohakuVoice);
				chara1.GetComponent<Animator>().SetBool ("Win1",true);
			}else if(p1Chara == PlayerInfo.YUKO){
				SoundSelectScene.Instans.PlaySe (SoundSelectScene.Instans.yukoVoice);
				chara1.GetComponent<Animator>().SetBool ("Win2",true);
			}else if(p1Chara == PlayerInfo.MISAKI){
				SoundSelectScene.Instans.PlaySe (SoundSelectScene.Instans.misakiVoice);
				chara1.GetComponent<Animator>().SetBool ("Win3",true);
			}
			p1Ok = true;
			p1Button.GetComponent<Button> ().enabled = false;
			Image buttonImage = p1Button.GetComponent<Image> ();
			DOTween.ToAlpha (() => buttonImage.color, color => buttonImage.color = color, alpha, 0f);
			p1LeftArrow.GetComponent<Button> ().enabled = false;
			p1RightArrow.GetComponent<Button> ().enabled = false;
			Image aLImage = p1LeftArrow.GetComponent<Image> ();
			Image aRImage = p1RightArrow.GetComponent<Image> ();
			DOTween.ToAlpha (() => aLImage.color, color => aLImage.color = color, alpha, 0f);
			DOTween.ToAlpha (() => aRImage.color, color => aRImage.color = color, alpha, 0f);

		} 
		if(num == 2 && !p2Ok){
			
			//右の決定ボタン
			GameMaster.Instance.player2Chara = p2Chara + p2Index;
			GameObject chara2 = GetCharactor (PlayerInfo.PlayerType.Player2, p2Chara, p2Index);
			if (p2Chara == PlayerInfo.KOHAKU) {
				SoundSelectScene.Instans.PlaySe (SoundSelectScene.Instans.kohakuVoice);
				chara2.GetComponent<Animator>().SetBool ("Win1",true);
			}else if(p2Chara == PlayerInfo.YUKO){
				SoundSelectScene.Instans.PlaySe (SoundSelectScene.Instans.yukoVoice);
				chara2.GetComponent<Animator>().SetBool ("Win2",true);
			}else if(p2Chara == PlayerInfo.MISAKI){
				SoundSelectScene.Instans.PlaySe (SoundSelectScene.Instans.misakiVoice);
				chara2.GetComponent<Animator>().SetBool ("Win3",true);
			}
			p2Ok = true;
			p2Button.GetComponent<Button> ().enabled = false;
			Image buttonImage = p2Button.GetComponent<Image> ();
			DOTween.ToAlpha (() => buttonImage.color, color => buttonImage.color = color, alpha, 0f);
			p2LeftArrow.GetComponent<Button> ().enabled = false;
			p2RightArrow.GetComponent<Button> ().enabled = false;
			Image aLImage = p2LeftArrow.GetComponent<Image> ();
			Image aRImage = p2RightArrow.GetComponent<Image> ();
			DOTween.ToAlpha (() => aLImage.color, color => aLImage.color = color, alpha, 0f);
			DOTween.ToAlpha (() => aRImage.color, color => aRImage.color = color, alpha, 0f);
		}


		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode
		    || GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {

			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				if (p1Ok) {
					LoadMainScene ();
				}
			} else {
				if (p2Ok) {
					LoadMainScene ();
				}
			}
		} else {
			if (p1Ok && p2Ok) {
				LoadMainScene ();	
			}
		}
	}

	private void LoadMainScene(){
		//フェードアウト
		Sequence seq = DOTween.Sequence();
		seq.Append(DOVirtual.DelayedCall(3.5f,()=>{
			GameMaster.Instance.FadeOut();
		}));
		seq.Append(DOVirtual.DelayedCall(1.5f,()=>{
			SceneManager.LoadScene ("Main");
		}));
		seq.OnUpdate (() => {
			if(GameMaster.Instance.mode == GameMaster.GameMode.Title){
				seq.Kill();
			}
		});
		seq.Play ();
	}

	public void SelectChara(int chara){
		if (p1Ok) {
			return;
		}
		p1Index = 0;
		p1Chara = chara;
		GameObject obj = null;
		string charaName = GameMaster.GetCharaName (chara);
		if (chara == PlayerInfo.KOHAKU) {
			onePText.GetComponent<TextMeshProUGUI>().text = charaName;
			kohaku.SetActive (true);
			yuko.SetActive (false);
			misaki.SetActive (false);
			obj = SwitchChara (kohaku1Array, p1Index);
		} else if (chara == PlayerInfo.YUKO) {
			onePText.GetComponent<TextMeshProUGUI>().text = charaName;
			kohaku.SetActive (false);
			yuko.SetActive (true);
			misaki.SetActive (false);
			obj = SwitchChara (yuko1Array, p1Index);
		} else if (chara == PlayerInfo.MISAKI) {
			onePText.GetComponent<TextMeshProUGUI>().text = charaName;
			kohaku.SetActive (false);
			yuko.SetActive (false);
			misaki.SetActive (true);
			obj = SwitchChara (misaki1Array, p1Index);
		}
		SetBuyButton (PlayerInfo.PlayerType.Player1, obj);
	}

	public void SelectChara2(int chara){
		if (p2Ok) {
			return;
		}
		p2Index = 0;
		p2Chara = chara;
		string charaName = GameMaster.GetCharaName (chara);
		GameObject obj = null;
		if (chara == PlayerInfo.KOHAKU) {
			twoPText.GetComponent<TextMeshProUGUI>().text = charaName;
			kohaku2.SetActive (true);
			yuko2.SetActive (false);
			misaki2.SetActive (false);
			obj = SwitchChara (kohaku2Array, p2Index);
		} else if (chara == PlayerInfo.YUKO) {
			twoPText.GetComponent<TextMeshProUGUI>().text = charaName;
			kohaku2.SetActive (false);
			yuko2.SetActive (true);
			misaki2.SetActive (false);
			obj = SwitchChara (yuko2Array, p2Index);
		} else if (chara == PlayerInfo.MISAKI) {
			twoPText.GetComponent<TextMeshProUGUI>().text = charaName;
			kohaku2.SetActive (false);
			yuko2.SetActive (false);
			misaki2.SetActive (true);
			obj = SwitchChara (misaki2Array, p2Index);
		}
		SetBuyButton (PlayerInfo.PlayerType.Player2, obj);
	}

	public void OnClickP1RightArrow(){
		p1Index += 1;
		int maxIndex = GetMaxIndex (p1Chara);
		if (p1Index > maxIndex) {
			p1Index = 0;
		}
		ShowChara (PlayerInfo.PlayerType.Player1, p1Chara, p1Index);
	}

	public void OnClickP1LeftArrow(){
		p1Index -= 1;
		if (p1Index < 0) {
			p1Index = GetMaxIndex (p1Chara);
		}
		ShowChara (PlayerInfo.PlayerType.Player1, p1Chara, p1Index);
	}

	public void OnClickP2RightArrow(){
		p2Index += 1;
		int maxIndex = GetMaxIndex (p2Chara);
		if (p2Index > maxIndex) {
			p2Index = 0;
		}
		ShowChara (PlayerInfo.PlayerType.Player2, p2Chara, p2Index);
	}

	public void OnClickP2LeftArrow(){
		p2Index -= 1;
		if (p2Index < 0) {
			p2Index = GetMaxIndex (p2Chara);
		}
		ShowChara (PlayerInfo.PlayerType.Player2, p2Chara, p2Index);
	}

	private int GetMaxIndex(int chara){
		if (chara == PlayerInfo.KOHAKU) {
			return kohakuLen - 1;
		}else if (chara == PlayerInfo.YUKO) {
			return yukoLen - 1;
		}else if (chara == PlayerInfo.MISAKI) {
			return misakiLen - 1;
		}
		return 0;
	}

	private void ShowChara(PlayerInfo.PlayerType player,int chara,int index){

		GameObject charaObj = null;

		if (chara == PlayerInfo.KOHAKU) {
			
			if (player == PlayerInfo.PlayerType.Player1) {
				charaObj = SwitchChara (kohaku1Array, index);
			} else {
				charaObj = SwitchChara (kohaku2Array, index);
			}
			
		}else if (chara == PlayerInfo.YUKO) {
			
			if (player == PlayerInfo.PlayerType.Player1) {
				charaObj = SwitchChara (yuko1Array, index);
			} else {
				charaObj = SwitchChara (yuko2Array, index);
			}
			
		}else if (chara == PlayerInfo.MISAKI) {
			
			if (player == PlayerInfo.PlayerType.Player1) {
				charaObj = SwitchChara (misaki1Array, index);
			} else {
				charaObj = SwitchChara (misaki2Array, index);
			}
		}

		SetBuyButton (player, charaObj);
	}

	//決定ボタン制御
	private void SetBuyButton(PlayerInfo.PlayerType player,GameObject charaObj){
		if (PlayerInfo.PlayerType.Player1 == player) {
			buyChara1 = charaObj;
		} else {
			buyChara2 = charaObj;
		}

		SelectPlayer selectChara = charaObj.GetComponent<SelectPlayer> ();

    	if (selectChara.enableFlg) {
			// 使える
			if (player == PlayerInfo.PlayerType.Player1) {
				p1Button.SetActive (true);
				p1BuyButton.SetActive (false);
			}else{
				p2Button.SetActive (true);
				p2BuyButton.SetActive (false);
			}
		} else {
			
			float alpha = 0.3f;
			//ボタンに値段をセット
			int price = selectChara.price;
			TextMeshProUGUI priceText;
			Image buttonImage;
			if (player == PlayerInfo.PlayerType.Player1) {
				p1Button.SetActive (false);
				p1BuyButton.SetActive (true);
				buttonImage = p1BuyButton.GetComponent<Image> ();
				priceText = p1PriceText.GetComponent<TextMeshProUGUI> ();
				priceText.text = price.ToString ();
			}else{
				p2Button.SetActive (false);
				p2BuyButton.SetActive (true);
				buttonImage = p2BuyButton.GetComponent<Image> ();
				priceText = p2PriceText.GetComponent<TextMeshProUGUI> ();
				priceText.text = price.ToString ();
			}
			int coin = EncryptedPlayerPrefs.LoadInt(Const.KEY_COIN,0);
			if (coin >= price) {
				// 買える
				if (player == PlayerInfo.PlayerType.Player1) {
					p1BuyButton.GetComponent<Button> ().enabled = true;
				}else{
					p2BuyButton.GetComponent<Button> ().enabled = true;
				}
				DOTween.ToAlpha (() => buttonImage.color, color => buttonImage.color = color, 1f, 0f);
				DOTween.ToAlpha (() => priceText.color, color => priceText.color = color, 1f, 0f);
			} else {
				// 買えない
				if (player == PlayerInfo.PlayerType.Player1) {
					p1BuyButton.GetComponent<Button> ().enabled = false;
				}else{
					p2BuyButton.GetComponent<Button> ().enabled = false;
				}
				DOTween.ToAlpha (() => buttonImage.color, color => buttonImage.color = color, alpha, 0f);
				DOTween.ToAlpha (() => priceText.color, color => priceText.color = color, alpha, 0f);
			}
		}
	}

	public void OnClickBuyButton(int pType){
		Sound.Instans.PlaySe (Sound.Instans.pushSound);
		onePArrow.SetActive (false);
		twoPArrow.SetActive (false);
		p1Button.SetActive (false);
		p2Button.SetActive (false);
		p1BuyButton.SetActive (false);
		p2BuyButton.SetActive (false);
		//確認ダイアログ buyChara
		buyPanelContainer.SetActive(true);
		buyPanel.SetActive(true);
		if (pType == 1) {
			buyChara = buyChara1;
		} else {
			buyChara = buyChara2;
		}
		buySelectChara = Instantiate (buyChara) as GameObject;
		buySelectChara.transform.SetParent (backPanel.transform);
		buySelectChara.transform.localPosition = new Vector3 (0f,-0.91f,-1.21f);
		buySelectChara.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 180f, 0f));
		buySelectChara.transform.localScale = new Vector3 (1f, 1f, 1f);

		SelectPlayer sp = buyChara.GetComponent<SelectPlayer> ();
		string text = string.Format ("コイン{0:D}まいでかいますか？",sp.price);
		buyText.GetComponent<TextMeshProUGUI> ().text = text;
	}

	public void OnClickBuyFixButton(){
		Sound.Instans.PlaySe (Sound.Instans.fixSound);
		buyPanel.SetActive(false);
		//コイン減らす playerPrefsに追加
		SelectPlayer sp = buyChara.GetComponent<SelectPlayer> ();
		int currentCoin = EncryptedPlayerPrefs.LoadInt(Const.KEY_COIN,0);
		int afterCoin = currentCoin - sp.price;
		EncryptedPlayerPrefs.SaveInt (Const.KEY_COIN, afterCoin);
		int charaNo = sp.charaType + sp.charaSeq;
		AddEnableCharas (charaNo);
		coinText.GetComponent<TextMeshProUGUI>().text = afterCoin.ToString();

		Animator animator = buySelectChara.GetComponent<Animator> ();
		animator.enabled = true;
		if (sp.charaType == PlayerInfo.KOHAKU) {
			animator.SetBool ("Win1",true);
		}else if(sp.charaType == PlayerInfo.YUKO){
			animator.SetBool ("Win2",true);
		}else if(sp.charaType == PlayerInfo.MISAKI){
			animator.SetBool ("Win3",true);
		}

		DOVirtual.DelayedCall (4f, () => {
			//UIを再構築
			EnableChara (ref kohaku1Array);
			EnableChara (ref yuko1Array);
			EnableChara (ref misaki1Array);
			EnableChara (ref kohaku2Array);
			EnableChara (ref yuko2Array);
			EnableChara (ref misaki2Array);
			CloseBuyPanel ();
		});
	}

	public void OnClickBuyCancelButton(){
		Sound.Instans.PlaySe (Sound.Instans.cancelSound);
		CloseBuyPanel ();
	}

	private void CloseBuyPanel(){
		if (GameMaster.Instance.mode == GameMaster.GameMode.OnePlayerMode
			|| GameMaster.Instance.mode == GameMaster.GameMode.EndlessMode) {

			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				onePArrow.SetActive (true);
				p1Button.SetActive (true);
				p1BuyButton.SetActive (true);
			} else {
				twoPArrow.SetActive (true);
				p2Button.SetActive (true);
				p2BuyButton.SetActive (true);
			}
		} else {
			p1Button.SetActive (true);
			p2Button.SetActive (true);
			p1BuyButton.SetActive (true);
			p2BuyButton.SetActive (true);
			onePArrow.SetActive (true);
			twoPArrow.SetActive (true);
		}
		if(buyChara1 != null){
			SetBuyButton (PlayerInfo.PlayerType.Player1,buyChara1);
		}
		if(buyChara2 != null){
			SetBuyButton (PlayerInfo.PlayerType.Player2,buyChara2);
		}
		Destroy (buySelectChara);
		buyPanel.SetActive(false);
		buyPanelContainer.SetActive(false);
	}

	private GameObject SwitchChara(GameObject[] objArray,int index){
		GameObject charaObj = null;
		int len = objArray.Length;
		for (int i = 0; i < len; i++) {
			if (index == i) {
				objArray [i].SetActive (true);
				charaObj = objArray [i];
			} else {
				objArray [i].SetActive (false);
			}
		}
		return charaObj;
	}

	private GameObject GetCharactor(PlayerInfo.PlayerType player,int chara,int index){

		GameObject charactor = null;
		if (chara == PlayerInfo.KOHAKU) {

			if (player == PlayerInfo.PlayerType.Player1) {
				charactor = GetChara (kohaku1Array, index);
			} else {
				charactor = GetChara (kohaku2Array, index);
			}

		}else if (chara == PlayerInfo.YUKO) {

			if (player == PlayerInfo.PlayerType.Player1) {
				charactor = GetChara (yuko1Array, index);
			} else {
				charactor = GetChara (yuko2Array, index);
			}

		}else if (chara == PlayerInfo.MISAKI) {

			if (player == PlayerInfo.PlayerType.Player1) {
				charactor = GetChara (misaki1Array, index);
			} else {
				charactor = GetChara (misaki2Array, index);
			}

		}
		return charactor;
	}

	private GameObject GetChara(GameObject[] objArray,int index){
		GameObject chara = null;
		int len = objArray.Length;
		for (int i = 0; i < len; i++) {
			if (index == i) {
				chara = objArray [i];
			}
		}
		return chara;
	}

	public void OnClickPauseButton(){
		GameMaster.Instance.OnClickPauseButton ();
	}

	private void AddEnableCharas(int charaNo){
		string enabeledCharas = EncryptedPlayerPrefs.LoadString (Const.KEY_ENABLE_CHARAS, Const.ENABLE_CHARAS_DEFAULT);
		enabeledCharas = enabeledCharas + "," + charaNo.ToString ();
		EncryptedPlayerPrefs.SaveString (Const.KEY_ENABLE_CHARAS, enabeledCharas);
	}
}
