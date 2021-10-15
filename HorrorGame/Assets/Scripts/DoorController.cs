using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorController : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] private float dir; //1=up -1=down
    [SerializeField] private Camera main;
    public int id;
    GameObject player;
    Vector3 pos;
    Vector3 pos1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameEvents.GM.onDoorEnter += OnDoorOpen;
    }

    private void OnDoorOpen(int id, Entity iEntity)
    {
        if (id == this.id)
        {
            pos = iEntity.transform.position;
            pos.y += 15 * dir;
            iEntity.transform.position = pos;

            pos1 = main.GetComponent<Transform>().position;
            pos1.y += 15 * dir;
            main.GetComponent<Transform>().position = pos1;
            main.GetComponent<CameraFollow>().enabled = true;
        }
    }
}

 
