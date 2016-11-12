using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapObjectGenerator : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject[] enemyPrefab;
	private GameObject map;
	void Awake(){
		map = GameObject.Find ("map");
		PlacePlayer ();
		placeEnemy ();

	}
	void PlacePlayer(){
		OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;//获取object列表
		int row = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].row;
		int column = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].column;
		//Vector2 pos = map.GetComponent<TilesManager> ().posTransform (row,column);
		GameObject a=(GameObject)Instantiate (playerPrefab,transform.position,transform.rotation);
		a.name="Player";
		a.GetComponent<playerMove> ().set (row,column);
		map.GetComponent<RoundControler> ().player = a;
		a.GetComponent<playerStatus> ().AI = false;
		a.GetComponent<playerStatus> ().HP = Random.Range (1, 100);;
		a.GetComponent<playerStatus> ().ATK = Random.Range (0, 20);;
		a.GetComponent<playerStatus> ().SPEED = Random.Range (0, 20);
		a.GetComponent<playerStatus> ().isDanger = false;
		a.GetComponent<playerStatus> ().isPlayerTeam = true;
		GameObject.Find ("Cameras").GetComponent<followCenter>().player=a;
		GameObject.Find ("lightCover").GetComponent<followCenter>().player=a;
		obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].thisOBJ = a;
		a.GetComponent<playerMove> ().MapOBJ = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0];
	}
	void placeEnemy(){
		OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;//获取object列表
		List<OBJTYPEData> ems = obj_list.getListByType(OBJTYPE.OBJTYPE_ENEMY);
		map.GetComponent<RoundControler> ().enemy=new List<GameObject>();
		for (int i = 0; i < ems.Count; i++) {
			GameObject a=(GameObject)Instantiate (enemyPrefab[((ObjectEnemy)ems[i]).Enemy_type],transform.position,transform.rotation);
			a.name="Enemy"+i;
			a.GetComponent<playerMove> ().set (ems[i].row,ems[i].column);
			map.GetComponent<RoundControler> ().enemy.Add(a);
			a.GetComponent<playerStatus> ().AI = true;
			a.GetComponent<playerStatus> ().HP = Random.Range (1, 100);;
			a.GetComponent<playerStatus> ().ATK = Random.Range (0, 20);;
			a.GetComponent<playerStatus> ().SPEED = Random.Range (0, 20);
			a.GetComponent<playerStatus> ().isDanger = true;
			a.GetComponent<playerStatus> ().isPlayerTeam = false;
			((ObjectEnemy)ems [i]).thisOBJ = a;
			a.GetComponent<playerMove> ().MapOBJ = (ObjectEnemy)ems [i];

		}
		
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
