using UnityEngine;
using System.Collections;
public  enum  ACTION_TYPE{
	ACTION_MOVE,
	ACTION_NULL
}
public class Action
{
	public ACTION_TYPE type;
	public GameObject SUBJECT;
	public GameObject OBJECT;
	public int[,] MOVEPOS;
	public Action(ACTION_TYPE T ){
		type = T;
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
	public Phase(){
		//Debug.Log ("this is a Phase Class");
	}
	public void  setPlayer(GameObject P){
		Player= P;
	}
	public virtual void handle(Action a){
		
	}
	public virtual void update(Transform tr){
		
	
	}
	public virtual PHASE_TYPE getType(){
		return PHASE_TYPE.PHASE_WAITING;
	}
}
public class WaitingPhase:Phase{
	public WaitingPhase(PhaseHandler ph){
		PH=ph;
		Debug.Log ("WaitingPhase");
	}
	public override void handle(Action a){
		
	}
	public override void update(Transform tr){
		//Debug.Log (Player.name+"is Waiting....");

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_WAITING;
	}

}
public class RoundBeginPhase:Phase{
	public RoundBeginPhase(PhaseHandler ph){
		PH=ph;
		Debug.Log ("RoundBeginPhase");
	}
	public override void handle(Action a){
		PH.state = new ThinkingPhase (PH);
		//PH.state.handle (new Action (ACTION_TYPE.ACTION_NULL));

	}
	public override void update(Transform tr){
		Debug.Log ("RoundBegin....");

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_BEGINING;
	}
}
public class ThinkingPhase:Phase{
	public ThinkingPhase(PhaseHandler ph){
		PH=ph;
		Debug.Log ("ThinkingPhase....");
	}
	public override void handle(Action a){
		Debug.Log (a.type);
		if (a.type == ACTION_TYPE.ACTION_MOVE) {
			PH.state = new ActionPhase (PH);
			//PH.state.handle (a);
		}

	}
	public override void update(Transform tr){
		
		Debug.Log (tr.name+"is Thinking...."+tr.gameObject.GetComponent<PhaseHandler>().isAI);

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_THINKING;
	}
}
public class ActionPhase:Phase{
	public ActionPhase(PhaseHandler ph){
		PH=ph;
		Debug.Log ("ActionPhase");
	}
	public override void handle(Action a){
		PH.state = new RoundEndPhase(PH);
		PH.state.handle (new Action (ACTION_TYPE.ACTION_NULL));
	}
	public override void update(Transform tr){
		//Debug.Log ("Actioning....");

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_ACTING;
	}
}
public class RoundEndPhase:Phase{
	public RoundEndPhase(PhaseHandler ph){
		PH=ph;
		Debug.Log ("RoundEndPhase");
	}
	public override void handle(Action a){
		PH.state = new WaitingPhase(PH);
		PH.state.handle (new Action (ACTION_TYPE.ACTION_NULL));
	}
	public override void update(Transform tr){
		//Debug.Log ("Ending....");

	}
	public override PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return PHASE_TYPE.PHASE_ENDING;
	}
}
public class PhaseHandler : MonoBehaviour {
	public Phase state;
	public bool isAI;
	// Use this for initialization
	void Start () {

	}
	void Awake(){
		state = new WaitingPhase (this);
		state.handle(new Action (ACTION_TYPE.ACTION_NULL));
	}
	public void PhaseBegin(){
		state = new RoundBeginPhase (this);
		state.handle (new Action (ACTION_TYPE.ACTION_NULL));
	}
	public  PHASE_TYPE getType(){
		//Debug.Log (Player.name+"is Waiting....");
		return state.getType();
	}
	// Update is called once per frame
	void Update () {
		state.update (transform);
	}
}
