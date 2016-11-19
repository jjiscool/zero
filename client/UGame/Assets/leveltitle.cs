using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class leveltitle : MonoBehaviour {
	public GameObject title;
	public GameObject info;
	public GameObject cover;
	private bool isFinish;
	// Use this for initialization
	void Awake(){
		title.GetComponent<Text>().text =GameObject.Find ("map").GetComponent<MapObjectGenerator> ().LevelTitle;
		info.GetComponent<Text>().text =GameObject.Find ("map").GetComponent<MapObjectGenerator> ().LevelInfo;
		isFinish = false;
	}
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isFinish&&GameObject.Find ("map").GetComponent<RoundControler> ().player != null) {
			isFinish = true;
			transform.GetComponent<Animator> ().SetTrigger ("fadeit");

		}
		if (isFinish&&transform.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName("fadetitle")&&transform.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
			Destroy (transform.gameObject);
		}
	
	}
}
