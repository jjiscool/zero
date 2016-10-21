using UnityEngine;
using System.Collections;

public class RoundControler : MonoBehaviour {
	public GameObject player;
	public GameObject[] enemy;
	// Use this for initialization
	void Start () {
		player.GetComponent<PhaseHandler> ().PhaseBegin();
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log (player.GetComponent<PhaseHandler> ().getType ());
		if (player.GetComponent<PhaseHandler> ().getType () == PHASE_TYPE.PHASE_WAITING) {
			player.GetComponent<PhaseHandler> ().PhaseBegin();
		}
	
	}
}
