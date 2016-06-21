using UnityEngine;
using System.Collections;

public class Opening : MonoBehaviour {
    public ArcherController archer;

	bool hasStart = false;
	// Use this for initialization
	void Start () {
		hasStart = false;
	}

	// Update is called once per frame
	void Update () {
		if (!hasStart) {
			if (Input.GetMouseButton (0)) {
				hasStart = true;
				LoadStartLevel ();
			}
		}
	}

    public void LoadStartLevel()
    {
        archer.ShootArrow();
        StartCoroutine(loadStartLevel());
    }

    IEnumerator loadStartLevel()
    {
        yield return new WaitForSeconds(1);

        yield return Application.LoadLevelAsync("level1");


    }
}
