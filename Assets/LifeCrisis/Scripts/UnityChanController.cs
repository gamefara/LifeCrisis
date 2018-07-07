using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
	//Unity-Inspectorから指定
	public Camera CameraObject;
	public bool StandingFlag = true;
	public float Speed = 2.0f;
	public float JumpPower = 60.0f;
	public Button ClearButton;

	Rigidbody Body;
	Animator AnimeAction;
	const string ParameterRun = "isRun";
	const string ParameterJump = "isJump";
	Vector3 RestartPosition;
	bool LastPushKeyLeft = false;
	const int MaxJumpCoolTime = 5;
	int NowJumpCoolTime = 0;
	const int MaxCollisionCount = 120;
	int NowCollisionCount = 0;
	[SerializeField] public bool GoalFlag = false;

	// Use this for initialization
	void Start()
	{
		Body = GetComponent<Rigidbody>();
		AnimeAction = GetComponent<Animator>();
		RestartPosition = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		CheckUpdatePlayer();
		CheckRestartPlayer();
		CameraUpdatePosition();

		if(GoalFlag) return;
		if(!StandingFlag){
			NowJumpCoolTime = 0;
			return;
		}
		else if(NowJumpCoolTime < MaxJumpCoolTime) NowJumpCoolTime++;

		bool leftMoveFlag = Input.GetKey(KeyCode.LeftArrow);
		bool rightMoveFlag = Input.GetKey(KeyCode.RightArrow);
		bool upRotateFlag = Input.GetKey(KeyCode.UpArrow);
		bool downRotateFlag = Input.GetKey(KeyCode.DownArrow);
		bool runFlag = (leftMoveFlag || rightMoveFlag);
		bool RotateFlag = (upRotateFlag || downRotateFlag);
		if(leftMoveFlag) LastPushKeyLeft = true;
		else if(rightMoveFlag) LastPushKeyLeft = false;

		//左右移動
		if(runFlag)
		{
			PlayerUpdateVelocity(leftMoveFlag);
			transform.rotation = Quaternion.Euler(0, (leftMoveFlag ? 180 : 0), 0);

			if(AnimeAction.GetBool(ParameterJump)) return;
			AnimeAction.SetBool(ParameterRun, true);
		}
		else AnimeAction.SetBool(ParameterRun, false);

		//ジャンプ
		if(RotateFlag)
		{
			if(AnimeAction.GetBool(ParameterJump) || NowJumpCoolTime < MaxJumpCoolTime) return;
			StandingFlag = false;
			Body.AddForce(new Vector3(0, JumpPower, 0), ForceMode.Impulse);
		}
	}

	void CameraUpdatePosition(){
		var c = CameraObject.transform.position;
		c.z = this.transform.position.z;
		CameraObject.transform.position = c;
	}

	void CheckUpdatePlayer(){
		if(LastPushKeyLeft) this.transform.rotation = Quaternion.Euler(0, 180, 0);
		else this.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	void CheckRestartPlayer(){
		if(transform.position.y <= -30.0f) RestartPlayer();
	}

	void RestartPlayer(){ transform.position = RestartPosition; }

	void PlayerUpdateVelocity(bool leftMoveFlag){
		var v = Body.velocity;
		v.z = Speed * (leftMoveFlag ? -1 : 1);
		Body.velocity = v;
	}

	void OnCollisionEnter(Collision col)
	{
		string tag = col.gameObject.tag;
		if(tag == "Block")
		{
			StandingFlag = true;
			AnimeAction.SetBool(ParameterJump, false);
		}
		else if(tag == "Goal") StartCoroutine("StageClearProcess");
		else if(tag == "DamageObject"){
			//落ちやすくなるよう2倍の反発力に変更
			var v = Body.velocity;
			v.x -= 2;
			v.y -= 2;
			v.z -= 2;
			Body.velocity = v;
		}
	}

	void OnCollisionExit(Collision col)
	{
		string name = col.gameObject.name;
		if(!StandingFlag) {
			AnimeAction.SetBool(ParameterJump, true);
			AnimeAction.SetBool(ParameterRun, false);
		}
		NowCollisionCount = 0;
	}

	private void OnCollisionStay(Collision col)
	{
		string tag = col.gameObject.tag;
		if(tag == "DamageObject")
		{
			if(NowCollisionCount < MaxCollisionCount) NowCollisionCount++;
			else RestartPlayer();
		}
		else if(tag == "Goal")
		{
			GoalFlag = true;
			AnimeAction.SetBool(ParameterRun, false);
			AnimeAction.SetBool(ParameterJump, false);
		}
	}

	IEnumerator StageClearProcess()
	{
		int count = 60;
		//クリアUI表示
		for(int i = 0; i < count; i++)
		{
			yield return null;
		}

		//ゴールについた後何らかの原因で落ちた場合何もしない
		if(!GoalFlag) yield break;

		ClearButton.gameObject.SetActive(true);
	}

}
