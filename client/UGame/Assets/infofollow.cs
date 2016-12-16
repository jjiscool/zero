using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infofollow : MonoBehaviour {
	public GameObject obj;
	public float rate;
	public int lv;
	// Use this for initialization
	void Start () {
		rate = 1;
		lv = 0;
		obj = transform.parent.gameObject;
	
		if (obj.GetComponent<playerStatus> ().AI)
			gameObject.SetActive (true);
		else
			gameObject.SetActive (false);
		lv = obj.GetComponent<playerStatus>().LV;
		transform.Find ("lv").GetComponent<MeshRenderer> ().sortingOrder = 7;
	}
	
	// Update is called once per frame
	void Update () {
		if (obj != null&&obj.GetComponent<playerStatus>().AI) {
			transform.position = obj.transform.position;
			rate = (float)(obj.GetComponent<playerStatus> ().HP) / (float)obj.GetComponent<playerStatus> ().HPMAX;
			if (rate < 0)
				rate = 0;
			if (rate > 1)
				rate = 1;
			transform.Find ("enemyHpFill").localScale = new Vector3 (rate*2.6f, 1, 1);
			transform.Find ("lv").GetComponent<TextMesh> ().text = ""+lv;
		}
	}
}
