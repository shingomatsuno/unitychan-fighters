using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class BarBox : MonoBehaviour {
 
	public GameObject bar;

	RectTransform barRectTransform;

    Transform barTransform;

    private float nowRate;
 
    private Coroutine barCoroutine;
 
    void Awake()
    {
		barTransform = bar.transform;

		barRectTransform = bar.GetComponent<RectTransform> ();

        barTransform.localScale = new Vector3(1, 1, 1);
    }
 
    void Start () {
     
    }
     
    void Update () {
     
    }
 
    //自身の子要素に数値に応じた動作を実行
    public void SetBar(int now)
    {
		Vector2 p = barRectTransform.anchoredPosition;
		int posX = Const.MAX_LIFE - now;
		if (bar.name == "Bar1") {
			if (posX < 0) {
				posX = 0;
			}
			p.x = posX;
		} else {
			if (posX > 100) {
				posX = 100;
			}
			p.x = -posX;
		}
		barRectTransform.anchoredPosition = p;
		nowRate = (float)now / Const.MAX_LIFE;
		if (nowRate < 0) {
			nowRate = 0;
		}
        barTransform.localScale = new Vector3(nowRate, 1, 1);
    }

	//自身の子要素に数値に応じた動作を実行
	public void SetSuperBar(int now)
	{
		Vector2 p = barRectTransform.anchoredPosition;
		int posX = Const.MAX_S_GAGE - now;
		if (bar.name == "Bar1") {
			if (posX < 0) {
				posX = 0;
			}
			p.x = -posX;
		} else {
			if (posX > 100) {
				posX = 100;
			}
			p.x = posX;
		}
		barRectTransform.anchoredPosition = p;
		nowRate = (float)now / Const.MAX_S_GAGE;
		if (nowRate < 0) {
			nowRate = 0;
		}
		barTransform.localScale = new Vector3(nowRate, 1, 1);
	}
}