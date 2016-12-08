using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;


public class popClickTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.gameObject.GetComponent<Button>().onClick.AddListener(TestClick);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TestClick(){
		popController.btnConfirmEvent += doConfirm;
		popController.btnCancelEvent += doCancel;
		popController.popTitText = "测试title";

		GameObject.Find ("popContainer").GetComponent<popController> ().fadeIn("ConfirmPop");
	}

	public void doConfirm(){
		Debug.Log ("Confirm");
	}

	public void doCancel(){
		Debug.Log ("Cancel");

		GameObject.Find ("popContainer").GetComponent<popController> ().fadeOut ();

	}
}
