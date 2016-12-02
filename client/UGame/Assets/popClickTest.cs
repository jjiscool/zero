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
		GameObject.Find ("popContainer").GetComponent<popController> ().type = popController.PopType.Exit;
		GameObject.Find ("popContainer").GetComponent<popController> ().fadeIn();
	}
}
