using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	enum JumpType
	{
		NoRotate,
		UpRotate,
		DownRotate
	}

	//Unity-Inspectorから指定
	public GameObject Block;
	public float Speed;
	public float MaxLength;
	public bool PlusDirection;

	//ブロックの移動制御
	Vector3 DefaultPos;
	float DefaultRadius;
	float DefaultRadian;
	Vector3 CenterPos;
	float CenterAngle;
	bool RotateFlag = false;
	int JumpMode = (int)JumpType.NoRotate;
	UnityChanController PlayerScript;
	float BlockTimer = 0.0f;
	float NowRatio = 0.0f;
	float MoveBlockRadius = 0.0f;
	float MoveBlockAngle;

	// Use this for initialization
	void Start()
	{
		DefaultPos = Block.transform.position;
		CenterPos = new Vector3(0.0f, -0.5f, Block.transform.position.z);
		DefaultRadius = Mathf.Pow(Mathf.Pow(DefaultPos.x - CenterPos.x, 2) + Mathf.Pow(DefaultPos.y - CenterPos.y, 2), 0.5f);
		DefaultRadian = Mathf.Atan2(DefaultPos.y - CenterPos.y, DefaultPos.x - CenterPos.x);
		CenterAngle = 0.0f;
		PlayerScript = GameObject.Find("unitychan").GetComponent<UnityChanController>();
		MoveBlockAngle = (PlusDirection ? 0.0f : 180.0f);
	}

	// Update is called once per frame
	void Update()
	{
		if(PlayerScript.GoalFlag) return;
		if(RotateFlag) return;

		bool isUp = Input.GetKey(KeyCode.UpArrow);
		bool isDown = Input.GetKey(KeyCode.DownArrow);
		if(isUp || isDown)
		{
			if(isUp) JumpMode = (int)JumpType.UpRotate;
			else if(isDown) JumpMode = (int)JumpType.DownRotate;
			else JumpMode = (int)JumpType.NoRotate;

			StartCoroutine("RotateBlock");
		}

		var pos = Block.transform.position;

		BlockTimer = (BlockTimer + 0.01f) % (360.0f * Mathf.Deg2Rad);
		NowRatio = Mathf.Sin(Speed * BlockTimer);
		MoveBlockRadius = MaxLength * NowRatio;
		//Time.timeのエイリアス(回転中はカウントしない)
		if(Block.name == "BlockMoveX")
		{
			//中心点そのものの円運動
			var radian = DefaultRadian + CenterAngle * Mathf.Deg2Rad;
			var baseX = CenterPos.x + DefaultRadius * Mathf.Cos(radian);
			var baseY = CenterPos.y + DefaultRadius * Mathf.Sin(radian);

			//中心からの移動距離を半径とした円運動
			radian = (CenterAngle + MoveBlockAngle) * Mathf.Deg2Rad;
			pos.x = baseX + MoveBlockRadius * Mathf.Cos(radian);
			pos.y = baseY + MoveBlockRadius * Mathf.Sin(radian);
		}
		else if(Block.name == "BlockMoveY")
		{
			var radian = DefaultRadian + CenterAngle * Mathf.Deg2Rad;
			var baseX = CenterPos.x + DefaultRadius * Mathf.Cos(radian);
			var baseY = CenterPos.y + DefaultRadius * Mathf.Sin(radian);

			radian = (CenterAngle + MoveBlockAngle) * Mathf.Deg2Rad + Mathf.PI / 2.0f;
			pos.x = baseX + MoveBlockRadius * Mathf.Cos(radian);
			pos.y = baseY + MoveBlockRadius * Mathf.Sin(radian);
		}
		else if(Block.name == "BlockMoveZ") pos.z = CenterPos.z + MoveBlockRadius;
		Block.transform.position = pos;
	}

	IEnumerator RotateBlock()
	{
		RotateFlag = true;
		for(int i = 0; i < 10; i++) yield return null;

		var rotateCount = 30;
		float addAngle = (JumpMode == (int)JumpType.UpRotate ? -1 : 1) * 90.0f / rotateCount;

		var pos = Block.transform.position;
		float radian;
		for(int i = 0; i < rotateCount; i++)
		{
			//オブジェクト回転
			if(JumpMode != (int)JumpType.NoRotate) CenterAngle += addAngle;
			if(CenterAngle < 0.0f) CenterAngle += 360.0f;
			else if(CenterAngle >= 360.0f) CenterAngle %= 360;
			Block.transform.rotation = Quaternion.Euler(0, 0, CenterAngle);

			//オブジェクト座標移動
			//中心点そのものの円移動
			radian = DefaultRadian + CenterAngle * Mathf.Deg2Rad;
			var baseX = CenterPos.x + DefaultRadius * Mathf.Cos(radian);
			var baseY = CenterPos.y + DefaultRadius * Mathf.Sin(radian);

			//中心点からの移動距離を半径とした円運動
			radian = (CenterAngle + MoveBlockAngle) * Mathf.Deg2Rad;
			if(Block.name == "Block" || Block.name == "BlockMoveZ"){
				pos.x = baseX;
				pos.y = baseY;
			}
			else{
				if(Block.name == "BlockMoveY") radian += Mathf.PI / 2.0f;
				pos.x = baseX + MoveBlockRadius * Mathf.Cos(radian);
				pos.y = baseY + MoveBlockRadius * Mathf.Sin(radian);
			}
			Block.transform.position = pos;
			yield return null;
		}

		while(!PlayerScript.StandingFlag) yield return null;
		RotateFlag = false;
	}
}
