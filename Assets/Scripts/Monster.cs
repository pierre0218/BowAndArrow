using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	float movingSpeed = 3;
	Vector3 leftDir = -Vector3.right;

	bool IsDie = false;

	SpriteRenderer sprite;
	// Use this for initialization
	void Start () {
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (IsDie) 
		{
			Color color = sprite.color;
			if(color.a <= 0)
			{
				Destroy (gameObject);
			}
			else
			{
			    color.a -= Time.deltaTime*2;
				sprite.color = color;
			}

			return;
		}

		gameObject.transform.Translate(Time.deltaTime * movingSpeed * leftDir);

		if (transform.position.x < -10) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (IsDie) 
		{
			return;
		}


		if(other.tag == "arrow")
		{
			IsDie = true;
			Destroy(other.gameObject);

			EnemyManager.DeadMonsterCount += 1;
		}
	}
}
