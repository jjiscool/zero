using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//jimjiao use ignore
public class RoomData{
	public int ID;
	public List<int[]> TileList;
	public int Width;
	public int Height;
	public int CenterX;
	public int CenterY;
	public RoomData(){
		TileList = new List<int[]>();
	}
	
}
public class DoorData{
	public int ID;
	public int x;
	public int y;
	public DoorData(){
		
	}

}
public class RandomDungeonCreator : MonoBehaviour {
	// Use this for initialization
	float lasttime;
	//配置贴图
	public GameObject wall;
	public GameObject room;
	public GameObject maze;
	public GameObject door;
	//地牢贴图数组
	private  GameObject[,] tile_gos;
	//地图数组
	private  int[,] map;
	//单元个数
	private int numOfObj;
	//单元类型字典
	private Dictionary<int,string> idtype; //ROOM,MAZE
	//地图参数
	public int MapWidth; //地图宽度
	public int MapHeight; //地图高度
	public float TileSize;//砖块宽高
	public int RoomDesity;//房间密度
	public int MaxRoomWidth;//房间最大宽度
	public int MaxRoomHeight;//房间最大高度
	public int MaxReduceLength;//死路总剔除长度
	//单元列表
	private List<int> mazesID;//走廊ID列表
	private List<RoomData> rooms;
	private List<DoorData> doors;
	//shilihua

	//调用贴图管理类
	public TilesManager tilesScript;

