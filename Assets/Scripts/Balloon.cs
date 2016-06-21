using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

	public GameObject balloon1;
	public GameObject balloon2;
	float movingSpeed = 3;
	bool IsDie = false;

	Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
		balloon1.SetActive (true);
		balloon2.SetActive (false);

		rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (IsDie) {
			if (transform.position.y < -6) {
				Destroy (gameObject);
			}
		}
		else 
		{
			gameObject.transform.Translate(Time.deltaTime * movingSpeed * Vector3.up);
			if(transform.position.y > 8)
			{
				Vector3 pos = transform.position;
				pos.y = -6;
				transform.position = pos;
			}
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
			rigidbody.isKinematic = false;
			balloon1.SetActive (false);
			balloon2.SetActive (true);

            BalloonManager.BrokenBalloonCount += 1;
		}
	}
}
