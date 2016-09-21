using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	private SpriteRenderer spriteRenderer;


	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
