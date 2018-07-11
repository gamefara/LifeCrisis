using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
	[System.NonSerialized] public string SelectableStage;
	public Button Stage1Button;
	public Button Stage2Button;
	public Button Stage3Button;

	// Use this for initialization
	void Start()
	{
		if(PlayerPrefs.HasKey("Select")) SelectableStage = PlayerPrefs.GetString("Select");
		else SelectableStage = "Stage1";

		//ステージ選択ボタンの表示/非表示
		string sceneName = SceneManager.GetActiveScene().name;
		if(sceneName != "Title") return;

		if(SelectableStage == "Stage3"){
			Stage2Button.image.fillCenter = true;
			Stage3Button.image.fillCenter = true;
		}
		else if(SelectableStage == "Stage2"){
			Stage2Button.image.fillCenter = true;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
