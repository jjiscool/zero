using UnityEngine;
using System.Collections;

public class lightCover : MonoBehaviour {


	public Texture[] mask;

	private float b = 0;
	private float c = 0.1f;
	private int i = 0;


	void Cycle(){
		if (Time.time > b) {
			b = b + c;
			if (i < mask.Length-1) {
				i++;

			} else {
				i = 0;
			}
			GetComponent<SpriteRenderer>().material.SetTexture ("_Mask", mask[i]);

		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Cycle ();
	}
}
