using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour {
	private bool isMoving;
	//private Vector3 beginxy;
	private Vector3 endxy;
	public float speed;
	public float step;
	// Use this for initialization
	void Start () {
		isMoving = false;
		endxy = transform.position;
//		Debug.Log (transform.position.y);
	}
	public void moveUp(){
		if (isMoving)
			return;
		//beginxy = transform.position;
		//Debug.Log("UP"+transform.position.y);
		endxy = new Vector3(transform.position.x,transform.position.y+step,0);

		isMoving = true;
	
	}
	public void moveDown(){
		if (isMoving)
			return;
		Debug.Log("Down");
		//beginxy = transform.position;
		endxy = new Vector3(transform.position.x,transform.position.y-step,0);
		isMoving = true;

	}
	public void moveLeft(){
		if (isMoving)
			return;
		Debug.Log("Left");
		//beginxy = transform.position;
		endxy = new Vector3(transform.position.x-step,transform.position.y,0);
		isMoving = true;

	}
	public void moveRight(){
		if (isMoving)
			return;
		Debug.Log("Rigth");
		//beginxy = transform.position;
		endxy = new Vector3(transform.position.x+step,transform.position.y,0);
		isMoving = true;

	}
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			transform.position = new Vector3 (Mathf.MoveTowards (transform.position.x, endxy.x, Time.deltaTime * speed), Mathf.MoveTowards (transform.position.y, endxy.y, Time.deltaTime * speed), 0);
			GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();
		}
		if (transform.position == endxy) {
			transform.position = endxy;
			isMoving = false;
			//Debug.Log("stop");
		}

	}
}
