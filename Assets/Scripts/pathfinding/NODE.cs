using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class NODE : IComparable<NODE>  {
	
	public Vector2 pos;
	public bool walkable;
	public int id;
	public NODE parent;
	public NODE end;

	public float f;
	public float g;

	public NODE(Vector2 p, bool walk, int i)
	{
		pos = p;

		walkable = walk;
		g = 1; f = 0; id = i;
	}

	private float manhattan(float dx, float dy) 
	{
		return dx + dy;
	}
	public void ManhattanF() 
	{
		f = g+manhattan (Mathf.Abs (this.pos.x - end.pos.x), Mathf.Abs (this.pos.y - end.pos.y));
	}

	public int CompareTo(NODE obj)
	{
		//f = g+manhattan (Mathf.Abs (this.transform.position.x - end.transform.position.x), Mathf.Abs (this.transform.position.y - end.transform.position.y));
		if ((g+manhattan (Mathf.Abs (this.pos.x - end.pos.x), Mathf.Abs (this.pos.y - end.pos.y))) < (g+manhattan (Mathf.Abs (obj.pos.x - end.pos.x), Mathf.Abs (obj.pos.y - end.pos.y)))) {
						return -1;
				} else
						return 1;
	}
}
