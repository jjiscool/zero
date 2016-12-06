using UnityEngine;
using System.Collections;

public class popController : MonoBehaviour {

	//弹框类型
	//Exit 下一层

	public GameObject popObj;

	// Use this for initialization
	void Start () {
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, 0, 0);



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fadeIn(string popType){
		switch (popType) {
		case "Exit":
			popObj = GameObject.Find ("UI/popContainer/popPannelExit");
			break;
		default:
			Debug.Log (22222);
			break;
		}

		if (popObj != null) {
			popObj.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		}

		transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		transform.Find ("popCover").GetComponent<Animator> ().SetTrigger ("fadeIn");

	}


}
