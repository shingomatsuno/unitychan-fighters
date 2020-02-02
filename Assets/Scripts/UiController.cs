using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiController : MonoBehaviour {

	public int animType;
	public GameObject videoButton;
	public GameObject continuePanel;

	const int ANIM_WIDE = 1;
	const int ANIM_WIDEY = 2;
	const int ANIM_WIDEX = 3;
	const int SLIDEY = 4;
	const int ANIM_WIDEX2 = 5;

	RectTransform rect;

	void Awake(){
		rect = GetComponent<RectTransform> ();
	}

	// Use this for initialization
	void Start () {
		switch (animType) {
		case ANIM_WIDE:
			Wide ();
			break;
		case ANIM_WIDEY:
			WideY ();
			break;
		case ANIM_WIDEX:
			WideX ();
			break;
		case SLIDEY:
			SlideY ();
			break;
		case ANIM_WIDEX2:
			WideX2 ();
			break;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Wide(){
		Sequence seq = DOTween.Sequence ();
		seq.Append (rect.DOScale (new Vector3 (1.2f, 1.2f, 1f), 0.7f));
		seq.Append (rect.DOScale (new Vector3 (1f, 1f, 1f), 0.3f));
		seq.OnComplete (() => {
			videoButton.SetActive(true);
		});
		seq.Play ();
	}

	private void WideY(){
		Sequence seq = DOTween.Sequence ();
		seq.Append (rect.DOScaleY (1.2f, 0.7f));
		seq.Append (rect.DOScaleY (1f, 0.2f));
		seq.Play ();
	}

	private void WideX(){
		Sequence seq = DOTween.Sequence ();
		seq.Append (rect.DOScaleX (1.2f, 0.7f));
		seq.Append (rect.DOScaleX (1f, 0.2f));
		seq.OnComplete (() => {
			Invoke("ShowContinue",1f);
		});
		seq.Play ();
	}
	private void WideX2(){
		Sequence seq = DOTween.Sequence ();
		seq.Append (rect.DOScaleX (2.2f, 0.7f));
		seq.Append (rect.DOScaleX (2f, 0.2f));
		seq.Play ();
	}
	private void ShowContinue(){
		continuePanel.SetActive(true);
	}

	private void SlideY(){
		float y = Screen.height;
		rect.DOMoveY (y/2f, 1f);
	}
}
