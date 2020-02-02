using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraAngle : MonoBehaviour {

	Vector3 initPos = new Vector3 (0f, 0.4f, 0.6f);

	Vector3 mainPos = new Vector3 (0.05f, 0.4f, -1.9f);
	Vector3 mainRotate = new Vector3 (0f, 0f, 0f);

	Vector3 p1WinPos = new Vector3(0.8f,0.28f,-0.53f);
	Vector3 p1WinRotate = new Vector3(-11.2f,-62.3f,0f);

	Vector3 p2WinPos = new Vector3(-0.7f,0.27f,0.3f);
	Vector3 p2WinRotate = new Vector3(-13.06f,115.74f,0f);

	Vector3 p1RWinPos = new Vector3(1f,0.18f,-0.15f);
	Vector3 p1RWinRotate = new Vector3(-17.01f,-81.3f,0f);

	Vector3 p2RWinPos = new Vector3(-0.8f,0.43f,-0.6f);
	Vector3 p2RWinRotate = new Vector3(-1.78f,60f,0f);

	Vector3 rLosePos = new Vector3(0.48f,1.44f,-0.13f);
	Vector3 rLoseRotate = new Vector3 (81.69f, -10.09f, 0f);

	Vector3 lLosePos = new Vector3(-0.52f,1.44f,0f);
	Vector3 lLoseRotate = new Vector3(87.3f,-10.09f,0f);

	// Use this for initialization
	void Start () {
		if (GameMaster.Instance.mode != GameMaster.GameMode.EndlessMode) {
			// キャラ対面アップ
			if (GameMaster.Instance.round == 0) {
				transform.position = new Vector3 (0.02f, 0.52f, -0.8f);
				transform.rotation = Quaternion.Euler (new Vector3 (-8.5f, 0f, 0f));
			}
		} else {
			transform.position = new Vector3 (0f, 0.4f, 0.6f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(){
		transform.position = initPos;
		transform.rotation = Quaternion.Euler (Vector3.zero);
		Vector3 pos = mainPos;
		Vector3 rt = mainRotate;
		Sequence sq = DOTween.Sequence ();
		sq.Append (transform.DOMove (pos,2f));
		sq.Join(transform.DORotate(rt,2f));
		sq.Play ();

	}

	public void Win(PlayerInfo.PlayerType player){
		if (GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode) {
			if (player == PlayerInfo.PlayerType.Player1) {
				Sequence sq = DOTween.Sequence ();
				sq.Append (transform.DOMove (p1WinPos, 2f));
				sq.Join (transform.DORotate (p1WinRotate, 2f));
				sq.Play ();
			} else {
				Sequence sq = DOTween.Sequence ();
				sq.Append (transform.DOMove (p2WinPos, 2f));
				sq.Join (transform.DORotate (p2WinRotate, 2f));
				sq.Play ();
			}
		} else {
			if (GameMaster.Instance.ctrPos == GameMaster.CtrLeft) {
				if (player == PlayerInfo.PlayerType.Player1) {
					Sequence sq = DOTween.Sequence ();
					sq.Append (transform.DOMove (p1WinPos, 2f));
					sq.Join (transform.DORotate (p1WinRotate, 2f));
					sq.Play ();
				} else {
					Sequence sq = DOTween.Sequence ();
					sq.Append (transform.DOMove (p2WinPos, 2f));
					sq.Join (transform.DORotate (p2WinRotate, 2f));
					sq.Play ();
				}
			} else {
				if (player == PlayerInfo.PlayerType.Player1) {
					Sequence sq = DOTween.Sequence ();
					sq.Append (transform.DOMove (p1RWinPos, 2f));
					sq.Join (transform.DORotate (p1RWinRotate, 2f));
					sq.Play ();
				} else {
					Sequence sq = DOTween.Sequence ();
					sq.Append (transform.DOMove (p2RWinPos, 2f));
					sq.Join (transform.DORotate (p2RWinRotate, 2f));
					sq.Play ();
				}
			}
		}
	}

	public void Lose(PlayerInfo.PlayerType player){
		Vector3 endPos = Vector3.zero;
		Vector3 endRotate =  Vector3.zero;
		if (player == PlayerInfo.PlayerType.Player1) {
			endPos = lLosePos;
			endRotate = lLoseRotate;
		} else {
			endPos = rLosePos;
			endRotate = rLoseRotate;
		}
		Sequence sq = DOTween.Sequence ();
		sq.Append (transform.DOMove (endPos, 3f));
		sq.Join (transform.DORotate (endRotate, 3f));
		sq.Play ();
	}

	public void SSAngle(PlayerInfo.PlayerType player){
		/*Time.timeScale = 0.1f;
		DOVirtual.DelayedCall(1f,()=>{
			Time.timeScale = 1;
		});*/
		/*Vector3 p = transform.position;
		Quaternion q = transform.rotation;

		if((GameMaster.Instance.mode == GameMaster.GameMode.TwoPlayerMode 
			&& player == PlayerInfo.PlayerType.Player1) ||
			(GameMaster.Instance.ctrPos == GameMaster.CtrLeft && player == PlayerInfo.PlayerType.Player1)){
			Sequence sq = DOTween.Sequence ();
			sq.Append (transform.DOMove (new Vector3(0.05f,0.6f,-0.61f), 0.3f));
			sq.Join (transform.DORotate (new Vector3(5.25f,-12.86f,0f), 0.3f));
			sq.OnComplete (() => {
				Time.timeScale = 0.1f;
				DOVirtual.DelayedCall(1f,()=>{
					Time.timeScale = 1;
				});

			});
			sq.Play ();
		}*/
	}
}
