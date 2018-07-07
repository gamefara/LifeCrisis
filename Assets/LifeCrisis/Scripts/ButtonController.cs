using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {


	//Click Event
	public void OnClick(){
		string name = gameObject.name;
		if(name == "StartButton"){
			//Stage移行
			SceneManager.LoadScene("Stage1");
		}
		else if(name == "ClearButton"){
			
			string sceneName = SceneManager.GetActiveScene().name;
			if(sceneName == "Stage1") SceneManager.LoadScene("Stage2");
			else if(sceneName == "Stage2") SceneManager.LoadScene("Stage3");
			else if(sceneName == "Stage3") SceneManager.LoadScene("Title");
		}
	}
}
