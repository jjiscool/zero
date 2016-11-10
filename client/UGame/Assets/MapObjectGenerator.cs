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
		int row = obj_list.getListByType (OBJTYPE.OBJTYPE_SPAWNPOINT) [0].row;
		int column = obj_list.getListByType (OBJTYPE.OBJTYPE_SPAWNPOINT) [0].column;
		//Vector2 pos = map.GetComponent<TilesManager> ().posTransform (row,column);
		GameObject a=(GameObject)Instantiate (playerPrefab,transform.position,transform.rotation);
		a.name="Player";
		a.GetComponent<playerMove> ().set (row,column);
		map.GetComponent<RoundControler> ().player = a;
		GameObject.Find ("Cameras").GetComponent<followCenter>().player=a;
		GameObject.Find ("lightCover").GetComponent<followCenter>().player=a;
	}
	void placeEnemy(){
		OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;//获取object列表
		List<OBJTYPEData> ems = obj_list.getListByType(OBJTYPE.OBJTYPE_Enemy);
		map.GetComponent<RoundControler> ().enemy=new GameObject[ems.Count];
		for (int i = 0; i < ems.Count; i++) {
			GameObject a=(GameObject)Instantiate (enemyPrefab[((ObjectEnemy)ems[i]).Enemy_type],transform.position,transform.rotation);
			a.name="Enemy"+i;
			a.GetComponent<playerMove> ().set (ems[i].row,ems[i].column);
			map.GetComponent<RoundControler> ().enemy [i] = a;

		}
		
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
