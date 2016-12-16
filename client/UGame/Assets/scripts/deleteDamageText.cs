using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class deleteDamageText : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
		//debrisSequence.AppendCallback (() => Destroy (transform.gameObject));
	}
	public void Cast(float x,float y){
		Sequence debrisSequence = DOTween.Sequence ();
		Transform dt = transform;
		dt.localPosition = new Vector3 (x, y, 0);
		dt.GetComponent<MeshRenderer> ().material.color=new Color(1,1,1,0);
		debrisSequence.Append (dt.DOLocalMove(new Vector3(x,y+0.5f,0),0.3f));
		debrisSequence.Join (dt.GetComponent<MeshRenderer> ().material.DOFade(1.0f,0.3f));
		debrisSequence.Append (dt.DOLocalMove(new Vector3(x,y+0.8f,0),0.3f).SetDelay(0.5f));
		debrisSequence.Join (dt.GetComponent<MeshRenderer> ().material.DOFade(0.0f,0.3f));
		debrisSequence.AppendCallback (() => Destroy (transform.gameObject));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
