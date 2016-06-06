using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {
	float speed = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;

		pos += Vector3.right * Time.deltaTime * speed;

		transform.position = pos;

		if (transform.position.x > 10) {
			Destroy(gameObject);
		}
	}
}
