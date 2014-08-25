using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{

	bool b_Paused = false;

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			b_Paused = !b_Paused;
			if(!b_Paused)
				Time.timeScale = 1;
			else
				Time.timeScale = 0;
		}
	}

	void OnGUI()
	{
		if(b_Paused)
		{
			GUIStyle centeredStyle = new GUIStyle();
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.normal.textColor = Color.white;
			GUI.Box(new Rect(Screen.width * 0.5f - 150, Screen.height * 0.5f - 150, 300, 300), "");
			GUI.Label(new Rect(Screen.width * 0.5f - 140, Screen.height * 0.5f - 130, 280, 25), "GAME PAUSED", centeredStyle);
			if(GUI.Button(new Rect(Screen.width * 0.5f - 120, Screen.height * 0.5f - 100, 240, 95), "Resume"))
			{
				b_Paused = false;
				Time.timeScale = 1;
			}
			if(GUI.Button(new Rect(Screen.width * 0.5f - 120, Screen.height * 0.5f + 25, 240, 95), "Quit"))
			{
				Application.Quit();
			}
		}
	}
}
