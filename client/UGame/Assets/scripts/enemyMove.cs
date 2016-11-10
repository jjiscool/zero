using UnityEngine;
using System.Collections;

public class enemyMove : MonoBehaviour {
	private bool isMoving;
	//private Vector3 beginxy;
	private Vector3 endxy;
	public float speed;
	public float step;
	// Use this for initialization

	private GameObject map;

	private GameObject role;
	private GameObject weapon;

	//player初始位置
	private int[] iniCell;
	private Vector2 iniPos;

	//player动画控制
	private Animator animator; 
	private Animator weaponAnimator;

	private int roleOrder;
	private int weaponOrder;

	private int pathid;
	//存储player的行，列，朝向；在移动的时候变化
	private int row;
	private int column;
	private string orientation;


	//	public Sprite weaponTileH;
	//	public Sprite weaponTileV;

	private Astar astar;
	void Awake(){
		map = GameObject.Find ("map");
		role = transform.Find ("man").gameObject;
		weapon = transform.Find ("weapon").gameObject;
		OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;//获取object列表
		row = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].row;
		column = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].column;
		orientation = "DOWN";
		iniCell = new int[2];
		iniCell [0] = row;
		iniCell [1] = column;
		iniPos = map.GetComponent<TilesManager>().posTransform(row,column);
		//初始化位置
		transform.position = iniPos;
		astar= new Astar();
	
	}

	void Start () {
		isMoving = false;
		endxy = transform.position;
		animator = role.GetComponent<Animator>();
		weaponAnimator = weapon.GetComponent<Animator>();


		//		Debug.Log (roleOrder);



	}

	private void PlaceRoleBehindWeapon(){
		roleOrder = role.GetComponent<SpriteRenderer> ().sortingOrder;
		weapon.GetComponent<SpriteRenderer> ().sortingOrder = roleOrder +1;
	}

	private void PlaceRoleBeforeWeapon(){
		roleOrder = role.GetComponent<SpriteRenderer> ().sortingOrder;
		weapon.GetComponent<SpriteRenderer> ().sortingOrder = roleOrder -1;
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
				//				Debug.Log (row + "," + column + " UP");
				break;
			case "DOWN":
				endxy = new Vector3 (transform.position.x, transform.position.y - step, 0);
				row++;
				//				Debug.Log (row + "," + column + " DOWN");
				break;
			case "LEFT":
				endxy = new Vector3 (transform.position.x - step, transform.position.y, 0);
				column--;
				//				Debug.Log (row + "," + column + " LEFT");
				break;
			case "RIGHT":
				endxy = new Vector3 (transform.position.x + step, transform.position.y, 0);
				column++;
				//				Debug.Log (row + "," + column + " RIGHT");
				break;
			}
			isMoving = true;
			break;
		case "WALL":
			Debug.Log ("cnot go");
			break;

		}
		return;
	}

	public void moveUp(){
		orientation = "UP";
		//		Debug.Log ("UP");
		PlaceRoleBehindWeapon();
		animator.SetTrigger ("PlayerMoveUp");
		weaponAnimator.SetTrigger ("WeaponOnMoveUp");
		AttemptMove (orientation,row - 1, column);

	}
	public void moveDown(){
		orientation = "DOWN";
		//Debug.Log ("Down");
		PlaceRoleBeforeWeapon();
		animator.SetTrigger ("PlayerMoveDown");
		weaponAnimator.SetTrigger ("WeaponOnMoveDown");
		AttemptMove (orientation,row+1, column);

	}
	public void moveLeft(){
		orientation = "LEFT";
		PlaceRoleBeforeWeapon();
		animator.SetTrigger ("PlayerMoveLeft");
		weaponAnimator.SetTrigger ("WeaponOnMoveLeft");
		AttemptMove (orientation,row, column-1);

	}
	public void moveRight(){
		orientation = "RIGHT";
		PlaceRoleBeforeWeapon();
		animator.SetTrigger ("PlayerMoveRight");
		weaponAnimator.SetTrigger ("WeaponOnMoveRight");
		AttemptMove (orientation,row, column+1);
	}
	public void Actioning(){
		if (isMoving) {
			transform.position = new Vector3 (Mathf.MoveTowards (transform.position.x, endxy.x, Time.deltaTime * speed), Mathf.MoveTowards (transform.position.y, endxy.y, Time.deltaTime * speed), 0);
			GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();
		}
		if (transform.position == endxy) {

			transform.position = endxy;
			pathid--;
			if (pathid < 0  && isMoving) {
				transform.gameObject.GetComponent<PhaseHandler>().state.handle (new Action (ACTION_TYPE.ACTION_NULL));
				isMoving = false;
				//				Debug.Log (orientation);
				//				根据朝向设置 player的动画
				switch (orientation){
				case "UP": 
					animator.SetTrigger ("PlayerIdleUp");
					weaponAnimator.SetTrigger ("WeaponOnIdleUp");
					break;
				case "DOWN": 
					animator.SetTrigger ("PlayerIdleDown");
					weaponAnimator.SetTrigger ("WeaponOnIdleDown");
					break;
				case "LEFT": 
					animator.SetTrigger ("PlayerIdleLeft");
					weaponAnimator.SetTrigger ("WeaponOnIdleLeft");
					break;
				case "RIGHT": 
					animator.SetTrigger ("PlayerIdleRight");
					weaponAnimator.SetTrigger ("WeaponOnIdleRight");
					break;
				default:
					animator.SetTrigger ("PlayerIdleDown");
					weaponAnimator.SetTrigger ("WeaponOnIdleDown");
					break;
				}

			}
			else if(pathid>=0){
				isMoving = false;
				//				Debug.Log ("path"+pathid+":"+row + "," + column + " to " + astar.finalpath [pathid] [0] + "," + astar.finalpath [pathid] [1]);
				if (astar.finalpath [pathid] [0] < row)
					moveUp ();
				if (astar.finalpath [pathid] [0] > row)
					moveDown ();
				if (astar.finalpath [pathid] [1] < column)
					moveLeft ();
				if (astar.finalpath [pathid] [1] > column)
					moveRight ();
				//Debug.Log (transform.position.x + "," + transform.position.y + " " + endxy.x + "," + endxy.y);
			}

		}

	}
	public void moveTo(int x,int y){
		int[] pos={x,y};
		astar= new Astar(row,column,pos[0],pos[1],map.GetComponent<RandomDungeonCreator>().getMap(),32,32);
		astar.Run ();
		//Debug.Log ("Path long = " + astar.finalpath.Count);
		pathid = astar.finalpath.Count-1;
		if (pathid >= 1) {
			//				Debug.Log ("path"+pathid+":"+row + "," + column + " to " + astar.finalpath [pathid] [0] + "," + astar.finalpath [pathid] [1]);
			if (astar.finalpath [pathid] [0] < row)
				moveUp ();
			if (astar.finalpath [pathid] [0] > row)
				moveDown ();
			if (astar.finalpath [pathid] [1] < column)
				moveLeft ();
			if (astar.finalpath [pathid] [1] > column)
				moveRight ();
		}
	}
	// Update is called once per frame
	void Update () {

	}
}
