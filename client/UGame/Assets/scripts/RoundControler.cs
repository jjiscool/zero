using UnityEngine;
using System.Collections;

public class RoundControler : MonoBehaviour {
	public GameObject player;
	public GameObject[] enemy;
	public int round;
	private int movedEnemy;
	void Awake(){
		round = 0;
		movedEnemy = -1;
	
	}
	void reset(GameObject p){
		GameObject.Find ("Cameras").GetComponent<followCenter> ().player = p;
		GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = p;
		GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight();
		GameObject.Find ("lightCover").GetComponent<followCenter>().player=p;
	}
	// Use this for initialization
	void Start () {
		player.GetComponent<PhaseHandler> ().PhaseBegin();
		reset (player);
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log (player.GetComponent<PhaseHandler> ().getType ());
		if (enemy.Length>0&&movedEnemy==-1&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			//player.GetComponent<PhaseHandler> ().PhaseBegin();
			movedEnemy=0;
			enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();
			reset (enemy[movedEnemy]);

		}
		if (enemy.Length>0&&movedEnemy>=0&&enemy [movedEnemy].GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			movedEnemy++;
			//Debug.Log (movedEnemy);
			if (movedEnemy >= enemy.Length) {
				player.GetComponent<PhaseHandler> ().PhaseBegin ();
				reset (player);
				movedEnemy = -1;
			} else {
				enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();
				reset (enemy[movedEnemy]);
			} 
		}
		if (enemy.Length == 0&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			player.GetComponent<PhaseHandler> ().PhaseBegin();
			reset (player);
		}
	}
}
