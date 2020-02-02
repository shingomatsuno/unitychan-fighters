using UnityEngine;
using System.Collections;
using DG.Tweening;
public class MultiShooter : MonoBehaviour {

	public GameObject Shot1;
    public GameObject Wave;
	public float Disturbance = 0;

	public int ShotType = 0;

	private GameObject NowShot;

	void Start () {
		NowShot = null;
	}

	public void Shot(GameObject player){
			GameObject Bullet;
			Vector3 p = player.transform.position;
			p.y = 0.55f;

			Quaternion q = player.transform.rotation;

			Bullet = Shot1;
			//Fire
			GameObject s1 = (GameObject)Instantiate (Bullet, p, q);
			s1.GetComponent<BeamParam> ().SetBeamParam (this.GetComponent<BeamParam> ());

			GameObject wav = (GameObject)Instantiate (Wave, p, q);
			wav.transform.localScale *= 0.25f;
			wav.transform.Rotate (Vector3.left, 90.0f);
			wav.GetComponent<BeamWave> ().col = this.GetComponent<BeamParam> ().BeamColor;
	}
}
