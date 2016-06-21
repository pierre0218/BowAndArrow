using UnityEngine;
using System.Collections;

public class MonsterManager : EnemyManager {

	public enum SpawnMode
	{
		RandomNote,
		Melody
	}

	float waitTime = 3; //seconds
	float timer = 0;
	float[] positions_y = {5.72f, 4.12f, 2.52f, 0.92f, -0.68f, -2.28f, -3.88f};

	public string prefabName;
	public SpawnMode Mode;

	int noteIndex = 0;
	int[] song_notes;
	float[] song_beats;

	bool stop = false;
    bool startSpawn = false;

    int monsterCount = 0;

    public override void Spawn()
    {
        startSpawn = true;
    }
    // Use this for initialization
    void Start () {
		timer = 0;
		song_notes = MelodyDefine.Song1_notes;
		song_beats = MelodyDefine.Song1_beats;
	}
	
	// Update is called once per frame
	void Update () {
		if (stop)
			return;
        if (!startSpawn)
            return;
		if (timer > waitTime) {
			if (Mode == SpawnMode.Melody) {

				if (noteIndex < song_notes.Length) {
					GameObject monsterGo = Instantiate (Resources.Load (prefabName)) as GameObject;
					int note = song_notes [noteIndex];
					float y = positions_y [7-note];
					Vector3 pos = new Vector3 (10, y, 0);
				
					monsterGo.transform.position = pos;
				
					timer = 0;
					waitTime = song_beats [noteIndex];
					noteIndex++;
				} else {
					Stop ();
                }

			} else {

                if (monsterCount < 40)
                {
                    GameObject monsterGo = Instantiate(Resources.Load(prefabName)) as GameObject;
                    int rand = Random.Range(0, 7);
                    float y = positions_y[rand];
                    Vector3 pos = new Vector3(10, y, 0);

                    monsterGo.transform.position = pos;

                    monsterCount++;

                    timer = 0;
                    waitTime = Random.Range(1, 3);
                }
                else
                {
                    Stop();
                }

			}
		}

		timer += Time.deltaTime;
	}

	public void Stop()
	{
		stop = true;
	}
}
