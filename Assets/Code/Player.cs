using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Golem	mPet;
	public Combat	mCombat;

	Rigidbody	mRigidBody;
	Combatant	mMyStats;

	const float	MoveSpeed	=7f;
	const float	TurnSpeed	=70f;
	const float	JumpSpeed	=50f;


	void Start()
	{
		mRigidBody	=GetComponent<Rigidbody>();
		mMyStats	=GetComponent<Combatant>();

		mCombat.RegisterCombatant(mMyStats);
	}
	

	void Update()
	{
		float	side	=Input.GetAxis("Horizontal");
		float	forw	=Input.GetAxis("Vertical");
		float	mx		=Input.GetAxis("Mouse X");
		float	m2		=Input.GetAxis("Fire2");
		float	jmp		=Input.GetAxis("Jump");
		float	sprint	=1f + Input.GetAxis("Sprint");
		float	spawn	=Input.GetAxis("DebugSpawn");

		if(spawn > 0f)
		{
			mCombat.DebugSpawn();
		}

		Vector3	move	=transform.forward * forw;

		move	+=side * transform.right;

		move.Normalize();

		transform.position	+=move * Time.deltaTime * (MoveSpeed * sprint);

		if(m2 > 0f)
		{
			Cursor.lockState	=CursorLockMode.Locked;

			Vector3	ang	=transform.eulerAngles;

			ang.y	+=(mx * TurnSpeed * Time.deltaTime);

			transform.eulerAngles	=ang;
		}
		else
		{
			Cursor.lockState	=CursorLockMode.None;
		}

		if(jmp > 0)
		{
			mRigidBody.AddRelativeForce(Vector3.up
				* Time.deltaTime * JumpSpeed, ForceMode.Impulse);
		}

//		print("Sprint: " + sprint);
	}	
}
