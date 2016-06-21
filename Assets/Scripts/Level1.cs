using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PitchDetector;

public class Level1 : MonoBehaviour {
	public ArchersManager m_ArcherManager;
	public EnemyManager m_EnemyManager;
    public string CurrentLevel;
    public string NextLevel;

	bool IsGameOver = false;
    bool IsWin = false;

	public TextMesh NoteTextUI;
    public TextMesh TitleText;
    private Detector pitchDetector;						//Pitch detector object
	
	private int minFreq, maxFreq; 						//Max and min frequencies window
	public string selectedDevice { get; private set; }	//Mic selected
	private bool micSelected = false;					//Mic flag
	
	float[] data;										//Sound samples data
	public int cumulativeDetections= 5; 				//Number of consecutive detections used to determine current note
	int [] detectionsMade;								//Detections buffer
	private int maxDetectionsAllowed=50;				//Max buffer size
	private int detectionPointer=0;						//Current buffer pointer
	public int pitchTimeInterval=100; 					//Millisecons needed to detect tone
	private float refValue = 0.1f; 						// RMS value for 0 dB
	public float minVolumeDB=-17f;						//Min volume in bd needed to start detection
	
	private int currentDetectedNote =0;					//Las note detected (midi number)
	private string currentDetectedNoteName;				//Note name in modern notation (C=Do, D=Re, etc..)

	float triggerFrames = 1;
	float UI_x = -8.95f;

    bool hasStartGame = false;

	void Awake() {
		pitchDetector = new Detector();
		pitchDetector.setSampleRate(AudioSettings.outputSampleRate);
	}

	float[] noteTimer = new float[7]; 

	//Start function for web player (also works on other platforms)
	IEnumerator Start()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
		if (Application.HasUserAuthorization(UserAuthorization.Microphone))
		{
			selectedDevice = Microphone.devices[0].ToString();
			micSelected = true;
			GetMicCaps();
			
			//Estimates bufer len, based on pitchTimeInterval value
			int bufferLen = (int)Mathf.Round(AudioSettings.outputSampleRate * pitchTimeInterval / 1000f);
			Debug.Log("Buffer len: " + bufferLen);
			data = new float[bufferLen];
			
			detectionsMade = new int[maxDetectionsAllowed]; //Allocates detection buffer
			
			setUptMic();
		}
		else
		{
		}

