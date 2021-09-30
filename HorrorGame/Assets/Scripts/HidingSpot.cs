using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : Useable
{
	public Camera hidingSpotCamera;
	Transform returnPoint;
	Vector3 returnPosition;
	Player mPlayer { get { return GameEngine.sPlayer; } }

	private void Awake()
	{
		if (returnPoint == null)
		{
			returnPoint = transform.GetChild(0);
		}
		if (returnPoint == null)
		{
			Debug.LogError("No return point avaiable for hiding spot, defaulting to just above position.");
			returnPosition = transform.position + Vector3.up * 10;
			return;
		}
		RaycastHit2D[] lFloor = new RaycastHit2D[5];
		if (Physics2D.Raycast(new Vector2(returnPoint.position.x, returnPoint.position.y), Vector2.down, ConstantResources.sGroundMask, lFloor, 500f) > 0)
		{
			returnPosition = lFloor[0].point - mPlayer.mCollider.offset + (Vector2.up * mPlayer.mCollider.size.y * .5f);
		}
		else
		{
			Debug.LogError("No floor detected under returnPoint, defaulting to just above position.");
			returnPosition = transform.position + Vector3.up * 10;
			return;
		}
	}

	public override void Use(Player iPlayer)
	{
		if (iPlayer.mHidden)
		{
			hidingSpotCamera.depth = -2;
			iPlayer.mHidden = false;
			iPlayer.mPlayerControls._2Dmovement.Enable();
		}
		else
		{
			iPlayer.mPlayerControls._2Dmovement.Disable();
			iPlayer.mHidden = true;
			hidingSpotCamera.depth = 1;
			iPlayer.transform.position = new Vector3(returnPosition.x, returnPosition.y, iPlayer.transform.position.z);
		}
	}
}
