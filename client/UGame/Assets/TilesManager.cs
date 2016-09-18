using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random =UnityEngine.Random;

//贴图管理
public class TilesManager : MonoBehaviour {

	//配置贴图
	public GameObject wall;
	public GameObject room;
	public GameObject maze;
	public GameObject door;

	//贴图prefab的数组
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;

	private  GameObject[,] tile_gos;


	private RandomDungeonCreator randomMapScript;
	void Awake(){
		
		randomMapScript = GetComponent<RandomDungeonCreator>();
		tile_gos = new GameObject[randomMapScript.MapHeight, randomMapScript.MapWidth];
		Debug.Log (randomMapScript.TileSize);
		placeMap (randomMapScript.MapWidth,randomMapScript.MapHeight,randomMapScript.TileSize);
	}


	public void placeMap(int mapWidth,int mapHeight,float tileSzie){
		
		string tileTypeLeft = "";
		string tileTypeRight = "";
		string tileTypeUp = "";
		string tileTypeDown = "";

		for (int i = 0; i < mapWidth; i++){
			for (int j = 0; j < mapHeight; j++)
			{
				//获取砖块类型 i:行 j:列
				string tileType = randomMapScript.getMapTileType (i, j);;

				float MaxW = (float)mapWidth * tileSzie;
				float MaxH = (float)mapHeight * tileSzie;
				float posx = transform.position.x + j * tileSzie - MaxW/2+tileSzie/2;
				float posy = transform.position.y - i * tileSzie + MaxH/2-tileSzie/2;
				Vector3 newp = new Vector3 (posx, posy, transform.position.z);

				//处理边缘
				if (i == 0 || i == mapWidth-1) {
					switch (i) {
					default:
						tile_gos [i, j] = (GameObject)Instantiate (wallTiles[4], newp, Quaternion.identity);
						tile_gos [i, j].layer = 12;
						tile_gos [i, j].transform.SetParent(transform);
						tile_gos [i, j].transform.localScale= new Vector3(tileSzie,tileSzie,1);
						break;
					case 0:
						tileTypeDown = randomMapScript.getMapTileType (1, j);
						if (tileTypeDown != "WALL") {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [7], newp, Quaternion.identity);


						} else {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles[4], newp, Quaternion.identity);
						}
						tile_gos [i, j].layer = 12;
						tile_gos [i, j].transform.SetParent(transform);
						tile_gos [i, j].transform.localScale = new Vector3 (tileSzie, tileSzie, 1);
						break;
	

					}


					continue;
				} else if(j == 0 || j == mapHeight-1){
					tile_gos [i, j] = (GameObject)Instantiate (wallTiles[4], newp, Quaternion.identity);
					tile_gos [i, j].layer = 12;
					tile_gos [i, j].transform.SetParent(transform);
					tile_gos [i, j].transform.localScale= new Vector3(tileSzie,tileSzie,1);
					continue;
				}else {
					tileTypeLeft = randomMapScript.getMapTileType (i, j-1);
					tileTypeRight = randomMapScript.getMapTileType (i, j+1);
					tileTypeUp = randomMapScript.getMapTileType (i+1, j);
					tileTypeDown = randomMapScript.getMapTileType (i+1, j);

				}



				switch(tileType){
				default:
					if (tileTypeDown != "WALL") {

						Debug.Log (tileType +":" + i  + "," + j);
						tile_gos [i, j] = (GameObject)Instantiate (wallTiles [7], newp, Quaternion.identity);
						tile_gos [i, j].layer = 12;
						break;
					} else {
						tile_gos [i, j] = (GameObject)Instantiate (wallTiles[4], newp, Quaternion.identity);
						string name = "wall("+i+","+j+"):";
						tile_gos [i, j].name = name;
						tile_gos [i, j].layer=12;
						break;
					}

				case "ROOM":
					tile_gos [i, j] = (GameObject)Instantiate (room, newp, Quaternion.identity);
					string name2 = "room("+i+","+j+"):";
					tile_gos [i, j].name = name2;
					tile_gos [i, j].layer=15;
					break;
				case "MAZE":
					tile_gos [i, j] = (GameObject)Instantiate (maze, newp, Quaternion.identity);
					string name3 = "maze("+i+","+j+"):";
					tile_gos [i, j].name = name3;
					tile_gos [i, j].layer=13;
					break;
				case "DOOR":
					tile_gos [i, j] = (GameObject)Instantiate (door, newp, Quaternion.identity);
					string name4 = "door("+i+","+j+"):";
					tile_gos [i, j].name = name4;
					tile_gos [i, j].layer=14;
					break;
				}
				//if (map [i, j] ==-1) {
				//	tile_gos [i, j] = (GameObject)Instantiate (tile, newp, transform.rotation);
				//	string name = "wall("+i+","+j+"):"+map [i, j];
				//	tile_gos [i, j].name = name;
				//}else {
				//	tile_gos [i,j] =(GameObject) Instantiate (room,newp,transform.rotation);
				//	string name = "tile("+i+","+j+"):"+map [i, j];
				//	tile_gos [i, j].name = name;
				//} 
				tile_gos [i, j].transform.SetParent(transform);
				tile_gos [i, j].transform.localScale= new Vector3(tileSzie,tileSzie,1);

			}

		}
	}


}
