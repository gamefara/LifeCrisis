using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

	//Unity-Inspectorから指定
	public GameObject block;
	public float Speed;

	//ブロックの移動制御
	private Vector3 defaultPos;
	private float defaultRadius;
	private Vector3 centerPos;
	private float centerAngle = 0.0f;
	private bool isRotate = false;
	private int jumpProcess = (int)JumpType.NoRotate;
	private GameObject player;
	private float blockTimer = 0.0f;

	enum JumpType
	{
		NoRotate,
		UpRotate,
		DownRotate
	}

	// Use this for initialization
	void Start()
	{
		defaultPos = block.transform.position;
		centerPos = new Vector3(0.0f, -0.5f, block.transform.position.z);
		defaultRadius = Mathf.Pow(Mathf.Pow(defaultPos.x - centerPos.x, 2) + Mathf.Pow(defaultPos.y - centerPos.y, 2), 0.5f);
		player = GameObject.Find("unitychan");
	}

	// Update is called once per frame
	void Update()
	{
		if(isRotate) return;

		bool isUp = Input.GetKey(KeyCode.UpArrow);
		bool isDown = Input.GetKey(KeyCode.DownArrow);
		if(isUp || isDown)
		{
			if(isUp) jumpProcess = (int)JumpType.UpRotate;
			else if(isDown) jumpProcess = (int)JumpType.DownRotate;
			else jumpProcess = (int)JumpType.NoRotate;

			StartCoroutine("RotateBlock");
		}

		var pos = block.transform.position;
		//Time.timeのエイリアス(回転中はカウントしない)
		blockTimer = (blockTimer + 0.01f) % (360.0f * Mathf.Deg2Rad);
		if(block.name == "BlockMoveX")
		{
			//回転後移動方向も変わる
		}
		else if(block.name == "BlockMoveY")
		{

		}
		else if(block.name == "BlockMoveZ") pos.z = centerPos.z + 4 * Mathf.Sin(Speed * blockTimer);
		block.transform.position = pos;
	}

	IEnumerator RotateBlock()
	{
		isRotate = true;
		for(int i = 0; i < 10; i++) yield return null;

		var rotateCount = 30;
		float addAngle = (jumpProcess == (int)JumpType.UpRotate ? -1 : 1) * 90.0f / rotateCount;

		var pos = block.transform.position;
		var squareX = Mathf.Pow(pos.x - centerPos.x, 2);
		var squareY = Mathf.Pow(pos.y - centerPos.y, 2);
		var radius = Mathf.Pow(squareX + squareY, 0.5f);
		float radian;
		var defaultRadian = Mathf.PI / 2.0f;
		for(int i = 0; i < rotateCount; i++)
		{
			//オブジェクト回転
			if(jumpProcess != (int)JumpType.NoRotate) centerAngle += addAngle;
			block.transform.rotation = Quaternion.Euler(0, 0, centerAngle);

			//オブジェクト座標移動
			radian = centerAngle * Mathf.Deg2Rad + defaultRadian;
			pos.x = centerPos.x + radius * Mathf.Cos(radian);
			pos.y = centerPos.y + radius * Mathf.Sin(radian);
			block.transform.position = pos;
			yield return null;
		}

		var playerScript = player.GetComponent<UnityChanController>();
		while(!playerScript.isGround) yield return null;
		isRotate = false;
	}
}
