using UnityEngine;
using System.Collections;

public class popController : MonoBehaviour {


	// Use this for initialization
	void Start () {
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, 0, 0);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fadeIn(){
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		transform.Find ("popCover").GetComponent<Animator> ().SetTrigger ("fadeIn");

	}


}
