using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class obstacleAvoidance : MonoBehaviour {

	public Vector2 dir;
	public Vector2 avoidanceForce;

	public Vector2 dirAhead;
	public Vector2 dirAhead2;

	public float LOOKDIST;

	private Vector2 prevPos;
	public Vector2 velocity;

	public List<NODE> path = new List<NODE> ();
	public int pathCount = 0;
	public float updateTime = 0;

	// Use this for initialization
	void Start () {
	
	}

	Vector2 avoidanceCheck()
	{
		RaycastHit2D hit;
		Vector2 ret = Vector2.zero;

		if (hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y)+dirAhead2, dirAhead, LOOKDIST))
		{
			ret = (new Vector2(transform.position.x, transform.position.y)+dir)-((Vector2)hit.collider.bounds.center);
			ret.Normalize();
			ret *= 1.1f;
		}

		return ret;
	}

//	void OnDrawGizmos()
//	{
//		foreach(NODE n in path)
//		{
//			Gizmos.DrawCube(n.pos, Vector3.one*.75f);
//		}
//	}

	void pathing()
	{
		updateTime -= Time.fixedDeltaTime;
		
		if(updateTime < 0)
		{
			updateTime = 1.25f;
			List<NODE> tmp = GetComponent<Grid>().getPathToPos((Vector2)transform.position, (Vector2)GameObject.Find ("poop").transform.position);
			if(tmp != path)
			{
				path = GetComponent<Grid>().getPathToPos((Vector2)transform.position, (Vector2)GameObject.Find ("poop").transform.position);
				pathCount = path.Count-1;
			}
		}
		if (path.Count <= 1) {
			return;
		}
		
		if(Vector2.Distance(path[pathCount].pos, (Vector2)transform.position) < .5f)
		{
			pathCount--;
			if(pathCount < 0)
				pathCount = 0;
		}
		
		dir = (path[pathCount].pos - (Vector2)transform.position); dir.Normalize ();
		
		velocity = GetComponent<Rigidbody2D> ().position - prevPos; velocity.Normalize ();
		float velClamp = Mathf.Clamp (velocity.magnitude, .5f, Mathf.Infinity);
		
		dirAhead2 = dir*velClamp*LOOKDIST;
		dirAhead = (dirAhead2) * LOOKDIST;
		
		GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + ((dir+avoidanceForce) * 3 * Time.fixedDeltaTime));
		Debug.DrawRay (transform.position, dirAhead2);
		Debug.DrawRay ((Vector2)(transform.position)+dirAhead2, dirAhead, Color.blue);
		Debug.DrawRay ((Vector2)(transform.position), avoidanceForce, Color.red);
		
		avoidanceForce = Vector2.Lerp (avoidanceForce, avoidanceCheck (), Time.fixedDeltaTime * 2);
		
		prevPos = GetComponent<Rigidbody2D> ().position;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		pathing ();
	}
}