	void Start () {
		//lasttime = 0.0f;


		//Debug.Log ("ROOM ID= " + rooms[0].ID+" : "+rooms[0].TileList[0][0]+","+rooms[0].TileList[0][1]);

	}
	//初始化地图
	void iniMap(){
		numOfObj = 0;
		//roomsID = new List<int>();
		rooms = new List<RoomData>();
		doors = new List<DoorData>();
		mazesID = new List<int>();
		//doorsID = new List<int>();
		idtype= new Dictionary<int,string>();
		map  = new int[MapHeight, MapWidth];
		tile_gos = new GameObject[MapHeight, MapWidth];
		for (int i = 0; i < MapHeight; i++)
			for (int j = 0; j < MapWidth; j++)
			{

				map [i,j] = -1;

			}
	}
	//随机放置房间
	void placeRandomRoom(){
		for (int i = 0; i < RoomDesity; i++) {
			int x = Random.Range(1,MapWidth);
			int y =Random.Range(1,MapHeight);
			if (MaxRoomWidth < 3)
				MaxRoomWidth = 3;
			if (MaxRoomHeight < 3)
				MaxRoomHeight = 3;
			int w = Random.Range(3,MaxRoomWidth+1);
			int h = Random.Range(3,MaxRoomHeight+1);
			createRoom (x, y, w, h);
		}
	}
	//产生一个房间
	bool createRoom(int x,int y,int w,int h){
		int W = w;
		int H = h;
		if( y - H / 2-2<0||x - W / 2-2<0||y + H / 2+1>MapHeight||x + W / 2+1>MapWidth) return false;
		for (int i = y - H / 2-2; i < y + H / 2+1; i++)
			for (int j = x - W / 2-2; j < x + W / 2+1; j++) {
				if (map [i, j] !=-1 )
					return false;
			}
		RoomData r=new RoomData();
		r.ID = numOfObj;
		r.Width=w;
		r.Height=h;
		r.CenterX = x;
		r.CenterY = y;
		for (int i = y - H / 2-1; i < y + H / 2; i++)
			for (int j = x - W / 2-1; j < x + W / 2; j++) {
				map [i,j] = numOfObj;
				int[] tp = { i, j };
				r.TileList.Add (tp);

			}
		idtype.Add(numOfObj,"ROOM");
		//roomsID.Add (numOfObj);
		rooms.Add (r);
		numOfObj++;
		return true;
	}
	//生成全部走廊
	void StartMaze(){
		for (int i = 1; i < MapHeight-1; i++)
			for (int j = 1; j < MapWidth-1; j++)
			{
				int[,] factor = { { -1, -1 }, { 0, -1 }, { 1, -1 }, { -1, 0 },  { 1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 } };
				int num = 0;
				for (int a = 0; a < 8; a++) {
					int tx = factor [a,0]+i;
					int ty = factor [a,1]+j;
					if (map [tx, ty] != -1)
						num++;

				}
				if(num==0) placeOneMazeWay (i, j);

			}
	}
	//生成一条走廊
	void placeOneMazeWay(int i,int j){
		//if(map [i-1,j-1]==-1&&map [i,j-1]==-1&&map [i+1,j-1]==-1&&map [i-1,j]==-1&&map [i,j]==-1&&map [i+1,j]==-1&&map [i-1,j-1]==-1&&map [i,j-1]==-1&&map [i+1,j-1]==-1) {
			map [i,j] = numOfObj;
			idtype.Add(numOfObj,"MAZE");
			mazesID.Add (numOfObj);
			int[] txy = {i,j};
			List<int[]> Mazeable = new List<int[]>();
			Mazeable.Add (txy);
			while (Mazeable.Count != 0) {
				int lastid=Mazeable.Count-1;
				int lastx = Mazeable [lastid] [0];
				int lasty = Mazeable [lastid] [1];
				int numOfMazeable = 0;
				List<int[]> NextMaze = new List<int[]>();
				int[,] factor = { { 0, -1 }, { -1, 0 },  { 1, 0 }, { 0, 1 }};
				for (int a = 0; a < 4; a++) {
					int nextx = factor [a,0]+lastx;
					int nexty = factor [a,1]+lasty;
					if (nextx == 0 || nexty == 0 || nextx == MapHeight-1 || nexty == MapWidth-1)
						continue;
					if (map [nextx, nexty] != -1)
						continue;
					if (isCanPlaceMaze (lastx, lasty, nextx, nexty)) {
						numOfMazeable++;
						int[] pxy = { nextx, nexty };
						NextMaze.Add (pxy);
					}
				}
				if (numOfMazeable > 1) {
					int choose = Random.Range (0, NextMaze.Count);	
					Mazeable.Add (NextMaze[choose]); 
					map [NextMaze[choose][0],NextMaze[choose][1]] = numOfObj;
					//for (int jj = 0; jj < NextMaze.Count; jj++)
					//	Debug.Log ("Choose : "+NextMaze[jj][0]+","+NextMaze[jj][1]);
					//Debug.Log (lastx+","+lasty+" to "+NextMaze[choose][0]+","+NextMaze[choose][1]);

				} else if(numOfMazeable==1){
					Mazeable.RemoveAt (lastid);
					Mazeable.Add (NextMaze[0]);
					map [NextMaze[0][0],NextMaze[0][1]] = numOfObj;
					//for (int jj = 0; jj < NextMaze.Count; jj++)
					//	Debug.Log ("Choose : "+NextMaze[jj][0]+","+NextMaze[jj][1]);
					//Debug.Log (lastx+","+lasty+" to "+NextMaze[0][0]+","+NextMaze[0][1]);

				}
				else {
					Mazeable.RemoveAt (lastid);
				}
			}
			numOfObj++;
			return;	//}	
	}
	//判断区域是否可以放置走廊
	bool isCanPlaceMaze(int fromx,int fromy,int tox,int toy){	
		int[,] factor = { { -1, -1 }, { 0, -1 }, { 1, -1 }, { -1, 0 },  { 1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 } };
		for (int a = 0; a < 8; a++) {
			int tx = factor [a,0]+tox;
			int ty = factor [a,1]+toy;
			if (tx == 0 || ty == 0 || tx == MapHeight || ty == MapWidth)
				continue;
			if (System.Math.Abs (fromx - tx) + System.Math.Abs (fromy - ty) <= 1)
				continue;
			if (map [tx, ty] != -1)
				return false;
		}
		return true;
	}
	//链接区域
	void connectArea(){
		int beginroomID = rooms [Random.Range (0, rooms.Count)].ID;
		//Debug.Log ("Pick first object ID "+ beginroomID);
		List<int> connectAreasID=new List<int>();
		List<int[]> connector=new List<int[]>();
		List<int[]> connectorInfo=new List<int[]>();
		connectAreasID.Add (beginroomID);
		//Find ALL connector
		for (int i = 1; i < MapHeight-1; i++) {
			for (int j = 1; j < MapWidth-1; j++) {
				if (map [i, j] == -1) {
					if (map [i - 1, j] != -1 && map [i + 1, j] != -1 && map [i, j - 1] == -1 && map [i, j + 1] == -1&&map [i - 1, j] !=map [i + 1, j]) {
						int[] pos = { i, j };
						connector.Add (pos);
						int[] IDtoID = { map [i - 1, j], map [i + 1, j] };
						connectorInfo.Add (IDtoID);

					}else if (map [i - 1, j] == -1 && map [i + 1, j] == -1 && map [i, j - 1] != -1 && map [i, j + 1] != -1&& map [i, j - 1] !=map [i, j + 1] ) {
						int[] pos = { i, j };
						connector.Add (pos);
						int[] IDtoID = { map [i , j+1], map [i, j-1] };
						connectorInfo.Add (IDtoID);
					}

				}
			}
		}
		List<int> connectorRemoveID = new List<int> ();
		int numofRoomAndMaze=rooms.Count+mazesID.Count;

		while(connectAreasID.Count<=numofRoomAndMaze||connectorRemoveID.Count<connectorInfo.Count){
			//FindAllconnectorinAreas
			List<int> connectorIDtoAreas=new List<int>();
			List<int> connectorIDtoAreasID=new List<int>();
			for (int i = 0; i < connectorInfo.Count; i++) { //all Connector
				for (int j = 0; j < connectAreasID.Count; j++) { //all MainAreas ID
					bool isInRemove=false;
					for (int l = 0; l < connectorRemoveID.Count; l++) {
						if (connectorRemoveID [l] == i)
							isInRemove = true;
					}
					if (!isInRemove) {
						if (connectorInfo [i] [0] == connectAreasID [j]) {
							connectorIDtoAreas.Add (i);
							connectorIDtoAreasID.Add (connectorInfo [i] [1]);
						} else if (connectorInfo [i] [1] == connectAreasID [j]) {
							connectorIDtoAreas.Add (i);
							connectorIDtoAreasID.Add (connectorInfo [i] [0]);
						}
					}
				}
			}
			if (connectorIDtoAreas.Count == 0)
				break;
			int pick = Random.Range (0, connectorIDtoAreas.Count);
			int pickconntorID = connectorIDtoAreas [pick];
			int pickconntorAreasID = connectorIDtoAreasID [pick];
			connectAreasID.Add (pickconntorAreasID);
			int x = connector [pickconntorID] [0];
			int y = connector [pickconntorID] [1];
			map [x,y] = numOfObj;
			idtype.Add(numOfObj,"DOOR");
			DoorData d = new DoorData ();
			d.ID = numOfObj;
			d.x = x;
			d.y = y;
			doors.Add (d);
			numOfObj++;
			//Debug.Log (" connect object ID "+ pickconntorAreasID+" From["+x+","+y+"]");
			for (int k = 0; k < connectorIDtoAreas.Count; k++) {
				int delconntorID = connectorIDtoAreas [k];
				int delconntorAreasID = connectorIDtoAreasID [k];
				if (pickconntorAreasID == delconntorAreasID) {
					connectorRemoveID.Add (delconntorID);
					//Debug.Log (" del connect"+ k+" From["+delx+","+dely+"]");
				}
			}
		}
		Debug.Log ("ObecjtNum = "+numOfObj+" , ROOM NUM = " + rooms.Count+" , MAZE NUM = " +mazesID.Count+", DOOR NNUM = "+doors.Count);
	}
	//剔除死路
	void removeDeadway(int MAX){
		for (int t = 0; t < MAX; t++) {
			bool change = false;
			for (int i = 1; i < MapHeight - 1; i++) {
				for (int j = 1; j < MapWidth - 1; j++) {
					int countWALL = 0;
					if (map [i - 1, j] == -1)
						countWALL++;
					if (map [i +1, j] == -1)
						countWALL++;
					if (map [i, j-1] == -1)
						countWALL++;
					if (map [i , j+1] == -1)
						countWALL++;
					if (countWALL == 3) {
						map [i , j] =-1;
						change = true;
					}

				}
			}
			if (!change)
				return;
		}


	} 
	//剔除一格死路
	void removeOneDeadway(){
		for (int i = 1; i < MapHeight - 1; i++) {
			for (int j = 1; j < MapWidth - 1; j++) {
				int countWALL = 0;
				if (map [i - 1, j] == -1)
					countWALL++;
				if (map [i +1, j] == -1)
					countWALL++;
				if (map [i, j-1] == -1)
					countWALL++;
				if (map [i , j+1] == -1)
					countWALL++;
				if (countWALL == 3) {
					map [i , j] =-1;

				}

			}
		}
	}
	//获取砖块类型
	public string getMapTileType(int i,int j){
		if (idtype.ContainsKey(map [i, j]))
		{
			//Debug.Log ("["+i+","+j+"] ID = "+map [i, j]+", Type = "+idtype[map [i, j]]);
			return idtype [map [i, j]];
		}
		return "WALL";
	}
	//

