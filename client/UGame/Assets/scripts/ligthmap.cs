using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Edge{
	public Vector2 p1;
	public Vector2 p2;
	public int prev;
	public int  next;
	public float dis;
}
public class ligthmap : MonoBehaviour {
	public GameObject lightsource;
	public float lineWidth;
	public int LightLength;

	public GameObject linePb;
	public GameObject lightarea;
	public GameObject nolightarea;
	public GameObject pointPb;

	private List<Edge> edges;
	private List<Vector2> LightShape;
	private float lineLength;
	private int MapWidth;
	private int MapHeight;
	private int[,] localmap;
	private int localmapWidth;
	private int localmapHeight;
	private GameObject[,] tile_gos;

	void buildLocalMap(int width, int heght){
		if (width % 2 == 0)
			width += 1;
		if (heght % 2 == 0)
			heght += 1;
		localmapWidth = width;
		localmapHeight = heght;
		int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
		int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
		//Debug.Log ("Light in "+centerx+","+centery);
		float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
		float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
		localmap  = new int[heght, width];
		for (int i = 0; i < heght; i++)
			for (int j = 0; j < width; j++)
			{

				localmap [i,j] = -1;

			}
		int beginx = centerx - (width-1)/2;
		int beginy = centery - (heght-1)/2;
		int endx = centerx + (width-1)/2;
		int endy = centery + (heght-1)/2;
		if (beginx < 0)
			beginx = 0;
		if (beginy < 0)
			beginy = 0;
		if (endx >= MapWidth)
			endx = MapWidth - 1;
		if (endy >= MapHeight)
			endy = MapHeight - 1;
		for (int iy = beginy+1; iy <= endy-1; iy++) {
			for (int ix = beginx+1; ix <= endx-1; ix++) {
				if (GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().getMapTileType (iy, ix) != "WALL") {
					GameObject map = GameObject.Find ("map");
					OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;
					if (obj_list.hasItemInRowColumn (iy, ix)) {
						OBJTYPEData od = obj_list.getItemByRowColumn (iy, ix);
						if(od.lightable) localmap [iy - (centery - (heght-1)/2), ix - (centerx - (width-1)/2)]=0;
					} else {
						localmap [iy - (centery - (heght-1)/2), ix - (centerx - (width-1)/2)]=0;
					}
				}
			}
		}

	}
	void DrawLocalMap(){
		tile_gos = new GameObject[MapHeight, MapWidth];
		localmapWidth = localmapWidth;
		localmapHeight = localmapHeight;
		int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
		int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
		//Debug.Log ("Light in "+centerx+","+centery);
		float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
		float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
		for (int i = 0; i < localmapHeight; i++) {
			for (int j = 0; j < localmapWidth; j++) {
				float MaxW = (float)localmapWidth * lineLength;
				float MaxH = (float)localmapHeight * lineLength;
				float posx = newcenterx + j * lineLength - MaxW/2+lineLength/2;
				float posy = newcentery - i * lineLength + MaxH/2-lineLength/2;
				Vector3 newp = new Vector3 (posx, posy, 0);
				switch(getTile(i,j))
				{
				default:
					Debug.Log ("FIND UNKNOW TYPE!");
					break;
				case "WALL" :
					tile_gos [i, j] = (GameObject)Instantiate (lightarea, newp, lightsource.transform.rotation);
					string name = "nolight(" + i + "," + j + "):" + localmap [i, j];
					tile_gos [i, j].name = name;
					break;
				case "NOTWALL" :
					tile_gos [i, j] = (GameObject)Instantiate (nolightarea, newp, lightsource.transform.rotation);
					string name3 = "light(" + i + "," + j + "):" + localmap [i, j];
					tile_gos [i, j].name = name3;
					break;
				}
				tile_gos [i, j].transform.SetParent(transform);
				tile_gos [i, j].transform.localScale= new Vector3(lineLength,lineLength,1);
			}
		}
	}
	string getTile(int i,int j){
		if(localmap[i,j]==-1) return "WALL";
		if(localmap[i,j]==0) return "NOTWALL";
		return "WALL";
	}
	void drawLine(Vector2 x1,Vector2 x2,Color c){
		int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
		int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
		float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
		float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
		GameObject line = (GameObject)Instantiate (linePb, new Vector3(newcenterx,newcentery,-1), transform.rotation);
		line.GetComponent<LineRenderer> ().SetPosition(0,x1);
		line.GetComponent<LineRenderer> ().SetPosition(1,x2);
		line.GetComponent<LineRenderer> ().SetWidth(lineWidth,lineWidth);
		line.GetComponent<LineRenderer> ().SetColors (c, c);
		line.transform.SetParent(transform);
		line.name = "LightCrossLine ";

	}

