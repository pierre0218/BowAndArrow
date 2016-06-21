using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArchersManager : MonoBehaviour {
	float PosX = -7.43f;
	float TopY = 5.72f;
	float Gap = 1.6f;

	public List<ArcherController> Archers = new List<ArcherController>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitArchers()
	{
		float posY = TopY;
		for (int i=0; i<7; i++) 
		{
			GameObject go = Instantiate(Resources.Load("archer")) as GameObject;
			go.transform.position = new Vector3(PosX,posY,0);

			posY -= Gap;
			ArcherController archer = go.GetComponent<ArcherController>();
			Archers.Add(archer);
		}
	}
}
