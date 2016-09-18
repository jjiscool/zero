using UnityEngine;
using System.Collections;
using System.Collections.Generic;
class Triangle
{
	public int p1;
	public int p2;
	public int p3;
	public Triangle(int point1, int point2, int point3)
	{
		p1 = point1; p2 = point2; p3 = point3;
	}
}
class MEdge
{
	public int p1;
	public int p2;
	public MEdge(int point1, int point2)
	{
		p1 = point1; p2 = point2;
	}
}


public class IMESH : MonoBehaviour {

	//网格模型顶点数量
	private int VERTICES_COUNT ;

	void Start ()
	{
		
	}
	public void DrawLightMesh(){
		MeshFilter meshFilter = (MeshFilter)GameObject.Find("mask").GetComponent(typeof(MeshFilter));
		Mesh mesh = meshFilter.mesh;

		List<Vector2> ivertices= (List<Vector2>)GameObject.Find("light").GetComponent<ligthmap>().getFinalLightShape();
		VERTICES_COUNT = ivertices.Count;

		Vector2 c = (Vector2)GameObject.Find ("light").GetComponent<ligthmap> ().getNewCenter ();
		Vector2 ls = (Vector2)GameObject.Find ("light").GetComponent<ligthmap> ().lightsource.transform.position;
		Vector3[] v = new Vector3[VERTICES_COUNT+1];
		v[0]=new Vector3 (ls.x,ls.y,0);
		for (int i = 1; i < VERTICES_COUNT+1; i++) {
			v[i] = new Vector3 (ivertices[i-1].x+c.x,ivertices[i-1].y+c.y,0);
		}

		int[] ids = new int[(VERTICES_COUNT +0)* 3];
		for (int i = 0; i < VERTICES_COUNT-1; i++) {
			int ii = i+1;
			ids [i*3+2] = 0;
			ids [i*3+1] = ii;
			ids [i*3] = ii+1;
			//Debug.Log ("tri"+i+":"+ids[i]+","+ids [i*3+1]+","+ids [i*3+2] );
		}
		ids [(VERTICES_COUNT - 1)*3+2] = 0;
		ids [(VERTICES_COUNT - 1)*3+1] = VERTICES_COUNT;
		ids [(VERTICES_COUNT - 1)*3+0] = 1;
		//绘制三角形
		mesh.Clear ();
		mesh.vertices = v;
		mesh.triangles = ids;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();



	}
	// Update is called once per frame
	void Update () {
		
	}
}
