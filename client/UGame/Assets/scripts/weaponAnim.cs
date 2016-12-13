using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class weaponAnim : MonoBehaviour {

	public GameObject animObj;
	private Animator anim;
	public Vector3 startxy;
	public Vector3 endxy;


	// Use this for initialization
	void Start () {
		if (animObj!=null) {
			anim = animObj.GetComponent<Animator>();

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
