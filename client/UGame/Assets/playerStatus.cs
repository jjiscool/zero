using UnityEngine;
using System.Collections;

public class playerStatus : MonoBehaviour {
	public int HP;
	public int Money;
	public int SP;
	public int MOV;
	public bool isPlayerTeam;
	public bool AI;
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
