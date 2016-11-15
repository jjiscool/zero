using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoundControler : MonoBehaviour {
	public GameObject player;
	public List<GameObject> enemy;
	public List<GameObject> WaitingList;
	public List<GameObject> ActionedList;
	public bool playerInBattle;
	void Awake(){
		playerInBattle = false;
		WaitingList = new List<GameObject> ();
		ActionedList = new List<GameObject> ();
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
	void renewWaitingList(){
		List<GameObject>  newWaitingList = new List<GameObject> ();
		newWaitingList.Add (player);
		for (int i = 0; i < enemy.Count; i++) {
			int dis = Mathf.Abs (enemy [i].GetComponent<playerMove> ().row - player.GetComponent<playerMove> ().row) + Mathf.Abs (enemy [i].GetComponent<playerMove> ().column - player.GetComponent<playerMove> ().column);
			if (!isGOinWaitingList (enemy [i])&&!isGOinActionedList (enemy [i])) {
				if (dis <= enemy [i].GetComponent<playerStatus> ().MinSight && enemy [i].GetComponent<playerStatus> ().isDanger) {
					int ii;
					int ispeed = enemy [i].GetComponent<playerStatus> ().SPEED;
					for (ii = 0; ii < newWaitingList.Count; ii++) {
						if (newWaitingList [ii].GetInstanceID() == player.GetInstanceID()) {
							if (ispeed >= player.GetComponent<playerStatus> ().SPEED)
								break;
						} else {
							if (ispeed >= enemy [i].GetComponent<playerStatus> ().SPEED)
								break;
						}
					}
					newWaitingList.Insert (ii, enemy[i]);	
					//order.Insert (ii, i);	
					//order.Add (i);
				}
			} 
			else 
			{
					if (dis > enemy [i].GetComponent<playerStatus> ().MaxSight || !enemy [i].GetComponent<playerStatus> ().isDanger) {
						//order.RemoveAt (FindOderIndexByID (i));
					} else{
						int ii;
						int ispeed = enemy [i].GetComponent<playerStatus> ().SPEED;
						for (ii = 0; ii < newWaitingList.Count; ii++) {
							if (newWaitingList [ii].GetInstanceID() == player.GetInstanceID()) {
								if (ispeed >= player.GetComponent<playerStatus> ().SPEED)
									break;
							} else {
								if (ispeed >= enemy [i].GetComponent<playerStatus> ().SPEED)
									break;
							}
						}
						newWaitingList.Insert (ii, enemy[i]);	
					}	
			}
			

		}
		if (newWaitingList.Count > 1)
			playerInBattle = true;
		else
			playerInBattle = false;
		WaitingList = newWaitingList;
		ActionedList = new List<GameObject> ();
	}
	public void updateWaitingList(){
		List<GameObject>  newWaitingList = new List<GameObject> ();
		for (int i = 0; i < WaitingList.Count; i++) {
			int dis = Mathf.Abs (WaitingList [i].GetComponent<playerMove> ().row - player.GetComponent<playerMove> ().row) + Mathf.Abs (WaitingList [i].GetComponent<playerMove> ().column - player.GetComponent<playerMove> ().column);
			if (WaitingList [i].GetInstanceID() == player.GetInstanceID ()) {
				newWaitingList.Add (WaitingList [i]);
				continue;
			}
			if (dis > WaitingList [i].GetComponent<playerStatus> ().MaxSight || !WaitingList [i].GetComponent<playerStatus> ().isDanger) {
				
			} else {
				newWaitingList.Add (WaitingList [i]);
			}			
		}
		WaitingList = newWaitingList;
	}
	public bool isGOinWaitingList(GameObject GO){
		for (int i = 0; i < WaitingList.Count; i++) {
			if (WaitingList [i].GetInstanceID () == GO.GetInstanceID ())
				return true;
		}
		return false;
	}
	public bool isGOinActionedList(GameObject GO){
		for (int i = 0; i < ActionedList.Count; i++) {
			if (ActionedList[i].GetInstanceID () == GO.GetInstanceID ())
				return true;
		}
		return false;
	}
	public bool isAllGOEndinActionedList(){
		for (int i = 0; i < ActionedList.Count; i++) {
			if (ActionedList[i].GetComponent<PhaseHandler> ().getType () != PHASE_TYPE.PHASE_WAITING)
				return false;
		}
		return true;
	}
	public void RemoveGOFromRoundControler(GameObject GO){
		for (int i = 0; i < WaitingList.Count; i++) {
			if (WaitingList [i].GetInstanceID () == GO.GetInstanceID ())
				WaitingList.RemoveAt (i);
		}
		for (int i = 0; i < ActionedList.Count; i++) {
			if (ActionedList [i].GetInstanceID () == GO.GetInstanceID ())
				ActionedList.RemoveAt (i);
		}
		for (int i = 0; i < enemy.Count; i++) {
			if (enemy [i].GetInstanceID () == GO.GetInstanceID ())
				enemy.RemoveAt (i);
		}

	
	
	}
	public GameObject returnPlayer(){
		return player;
	}
//	//回合切换时重设摄像头、光线、遮罩等
	void reset(GameObject p){
		GameObject.Find ("Cameras").GetComponent<followCenter> ().player = p;
		GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = p;
		GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight();
		GameObject.Find ("lightCover").GetComponent<followCenter>().player=p;
	}
	// Update is called once per frame
	void Update () {
		if (!playerInBattle) { 
			//当前为非战斗模式，如果玩家的状态为等待
			if (player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
				if (!CheckPlayerInSeeSight ()) {
					player.GetComponent<PhaseHandler> ().PhaseBegin ();
					reset (player);
				} else {
					renewWaitingList ();
					GameObject nowGO = WaitingList [0];
					WaitingList.RemoveAt (0);
					ActionedList.Add (nowGO);
					nowGO.GetComponent<PhaseHandler> ().PhaseBegin ();
					//reset (nowGO);
				}
			}
		} else {
			if (isAllGOEndinActionedList()) {
				if (WaitingList.Count > 0) {
					updateWaitingList ();
					if (WaitingList.Count > 0) {
						GameObject nowGO = WaitingList [0];
						WaitingList.RemoveAt (0);
						ActionedList.Add (nowGO);
						nowGO.GetComponent<PhaseHandler> ().PhaseBegin ();
						//reset (nowGO);
					}
				} else {
					renewWaitingList();
					GameObject nowGO = WaitingList [0];
					WaitingList.RemoveAt (0);
					ActionedList.Add (nowGO);
					nowGO.GetComponent<PhaseHandler> ().PhaseBegin ();
					//reset (nowGO);
				}
			}
		}
		return;
	}
}
