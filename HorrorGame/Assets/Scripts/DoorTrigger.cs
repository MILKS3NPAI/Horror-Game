using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Useable
{
	public int id;

	public override void Use(Entity iEntity)
	{
		GameEvents.GM.DoorEnter(id, iEntity);
	}
}

