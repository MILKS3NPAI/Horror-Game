using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorController : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] private float dir; //1=up -1=down
    public int id;
    GameObject player;
    Vector3 pos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameEvents.GM.onDoorEnter += OnDoorOpen;
    }

    private void OnDoorOpen(int id)
    {
        if (id == this.id)
        {
            pos = player.GetComponent<Transform>().position;
            pos.y += 7 * dir;
            player.GetComponent<Transform>().position = pos;
        }
    }
}

 
