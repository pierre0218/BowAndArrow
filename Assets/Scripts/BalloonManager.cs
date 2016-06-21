using UnityEngine;
using System.Collections;

public class BalloonManager : EnemyManager {

    public static int BrokenBalloonCount = 0;

	float gap = 0.7f;
	// Use this for initialization
	void Start () {
	
	}

    public override void Spawn()
    {
        BrokenBalloonCount = 0;

        for (int i = 0; i < 10; i++)
        {
            GameObject balloonGo = Instantiate(Resources.Load("balloon")) as GameObject;
            float x = 0 + i * gap;
            Vector3 pos = new Vector3(x, -6, 0);

            balloonGo.transform.position = pos;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
