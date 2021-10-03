using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents GM;
  
    void Awake()
    {
        GM = this;
    }

    public event Action<int> onDoorEnter;
    public void DoorEnter(int id)
    {
        if(onDoorEnter != null)
        {
            onDoorEnter(id);
        }
    }
}