        hasStartGame = false;
        TitleText.text = CurrentLevel;
        m_ArcherManager.InitArchers ();
	}
	void Update () {
		if (selectedDevice == null)
			return;

        if (!hasStartGame)
        {
            if(Input.GetMouseButtonDown(0))
            {
                hasStartGame = true;
                TitleText.gameObject.SetActive(false);

				EnemyManager.DeadMonsterCount = 0;
                m_EnemyManager.Spawn();
            }
            return;
        }
        if (CurrentLevel == "level1")
        {
            if (!IsWin)
            {
                if (BalloonManager.BrokenBalloonCount > 9)
                {
                    IsWin = true;
                    StartCoroutine(loadNextLevel());
                }
            }
        }
        else if (CurrentLevel == "level2")
        {
            if (!IsWin)
            {
				if (EnemyManager.DeadMonsterCount >= 24)
                {
                    IsWin = true;
                    StartCoroutine(loadNextLevel());
                }
            }
        }
		else if (CurrentLevel == "level3")
		{
			if (!IsWin)
			{
				if (EnemyManager.DeadMonsterCount >= 40)
				{
					IsWin = true;
					StartCoroutine(loadNextLevel());
				}
			}
		}

        if (IsWin)
            return;

		GetComponent<AudioSource>().GetOutputData(data,0);
		float sum = 0f;
		for(int i=0; i<data.Length; i++)
			sum += data[i]*data[i];
		float rmsValue = Mathf.Sqrt(sum/data.Length);
		float dbValue = 20f*Mathf.Log10(rmsValue/refValue);

		string noteText = null;
		if(dbValue<minVolumeDB) {
			//Sound too low
			noteText = null;
			NoteTextUI.text = "";
			return;
		}

		//PITCH DTECTED!!
		pitchDetector.DetectPitch (data);
		int midiant = pitchDetector.lastMidiNote ();
		int midi = findMode ();
		noteText = pitchDetector.midiNoteToString(midi);
		detectionsMade [detectionPointer++] = midiant;
		detectionPointer %= cumulativeDetections;

		if (noteText == null) {
			noteTimer [0] = 0;
			noteTimer [1] = 0;
			noteTimer [2] = 0;
			noteTimer [3] = 0;
			noteTimer [4] = 0;
			noteTimer [5] = 0;
			noteTimer [6] = 0;
		} else if (noteText == "C 3") {
			noteTimer [0] += 1;
			noteTimer [1] = 0;
			noteTimer [2] = 0;
			noteTimer [3] = 0;
			noteTimer [4] = 0;
			noteTimer [5] = 0;
			noteTimer [6] = 0;

			if (noteTimer [0] > triggerFrames) {
				m_ArcherManager.Archers [6].ShootArrow ();

				Vector3 pos = m_ArcherManager.Archers [6].transform.position;
				pos.x = UI_x;
				NoteTextUI.transform.position = pos;
			}
		} else if (noteText == "D 3") {
			noteTimer [0] = 0;
			noteTimer [1] += 1;
			noteTimer [2] = 0;
			noteTimer [3] = 0;
			noteTimer [4] = 0;
			noteTimer [5] = 0;
			noteTimer [6] = 0;

			if (noteTimer [1] > triggerFrames) {
				m_ArcherManager.Archers [5].ShootArrow ();

				Vector3 pos = m_ArcherManager.Archers [5].transform.position;
				pos.x = UI_x;
				NoteTextUI.transform.position = pos;
			}
		} else if (noteText == "E 3") {
			noteTimer [0] = 0;
			noteTimer [1] = 0;
			noteTimer [2] += 1;
			noteTimer [3] = 0;
			noteTimer [4] = 0;
			noteTimer [5] = 0;
			noteTimer [6] = 0;

			if (noteTimer [2] > triggerFrames) {
				m_ArcherManager.Archers [4].ShootArrow ();

				Vector3 pos = m_ArcherManager.Archers [4].transform.position;
				pos.x = UI_x;
				NoteTextUI.transform.position = pos;
			}
		} else if (noteText == "F 3") {
			noteTimer [0] = 0;
			noteTimer [1] = 0;
			noteTimer [2] = 0;
			noteTimer [3] += 1;
			noteTimer [4] = 0;
			noteTimer [5] = 0;
			noteTimer [6] = 0;

			if (noteTimer [3] > triggerFrames) {
				m_ArcherManager.Archers [3].ShootArrow ();

				Vector3 pos = m_ArcherManager.Archers [3].transform.position;
				pos.x = UI_x;
				NoteTextUI.transform.position = pos;
			}
		} else if (noteText == "G 3") {
			noteTimer [0] = 0;
			noteTimer [1] = 0;
			noteTimer [2] = 0;
			noteTimer [3] = 0;
			noteTimer [4] += 1;
			noteTimer [5] = 0;
			noteTimer [6] = 0;

			if (noteTimer [4] > triggerFrames) {
				m_ArcherManager.Archers [2].ShootArrow ();

				Vector3 pos = m_ArcherManager.Archers [2].transform.position;
				pos.x = UI_x;
				NoteTextUI.transform.position = pos;
			}
		} else if (noteText == "A 3") {
			noteTimer [0] = 0;
			noteTimer [1] = 0;
			noteTimer [2] = 0;
			noteTimer [3] = 0;
			noteTimer [4] = 0;
			noteTimer [5] += 1;
			noteTimer [6] = 0;

			if (noteTimer [5] > triggerFrames) {
				m_ArcherManager.Archers [1].ShootArrow ();

				Vector3 pos = m_ArcherManager.Archers [1].transform.position;
				pos.x = UI_x;
				NoteTextUI.transform.position = pos;
			}
		} else if (noteText == "B 3") {
			noteTimer [0] = 0;
			noteTimer [1] = 0;
			noteTimer [2] = 0;
			noteTimer [3] = 0;
			noteTimer [4] = 0;
			noteTimer [5] = 0;
			noteTimer [6] += 1;

			if (noteTimer [6] > triggerFrames) {
				m_ArcherManager.Archers [0].ShootArrow ();

				Vector3 pos = m_ArcherManager.Archers [0].transform.position;
				pos.x = UI_x;
				NoteTextUI.transform.position = pos;
			}
		} else {
			noteTimer [0] = 0;
			noteTimer [1] = 0;
			noteTimer [2] = 0;
			noteTimer [3] = 0;
			noteTimer [4] = 0;
			noteTimer [5] = 0;
			noteTimer [6] = 0;
		}

		NoteTextUI.text = noteText;
		//Debug.Log (noteText);
	}
	
	void setUptMic() {
		//GetComponent<AudioSource>().volume = 0f;
		GetComponent<AudioSource>().clip = null;
		GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
		GetComponent<AudioSource>().mute = false; // Mute the sound, we don't want the player to hear it
		StartMicrophone();
	}
	
	public void GetMicCaps () {
		Microphone.GetDeviceCaps(selectedDevice, out minFreq, out maxFreq);//Gets the frequency of the device
		if ((minFreq + maxFreq) == 0)
			maxFreq = 44100;
	}
	
	public void StartMicrophone () {
		Debug.Log("Setting");
		GetComponent<AudioSource>().clip = Microphone.Start(selectedDevice, true, 10, maxFreq);//Starts recording
		while (!(Microphone.GetPosition(selectedDevice) > 0)){} // Wait until the recording has started
		GetComponent<AudioSource>().Play(); // Play the audio source!
		Debug.Log("End Setting");
	}
	
	public void StopMicrophone () {
		GetComponent<AudioSource>().Stop();//Stops the audio
		Microphone.End(selectedDevice);//Stops the recording of the device	
	}
	
	int repetitions(int element) {
		int rep = 0;
		int tester=detectionsMade [element];
		for (int i=0; i<cumulativeDetections; i++) {
			if(detectionsMade [i]==tester)
				rep++;
		}
		return rep;
	}
	
	public int findMode() {
		cumulativeDetections = (cumulativeDetections >= maxDetectionsAllowed) ? maxDetectionsAllowed : cumulativeDetections;
		int moda = 0;
		int veces = 0;
		for (int i=0; i<cumulativeDetections; i++) {
			if(repetitions(i)>veces)
				moda=detectionsMade [i];
		}
		return moda;
	}

    IEnumerator loadNextLevel()
    {
        yield return new WaitForSeconds(1);

        yield return Application.LoadLevelAsync(NextLevel);


    }

	IEnumerator loadCurrentLevel()
	{
		yield return Application.LoadLevelAsync(CurrentLevel);
	}

    void OnTriggerEnter2D(Collider2D other)
	{
		if (IsGameOver) 
		{
			return;
		}
		
		
		if(other.tag == "monster")
		{
			IsGameOver = true;
			Debug.Log("game over!!");

			StartCoroutine (loadCurrentLevel());
		}
	}
}
