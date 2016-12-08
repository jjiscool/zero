using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class popController : MonoBehaviour {

	//弹框prefabs
	public GameObject popExitPb;//Exit 下一层

	private GameObject popObj;
	private Text popTit;
	private Button popBtnConfirm;
	private Button popBtnCancel;
	private Animator popAnimator;

	//按钮事件
	public delegate void BtnConfirm();
	public static event BtnConfirm btnConfirmEvent;

	public delegate void BtnCancel();
	public static event BtnCancel btnCancelEvent;

	public static string popTitText;

	void Start () {
		transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fadeIn(string popType){
		switch (popType) {
		case "ConfirmPop":
			popObj= (GameObject)Instantiate (popExitPb,new Vector2(0,0),transform.rotation);

			break;
		default:
			Debug.Log (22222);
			break;
		}

		if (popObj != null) {
			


			popAnimator = popObj.transform.FindChild("popCover").GetComponent<Animator>();
			popBtnConfirm = popObj.transform.Find("popPannel/BtnConfirm").GetComponent<Button>();
			popBtnCancel = popObj.transform.Find("popPannel/BtnCancel").GetComponent<Button>();
			popTit = popObj.transform.Find("popPannel/popTit").GetComponent<Text>();

			transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

			popObj.transform.parent = transform;
			popObj.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			popObj.GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, 0);
			popAnimator.SetTrigger ("fadeIn");
			popObj.transform.FindChild("popPannel").DOMoveY(8, 0.5f).From().SetEase(Ease.InOutBack);

			//绑定按钮事件
//			popBtnConfirm.onClick.AddListener (btnConfirmEvent);  为什么不行

			popBtnConfirm.onClick.AddListener (()=>btnConfirmEvent());
			popBtnCancel.onClick.AddListener (()=>btnCancelEvent());

			//标题文案
			popTit.text = popTitText;
		}
			


	}

	public void fadeOut(){
		if (popObj != null) {
			transform.GetComponent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
			GameObject.Destroy (popObj);
		}
	}


}
