using UnityEngine;
using System.Collections;

public class Adventurer : MonoBehaviour {

	public Vector2 vel;
	public float speed;
	public GameObject sword;
	public GameObject swordBlade;
	public float sBounce = 0;

	public float rollTime = 0;
	public float velGrav = 8;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		rollTime -= Time.fixedDeltaTime;

		Rigidbody2D rigid = GetComponent<Rigidbody2D> ();

		vel = Vector2.Lerp (vel, Vector2.zero, Time.fixedDeltaTime * velGrav);
		sBounce = Mathf.Lerp (sBounce, 0, Time.fixedDeltaTime * 8);

		if (rollTime < 0) {
			speed = Mathf.Lerp (speed, 5, Time.fixedDeltaTime * 15);
			velGrav = Mathf.Lerp (velGrav, 8, Time.fixedDeltaTime * 15);
		} else {
			speed = Mathf.Lerp (speed, 1, Time.fixedDeltaTime * 15);
			vel *= 1.012f;
			velGrav = Mathf.Lerp (speed, 2, Time.fixedDeltaTime * 15);
		}

		if(Input.GetKey(KeyCode.W))
		{
			vel.y = Mathf.Lerp(vel.y, speed, Time.fixedDeltaTime*7);
		}
		if(Input.GetKey(KeyCode.S))
		{
			vel.y = Mathf.Lerp(vel.y, -speed, Time.fixedDeltaTime*7);
		}
		if(Input.GetKey(KeyCode.A))
		{
			vel.x = Mathf.Lerp(vel.x, -speed, Time.fixedDeltaTime*7);
		}
		if(Input.GetKey(KeyCode.D))
		{
			vel.x = Mathf.Lerp(vel.x, speed, Time.fixedDeltaTime*7);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			sBounce = Random.Range(-180, 180);
		}
		if(Input.GetKeyDown(KeyCode.LeftShift) && rollTime < 0)
		{
			rollTime = .26f;
		}

		if (Input.GetMouseButton (0)) 
		{
			if(!swordBlade.activeSelf) sBounce = 360;
			swordBlade.SetActive(true);
		}
		else
		{
			swordBlade.SetActive(false);
		}

		rigid.MovePosition (rigid.position + vel * Time.fixedDeltaTime);

		float cameraDif = Camera.main.transform.position.y - transform.position.y;
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		Vector2 mWorldPos = Camera.main.ScreenToWorldPoint( new Vector3(mouseX, mouseY, cameraDif));
		Vector2 mainPos = transform.position;
		
		float diffX = mWorldPos.x - mainPos.x;
		float diffY = mWorldPos.y - mainPos.y;
		sword.GetComponent<Rigidbody2D> ().MovePosition (this.transform.position); 
		sword.transform.rotation = Quaternion.Lerp(sword.transform.rotation, Quaternion.Euler(0, 0, Mathf.Rad2Deg*Mathf.Atan2 (diffY, diffX)-90+sBounce), Time.fixedDeltaTime*10);
	}
}
