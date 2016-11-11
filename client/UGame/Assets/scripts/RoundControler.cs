using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoundControler : MonoBehaviour {
	public GameObject player;
	public GameObject[] enemy;
	public List<int> order;
	public int round;
	private int movedEnemy;
	public bool playerInBattle;
	void Awake(){
		round = -1;
		movedEnemy = -1;
		playerInBattle = false;
		order = new List<int> ();
		order.Add (-1);
	
	}
	public bool CheckPlayerInSeeSight(){
		for (int i = 0; i < enemy.Length; i++) {
			int dis = Mathf.Abs (enemy[i].GetComponent<playerMove>().row-player.GetComponent<playerMove>().row)+Mathf.Abs (enemy[i].GetComponent<playerMove>().column-player.GetComponent<playerMove>().column);
			if (dis <= enemy[i].GetComponent<playerStatus>().MinSight&&enemy[i].GetComponent<playerStatus>().isDanger)
				return true;
		}
		return false;
	}
	public bool CheckPlayerInBattle(){
		return playerInBattle;
	}
	void renewOrder(){
		for (int i = 0; i < enemy.Length; i++) {
			int dis = Mathf.Abs (enemy[i].GetComponent<playerMove>().row-player.GetComponent<playerMove>().row)+Mathf.Abs (enemy[i].GetComponent<playerMove>().column-player.GetComponent<playerMove>().column);
			if (!isInOder (i)) {
				if (dis <= enemy [i].GetComponent<playerStatus> ().MinSight && enemy [i].GetComponent<playerStatus> ().isDanger) {
					int ii;
					int ispeed = enemy [i].GetComponent<playerStatus> ().SPEED;
					for (ii = 0; ii < order.Count; ii++) {
						if (order [ii] == -1) {
							if (ispeed >= player.GetComponent<playerStatus> ().SPEED)
								break;
						} else {
							if (ispeed >= enemy [i].GetComponent<playerStatus> ().SPEED)
								break;
						
						}
					}
					order.Insert (ii, i);	
					//order.Add (i);
				} 
			} 
			else {
				if (dis > enemy [i].GetComponent<playerStatus> ().MaxSight || !enemy [i].GetComponent<playerStatus> ().isDanger) {
					order.RemoveAt (FindOderIndexByID(i));
				} 	
			}
		}
		if (order.Count > 1)
			playerInBattle = true;
		else
			playerInBattle = false;
		
	}
	void reset(GameObject p){
		GameObject.Find ("Cameras").GetComponent<followCenter> ().player = p;
		GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = p;
		GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight();
		GameObject.Find ("lightCover").GetComponent<followCenter>().player=p;
	}
	// Use this for initialization
	void Start () {
		renewOrder ();
		player.GetComponent<PhaseHandler> ().PhaseBegin();
		reset (player);
	}
	GameObject getGOInOderIndex(int idx){
		if (order [idx] == -1)
			return player;
		else
			return enemy [order [idx]];
	}
	int FindOderIndexByID(int id){
		for (int i = 0; i < order.Count; i++) {
			if (order [i] == id)
				return i;
			
		}
		return -2;
	}
	int FindNextOderIndexByID(int id){
		for (int i = 0; i < order.Count; i++) {
			if (order [i] == id)
				return i+1;
		}
		return -2;
		
	}
	bool isInOder(int id){
		for (int i = 0; i < order.Count; i++) {
			if (order [i] == id)
				return true;

		}
		return false;
	}
	// Update is called once per frame
	void Update () {
		if (!playerInBattle) {
			if (player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
				renewOrder ();
				if (!playerInBattle) {
					player.GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (player);
				}
				else
				{
					round = order[0];
					Debug.Log ("Round for " + round);
					getGOInOderIndex(FindOderIndexByID(round)).GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (getGOInOderIndex(FindOderIndexByID(round)));
				}
			}
		} else {
			if (getGOInOderIndex(FindOderIndexByID(round)).GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
				int next=FindNextOderIndexByID(round);
				if (next >= order.Count)
					next = 0;
				int nextround = order [next];
				if (round != -1) {
					round = nextround;
					Debug.Log ("Round for " + round);
					renewOrder ();
					getGOInOderIndex (FindOderIndexByID (round)).GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (getGOInOderIndex(FindOderIndexByID(round)));

				} else {
					renewOrder ();
					next=FindNextOderIndexByID(round);
					if(next>=order.Count)
						next = 0;
						nextround = order [next];
						round = nextround;
						Debug.Log ("Round for " + round);
						getGOInOderIndex (FindOderIndexByID (round)).GetComponent<PhaseHandler> ().PhaseBegin ();
						reset (getGOInOderIndex(FindOderIndexByID(round)));			
				}


					
				
			}
		
		
		}
		return;
		//Debug.Log (player.GetComponent<PhaseHandler> ().getType ());
		if (enemy.Length>0&&movedEnemy==-1&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			//player.GetComponent<PhaseHandler> ().PhaseBegin();
			movedEnemy=0;
			enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();renewOrder ();
			reset (enemy[movedEnemy]);

		}
		if (enemy.Length>0&&movedEnemy>=0&&enemy [movedEnemy].GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			movedEnemy++;
			//Debug.Log (movedEnemy);
			if (movedEnemy >= enemy.Length) {
				player.GetComponent<PhaseHandler> ().PhaseBegin ();renewOrder ();
				reset (player);
				movedEnemy = -1;
			} else {
				enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();renewOrder ();
				reset (enemy[movedEnemy]);
			} 
		}
		if (enemy.Length == 0&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			player.GetComponent<PhaseHandler> ().PhaseBegin();renewOrder ();
			reset (player);
		}
	}
}
