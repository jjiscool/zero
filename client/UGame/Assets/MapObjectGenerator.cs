using UnityEngine;
using System.Collections;

public class MapObjectGenerator : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject enemyPrefab;
	private GameObject map;
	void Awake(){
		map = GameObject.Find ("map");
		PlacePlayer ();

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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
