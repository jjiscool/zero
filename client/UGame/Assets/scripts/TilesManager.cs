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
	public GameObject[] roomTiles;
	public GameObject[] wallTiles;

	private  GameObject[,] tile_gos;


	private int mapHeight;
	private int mapWidth;
	private float tileSize;


	private RandomDungeonCreator randomMapScript;
	void Awake(){
		
		randomMapScript = GetComponent<RandomDungeonCreator>();
		mapHeight = randomMapScript.MapHeight;
		mapWidth = randomMapScript.MapWidth;
		tileSize = randomMapScript.TileSize;
		tile_gos = new GameObject[mapHeight, mapWidth];
//		Debug.Log (randomMapScript.TileSize);
		placeMap (mapWidth,mapHeight,tileSize);
	}
		

	//行列 转换到 坐标
	public Vector2 posTransform(int i,int j){
		float MaxW = (float)mapWidth * tileSize;
		float MaxH = (float)mapHeight * tileSize;
		float posx = transform.position.x + j * tileSize - MaxW/2+tileSize/2;
		float posy = transform.position.y - i * tileSize + MaxH/2-tileSize/2;

		Vector2 pos = new Vector2 (posx,posy);
		return pos;
	}
	// 坐标 转换到 行列
	public int[] posTransform2(float posx,float posy){
		float MaxW = (float)mapWidth * tileSize;
		float MaxH = (float)mapHeight * tileSize;
		int j = Mathf.RoundToInt((posx-( - MaxW/2+tileSize/2))/tileSize);
		int i = -Mathf.RoundToInt((posy-( MaxH/2-tileSize/2))/tileSize);
		int[] cellpos = { i, j };
		return cellpos;

	}
	public void placeMap(int mapWidth,int mapHeight,float tileSize){
		
		string tileTypeLeft = "";
		string tileTypeRight = "";
		string tileTypeUp = "";
		string tileTypeDown = "";

		for (int i = 0; i < mapWidth; i++){
			for (int j = 0; j < mapHeight; j++)
			{
				//获取砖块类型 i:行 j:列
				string tileType = randomMapScript.getMapTileType (i, j);

				Vector2 newp = posTransform (i, j);

				//处理边缘
				if (i == 0 || i == mapWidth-1) {
					switch (i) {
					default:
						tileTypeUp = randomMapScript.getMapTileType (mapWidth-2, j);
						if (tileTypeUp != "WALL") {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (1, 2)], newp, Quaternion.identity);


						} else {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (10, 13)], newp, Quaternion.identity);
						}
//						tile_gos [i, j] = (GameObject)Instantiate (wallTiles[4], newp, Quaternion.identity);
						tile_gos [i, j].layer = 12;
						tile_gos [i, j].transform.SetParent(transform);
						tile_gos [i, j].transform.localScale= new Vector3(tileSize,tileSize,1);
						break;
					case 0:
						tileTypeDown = randomMapScript.getMapTileType (1, j);
						if (tileTypeDown != "WALL") {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (20, 21)], newp, Quaternion.identity);


						} else {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (10, 13)], newp, Quaternion.identity);
						}
						tile_gos [i, j].layer = 12;
						tile_gos [i, j].transform.SetParent(transform);
						tile_gos [i, j].transform.localScale = new Vector3 (tileSize, tileSize, 1);
						break;
	

					}


					continue;
				} else if(j == 0 || j == mapHeight-1){
					switch (j) {
					case 0:
						tileTypeRight = randomMapScript.getMapTileType (i, 1);
						if (tileTypeRight != "WALL") {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (8, 9)], newp, Quaternion.identity);
						} else {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (10, 13)], newp, Quaternion.identity);
						}
						tile_gos [i, j].layer = 12;
						tile_gos [i, j].transform.SetParent(transform);
						tile_gos [i, j].transform.localScale= new Vector3(tileSize,tileSize,1);
						break;
					default:
						tileTypeRight = randomMapScript.getMapTileType (i, mapHeight-2);
						if (tileTypeRight != "WALL") {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (5, 6)], newp, Quaternion.identity);
						} else {
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (10, 13)], newp, Quaternion.identity);
						}
						tile_gos [i, j].layer = 12;
						tile_gos [i, j].transform.SetParent(transform);
						tile_gos [i, j].transform.localScale= new Vector3(tileSize,tileSize,1);
						break;
						
					}

