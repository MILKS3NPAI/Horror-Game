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

    public event Action<int, Entity> onDoorEnter;
    public void DoorEnter(int id, Entity iEntity)
    {
        if(onDoorEnter != null)
        {
            onDoorEnter(id, iEntity);
        }
    }
}
