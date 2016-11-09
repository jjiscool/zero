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

	// Use this for initialization
	void Start () {
		player.GetComponent<PhaseHandler> ().PhaseBegin();
		GameObject.Find ("Cameras").GetComponent<followCenter> ().player = player;
		GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = player;
		GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log (player.GetComponent<PhaseHandler> ().getType ());
		if (enemy.Length>0&&movedEnemy==-1&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			//player.GetComponent<PhaseHandler> ().PhaseBegin();
			movedEnemy=0;
			enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();
			GameObject.Find ("Cameras").GetComponent<followCenter> ().player = enemy [movedEnemy];
			GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = enemy [movedEnemy];
			GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();

		}
		if (enemy.Length>0&&movedEnemy>=0&&enemy [movedEnemy].GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			movedEnemy++;
			//Debug.Log (movedEnemy);
			if (movedEnemy >= enemy.Length) {
				player.GetComponent<PhaseHandler> ().PhaseBegin ();
				GameObject.Find ("Cameras").GetComponent<followCenter> ().player = player;
				GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = player;
				GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();
				movedEnemy = -1;
			} else {
				enemy[movedEnemy].GetComponent<PhaseHandler> ().PhaseBegin();
				GameObject.Find ("Cameras").GetComponent<followCenter> ().player = enemy [movedEnemy];
				GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = enemy [movedEnemy];
				GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();
			} 
		}
		if (enemy.Length == 0&&player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			player.GetComponent<PhaseHandler> ().PhaseBegin();
			GameObject.Find ("light").GetComponent<ligthmap> ().lightsource = player;
			GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();
		}
	}
}
