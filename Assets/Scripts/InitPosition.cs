using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InitPosition : MonoBehaviour {

	Vector3 initPosition;
	Quaternion initRotation;
	bool onStart;
	// Use this for initialization
	void Start () {
		initPosition = transform.position;
		initRotation = transform.rotation;
	}

	// Update is called once per frame
	void Update () {

		if (!onStart) {
			return;
		}

		RaycastHit hit;
		Vector3 dir = transform.TransformDirection (Vector3.down);
		Vector3 p = transform.position;
		p.y += 0.1f;
		if (Physics.Raycast (p, dir, out hit, 0.3f)) {
			if (hit.collider.tag == "Ground") {
				transform.DORotate (initRotation.eulerAngles, 0.2f);
				transform.DOMove (initPosition, 0.1f);
			}
		}
	}

	public void OnStart(Transform t){
		initPosition = t.position;
		initRotation = t.rotation;
		onStart = true;
	}
}
