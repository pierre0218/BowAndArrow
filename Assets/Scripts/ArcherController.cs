using UnityEngine;
using System.Collections;

public class ArcherController : MonoBehaviour {

	Animator mAnimator;
	bool isPlayingShoot = false;

	float cooldown = 0;
	// Use this for initialization
	void Start () {
		mAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
			if (cooldown < 0) {
				cooldown = 0;
			}
		}

	}

	public void ShootArrow()
	{
		if (cooldown == 0) {
			//mAnimator.SetTrigger ("shootTrigger");
			GameObject arrowGo = Instantiate (Resources.Load("arrow")) as GameObject;

			Vector3 pos = transform.position;
			pos.x += 0f;
			pos.y += 0.12f;
			arrowGo.transform.position = pos;

			cooldown = 0.5f;
		}
	}

	public void CastArrow()
	{
		/*GameObject arrowGo = Instantiate (Resources.Load("arrow")) as GameObject;

		Vector3 pos = transform.position;
		pos.x += 0f;
		pos.y += 0.12f;
		arrowGo.transform.position = pos;*/
	}
}
