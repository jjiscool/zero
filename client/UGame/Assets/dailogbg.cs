using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class dailogbg : MonoBehaviour {
	public GameObject dtextgo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (dtextgo.GetComponent<Text> ().rectTransform.position.x);
	}
}