	//产生贴图
	void placeMap(){
		for (int i = 0; i < MapHeight; i++){
			for (int j = 0; j < MapWidth; j++)
			{

				float MaxW = (float)MapWidth * TileSize;
				float MaxH = (float)MapHeight * TileSize;
				float posx = transform.position.x + j * TileSize - MaxW/2+TileSize/2;
				float posy = transform.position.y - i * TileSize + MaxH/2-TileSize/2;
				Vector3 newp = new Vector3 (posx, posy, transform.position.z);
				switch(getMapTileType(i,j)){
				default:
					tile_gos [i, j] = (GameObject)Instantiate (wall, newp, transform.rotation);
					string name = "wall("+i+","+j+"):"+map [i, j];
					tile_gos [i, j].name = name;
					tile_gos [i, j].layer=12;
					break;
				case "ROOM":
					tile_gos [i, j] = (GameObject)Instantiate (room, newp, transform.rotation);
					string name2 = "room("+i+","+j+"):"+map [i, j];
					tile_gos [i, j].name = name2;
					tile_gos [i, j].layer=15;
					break;
				case "MAZE":
					tile_gos [i, j] = (GameObject)Instantiate (maze, newp, transform.rotation);
					string name3 = "maze("+i+","+j+"):"+map [i, j];
					tile_gos [i, j].name = name3;
					tile_gos [i, j].layer=13;
					break;
				case "DOOR":
					tile_gos [i, j] = (GameObject)Instantiate (door, newp, transform.rotation);
					string name4 = "door("+i+","+j+"):"+map [i, j];
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
				tile_gos [i, j].transform.localScale= new Vector3(TileSize,TileSize,1);
				//getMapTileType (i, j);
			}

		}
	}
	void Awake(){
		iniMap ();
		placeRandomRoom (); 
		StartMaze ();
		connectArea ();
		removeDeadway (MaxReduceLength);


//		placeMap ();
	}

	// Update is called once per frame
	void Update () {
		//removeOneDeadway();
		//foreach(Transform child in transform){
		//	Destroy (child.gameObject);
		//}
		//placeMap ();
	}
}
