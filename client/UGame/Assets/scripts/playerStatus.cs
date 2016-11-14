using UnityEngine;
using System.Collections;

public class playerStatus : MonoBehaviour {
	public int HP;
	public int HPMAX;
	public int Money;
	public int AP;
	public int APMAX;
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
