using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class playerStatus : MonoBehaviour {
	public int MinSight;
	public int MaxSight;
	public int Money;
	public int HP;
	public int HPMAX;
	public int AP;
	public int APMAX;
	public int ATK;
	public int ATKRange;
	public int INT;
	public int DEF;
	public int RES_Metal;
	public int RES_Wood;
	public int RES_Water;
	public int RES_Fire;
	public int RES_Earth;
	public int CritRate;
	public int MOV;
	public int SPEED;
	public bool isPlayerTeam;
	public bool AI;
	public bool isDanger;
	public bool isAI(){
		return AI;	
	}
	void Awake(){
		//initHeadIcon ();


	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
