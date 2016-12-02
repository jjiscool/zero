using UnityEngine;
using System.Collections;

public class popController : MonoBehaviour {

	//枚举弹框类型
	public enum PopType
	{
		Exit,//出口
		Poison
	}

	public PopType type;

	public GameObject popObj;

	// Use this for initialization
	void Start () {
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, 0, 0);



	}
	
	// Update is called once per frame
	void Update () {
	

		switch (type) {
		case PopType.Exit:
			popObj = GameObject.Find ("UI/popContainer/popPannelExit");
			break;
		default:
			Debug.Log (22222);
			break;
		}
	}

	public void fadeIn(){


		if (popObj != null) {
			popObj.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		}

		transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		transform.Find ("popCover").GetComponent<Animator> ().SetTrigger ("fadeIn");

	}


}
