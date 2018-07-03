using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{

	Rigidbody body;
	private Animator animator;

	private const string parameterRun = "isRun";
	private const string parameterJump = "isJump";
	private const string parameterDamage = "isDamage";
	public bool isGround = true;
	public float speed = 2.0f;
	public float jumpPower = 60.0f;

	// Use this for initialization
	void Start()
	{
		this.body = GetComponent<Rigidbody>();
		this.animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if(!isGround) return;

		bool isLeft = Input.GetKey(KeyCode.LeftArrow);
		bool isRight = Input.GetKey(KeyCode.RightArrow);
		bool isUp = Input.GetKey(KeyCode.UpArrow);
		bool isDown = Input.GetKey(KeyCode.DownArrow);
		bool isJump = Input.GetKey(KeyCode.Space);
		bool isRun = (isLeft || isRight);
		bool isRotate = (isUp || isDown);

		//左右移動
		if(isRun)
		{
			var v = body.velocity;
			v.z = speed * (isLeft ? -1 : 1);
			body.velocity = v;
			transform.rotation = Quaternion.Euler(0, (isLeft ? 180 : 0), 0);
			animator.SetBool(parameterRun, true);
		}
		else animator.SetBool(parameterRun, false);

		//ジャンプ
		if(isJump || isRotate)
		{
			if(animator.GetBool(parameterJump)) return;
			body.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		string tag = col.gameObject.tag;
		if(tag == "Block")
		{
			isGround = true;
			animator.SetBool(parameterJump, false);
			var v = body.velocity;
			v.x = 0;
			body.velocity = v;
		}
		else if(tag == "DamageObject")
		{
			if(animator.GetBool(parameterDamage)) return;
			StartCoroutine("DamageProcess");
		}
	}

	void OnCollisionExit(Collision col)
	{
		string tag = col.gameObject.tag;
		if(tag == "Block")
		{
			isGround = false;
			animator.SetBool(parameterRun, false);
			animator.SetBool(parameterJump, true);
		}
		else if(tag == "DamageObject")
		{
			isGround = false;
			animator.SetBool(parameterRun, false);
			animator.SetBool(parameterJump, false);
		}
	}

	IEnumerator DamageProcess()
	{
		animator.SetBool(parameterDamage, true);
		for(int i = 0; i < 60; i++)
		{
			yield return null;
		}
		animator.SetBool(parameterDamage, false);
	}
}
