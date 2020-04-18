using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Golem	mPet;
	public Combat	mCombat;

	Rigidbody	mRigidBody;
	Combatant	mMyStats;
	Camera		mMyCam;
	float		mCamDist;

	const float	MoveSpeed	=700f;
	const float	ZoomSpeed	=100f;
	const float	TurnSpeed	=70f;
	const float	JumpSpeed	=50f;
	const float	MinZoom		=5f;
	const float	MaxZoom		=25;


	void Start()
	{
		mRigidBody	=GetComponent<Rigidbody>();
		mMyStats	=GetComponent<Combatant>();

		GameObject	go	=GameObject.Find("Main Camera");

		mMyCam	=go.GetComponent<Camera>();

		mCombat.RegisterCombatant(mMyStats);

		mCamDist	=mMyCam.transform.localPosition.magnitude;
	}
	

	void Update()
	{
		float	side	=Input.GetAxis("Horizontal");
		float	forw	=Input.GetAxis("Vertical");
		float	mx		=Input.GetAxis("Mouse X");
		float	my		=Input.GetAxis("Mouse Y");
		float	m2		=Input.GetAxis("Fire2");
		float	jmp		=Input.GetAxis("Jump");
		float	sprint	=1f + Input.GetAxis("Sprint");
		float	spawn	=Input.GetAxis("DebugSpawn");
		float	wheel	=Input.GetAxis("Mouse ScrollWheel");

		bool	bSnapCameraLocal	=false;
		if(spawn > 0f)
		{
			mCombat.DebugSpawn();
		}

		if(wheel != 0f)
		{
			mCamDist	-=ZoomSpeed * wheel * Time.deltaTime;
			mCamDist	=Mathf.Clamp(mCamDist, MinZoom, MaxZoom);

			bSnapCameraLocal	=true;
		}

		//rotate camera if right mouse held
		if(m2 > 0f)
		{
			Cursor.lockState	=CursorLockMode.Locked;

			Vector3	ang	=mMyCam.transform.eulerAngles;

			ang.y	+=(mx * TurnSpeed * Time.deltaTime);

			//do camera look up down
			ang.x	-=(my * TurnSpeed * Time.deltaTime);

			ang.x	=Mathf.Clamp(ang.x, 0.01f, 65f);

			mMyCam.transform.eulerAngles	=ang;

			bSnapCameraLocal	=true;
		}
		else
		{
			Cursor.lockState	=CursorLockMode.None;
		}

		Vector3	move	=mMyCam.transform.forward * forw;

		//flatten
		move.y	=0f;

		move	+=side * mMyCam.transform.right;

		if(move.magnitude > 0f)
		{
			//grab a copy of the world camera direction
			Quaternion	camCopy	=mMyCam.transform.rotation;

			move.Normalize();

			mRigidBody.AddForce(move * Time.deltaTime * (MoveSpeed * sprint));

//			transform.position	+=move * Time.deltaTime * (MoveSpeed * sprint);

			//rotate player to face camera direction if moving
			Vector3	dir	=mMyCam.transform.position - transform.position;

			//flatten
			dir.y	=0f;
			dir.Normalize();

			//when moving, rotate the player to face
			transform.rotation	=Quaternion.LookRotation(-dir);

			//adjust camera position for new rotation
			mMyCam.transform.rotation	=camCopy;

			//this can cause roll sometimes to build up slowly
			Vector3		eul	=mMyCam.transform.localEulerAngles;

			//clear the roll
			eul.z	=0;

			mMyCam.transform.localEulerAngles	=eul;

			bSnapCameraLocal	=true;
		}

		if(jmp > 0)
		{
			mRigidBody.AddRelativeForce(Vector3.up
				* Time.deltaTime * JumpSpeed, ForceMode.Impulse);
		}

		if(bSnapCameraLocal)
		{
			Vector3	dir	=mMyCam.transform.localRotation * -Vector3.forward;

			mMyCam.transform.localPosition	=dir * mCamDist;
		}

//		print("Sprint: " + sprint);
	}	
}
