using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public Sprite NoUsedSprite;
	public Sprite UsedSprite;

	static UnityChanController PlayerScript;
	static DataController DataScript;
	Image CheckImage;
	static string SceneName;
	string ButtonName;
	static string StageName = "Stage1";

	void Start()
	{
		SceneName = SceneManager.GetActiveScene().name;
		ButtonName = gameObject.name;
		if(SceneName != "Title") PlayerScript = GameObject.Find("unitychan").GetComponent<UnityChanController>();
		DataScript = GameObject.Find("DataObject").GetComponent<DataController>();
		CheckImage = GetComponent<Image>();
	}

	void Update()
	{
		if(SceneName == "Title") return;
		bool upFlag = PlayerScript.PushButtonUp;
		bool downFlag = PlayerScript.PushButtonDown;
		bool leftFlag = PlayerScript.PushButtonLeft;
		bool rightFlag = PlayerScript.PushButtonRight;
		bool stopFlag = PlayerScript.PushButtonStop;

		if(ButtonName == "JumpUpButton") CheckImage.sprite = (upFlag ? UsedSprite : NoUsedSprite);
		else if(ButtonName == "JumpDownButton") CheckImage.sprite = (downFlag ? UsedSprite : NoUsedSprite);
		else if(ButtonName == "LeftButton") CheckImage.sprite = (leftFlag ? UsedSprite : NoUsedSprite);
		else if(ButtonName == "RightButton") CheckImage.sprite = (rightFlag ? UsedSprite : NoUsedSprite);
		else if(ButtonName == "StopButton") CheckImage.sprite = (stopFlag ? UsedSprite : NoUsedSprite);
	}

	//Click Event
	public void OnClick()
	{
		string name = gameObject.name;
		if(ButtonName == "Stage1Button")
		{
			StageName = "Stage1";
			DataScript.Stage1Button.image.color = new Color(0, 1, 1);
			DataScript.Stage2Button.image.color = new Color(1, 1, 1);
			DataScript.Stage3Button.image.color = new Color(1, 1, 1);
		}
		else if(ButtonName == "Stage2Button" && DataScript.Stage2Button.image.fillCenter)
		{
			StageName = "Stage2";
			DataScript.Stage1Button.image.color = new Color(1, 1, 1);
			DataScript.Stage2Button.image.color = new Color(0, 1, 1);
			DataScript.Stage3Button.image.color = new Color(1, 1, 1);
		}
		else if(ButtonName == "Stage3Button" && DataScript.Stage3Button.image.fillCenter)
		{
			StageName = "Stage3";
			DataScript.Stage1Button.image.color = new Color(1, 1, 1);
			DataScript.Stage2Button.image.color = new Color(1, 1, 1);
			DataScript.Stage3Button.image.color = new Color(0, 1, 1);
		}
		else if(ButtonName == "StartButton") SceneManager.LoadScene(StageName);
		else if(ButtonName == "ClearButton")
		{
			if(SceneName == "Stage1")
			{
				SceneManager.LoadScene("Stage2");
				PlayerPrefs.SetString("Select", "Stage2");
			}
			else if(SceneName == "Stage2")
			{
				SceneManager.LoadScene("Stage3");
				PlayerPrefs.SetString("Select", "Stage3");
			}
		}
		else if(ButtonName == "TitleButton")
		{
			SceneManager.LoadScene("Title");
			DataScript.SelectableStage = PlayerPrefs.GetString("Select");
		}
		else if(ButtonName == "JumpUpButton")
		{
			PlayerScript.PushButtonUp = true;
			PlayerScript.PushButtonDown = false;
		}
		else if(ButtonName == "JumpDownButton")
		{
			PlayerScript.PushButtonUp = false;
			PlayerScript.PushButtonDown = true;
		}
		else if(ButtonName == "LeftButton")
		{
			PlayerScript.PushButtonUp = false;
			PlayerScript.PushButtonDown = false;
			PlayerScript.PushButtonLeft = true;
			PlayerScript.PushButtonRight = false;
			PlayerScript.PushButtonStop = false;
		}
		else if(ButtonName == "RightButton")
		{
			PlayerScript.PushButtonUp = false;
			PlayerScript.PushButtonDown = false;
			PlayerScript.PushButtonLeft = false;
			PlayerScript.PushButtonRight = true;
			PlayerScript.PushButtonStop = false;
		}
		else if(ButtonName == "StopButton")
		{
			PlayerScript.PushButtonLeft = false;
			PlayerScript.PushButtonRight = false;
			PlayerScript.PushButtonStop = true;
		}
	}
}
