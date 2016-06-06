using UnityEngine;
using System.Collections;

public class ArcherController : MonoBehaviour {
	Animator mAnimator;
	bool isPlayingShoot = false;
	// Use this for initialization
	void Start () {
		mAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			ShootArrow();
		}
	
	}

	public void ShootArrow()
	{
		mAnimator.SetTrigger ("shootTrigger");

	}

	public void CastArrow()
	{
		GameObject arrowGo = Instantiate (Resources.Load("arrow")) as GameObject;

		Vector3 pos = transform.position;
		pos.x += 0f;
		pos.y += 0.12f;
		arrowGo.transform.position = pos;
	}
}
