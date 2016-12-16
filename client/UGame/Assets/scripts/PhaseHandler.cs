using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public  enum  ACTION_TYPE{
	ACTION_MOVE,
	ACTION_ATTACK,
	ACTION_NULL
}
public class Action
{
	public ACTION_TYPE type;
	public GameObject SUBJECT;
	public GameObject OBJECT;
	public int[] MOVEPOS;
	public Action(ACTION_TYPE T,GameObject sub){
		type = T;
		SUBJECT = sub;
		MOVEPOS = new int[2];
	}

}
public  enum  PHASE_TYPE{
	PHASE_WAITING,
	PHASE_BEGINING,
	PHASE_THINKING,
	PHASE_ACTING,
	PHASE_ENDING

}
public class Phase
{	public GameObject Player;
	public PhaseHandler PH;
	public Action act;
	public Phase(){
		//Debug.Log ("this is a Phase Class");
	}
	public virtual void handle(Action a){
		
	}
	public virtual void update(Transform tr){

	
	}
	public virtual PHASE_TYPE getType(){
		return PHASE_TYPE.PHASE_WAITING;
	}
}
//等待阶段
public class WaitingPhase:Phase{
	public WaitingPhase(PhaseHandler ph,Action a){
		PH=ph;
		act = a;
		Debug.Log (a.SUBJECT.name+"'s "+"WaitingPhase");
	}
	public override void handle(Action a){
		//等待阶段-》回合开始阶段	
	}
	public override void update(Transform tr){
		//Debug.Log (tr.name+"is Waiting....");

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_WAITING;
	}

}
//回合开始阶段
public class RoundBeginPhase:Phase{
	private bool doneFirstFrame;
	public RoundBeginPhase(PhaseHandler ph,Action a){
		PH=ph;
		act = a;
		Debug.Log (a.SUBJECT.name+"'s "+"BeginPhase");
	}
	public override void handle(Action a){
		//回合开始阶段-》决策阶段
		//PH.state.handle (new Action (ACTION_TYPE.ACTION_NULL));

	}
	public override void update(Transform tr){
		//以下为首帧时触发
		if (!doneFirstFrame) {
			doneFirstFrame = true;
			switch (act.type) {
			default:
				Debug.Log ("No this action type");
				break;
			case ACTION_TYPE.ACTION_MOVE:
				
				break;
			case ACTION_TYPE.ACTION_NULL:
				PH.state = new ThinkingPhase (PH,act);
				if (!act.SUBJECT.GetComponent<playerStatus> ().isAI ()&&GameObject.Find("map").GetComponent<RoundControler>().playerInBattle) {
					act.SUBJECT.GetComponent<playerMove> ().DrawMovabletile ();
				}
				break;
			case ACTION_TYPE.ACTION_ATTACK:
				
				break;
			}
			if (!act.SUBJECT.GetComponent<playerStatus> ().isAI ()) {
				act.SUBJECT.GetComponent<playerMove> ().RemoveMoveClick ();
			}
		}
		//以下为每帧更新
		switch (act.type) {
		default:
			Debug.Log ("No this action type");
			break;
		case ACTION_TYPE.ACTION_MOVE:
			//tr.GetComponent<playerMove> ().Move_Actioning();
			break;
		case ACTION_TYPE.ACTION_NULL:
			//tr.GetComponent<playerMove> ().NOACTION_Actioning();
			break;
		case ACTION_TYPE.ACTION_ATTACK:
			//tr.GetComponent<playerMove> ().Attack_Actioning ();
			break;
		}
		//Debug.Log ("RoundBegin....");

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_BEGINING;
	}
}
//回合决策阶段
public class ThinkingPhase:Phase{
	public ThinkingPhase(PhaseHandler ph,Action a){
		PH=ph;
		act = a;
		Debug.Log (a.SUBJECT.name+"'s "+"ThinkingPhase");
	}
	public override void handle(Action a){
		//Debug.Log (a.type);
		//回合决策阶段-》行动动画执行
		switch (a.type) {
		default:
			Debug.Log ("No this action type");
			break;
		case ACTION_TYPE.ACTION_MOVE:
			PH.state = new ActionPhase (PH,a);
			break;
		case ACTION_TYPE.ACTION_NULL:
			PH.state = new ActionPhase (PH,a);
			break;
		case ACTION_TYPE.ACTION_ATTACK:
			PH.state = new ActionPhase (PH,a);
			break;
		}
		if (!a.SUBJECT.GetComponent<playerStatus> ().isAI ()) {
			a.SUBJECT.GetComponent<playerMove> ().RemoveMovabletile();
		}
	}
	public override void update(Transform tr){
		//决策阶段每帧更新
		//Debug.Log (tr.name+"is Thinking...."+tr.gameObject.GetComponent<playerStatus>().isAI());
		if (tr.gameObject.GetComponent<playerStatus> ().isAI()) {
			//如果为AI决策，执行AI
			tr.gameObject.GetComponent<playerMove> ().AI ();
			return;
		}
		//玩家决策
		Vector3	screenPosition = Camera.main.WorldToScreenPoint(tr.position);  
		Vector3 mousePositionOnScreen = Input.mousePosition;   
		mousePositionOnScreen.z = screenPosition.z;  
		Vector3	mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen); 
		if (Input.GetMouseButtonDown(0)) {

			//禁止点击 穿透 UI
			if (EventSystem.current.IsPointerOverGameObject ()) {
				//Debug.Log ("UI click");
				return;
			} 
			#if IPHONE || ANDROID
			if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch(0).fingerId)) {
				//移动端
				return;
			}
			#endif
			//输入为点击地图某处
			int[] pos=GameObject.Find ("map").GetComponent<TilesManager>().posTransform2(mousePositionInWorld.x,mousePositionInWorld.y);
			//点击的位置是否有地图对象
			if (GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().obj_list.hasObjInRowColumn (pos [0], pos [1])) {
				List<OBJTYPEData> objl = GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().obj_list.getObjByRowColumn (pos [0], pos [1]);
				int hasE = -1;
				int hasM = -1;
				for (int ii = 0; ii < objl.Count; ii++) {
					//是否是敌人
					if (objl[ii].type == OBJTYPE.OBJTYPE_ENEMY) {
						int er = objl[ii].thisOBJ.GetComponent<playerMove> ().row;
						int ec = objl[ii].thisOBJ.GetComponent<playerMove> ().column;
						int dis = Mathf.Abs (tr.GetComponent<playerMove> ().row - er) + Mathf.Abs (tr.GetComponent<playerMove> ().column - ec);
						if (dis <= tr.GetComponent<playerStatus> ().ATKRange) {
							bool isShortest = GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().isWalkShortest (pos [0], pos [1], tr.GetComponent<playerMove> ().row, tr.GetComponent<playerMove> ().column);
							if(isShortest)
								hasE = ii;
							else
								Debug.Log ("Fail,Some Objet on the Way!");
						} else {
							Debug.Log ("Out OF Range");
						}
					} else if (objl[ii].walkable) {
						hasM = ii;
					} else {
						Debug.Log ("No Obj is intercactive");
					}
				} 
				Debug.Log ("Player ATK!");
				if (hasE >= 0) {
					tr.GetComponent<playerMove> ().Attack (objl [hasE].thisOBJ);
					return;
				
				} else if (hasM >= 0) {
					//Debug.Log ("click DOOR");
					Astar astar= new Astar(tr.GetComponent<playerMove>().row,tr.GetComponent<playerMove>().column,pos[0],pos[1],GameObject.Find ("map").GetComponent<RandomDungeonCreator>().getMap(),GameObject.Find ("map").GetComponent<RandomDungeonCreator>().MapWidth,GameObject.Find ("map").GetComponent<RandomDungeonCreator>().MapHeight);
					astar.isWalkableFunc = GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().MapWalkable;
					astar.Run ();
					int pathid = astar.finalpath.Count-1;
					if (pathid >=tr.GetComponent<playerMove>().MAXSTEP) {
						Debug.Log ("TOO LONG!");
					}
					else tr.GetComponent<playerMove> ().moveTo (pos [0], pos [1]);
					return;
				} else {
					Debug.Log ("No Action");
				}
			}
			//空地则执行移动（moveto内进行阶段切换）
			else {
				Astar astar= new Astar(tr.GetComponent<playerMove>().row,tr.GetComponent<playerMove>().column,pos[0],pos[1],GameObject.Find ("map").GetComponent<RandomDungeonCreator>().getMap(),GameObject.Find ("map").GetComponent<RandomDungeonCreator>().MapWidth,GameObject.Find ("map").GetComponent<RandomDungeonCreator>().MapHeight);
				astar.isWalkableFunc = GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().MapWalkable;
				astar.Run ();
				int pathid = astar.finalpath.Count-1;
				if (pathid >=tr.GetComponent<playerMove>().MAXSTEP) {
					Debug.Log ("TOO LONG!");
				}
				else tr.GetComponent<playerMove> ().moveTo (pos[0], pos[1]);
				return;
			}
		}
		return;
	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_THINKING;
	}
}
//行动动画阶段
public class ActionPhase:Phase{
	public ActionPhase(PhaseHandler ph,Action a){
		PH=ph;
		act = a;
		Debug.Log (a.SUBJECT.name+"'s "+"ActionPhase");
	}
	public override void handle(Action a){
		//动画阶段-》回合结束阶段
		PH.state = new RoundEndPhase(PH,a);
	}
	public override void update(Transform tr){
		//Debug.Log ("Actioning....");
		//动画Update
		switch (act.type) {
		default:
			Debug.Log ("No this action type");
			break;
		case ACTION_TYPE.ACTION_MOVE:
			tr.GetComponent<playerMove> ().Move_Actioning();
			break;
		case ACTION_TYPE.ACTION_NULL:
			tr.GetComponent<playerMove> ().NOACTION_Actioning();
			break;
		case ACTION_TYPE.ACTION_ATTACK:
			//攻击的动画处理
			tr.GetComponent<playerMove> ().Attack_Actioning ();
			break;
		}
	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_ACTING;
	}
}
//回合结束阶段（结算阶段）
public class RoundEndPhase:Phase{
	private bool doneFirstFrame;
	public RoundEndPhase(PhaseHandler ph,Action a){
		PH=ph;
		act = a;
		Debug.Log (a.SUBJECT.name+"'s "+"EndPhase");
		doneFirstFrame = false;
	}
	public override void handle(Action a){

	}
	public override void update(Transform tr){
		//以下为首帧时触发
		if (!doneFirstFrame) {
			doneFirstFrame = true;
			switch (act.type) {
			default:
				Debug.Log ("No this action type");
				break;
			case ACTION_TYPE.ACTION_MOVE:
				Debug.Log ("MOVE END?"+act.SUBJECT.GetComponent<playerStatus> ().AI);
				//这里判断如果移动到出口、或触发其他结算，可阻碍进入等待阶段
				if (!act.SUBJECT.GetComponent<playerStatus> ().AI) {
					int r = act.SUBJECT.gameObject.GetComponent<playerMove> ().MapOBJ.row;
					int c = act.SUBJECT.gameObject.GetComponent<playerMove> ().MapOBJ.column;
					List<OBJTYPEData> objs = GameObject.Find ("map").GetComponent<RandomDungeonCreator> ().obj_list.getObjByRowColumn (r, c);
					bool isblock = false;
					for (int i = 0; i < objs.Count; i++) {
						if (objs [i].type == OBJTYPE.OBJTYPE_DOWNSTAIRS)
							isblock = true;
					}
					Debug.Log ("block?"+isblock);
					if (isblock) {
						popController.btnConfirmEvent += () => {  
							Debug.Log ("OK");  
							GameObject.Find("map").GetComponent<MapObjectGenerator> ().NewDungeon ();
							GameObject.Find ("popContainer").GetComponent<popController> ().fadeOut ();
						}; 
						popController.btnCancelEvent += () => {  
							Debug.Log ("Not OK");  
							GameObject.Find ("popContainer").GetComponent<popController> ().fadeOut ();
							PH.state = new WaitingPhase (PH, act);
							PH.state.handle (new Action (ACTION_TYPE.ACTION_NULL, act.SUBJECT));
						};
						popController.popTitText = "是否进入下层？";
						GameObject.Find ("popContainer").GetComponent<popController> ().fadeIn ("ConfirmPop");

					} else {
						PH.state = new WaitingPhase (PH, act);
						PH.state.handle (new Action (ACTION_TYPE.ACTION_MOVE, act.SUBJECT));
					}

				} else {
					PH.state = new WaitingPhase (PH, act);
					PH.state.handle (new Action (ACTION_TYPE.ACTION_MOVE, act.SUBJECT));
				}
				break;
			case ACTION_TYPE.ACTION_NULL:
				PH.state = new WaitingPhase(PH,act);
				PH.state.handle (new Action (ACTION_TYPE.ACTION_NULL,act.SUBJECT));
				break;
			case ACTION_TYPE.ACTION_ATTACK:
				//攻击后不立刻切换回合
				act.OBJECT.GetComponent<playerStatus> ().HP -= act.SUBJECT.GetComponent<playerStatus> ().ATK;
				act.OBJECT.GetComponent<playerMove>().CastDamage(act.SUBJECT.GetComponent<playerStatus> ().ATK);
				break;
			}
			if (!act.SUBJECT.GetComponent<playerStatus> ().isAI ()) {
				act.SUBJECT.GetComponent<playerMove> ().RemoveMoveClick ();
			}
		}
		//以下为每帧更新
		switch (act.type) {
		default:
			Debug.Log ("No this action type");
			break;
		case ACTION_TYPE.ACTION_MOVE:
			//tr.GetComponent<playerMove> ().Move_Actioning();
			break;
		case ACTION_TYPE.ACTION_NULL:
			//tr.GetComponent<playerMove> ().NOACTION_Actioning();
			break;
		case ACTION_TYPE.ACTION_ATTACK:
			//tr.GetComponent<playerMove> ().Attack_Actioning ();
			if(act.OBJECT.GetComponent<playerMove>().DamageText==null){
				if (act.OBJECT.GetComponent<playerStatus> ().HP <= 0) {
					act.OBJECT.GetComponent<playerMove> ().Dead();
				}
				if (act.SUBJECT.GetComponent<playerStatus> ().HP == 0) {
					act.SUBJECT.GetComponent<playerMove> ().Dead();
				}
				PH.state = new WaitingPhase(PH,act);
				PH.state.handle (new Action (ACTION_TYPE.ACTION_NULL,act.SUBJECT));
			}
			break;
		}

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_ENDING;
	}
}
public class PhaseHandler : MonoBehaviour {
	//角色动状态
	public Phase state;
	// Use this for initialization
	void Start () {

	}
	void Awake(){
		//初始化为等待阶段
		state = new WaitingPhase (this,new Action (ACTION_TYPE.ACTION_NULL,transform.gameObject));
		state.handle(new Action (ACTION_TYPE.ACTION_NULL,transform.gameObject));
	}
	public void PhaseBegin(){
		//触发回合开始
		state = new RoundBeginPhase (this,new Action (ACTION_TYPE.ACTION_NULL,transform.gameObject));
		//state.handle (new Action (ACTION_TYPE.ACTION_NULL,transform.gameObject));
	}
	public  PHASE_TYPE getType(){
		//返回当前阶段状态
		return state.getType();
	}
	// Update is called once per frame
	void Update () {
		//执行对应阶段的更新
		if(transform!=null)state.update (transform);
	}
}