	Vector2[] getMapEdege(int i, int j){
		Vector2[] r=new Vector2[4];
		float ox = -lineLength * (float)localmapWidth / 2;
		float oy = lineLength * (float)localmapHeight / 2;
		r [0] = new Vector2 (ox+j*lineLength,oy-i*lineLength);
		r [1] = new Vector2 (ox+(j+1)*lineLength,oy-i*lineLength);
		r [2] = new Vector2 (ox+(j+1)*lineLength,oy-(i+1)*lineLength);
		r [3] = new Vector2 (ox+j*lineLength,oy-(i+1)*lineLength);
		return r;

	}
	void FindAllEdegeInLocalMap(){
		for (int i = 0; i <  localmapHeight; i++) {
			for (int j = 0; j < localmapWidth; j++) {
				if (getTile(i, j) == "WALL") {
					FindLineLightFace (0, 0, i, j);
				}
			}
		}

	}
	void FindLineLightFace(float lsx,float lsy,int mx,int my){
		Vector2[] p= getMapEdege (mx, my);
		if (lsx >= p [0].x && lsx < p [2].x && lsy >= p [0].y) {
			//top
			if(mx>0&&getTile(mx-1, my)!="WALL"){
				//Debug.Log("TOP");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [0].x, p [0].y);
				t.p2 = new Vector2 (p [1].x, p [1].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}


		}
		if (lsx >= p [0].x && lsx < p [2].x && lsy < p [2].y) {
			//down
			if (mx<localmapWidth-1&&getTile(mx+1, my) != "WALL") {
				//Debug.Log ("DOWN");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [2].x, p [2].y);
				t.p2 = new Vector2 (p [3].x, p [3].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}
		}
		if (lsy < p [0].y && lsy >= p [2].y && lsx >= p [2].x) {
			if (my<localmapHeight-1&&getTile(mx, my+1) != "WALL") {
				//Debug.Log ("RIGHT");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [1].x, p [1].y);
				t.p2 = new Vector2 (p [2].x, p [2].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}
		}
		if (lsy < p [0].y && lsy >= p [2].y && lsx < p [0].x) {
			if (my>0&&getTile(mx, my-1) != "WALL") {
				//Debug.Log ("LEFT");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [3].x, p [3].y);
				t.p2 = new Vector2 (p [0].x, p [0].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}
		}
		if (lsx < p [0].x && lsy >= p [0].y) {
			//Debug.Log("TOP LEFT");
			if(mx>0&&getTile(mx-1, my)!="WALL"){
				//Debug.Log("TOP");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [0].x, p [0].y);
				t.p2 = new Vector2 (p [1].x, p [1].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}
			if (my>0&&getTile(mx, my-1) != "WALL") {
				//Debug.Log ("LEFT");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [3].x, p [3].y);
				t.p2 = new Vector2 (p [0].x, p [0].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}

		}
		if (lsx >= p [2].x && lsy >= p [0].y) {
			//Debug.Log("TOP RIGHT");
			if(mx>0&&getTile(mx-1, my)!="WALL"){
				//Debug.Log("TOP");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [0].x, p [0].y);
				t.p2 = new Vector2 (p [1].x, p [1].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}
			if (my<localmapHeight-1&&getTile(mx, my+1) != "WALL") {
				//Debug.Log ("RIGHT");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [1].x, p [1].y);
				t.p2 = new Vector2 (p [2].x, p [2].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}

		}
		if (lsx >= p [2].x && lsy < p [2].y) {
			//Debug.Log("DOWN RIGHT");
			if (mx<localmapWidth-1&&getTile(mx+1, my) != "WALL") {
				//Debug.Log ("DOWN");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [2].x, p [2].y);
				t.p2 = new Vector2 (p [3].x, p [3].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}

			if (my<localmapHeight-1&&getTile(mx, my+1) != "WALL") {
				//Debug.Log ("RIGHT");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [1].x, p [1].y);
				t.p2 = new Vector2 (p [2].x, p [2].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}

		}
		if (lsx < p [0].x && lsy < p [2].y) {
			//Debug.Log("DOWN LEFT");
			if (mx<localmapWidth-1&&getTile(mx+1, my) != "WALL") {
				//Debug.Log ("DOWN");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [2].x, p [2].y);
				t.p2 = new Vector2 (p [3].x, p [3].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}
			if (my>0&&getTile(mx, my-1) != "WALL") {
				//Debug.Log ("LEFT");
				Edge t = new Edge ();
				t.p1 = new Vector2 (p [3].x, p [3].y);
				t.p2 = new Vector2 (p [0].x, p [0].y);
				t.prev = -1;
				t.next = -1;
				t.dis = 0;
				edges.Add (t);
			}
		}
	}
	void DrawLineLightFace(){
		for (int i = 0; i < edges.Count; i++) {
			//drawLine(,);
			Vector2 x1=edges[i].p1;
			Vector2 x2=edges[i].p2;
			int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
			int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
			float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
			float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
			GameObject line = (GameObject)Instantiate (linePb, new Vector3(newcenterx,newcentery,-1), transform.rotation);
			line.GetComponent<LineRenderer> ().SetPosition(0,x1);
			line.GetComponent<LineRenderer> ().SetPosition(1,x2);
			line.GetComponent<LineRenderer> ().SetWidth(lineWidth,lineWidth);
			line.transform.SetParent(transform);
			line.name = "Line " + i;
		}


	}
	public List<Vector2> getFinalLightShape(){
		LightShape = new List<Vector2>();
		int  current = 0;
		int last = 0;
		LightShape.Add (edges[0].p1);
		LightShape.Add (edges[0].p2);
		current = edges [current].next;
		while (current!=0) {
			if(current==-1) break;
			if (edges [last].p2 != edges [current].p1) {
				LightShape.Add (edges [current].p1);
				if(edges [current].p2!=edges [0].p1)LightShape.Add (edges [current].p2);
			} else {
				LightShape.Add (edges[current].p2);
			}

			last = current;
			current = edges [current].next;
		}
		//LightShape.Add (edges[0].p1);
		//LightShape.Add (edges[0].p2);
		return LightShape;
	}
	void computedistance(){
		int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
		int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
		float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
		float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
		Vector2 lxy = new Vector2 (lightsource.transform.position.x-newcenterx,lightsource.transform.position.y-newcentery);
		float lx = lxy.x;
		float ly = lxy.y;
		for (int i = 0; i < edges.Count; i++) {
			float lctx = (edges [i].p1.x + edges [i].p2.x) / 2;
			float lcty = (edges [i].p1.y + edges [i].p2.y) / 2;
			float d = (float)System.Math.Sqrt ((lctx - lx) * (lctx - lx) + (lcty - ly) * (lcty - ly));
			edges [i].dis = d;
		}
		for (int i = 0; i < edges.Count; i++) {
			int mini=i;
			if (i == 0) {
				mini = 0;
			}
			for (int j = i + 1; j < edges.Count; j++) {
				if (edges [j].dis < edges [mini].dis) {
					mini = j;
				}

			}
			Edge t = new Edge ();
			t.p1 = new Vector2( edges [i].p1.x,edges [i].p1.y);
			t.p2 = new Vector2( edges [i].p2.x,edges [i].p2.y);
			t.dis = edges [i].dis;

			edges [i].p1 = new Vector2 (edges [mini].p1.x, edges [mini].p1.y);
			edges [i].p2 = new Vector2 (edges [mini].p2.x, edges [mini].p2.y);
			edges [i].dis = edges [mini].dis;

			edges [mini].p1 = new Vector2 (t.p1.x, t.p1.y);
			edges [mini].p2 = new Vector2 (t.p2.x, t.p2.y);
			edges [mini].dis = t.dis;


		}
		for (int i = 0; i < edges.Count; i++) {
			//Debug.Log (">>"+edges [i].dis);
			for (int j = 0; j < edges.Count; j++) {

					if (edges [i].p2 == edges [j].p1) {
						edges [i].next = j;
						edges [j].prev = i;
						//Debug.Log ("Find "+i+" to "+j);
					}

			}
		}
	}
	void CutLine(){
		for (int i = 0; i < edges.Count; i++) {
//			Debug.Log ("Check Line " + i +" Next="+edges [i].next+"PREV="+edges [i].prev);
			if (edges [i].next == -1) {
				bool isok = false;
				for (int j = i+1; j < edges.Count&&!isok; j++) {
					if (i != j) {
						int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
						int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
						float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
						float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
						Vector2 lxy = new Vector2 (lightsource.transform.position.x-newcenterx,lightsource.transform.position.y-newcentery);
						float k1 = (lxy.y - edges [i].p2.y) / (lxy.x - edges [i].p2.x);
						float k2 =(edges [j].p1.y-edges[j].p2.y)/(edges [j].p1.x-edges [j].p2.x);
						//Debug.Log (k1 + ":" + k2);
						if (k1!=k2&&isRayCrossSegment (lxy, edges [i].p2, edges [j].p1, edges [j].p2)) {
//							Debug.Log("Cut Line " +j+" From Ray "+i+" p2");
							edges [i].next = j;
							edges [j].p1 = getCrossSegment(lxy, edges [i].p2, edges [j].p1, edges [j].p2);
							edges [j].prev = i;
							isok = true;
						
						}
						//Vector2 p= LianZX_JD (lxy, edges [i].p2, edges [j].p1, edges [j].p2);
						//if (isCrossOnLine (p, edges [j].p1, edges [j].p2)&&isCrossOnRay (p, lxy, edges [i].p2)) {
						//	edges [i].next = j;
						//	edges [j].p1 = p;
						//	edges [j].prev = i;
						//	isok = true;
						//}

					}
				}
			}

			if (edges [i].prev == -1) {
				bool isok = false;
				for (int j = i+1; j < edges.Count&&!isok; j++) {
					if (i != j) {
						int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
						int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
						float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
						float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
						Vector2 lxy = new Vector2 (lightsource.transform.position.x-newcenterx,lightsource.transform.position.y-newcentery);
						float k1 = (lxy.y - edges [i].p2.y) / (lxy.x - edges [i].p2.x);
						float k2 =(edges [j].p1.y-edges[j].p2.y)/(edges [j].p1.x-edges [j].p2.x);
						//Debug.Log (k1 + ":" + k2);
						if (k1!=k2&&isRayCrossSegment (lxy, edges [i].p1, edges [j].p1, edges [j].p2)) {
//							Debug.Log("Cut Line " +j+" From Ray "+i+" p1");
							edges [i].prev = j;
							edges [j].p2 = getCrossSegment(lxy, edges [i].p1, edges [j].p1, edges [j].p2);
							edges [j].next = i;
							isok = true;

						}

						//Vector2 p= LianZX_JD (lxy, edges [i].p1, edges [j].p1, edges [j].p2);


						//if (isCrossOnLine (p, edges [j].p1, edges [j].p2)&&isCrossOnRay (p, lxy, edges [i].p1)) {
						//	edges [i].prev = j;
						//	edges [j].p2 = p;
						//	edges [j].next = i;
						//	isok = true;
						//}

					}
				}
			}
		}
	}
	public static Vector3 LianZX_JD(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	{
		Vector3 Jiaod = Vector3.zero;
		float P1x = 0.0f;
		float P1y = 0.0f;
		float P1z = 0.0f;
		double plr1_x = p2.x - p1.x;
		double plr1_y = p2.y - p1.y;
		double plr1_z = p2.z - p1.z;
		double plr2_x = p4.x - p3.x;
		double plr2_y = p4.y - p3.y;
		double plr2_z = p4.z - p3.z;
		double t = 1.0f;
		if (((plr1_x != 0) && (plr2_x == 0)) || ((plr1_x == 0) && (plr2_x != 0)))
		{
			if (plr2_x == 0)
			{
				t = (p3.x - p1.x) / plr1_x;
				P1x = (float)(p1.x + t * plr1_x);
				P1y = (float)(p1.y + t * plr1_y);
				P1z = (float)(p1.z + t * plr1_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
			else
			{
				t = (p1.x - p3.x) / plr2_x;
				P1x = (float)(p3.x + t * plr2_x);
				P1y = (float)(p3.y + t * plr2_y);
				P1z = (float)(p3.z + t * plr2_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
		}
		else if (((plr1_y != 0) && (plr2_y == 0)) || ((plr1_y == 0) && (plr2_y != 0)))
		{
			if (plr2_y == 0)
			{
				t = (p3.y - p1.y) / plr1_y;
				P1x = (float)(p1.x + t * plr1_x);
				P1y = (float)(p1.y + t * plr1_y);
				P1z = (float)(p1.z + t * plr1_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
			else
			{
				t = (p1.y - p3.y) / plr2_y;
				P1x = (float)(p3.x + t * plr2_x);
				P1y = (float)(p3.y + t * plr2_y);
				P1z = (float)(p3.z + t * plr2_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
		}
		else if (((plr1_z != 0) && (plr2_z == 0)) || ((plr1_z == 0) && (plr2_z != 0)))
		{
			if (plr2_z == 0)
			{
				t = (p3.z - p1.z) / plr1_z;
				P1x = (float)(p1.x + t * plr1_x);
				P1y = (float)(p1.y + t * plr1_y);
				P1z = (float)(p1.z + t * plr1_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
			else
			{
				t = (p1.z - p3.z) / plr2_z;
				P1x = (float)(p3.x + t * plr2_x);
				P1y = (float)(p3.y + t * plr2_y);
				P1z = (float)(p3.z + t * plr2_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
		}
		else
		{
			if (((plr1_x != 0) && (plr2_x != 0)) && ((plr1_y != 0) && (plr2_y != 0)))
			{
				double fz = (p3.x * plr2_y - p3.y * plr2_x - plr2_y * p1.x + plr2_x * p1.y);
				double fm = (plr1_x * plr2_y - plr1_y * plr2_x);
				t = fz / fm;
				P1x = (float)(p1.x + t * plr1_x);
				P1y = (float)(p1.y + t * plr1_y);
				P1z = (float)(p1.z + t * plr1_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
			else if (((plr1_x != 0) && (plr2_x != 0)) && ((plr1_z != 0) && (plr2_z != 0)))
			{
				double fz = (p3.x * plr2_z - p3.z * plr2_x - plr2_z * p1.x + plr2_x * p1.z);
				double fm = (plr1_x * plr2_z - plr1_z * plr2_x);
				t = fz / fm;
				P1x = (float)(p1.x + t * plr1_x);
				P1y = (float)(p1.y + t * plr1_y);
				P1z = (float)(p1.z + t * plr1_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
			else if (((plr1_y != 0) && (plr2_y != 0)) && ((plr1_z != 0) && (plr2_z != 0)))
			{
				double fz = (p3.y * plr2_z - p3.z * plr2_y - plr2_z * p1.y + plr2_y * p1.z);
				double fm = (plr1_y * plr2_z - plr1_z * plr2_y);
				t = fz / fm;
				P1x = (float)(p1.x + t * plr1_x);
				P1y = (float)(p1.y + t * plr1_y);
				P1z = (float)(p1.z + t * plr1_z);
				Jiaod = new Vector3(P1x, P1y, P1z);
				return Jiaod;
			}
			else
			{
				return Vector3.zero;
			}

		}
		return Vector3.zero;
	}
	bool isRayCrossSegment(Vector2 p1,Vector2 p2,Vector2 p3,Vector2 p4){
		float r_px=p1.x;
		float r_py=p1.y;
		float s_px=p3.x;
		float s_py=p3.y;
		float r_dx=p2.x-p1.x;
		float r_dy=p2.y-p1.y;
		float s_dx=p4.x-p3.x;
		float s_dy=p4.y-p3.y;
		float T2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
		float T1 = (s_px + s_dx * T2 - r_px) / r_dx;
		if (T1 > 0 && T2 >= 0 && T2 <= 1)
			return true;
		return false;

	}
	Vector2 getCrossSegment(Vector2 p1,Vector2 p2,Vector2 p3,Vector2 p4){
		float r_px=p1.x;
		float r_py=p1.y;
		float s_px=p3.x;
		float s_py=p3.y;
		float r_dx=p2.x-p1.x;
		float r_dy=p2.y-p1.y;
		float s_dx=p4.x-p3.x;
		float s_dy=p4.y-p3.y;
		float T2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
		float T1 = (s_px + s_dx * T2 - r_px) / r_dx;
		return new Vector2 (p1.x+(p2.x-p1.x)*T1,p1.y+(p2.y-p1.y)*T1);
	}
	bool isCrossOnLine(Vector2 p, Vector2 p3,Vector2 p4){
		if (System.Math.Min (p3.x, p4.x) <= p.x && p.x <= System.Math.Max (p3.x, p4.x) && System.Math.Min (p3.y, p4.y) <= p.y && p.y <= System.Math.Max (p3.y, p4.y))
			return  true;
		return false;
	}
	bool isCrossOnRay(Vector2 p, Vector2 p1,Vector2 p2){
		bool r = true;
		if (p1.x > p2.x) {
			if(p.x>p2.x) r=false;
		}
		else{
			if(p.x<p2.x) r=false;
		}
		if (p1.y > p2.y) {
			if(p.y>p2.y) r=false;
		}else{
			if(p.y<p2.y) r=false;
		}
		return r;
	}
	void Awake(){
		lineLength = GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().TileSize;
		MapWidth = (int)GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().MapWidth;
		MapHeight= (int)GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().MapHeight;
		lightsource.transform.position = new Vector3 (1, 1);

	}
	void DrawShap(List<Vector2> v){
		Debug.Log ("NUM of Vetices "+v.Count);
		//for (int i = 0; i < v.Count-1; i++) {
		//	drawLine (v [i], v [i+1], new Color (255, 0, 0));
		//}
		for (int i = 0; i < v.Count; i++) {
			int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
			int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
			float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
			float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
			GameObject t = (GameObject)Instantiate (pointPb, new Vector3(newcenterx+v[i].x,newcentery+v[i].y,-1), transform.rotation);
			t.transform.SetParent(transform);
			t.name = "point " + i;
		}

	}
	public Vector2 getNewCenter(){
		int centerx =(int)((lightsource.transform.position.x + MapWidth*lineLength*0.5)/lineLength);
		int centery =(int)((-lightsource.transform.position.y + MapWidth* lineLength*0.5)/lineLength);
		float newcenterx = lineLength * centerx - lineLength * MapWidth / 2 + lineLength / 2;
		float newcentery = -lineLength * centery + lineLength * MapHeight / 2 - lineLength / 2;
		return new Vector2 (newcenterx, newcentery);
		
	}
	public void reDrawLight(){
		foreach(Transform child in transform){
			Destroy (child.gameObject);
		}
		lineLength = GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().TileSize;
		MapWidth = (int)GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().MapWidth;
		MapHeight= (int)GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().MapHeight;
		edges=new List<Edge>();
		buildLocalMap (LightLength,LightLength);
		FindAllEdegeInLocalMap ();
		computedistance ();
		CutLine ();
		GameObject.Find ("mask").GetComponent<IMESH> ().DrawLightMesh ();

	}
	// Use this for initialization
	void Start () {
		reDrawLight ();

	}

	// Update is called once per frame
	void Update () {
		
	}
}
