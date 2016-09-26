using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour {
	private bool isMoving;
	//private Vector3 beginxy;
	private Vector3 endxy;
	public float speed;
	public float step;
	// Use this for initialization

	private GameObject map;

	//player初始位置
	private int[] iniCell;
	private Vector2 iniPos;

	//player动画控制
	private Animator animator; 

	//存储player的行，列；在移动的时候变化
	private int row;
	private int column;

	void Awake(){
		map = GameObject.Find ("map");

		iniCell = map.GetComponent<RandomDungeonCreator>().getRoomRandomCell();

		row = iniCell [0];
		column = iniCell [1];

		iniPos = map.GetComponent<TilesManager>().posTransform(row,column);
		//初始化位置
		transform.position = iniPos;

	}

	void Start () {
		isMoving = false;
		endxy = transform.position;
		animator = GetComponent<Animator>();
	}

	//根据单元格做碰撞检测
	private void AttemptMove(string dir,int i,int j){
		string tileType = map.GetComponent<RandomDungeonCreator>().getMapTileType(i,j);
		switch (tileType) {
			default:

				if (isMoving)
					return;
				switch (dir) {
				case "UP":
					endxy = new Vector3 (transform.position.x, transform.position.y + step, 0);
					row--;
				Debug.Log (row + "," + column + " UP");
					break;
				case "DOWN":
					endxy = new Vector3 (transform.position.x, transform.position.y - step, 0);
					row++;
				Debug.Log (row + "," + column + " DOWN");
					break;
				case "LEFT":
					endxy = new Vector3 (transform.position.x - step, transform.position.y, 0);
					column--;
				Debug.Log (row + "," + column + " LEFT");
					break;
				case "RIGHT":
					endxy = new Vector3 (transform.position.x + step, transform.position.y, 0);
					column++;
				Debug.Log (row + "," + column + " RIGHT");
					break;
				}
				isMoving = true;
			break;
			case "WALL":
				Debug.Log ("cnot go");
				return;
			break;
		}
	}

	public void moveUp(){
		animator.SetTrigger ("PlayerMoveUp");
		AttemptMove ("UP",row - 1, column);
	
	}
	public void moveDown(){
		animator.SetTrigger ("PlayerMoveDown");
		AttemptMove ("DOWN",row+1, column);

	}
	public void moveLeft(){
		animator.SetTrigger ("PlayerMoveLeft");
		AttemptMove ("LEFT",row, column-1);

	}
	public void moveRight(){
		animator.SetTrigger ("PlayerMoveRight");
		AttemptMove ("RIGHT",row, column+1);
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
