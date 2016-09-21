using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	//移动的时间
	public float moveTime = 0.1f;
	//可能碰撞的层
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollier;
	private Rigidbody2D rb2D;

	private float inverseMoveTime;



	protected virtual void Start () {
		boxCollier = GetComponent<BoxCollider2D> ();
		rb2D = GetComponent<Rigidbody2D> ();

		inverseMoveTime = 1f / moveTime;//1秒移动的距离
	}

	//判断是否可以移动
	protected bool Move(int xDir,int yDir, out RaycastHit2D hit)
	{
		//out 让函数返回除bool值以外 还返回 RaycastHit2D 类型的hit
		Vector2 start = transform.position;

		Vector2 end = start + new Vector2 (xDir, yDir);

		boxCollier.enabled = false;//避免检测到跟自身的碰撞

		hit = Physics2D.Linecast (start, end, blockingLayer);

		boxCollier.enabled = true;

		if (hit.transform == null) {
			//如果没有检测到碰撞
			StartCoroutine(SmoothMovement(end));
			return true;
		}

		return true;
	}

	protected IEnumerator SmoothMovement(Vector3 end)
	{
		float sqrRemainDistance = (transform.position - end).sqrMagnitude;
		while (sqrRemainDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position,end,inverseMoveTime*Time.deltaTime);
			rb2D.MovePosition (newPosition);
			sqrRemainDistance = (transform.position - end).sqrMagnitude;//更新距离
			yield return null;
		}
	}

	protected virtual void AttemptMove<T>(int xDir,int yDir)
		where T:Component
	
	{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);
		if (hit.transform == null)
			return;
		T hitComponent = hit.transform.GetComponent<T> ();

		if (!canMove && hitComponent != null)
			OnCantMove (hitComponent);
		
	}

	protected abstract void OnCantMove<T> (T component)
		where T:Component; //在继承类中重写


}
