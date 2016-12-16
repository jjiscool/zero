using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infofollow : MonoBehaviour {
	public GameObject obj;
	public float rate;
	// Use this for initialization
	void Start () {
		rate = 1;
		obj = transform.parent.gameObject;
		if (obj.GetComponent<playerStatus> ().AI)
			gameObject.SetActive (true);
		else
			gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (obj != null&&obj.GetComponent<playerStatus>().AI) {
			transform.position = obj.transform.position;
			rate = (float)(obj.GetComponent<playerStatus> ().HP) / (float)obj.GetComponent<playerStatus> ().HPMAX;
			transform.Find ("enemyHpFill").localScale = new Vector3 (rate*2.6f, 1, 1);
		}
	}
}
