using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviour {

	Animator animator;

	//AIに渡すプレイヤーのオブジェクト
	GameObject humanPlayer;

	EnemyAi enemyAi;

	//攻撃
	bool guard;
	bool firstPunch;
	bool firstKick;
	bool special;
	bool specialMove;

	public PlayerInfo playerInfo = new PlayerInfo ();

	GameObject gameManager;

	bool isDamage;

	PlayerInfo.Action action = PlayerInfo.Action.None;
	Image ssButton;
	TextMeshProUGUI ssText;

	GameObject shooter;
	bool isHadouken;
	// Use this for initialization
	void Start () {

		gameManager = GameObject.Find ("MainGameManager");

		animator = GetComponent<Animator> ();
		animator.SetBool ("Guard", false);

		shooter = GameObject.Find ("Shooter");
	}

	// Update is called once per frame
	void Update () {
		if (GameMaster.Instance.state != GameMaster.GameState.Start) {
			return;
		}
		if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
			Controller (1);
		}
		if (playerInfo.playerType == PlayerInfo.PlayerType.Player2) {
			Controller (2);
		}
	}

	void Controller(int type){

		if (playerInfo.humanType == PlayerInfo.HumanType.Human) {
			if (CrossPlatformInputManager.GetButton ("Guard" + type)) {
				action = PlayerInfo.Action.Guard;
			} else if (CrossPlatformInputManager.GetButtonDown ("Attack" + type)) {
				action = PlayerInfo.Action.Punch;
			} else if (CrossPlatformInputManager.GetButtonDown ("Kick" + type)) {
				action = PlayerInfo.Action.Kick;
			} else if (CrossPlatformInputManager.GetButtonDown ("Special" + type)) {
				action = PlayerInfo.Action.Special;
			} else if (CrossPlatformInputManager.GetButtonDown ("SS" + type)) {
				action = PlayerInfo.Action.SS;
			} else {
				action = PlayerInfo.Action.None;
			}
		}
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Space)){
			action = PlayerInfo.Action.SS;
		}
		#endif
	}

	void FixedUpdate(){

		if (playerInfo.life <= 0) {
			return;
		}

		if (GameMaster.Instance.state != GameMaster.GameState.Start) {
			return;
		}

		if (playerInfo.humanType == PlayerInfo.HumanType.Com) {
			action = enemyAi.GetAction (playerInfo, humanPlayer.tag);
		}

		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo (0);

		if (isDamage || gameObject.tag == Const.TAG_NOMOVE || gameObject.tag == Const.TAG_NODAMAGE
			|| state.IsName("Solt")
			|| state.IsName("BigDamage")
			|| gameObject.tag == Const.TAG_SPECIAL_START 
			|| gameObject.tag == Const.TAG_SPECIAL
			|| gameObject.tag == Const.TAG_SPECIAL_FALL
			|| gameObject.tag == Const.TAG_SPECIAL_UPPER
			|| gameObject.tag == Const.TAG_HADOUKEN
			|| gameObject.tag == Const.TAG_MIDDLE_DAMAGE
			|| gameObject.tag == Const.TAG_DOWN
			|| isHadouken
			|| specialMove
			|| gameObject.tag == Const.TAG_DEAD) {

			return;
		}

		if (!firstPunch && !special && !specialMove && action == PlayerInfo.Action.Special) {
			special = true;
			Invoke ("SpecialOff", 0.5f);
			animator.SetBool ("Special", true);
		}else if (!firstPunch && action == PlayerInfo.Action.Punch) {
			firstPunch = true;
			Invoke ("PuchOff", 0.4f);
			animator.SetBool ("Jab", true);
		}else if (firstKick && action == PlayerInfo.Action.Kick) {
			animator.SetBool ("Solt", true);
		}else if (!firstKick && !state.IsName ("Hikick") && action == PlayerInfo.Action.Kick) {
			firstKick = true;
			Invoke ("KickOff", 0.6f);
			animator.SetBool ("Hikick", true);
		}else if (action == PlayerInfo.Action.Guard) {
			animator.SetBool ("Guard", true);
			return;
		}else if(action == PlayerInfo.Action.SS){
			if (playerInfo.sGage >= Const.MAX_S_GAGE) {
				animator.SetBool ("Hadouken", true);
			}
		} else {
			animator.SetBool ("Guard", false);
		}

		action = PlayerInfo.Action.None;
	}

	private void AddSuperBar(int addPoint){
		playerInfo.sGage += addPoint;
		if (playerInfo.sGage > Const.MAX_S_GAGE) {
			playerInfo.sGage = Const.MAX_S_GAGE;
		}
		SetSgage (playerInfo.sGage);
		gameManager.SendMessage ("AddSuperBar",playerInfo);
	}

	private void PunchTag(){
		StartCoroutine(TagChange(Const.TAG_PUNCH,0.05f,0.2f));
		transform.DOComplete ();
		float vecx = (playerInfo.playerType == PlayerInfo.PlayerType.Player1) ? 0.1f : -0.1f;
		GetComponent<InitPosition> ().enabled = false;
		transform.DOMoveX (vecx, 0.15f).SetRelative ().OnUpdate(()=>{
			if(isDamage){
				transform.DOComplete();
			}
		}).OnComplete (() => {
			isDamage = false;
			GetComponent<InitPosition> ().enabled = true;
		});
	}
	private void KickTag(){
		StartCoroutine(TagChange(Const.TAG_KICK,0.1f,0.3f));
	}
	private void SoltTag(){
		AddSuperBar (15);
		// キャラで声分け voice
		if (playerInfo.charaType == PlayerInfo.KOHAKU) {
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuSolt);
		} else if (playerInfo.charaType == PlayerInfo.YUKO) {
			SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.yukoSolt);
		}else if(playerInfo.charaType == PlayerInfo.MISAKI){
			SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.misakiSolt);
		}else if(playerInfo.charaType == PlayerInfo.BLACKKOHAKU){
			SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.yamikoSolt);
		}

		StartCoroutine(TagChangeSolt(Const.TAG_SOLT,0.1f,0.4f));
	}

	private void SpecialTag(){
		Vector3 p = transform.position;
		float vecx = (p.x < 0) ? 0.15f : -0.15f;
		Vector3 vec2 = (p.x < 0) ? new Vector3 (0.05f, 0.3f, -0.05f) : new Vector3 (-0.05f, 0.3f, 0.05f);
		if (GameMaster.Instance.mode != GameMaster.GameMode.TwoPlayerMode) {
			if (GameMaster.Instance.ctrPos == GameMaster.CtrRight) {
				vec2.z = -vec2.z;
			}
		}
		gameObject.tag = Const.TAG_SPECIAL_START;
		transform.DOKill ();
		GetComponent<InitPosition> ().enabled = false;
		Sequence sq = DOTween.Sequence ();
		sq.Append (transform.DOMoveX (vecx,0.2f).SetRelative ().OnStart(()=>{
			specialMove = true;
		}));
		sq.Append (transform.DOMove (vec2, 0.4f).SetRelative ().OnStart (() => {
			AddSuperBar (20);
			StartCoroutine(TagChangeSp(Const.TAG_SPECIAL, Const.TAG_SPECIAL_UPPER,0.1f,0.1f));
			// キャラで声分け
			if (playerInfo.charaType == PlayerInfo.KOHAKU) {
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.KohakuSpecial);
			} else if (playerInfo.charaType == PlayerInfo.YUKO) {
				SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.yukoSpecial);
			}else if(playerInfo.charaType == PlayerInfo.MISAKI){
				SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.misakiSpecial);
			}else if(playerInfo.charaType == PlayerInfo.BLACKKOHAKU){
				SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.yamikoSpecial);
			}
		}));
		sq.Append(transform.DOMoveY (0f,0.2f).OnStart(()=>{
			gameObject.tag = Const.TAG_SPECIAL_FALL;
		}));
		sq.OnUpdate (() => {
			if(isDamage){

				sq.Complete();
			}
		}).OnComplete(()=>{
			isDamage = false;
			transform.position = p;
			gameObject.tag = Const.TAG_PLAYER;
			specialMove = false;
			GetComponent<InitPosition> ().enabled = true;
		});
		sq.Play ();

	}

	IEnumerator TagChange(string tagName,float start,float end){
		yield return new WaitForSeconds (start);
		if (gameObject.tag == Const.TAG_NODAMAGE
			|| gameObject.tag == Const.TAG_HEADSPRING
			|| gameObject.tag == Const.TAG_DEAD
			|| gameObject.tag == Const.TAG_DOWN
			|| gameObject.tag == Const.TAG_LITE_DAMAGE
			|| gameObject.tag == Const.TAG_MIDDLE_DAMAGE
			|| gameObject.tag == Const.TAG_HADOUKEN) {
		} else {
			gameObject.tag = tagName;
		}
		yield return new WaitForSeconds (end);
		if (gameObject.tag == tagName) {
			gameObject.tag = Const.TAG_PLAYER;
		}
	}
	
	IEnumerator TagChangeSolt(string tagName,float start,float end){
		yield return new WaitForSeconds (start);
		if (gameObject.tag == Const.TAG_NODAMAGE
			|| gameObject.tag == Const.TAG_HEADSPRING
			|| gameObject.tag == Const.TAG_DEAD
			|| gameObject.tag == Const.TAG_DOWN
			|| gameObject.tag == Const.TAG_LITE_DAMAGE
			|| gameObject.tag == Const.TAG_MIDDLE_DAMAGE
			|| gameObject.tag == Const.TAG_HADOUKEN) {
		} else {
			gameObject.tag = tagName;
		}
		yield return new WaitForSeconds (end);
		if (gameObject.tag == Const.TAG_NODAMAGE
			|| gameObject.tag == Const.TAG_GUARD
			|| gameObject.tag == Const.TAG_HEADSPRING
			|| gameObject.tag == Const.TAG_DEAD
			|| gameObject.tag == Const.TAG_DOWN
			|| gameObject.tag == Const.TAG_LITE_DAMAGE
			|| gameObject.tag == Const.TAG_MIDDLE_DAMAGE
			|| gameObject.tag == Const.TAG_HADOUKEN) {
		} else {
			gameObject.tag = Const.TAG_PLAYER;
		}
	}

	IEnumerator TagChangeSp(string tagName,string afterTag,float start,float end){
		yield return new WaitForSeconds (start);
		if (gameObject.tag == Const.TAG_NODAMAGE
			|| gameObject.tag == Const.TAG_HEADSPRING
			|| gameObject.tag == Const.TAG_DEAD
			|| gameObject.tag == Const.TAG_DOWN
			|| gameObject.tag == Const.TAG_LITE_DAMAGE
			|| gameObject.tag == Const.TAG_MIDDLE_DAMAGE
			|| gameObject.tag == Const.TAG_HADOUKEN) {
		} else {
			gameObject.tag = tagName;
		}
		yield return new WaitForSeconds (end);
		if (gameObject.tag == Const.TAG_NODAMAGE
			|| gameObject.tag == Const.TAG_GUARD
			|| gameObject.tag == Const.TAG_HEADSPRING
			|| gameObject.tag == Const.TAG_DEAD
			|| gameObject.tag == Const.TAG_DOWN
			|| gameObject.tag == Const.TAG_LITE_DAMAGE
			|| gameObject.tag == Const.TAG_MIDDLE_DAMAGE
			|| gameObject.tag == Const.TAG_HADOUKEN) {
		} else {
			gameObject.tag = afterTag;
		}
	}

	void SpecialOff(){
		special = false;
	}
	void PuchOff(){
		firstPunch = false;
	}
	void KickOff(){
		firstKick = false;
	}

	//被ダメージ
	void OnCollisionEnter(Collision collision){

		string otherTag = collision.gameObject.tag;
		string myTag = gameObject.tag;

		bool isHit = false;

		if (!Hit (collision)) {
			return;
		}

		if (otherTag == Const.TAG_PUNCH) {
			isHit = true;
			collision.gameObject.tag =  Const.TAG_PLAYER;
			LiteDamage ();
		} else if (otherTag == Const.TAG_KICK) {
			isHit = true;
			collision.gameObject.tag =  Const.TAG_PLAYER;
			MiddleDamage ();
		} else if (otherTag == Const.TAG_SOLT) {
			if (myTag == Const.TAG_SOLT) {
				return;
			}
			isHit = true;
			collision.gameObject.tag =  Const.TAG_PLAYER;
			BigDamage ();
		}
		if (isHit) {
			isDamage = true;
			Invoke ("OffDamage", 0.2f);
		}
	}

	void OffDamage(){
		isDamage = false;
	}

	void OnCollisionStay(Collision collision){

		if (collision.gameObject.tag == Const.TAG_SPECIAL) {
			if (!Hit (collision)) {
				return;
			}
			collision.gameObject.tag = Const.TAG_SPECIAL_UPPER;
			SpecialDamage ();
		}
	}

	public void LiteDamage(){
		playerInfo.life -= Const.DAMAGE_LITE;
		HitDamage (playerInfo,"Damage1");
	}

	public void MiddleDamage(){
		playerInfo.life -= Const.DAMAGE_MIDDLE;
		HitDamage (playerInfo,"BigDamage");
	}

	public void BigDamage(){
		playerInfo.life -= Const.DAMAGE_BIG;
		HitDamage (playerInfo,"DamageDown");
	}

	private void SpecialDamage(){
		isDamage = true;
		Invoke ("OffDamage", 0.2f);
		playerInfo.life -= Const.DAMAGE_SPECIAL;
		HitDamage (playerInfo,"DamageDown");
	}

	private void HitDamage(PlayerInfo playerInfo,string damageName){
		if (playerInfo.life > 0) {
			animator.Play(damageName,0,0f);
		} else {
			// キャラで声分け
			if (playerInfo.charaType == PlayerInfo.KOHAKU) {
				SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuDead);
			} else if (playerInfo.charaType == PlayerInfo.YUKO) {
				SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.yukoDead);
			}else if(playerInfo.charaType == PlayerInfo.MISAKI){
				SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.misakiDead);
			}else if(playerInfo.charaType == PlayerInfo.BLACKKOHAKU){
				SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.yamikoDead);
			}
			GetComponent<InitPosition> ().enabled = false;
			gameObject.tag = Const.TAG_DEAD;
			animator.SetBool ("Dead", true);
		}
		gameManager.SendMessage ("HitDamage",playerInfo);
	}

	private bool Hit(Collision other){
		if (gameObject.tag == Const.TAG_NODAMAGE
			|| gameObject.tag == Const.TAG_GUARD 
			|| gameObject.tag == Const.TAG_HEADSPRING
			|| gameObject.tag == Const.TAG_SPECIAL
			|| gameObject.tag == Const.TAG_SPECIAL_UPPER
			|| gameObject.tag == Const.TAG_SPECIAL_FALL
			|| gameObject.tag == Const.TAG_DEAD
			|| gameObject.tag == Const.TAG_DOWN
			|| gameObject.tag == Const.TAG_HADOUKEN
			|| isHadouken
			|| isDamage
			|| playerInfo.life <= 0 ) {
			if (gameObject.tag == Const.TAG_GUARD) {
				if (other.gameObject.tag == Const.TAG_SOLT) {
					other.gameObject.tag =  Const.TAG_PLAYER;
					AddSuperBar (5);
				} else if (other.gameObject.tag == Const.TAG_SPECIAL) {
					other.gameObject.tag =  Const.TAG_SPECIAL_UPPER;
					AddSuperBar (10);
				} else if (other.gameObject.tag == Const.TAG_PUNCH) {
					other.gameObject.tag =  Const.TAG_PLAYER;
					AddSuperBar (2);
				} else if (other.gameObject.tag == Const.TAG_KICK) {
					other.gameObject.tag =  Const.TAG_PLAYER;
					AddSuperBar (2);
				}
			}
			return false;
		}
		return true;
	}

	public void PlaySe(AudioClip se){
		SoundMainScene.Instans.PlaySe (se);
	}

	public void SetOnStart(Transform t){
		SendMessage ("OnStart",t);
	}

	public void SetSgage(int sgage){
		playerInfo.sGage = sgage;
		SetSSButton (playerInfo.sGage);
	}

	public void SetSSButton(int sgage){
		
		if (ssButton != null) {
			if (sgage >= Const.MAX_S_GAGE) {
				DOTween.ToAlpha (() => ssButton.color, color => ssButton.color = color, 1f, 0f);
				DOTween.ToAlpha (() => ssText.color, color => ssText.color = color, 1f, 0f);
			} else {
				DOTween.ToAlpha (() => ssButton.color, color => ssButton.color = color, 0.2f, 0f);
				DOTween.ToAlpha (() => ssText.color, color => ssText.color = color, 0.2f, 0f);
			}
		}
	}

	public void SSAction(){
		gameObject.tag = Const.TAG_HADOUKEN;
		isHadouken = true;
		// 超必殺
		if (playerInfo.charaType == PlayerInfo.KOHAKU) {
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.kohakuSS);
		} else if (playerInfo.charaType == PlayerInfo.YUKO) {
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.yukoSS);
		} else if (playerInfo.charaType == PlayerInfo.MISAKI) {
			SoundMainScene.Instans.PlaySe (SoundMainScene.Instans.misakiSS);
		}else if(playerInfo.charaType == PlayerInfo.BLACKKOHAKU){
			SoundMainScene.Instans.PlaySe(SoundMainScene.Instans.yamikoSS);
		}

		Tween tween = DOVirtual.DelayedCall (0.5f, () => {
			shooter.SendMessage ("Shot", gameObject);
		}).OnComplete(()=>{
			isHadouken = false;
		});

		tween.OnUpdate (() => {
			if(isDamage){
				isDamage = false;
				isHadouken = false;
				tween.Kill();
			}
		});

		playerInfo.sGage = 0;
		SetSSButton (playerInfo.sGage);
		gameManager.SendMessage ("AddSuperBar", playerInfo);
	}

	public void HitSS(){
		if (gameObject.tag == Const.TAG_NODAMAGE
			|| gameObject.tag == Const.TAG_HEADSPRING
			|| gameObject.tag == Const.TAG_DEAD
			|| gameObject.tag == Const.TAG_DOWN
			|| playerInfo.life <= 0){

			return;
		}
		isDamage = true;
		Invoke ("OffDamage", 0.2f);
		if (gameObject.tag == Const.TAG_GUARD) {
			playerInfo.life -= Const.DAMAGE_SS / 5;
			if (playerInfo.life <= 0) {
				HitDamage (playerInfo, "DamageDown");
			} else {
				AddSuperBar (10);
				gameManager.SendMessage ("HitDamage",playerInfo);
			}
		} else {
			playerInfo.life -= Const.DAMAGE_SS;
			gameObject.tag = Const.TAG_DOWN;
			HitDamage (playerInfo,"DamageDown");
		}
	}
	public void InitSSButton(int sgage){
		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {

			if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
				GameObject ssButtonObj = GameObject.Find ("SSButton1");
				ssButton = ssButtonObj.GetComponent<Image> ();
				GameObject ssTextObj = GameObject.Find ("SSText1");
				ssText = ssTextObj.GetComponent<TextMeshProUGUI> ();
			} else {
				GameObject ssButtonObj = GameObject.Find ("SSButton2");
				ssButton = ssButtonObj.GetComponent<Image> ();
				GameObject ssTextObj = GameObject.Find ("SSText2");
				ssText = ssTextObj.GetComponent<TextMeshProUGUI> ();
			}

		} else {

			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
					GameObject ssButtonObj = GameObject.Find ("SSButton1");
					ssButton = ssButtonObj.GetComponent<Image> ();
					GameObject ssTextObj = GameObject.Find ("SSText1");
					ssText = ssTextObj.GetComponent<TextMeshProUGUI> ();
				}
			} else {
				if (playerInfo.playerType == PlayerInfo.PlayerType.Player2) {
					GameObject ssButtonObj = GameObject.Find ("SSButton1");
					ssButton = ssButtonObj.GetComponent<Image> ();
					GameObject ssTextObj = GameObject.Find ("SSText1");
					ssText = ssTextObj.GetComponent<TextMeshProUGUI> ();
				}
			}
		}
	}

	public void Init(PlayerInfo.PlayerType type){
		
		playerInfo.playerType = type;
		gameObject.layer = LayerMask.NameToLayer("Player");
		action = PlayerInfo.Action.None;
		if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
			playerInfo.charaType = GameMaster.Instance.charaType1;
		} else {
			playerInfo.charaType = GameMaster.Instance.charaType2;
		}

		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			playerInfo.humanType = PlayerInfo.HumanType.Human;
			GetComponent<EnemyAi> ().enabled = false;

		} else {

			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				//左ならCOMは右
				if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
					GetComponent<EnemyAi> ().enabled = false;
					playerInfo.humanType = PlayerInfo.HumanType.Human;
				} else {
					humanPlayer = GameObject.Find ("Player1");
					playerInfo.humanType = PlayerInfo.HumanType.Com;
					GetComponent<EnemyAi> ().enabled = true;
					enemyAi = GetComponent<EnemyAi> ();
				}
			} else {
				//右ならCOMは左
				if (playerInfo.playerType == PlayerInfo.PlayerType.Player1) {
					humanPlayer = GameObject.Find ("Player2");
					playerInfo.humanType = PlayerInfo.HumanType.Com;
					GetComponent<EnemyAi> ().enabled = true;
					enemyAi = GetComponent<EnemyAi> ();
				} else {
					GetComponent<EnemyAi> ().enabled = false;
					playerInfo.humanType = PlayerInfo.HumanType.Human;
				}
			}
		}
	}
}
