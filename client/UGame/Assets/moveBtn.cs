using UnityEngine;
using System.Collections;

public class moveBtn : MonoBehaviour {
	public GameObject player;
	public float speed;
	public void onClickUp(){
		//player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + speed, 0);
		GameObject.Find ("player").GetComponent<playerMove> ().moveUp ();
		
	}
	public void onClickDown(){
		//player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - speed, 0);
		GameObject.Find ("player").GetComponent<playerMove> ().moveDown ();

	}
	public void onClickLeft(){
		//player.transform.position = new Vector3 (player.transform.position.x-speed, player.transform.position.y, 0);
		GameObject.Find ("player").GetComponent<playerMove> ().moveLeft ();
	}
	public void onClickRight(){
		//player.transform.position = new Vector3 (player.transform.position.x+speed, player.transform.position.y, 0);
		GameObject.Find ("player").GetComponent<playerMove> ().moveRight ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
