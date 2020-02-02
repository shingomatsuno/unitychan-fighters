using UnityEngine;
using System.Collections;

public class BeamCollision : MonoBehaviour {
	
	private bool bHit = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//RayCollision
		RaycastHit hit;
		int layerMask = 1 << LayerMask.NameToLayer("Player");
		if (!bHit && Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
			if (hit.collider.gameObject.tag == Const.TAG_NODAMAGE
				|| hit.collider.gameObject.tag == Const.TAG_HEADSPRING
				|| hit.collider.gameObject.tag == Const.TAG_DEAD
				|| hit.collider.gameObject.tag == Const.TAG_DOWN){

				return;
			}
			bHit = true;
            GameObject hitobj = hit.collider.gameObject;
			hitobj.SendMessage ("HitSS");
		}
	}
}
