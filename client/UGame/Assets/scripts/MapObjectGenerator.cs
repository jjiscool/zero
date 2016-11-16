using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapObjectGenerator : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject[] enemyPrefab;
	public GameObject downStairPrefab;
	private GameObject map;
	void Awake(){
		NewDungeon ();
	}
	public void NewDungeon(){
		map = GameObject.Find ("map");
		map.GetComponent<RandomDungeonCreator> ().ReBuildDungeon ();
		map.GetComponent<TilesManager> ().initMapTexture ();
		map.GetComponent<RoundControler> ().initRound ();
		GameObject player=PlacePlayer ();
		placeEnemy ();
		map.GetComponent<RoundControler> ().reset (player);
		placeDownStair ();
		
	}
	void placeDownStair(){
		int r = map.GetComponent<RandomDungeonCreator> ().obj_list.getListByType (OBJTYPE.OBJTYPE_DOWNSTAIRS) [0].row;
		int c=map.GetComponent<RandomDungeonCreator> ().obj_list.getListByType (OBJTYPE.OBJTYPE_DOWNSTAIRS) [0].column;
		Vector2 nxy = map.GetComponent<TilesManager> ().posTransform (r, c);
		GameObject d=(GameObject)Instantiate (downStairPrefab,nxy,transform.rotation);
		d.name="Exit";

	}
	GameObject PlacePlayer(){
		if(map.GetComponent<RoundControler> ().player!=null) Destroy (map.GetComponent<RoundControler> ().player.gameObject);
		OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;//获取object列表
		int row = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].row;
		int column = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].column;
		//Vector2 pos = map.GetComponent<TilesManager> ().posTransform (row,column);
		GameObject a=(GameObject)Instantiate (playerPrefab,transform.position,transform.rotation);
		a.name="Player";
		a.GetComponent<playerMove> ().set (row,column);
		map.GetComponent<RoundControler> ().player = a;
		a.GetComponent<playerStatus> ().AI = false;
		a.GetComponent<playerStatus> ().HP = Random.Range (1, 100);
		a.GetComponent<playerStatus> ().HPMAX = a.GetComponent<playerStatus> ().HP;//测试血槽 by kola
		a.GetComponent<playerStatus> ().ATK = Random.Range (1, 20);
		a.GetComponent<playerStatus> ().ATKRange=Random.Range (1, 2);
		a.GetComponent<playerStatus> ().SPEED = Random.Range (1, 20);
		a.GetComponent<playerStatus> ().MOV=Random.Range (1, 4);
		a.GetComponent<playerStatus> ().isDanger = false;
		a.GetComponent<playerStatus> ().isPlayerTeam = true;
		GameObject.Find ("Cameras").GetComponent<followCenter>().player=a;
		GameObject.Find ("lightCover").GetComponent<followCenter>().player=a;
		obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0].thisOBJ = a;
		a.GetComponent<playerMove> ().MapOBJ = obj_list.getListByType (OBJTYPE.OBJTYPE_PLAYER) [0];
		Debug.Log ("Create player" + a.name);
		return a;
	}
	void placeEnemy(){
		OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;//获取object列表
		List<OBJTYPEData> ems = obj_list.getListByType(OBJTYPE.OBJTYPE_ENEMY);
		if (map.GetComponent<RoundControler> ().enemy.Count > 0) {
			for (int i = 0; i < map.GetComponent<RoundControler> ().enemy.Count; i++) {
				Destroy (map.GetComponent<RoundControler> ().enemy [i].gameObject);
			}
		}
		map.GetComponent<RoundControler> ().enemy=new List<GameObject>();
		for (int i = 0; i < ems.Count; i++) {
			GameObject a=(GameObject)Instantiate (enemyPrefab[((ObjectEnemy)ems[i]).Enemy_type],transform.position,transform.rotation);
			a.name="Enemy"+i;
			a.GetComponent<playerMove> ().set (ems[i].row,ems[i].column);
			map.GetComponent<RoundControler> ().enemy.Add(a);
			a.GetComponent<playerStatus> ().AI = true;
			a.GetComponent<playerStatus> ().HP = Random.Range (1, 100);

			a.GetComponent<playerStatus> ().ATK = Random.Range (1, 20);
			a.GetComponent<playerStatus> ().ATKRange=Random.Range (1, 2);
			a.GetComponent<playerStatus> ().SPEED = Random.Range (1, 20);
			a.GetComponent<playerStatus> ().MOV=Random.Range (1, 4);
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
