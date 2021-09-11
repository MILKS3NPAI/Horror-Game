using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	protected override void FixedUpdate()
	{
		totalMovement = Vector2.zero;
		Vector2 lMovement = new Vector2();
		if (Input.GetKey(KeyCode.F))
		{
			lMovement.x = 1f;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			lMovement.x = -1f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			lMovement.y = 1f;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			lMovement.y = -1f;
		}
		RunPhysicsStep();
		MoveRelative(lMovement.normalized);
		body.MovePosition(body.position + (totalMovement * Time.fixedDeltaTime));
	}
}
