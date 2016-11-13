using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoundControler : MonoBehaviour {
	public GameObject player;
	public List<GameObject> enemy;
	public List<int> order;
	public int round;
	public bool playerInBattle;
	void Awake(){
		round = -1;
		playerInBattle = false;
		order = new List<int> ();
		order.Add (-1);
	
	}
	//判断敌人列表里是否有有敌人看到了玩家
	public bool CheckPlayerInSeeSight(){
		for (int i = 0; i < enemy.Count; i++) {
			int dis = Mathf.Abs (enemy[i].GetComponent<playerMove>().row-player.GetComponent<playerMove>().row)+Mathf.Abs (enemy[i].GetComponent<playerMove>().column-player.GetComponent<playerMove>().column);
			if (dis <= enemy[i].GetComponent<playerStatus>().MinSight&&enemy[i].GetComponent<playerStatus>().isDanger)
				return true;
		}
		return false;
	}
	//判断当前是否进入战斗模式
	public bool CheckPlayerInBattle(){
		return playerInBattle;
	}
	//更新战斗模式的队列
	void renewOrder(){
		for (int i = 0; i < enemy.Count; i++) {
			int dis = Mathf.Abs (enemy[i].GetComponent<playerMove>().row-player.GetComponent<playerMove>().row)+Mathf.Abs (enemy[i].GetComponent<playerMove>().column-player.GetComponent<playerMove>().column);
			if (!isInOder (i)) {
				if ( enemy [i].GetComponent<playerStatus> ().HP>0&&dis <= enemy [i].GetComponent<playerStatus> ().MinSight && enemy [i].GetComponent<playerStatus> ().isDanger) {
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
	//回合切换时重设摄像头、光线、遮罩等
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
	//根据索引值返回战斗队列中的GO
	public GameObject getGOInOderIndex(int idx){
		if (order [idx] == -1)
			return player;
		else
			return enemy [order [idx]];
	}
	//根据地图的对象id返回战斗队列中的GO
	public GameObject getGOInOderID(int id){
		int idx = FindOderIndexByID (id);
		if (order [idx] == -1)
			return player;
		else
			return enemy [order [idx]];
	}
	//查找队列中对象id的索引值
	int FindOderIndexByID(int id){
		for (int i = 0; i < order.Count; i++) {
			if (order [i] == id)
				return i;
			
		}
		return -2;
	}
	//根据GO的id删除掉战斗队列中对应的GO
	public void RemoveOderByInstanceID(GameObject rgo){
		int rid = -1;
		for (int i = 0; i < order.Count; i++) {
			if (getGOInOderIndex (i).GetInstanceID() == rgo.GetInstanceID()) {
				rid = i;
			}
		}
		if (rid >= 0)
			order.RemoveAt (rid);
	}
	//根据GO的id删除掉敌人列表的敌人
	public void RemoveEnemyByInstanceID(GameObject rgo){
		int rid = -1;
		for (int i = 0; i < enemy.Count; i++) {
			if (enemy[i].GetInstanceID() == rgo.GetInstanceID()) {
				rid = i;
			}
		}
		if (rid >= 0)
			enemy.RemoveAt(rid);
	}
	//查找地图对象id的下一个索引值
	int FindNextOderIndexByID(int id){
		for (int i = 0; i < order.Count; i++) {
			if (order [i] == id)
				return i+1;
		}
		return -2;
		
	}
	//判断地图对象是否值战斗队列中
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
			//当前为非战斗模式，如果玩家的状态为等待
			if (player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
				//更新战斗队列
				renewOrder ();
				if (!playerInBattle) {
					//如果更新后，玩家依然非战斗模式，激活玩回合
					Debug.Log ("Round for " + player.name);
					player.GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (player);
				}
				else
				{
					//如果进入了战斗模式，则激活战斗队列第一个角色
					round = order[0];
					Debug.Log ("Round for " + getGOInOderIndex (FindOderIndexByID (round)).name);
					getGOInOderIndex(FindOderIndexByID(round)).GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (getGOInOderIndex(FindOderIndexByID(round)));
				}
			}
		} else {
			//战斗模式，判断当前回合的角色是否进入了等待阶段
			if (getGOInOderIndex(FindOderIndexByID(round)).GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
				//如果等待，先获取下一个角色的索引
				int next=FindNextOderIndexByID(round);
				//如果下一个角色的索引超过了队列长度
				if (next >= order.Count)
					next = 0; //回到第一个角色
				int nextround = order [next];
				if (round != -1) {
					//如果当前不是玩家的回合，激活下一个敌人的回合
					round = nextround;
					Debug.Log ("Round for " + getGOInOderIndex (FindOderIndexByID (round)).name);
					renewOrder ();
					getGOInOderIndex (FindOderIndexByID (round)).GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (getGOInOderIndex(FindOderIndexByID(round)));

				} else {
					//如果是玩家的回合，先更新队列
					renewOrder ();
					//再次获得下一个回合的索引，再激活其回合
					next=FindNextOderIndexByID(round);
					if(next>=order.Count)
						next = 0;
					nextround = order [next];
					round = nextround;
					Debug.Log ("Round for " + getGOInOderIndex (FindOderIndexByID (round)).name);
					getGOInOderIndex (FindOderIndexByID (round)).GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (getGOInOderIndex(FindOderIndexByID(round)));			
				}
			}
		
		
		}
		return;
		//Debug.Log (player.GetComponent<PhaseHandler> ().getType ());
//		if (enemy.Length>0&&movedEnemy==-1&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
//			//player.GetComponent<PhaseHandler> ().PhaseBegin();
//			movedEnemy=0;
//			enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();renewOrder ();
//			reset (enemy[movedEnemy]);
//
//		}
//		if (enemy.Length>0&&movedEnemy>=0&&enemy [movedEnemy].GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
//			movedEnemy++;
//			//Debug.Log (movedEnemy);
//			if (movedEnemy >= enemy.Length) {
//				player.GetComponent<PhaseHandler> ().PhaseBegin ();renewOrder ();
//				reset (player);
//				movedEnemy = -1;
//			} else {
//				enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();renewOrder ();
//				reset (enemy[movedEnemy]);
//			} 
//		}
//		if (enemy.Length == 0&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
//			player.GetComponent<PhaseHandler> ().PhaseBegin();renewOrder ();
//			reset (player);
//		}
	}
}