//					tile_gos [i, j] = (GameObject)Instantiate (wallTiles[4], newp, Quaternion.identity);

					continue;
				}else {
					tileTypeLeft = randomMapScript.getMapTileType (i, j-1);
					tileTypeRight = randomMapScript.getMapTileType (i, j+1);
					tileTypeUp = randomMapScript.getMapTileType (i-1, j);
					tileTypeDown = randomMapScript.getMapTileType (i+1, j);

				}



				switch(tileType){
				default:
					if (tileTypeDown != "WALL") {
						//0x0
						//x1x
						//000
						if (tileTypeUp != "WALL") {
							//000
							//x1x
							//000
							if (tileTypeRight == "WALL" && tileTypeLeft != "WALL") {
								//000
								//011
								//000
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [4], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							} else if (tileTypeRight != "WALL" && tileTypeLeft == "WALL") {
								//000
								//110
								//000
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [7], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							}
							//000
							//111
							//000
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (23, 26)], newp, Quaternion.identity);
							tile_gos [i, j].layer = 12;
							break;

						} else {
							//010
							//x1x
							//000
							if (tileTypeRight == "WALL" && tileTypeLeft != "WALL") {
								//010
								//011
								//000
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [18], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							} else if (tileTypeRight != "WALL" && tileTypeLeft == "WALL") {
								//010
								//110
								//000
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [19], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							} else if (tileTypeRight != "WALL" && tileTypeLeft != "WALL") {
								//010
								//010
								//000
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [27], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							}

							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (20, 21)], newp, Quaternion.identity);
							tile_gos [i, j].layer = 12;
							break;
						}

						tile_gos [i, j] = (GameObject)Instantiate (wallTiles [7], newp, Quaternion.identity);
						tile_gos [i, j].layer = 12;
						break;
					} 
					else {
						//0x0
						//x1x
						//010
						if (tileTypeUp != "WALL") {
							//000
							//x1x
							//010
							if (tileTypeRight == "WALL" && tileTypeLeft != "WALL") {
								//000
								//011
								//010
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [0], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							} else if (tileTypeRight != "WALL" && tileTypeLeft == "WALL") {
								//000
								//110
								//010
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [3], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							} else if (tileTypeRight == "WALL" && tileTypeLeft == "WALL") {
								//000
								//111
								//010
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (1, 2)], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
								break;
							}
							//000
							//010
							//010
							tile_gos [i, j] = (GameObject)Instantiate (wallTiles [22], newp, Quaternion.identity);
							tile_gos [i, j].layer = 12;
							break;

							
						} else {
							//010
							//x1x
							//010
							if (tileTypeRight == "WALL" && tileTypeLeft != "WALL") {
								//010
								//011
								//010
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (5, 7)], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
							} else if (tileTypeRight != "WALL" && tileTypeLeft == "WALL") {
								//010
								//110
								//010
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (8, 10)], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
							} else if (tileTypeRight != "WALL" && tileTypeLeft != "WALL") {
								//010
								//010
								//010
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (14, 17)], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
							}
							else {
								//010
								//111
								//010
								tile_gos [i, j] = (GameObject)Instantiate (wallTiles [Random.Range (10, 13)], newp, Quaternion.identity);
								tile_gos [i, j].layer = 12;
							}
						}

						tile_gos [i, j] = (GameObject)Instantiate (wallTiles[4], newp, Quaternion.identity);
						tile_gos [i, j].layer=12;
						break;
					}

				case "ROOM":
					tile_gos [i, j] = (GameObject)Instantiate (roomTiles[Random.Range (0, roomTiles.Length)], newp, Quaternion.identity);
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
				tile_gos [i, j].transform.localScale= new Vector3(tileSize,tileSize,1);

			}

		}
	}


}
