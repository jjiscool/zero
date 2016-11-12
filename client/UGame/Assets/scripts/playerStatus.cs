using UnityEngine;
using System.Collections;

public class playerStatus : MonoBehaviour {
	public int HP;
	public int Money;
	public int SP;
	public int MOV;
	public int SPEED;
	public int MinSight;
	public int MaxSight;
	public int ATK;
	public int ATKRange;
	public bool isPlayerTeam;
	public bool AI;
	public bool isDanger;
	public bool isAI(){
		return AI;	
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
