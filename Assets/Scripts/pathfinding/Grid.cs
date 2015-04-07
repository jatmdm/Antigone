using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public List<NODE> nodes = new List<NODE>();
	private Vector3 lastPos;

	// Use this for initialization
	void Start () 
	{
		makeGrid ();
	}

	void makeGrid()
	{
		Vector2 pos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y))-new Vector2(8, -8);
		Vector2 start = pos;
		
		for (int i=0; i < (16*16)-1; i++) 
		{
			if(nodes.Find(x => x.pos.Equals(pos)) == null)
			{
				nodes.Add (new NODE (pos, true, (int)i));
			}
			
			pos.x += 1f;
			if (pos.x > start.x+16) 
			{
				pos.x = start.x;
				pos.y -= 1f;
			}
		}
		
		foreach(NODE n in nodes)
		{
			Collider2D hit;
			
			if(hit = Physics2D.OverlapCircle(n.pos, .2f))
			{
				if(hit.tag == "wall")
					n.walkable = false;
			}
		}
	}

	void Update()
	{
		if(!transform.position.Equals(lastPos))
		{
			//nodes.Clear();
			makeGrid();
		}
		lastPos = transform.position;
	}

	void OnDrawGizmos()
	{
		foreach(NODE n in nodes)
		{
			if(n.walkable)
				Gizmos.DrawCube(n.pos, Vector3.one*.75f);
		}
	}

	public List<NODE> getPath(int searchx, int searchy)
	{
		List<NODE> tmp = AStar(nodes[searchx], nodes[searchy]);
		return tracePath(tmp.Find(NODE => NODE == nodes[searchy]));
	}

	public List<NODE> getPathToPos(Vector2 pos, Vector2 to)
	{
		NODE start = nodes [0];

		foreach(NODE n in nodes)
		{
			if(n.walkable && (Vector2.Distance(pos, n.pos) <= Vector2.Distance(pos, start.pos)))
			{
				start = n;
			}
		}

		NODE end = nodes [0];
		
		foreach(NODE n in nodes)
		{
			if(n.walkable && (Vector2.Distance(to, n.pos) <= Vector2.Distance(to, end.pos)))
			{
				end = n;
			}
		}

		return getPath (isNum (start), isNum (end));
	}
	
	float manhattan(float dx, float dy) 
	{
		return dx + dy;
	}
	bool isAdjascent(NODE n1, NODE n2)
	{
		if (Mathf.Abs (Vector2.Distance (n1.pos, n2.pos)) == 1) 
		{
			return true;
		}
		else
			return false;
	}
	
	int isNum(NODE node)
	{
		for(int i=0; i < nodes.Count; i++)
		{
			if(node.pos.Equals(nodes[i].pos))
				return i;
		}
		return -256;
	}
	
	List<NODE> tracePath(NODE last)
	{
		List<NODE> ret = new List<NODE>();
		
		ret.Add (last);
		if (last == null || last.parent == null)
			return ret;
		
		do
		{
			last = last.parent;
			ret.Add (last);
		} while(last.parent != null);
		
		return ret;
	}
	
	
	
	List<NODE> AStar(NODE start, NODE end)
	{
		foreach (NODE n in nodes) 
		{
			n.parent = null;
			n.end = end;
			n.ManhattanF();
		}
		
		List<NODE> open = new List<NODE>();
		List<NODE> closed = new List<NODE>();
		NODE currentSquare = start;
		currentSquare.f = manhattan(Mathf.Abs(currentSquare.pos.x-end.pos.x), Mathf.Abs(currentSquare.pos.y-end.pos.y));
		
		open.Add (start);
		
		List<NODE> final = new List<NODE>();
		
		if (start.pos.Equals(end.pos))
		{
			final.Add(start);
			return final;
		}
		
		
		while(open.Count > 0 || currentSquare == end)
		{
			NODE lowest = currentSquare;
			open.Sort();
			
			if(lowest == end || open.Count == 0)
				return final;
			
			lowest = open[0];
			currentSquare = lowest;
			
			open.Remove (currentSquare);
			closed.Add (currentSquare);
			
			foreach (NODE n in nodes) 
			{
				if(isAdjascent(n, currentSquare) && n.walkable)
				{
					float g_score =  n.g + 1;
					float h_score = manhattan(Mathf.Abs(n.pos.x-end.pos.x), Mathf.Abs(n.pos.y-end.pos.y));
					float f_score = g_score + h_score;
					
					if(closed.Contains(n) && f_score >= n.f)
						continue;
					
					if(!open.Contains(n) || f_score < n.f)
					{
						n.parent = currentSquare;
						n.g = f_score;
						
						if(!open.Contains(n))
						{
							open.Add(n);
							final.Add(n);
						}
					}
				}
			}
		}
		
		return final;
	}
}
