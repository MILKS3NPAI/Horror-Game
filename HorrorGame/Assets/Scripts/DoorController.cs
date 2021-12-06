using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorController : MonoBehaviour
{
	static List<DoorController> _allDoors = new List<DoorController>();
	public static List<DoorController> sAllDoors { get { return _allDoors; } }
	private PlayerControls playerControls;
	[SerializeField] private float dir; //1=up -1=down
	[SerializeField] private Camera main;
	public int id;
	GameObject player;
	Vector3 pos;
	Vector3 pos1;

	private void Awake()
	{
		_allDoors.Clear();
	}

	public bool GoesToward(float iDir)
	{
		return (dir > 0 && iDir > 0) || (dir < 0 && iDir < 0);
	}

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		GameEvents.GM.onDoorEnter += OnDoorOpen;
		_allDoors.Add(this);
	}

	private void OnDoorOpen(int id, Entity iEntity)
	{
		if (id == this.id)
		{
			//FindObjectOfType<AudioManager>().gameObject.transform.position = transform.position;
			AudioManager.PlaySound("Door1");
			/*
            pos = iEntity.transform.position;
            pos.y += 15 * dir;
            iEntity.transform.position = pos;*/
			iEntity.transform.position += Vector3.up * (13.5f * dir);
		}
	}
}


