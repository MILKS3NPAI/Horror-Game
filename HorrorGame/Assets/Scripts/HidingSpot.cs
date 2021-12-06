using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : Useable
{
	public Camera hidingSpotCamera;
	[SerializeField] SpotType spotType;
	Transform returnPoint;
	Vector3 returnPosition;
	Player mPlayer { get { return GameEngine.sPlayer; } }
	Enemy mEnemy { get { return GameEngine.sEnemy; } }
	[SerializeField] Transform monsterShadow;
	[SerializeField] float xOffsetMultiplier = 1f;
	[SerializeField] float visibleDistance = 5f;
	public float mVisibleDistance { get { return visibleDistance; } }
	[SerializeField] float percentChanceToSee = 10f;
	public float mChanceToSee { get { return percentChanceToSee; } }
	SpriteRenderer monsterShadowSprite;

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

	private void Start()
	{
		if (monsterShadow == null)
		{
			monsterShadow = hidingSpotCamera.transform.Find("MonsterShadow");
			if (monsterShadow != null)
			{
				monsterShadowSprite = monsterShadow.GetComponent<SpriteRenderer>();
				if (monsterShadowSprite == null)
				{
					monsterShadowSprite = monsterShadow.GetComponentInChildren<SpriteRenderer>();
				}
			}
		}
		monsterShadowSprite = monsterShadow.GetComponent<SpriteRenderer>();
		if (monsterShadowSprite == null)
		{
			monsterShadowSprite = monsterShadow.GetComponentInChildren<SpriteRenderer>();
		}
		monsterShadow.gameObject.SetActive(false);
	}

	public override void Use(Entity iEntity)
	{
		if (spotType == SpotType.DOOR) //Box and table are only fillins, use whatever name makes sense. However, strings are slow.
		{
			//FindObjectOfType<AudioManager>().gameObject.transform.position = transform.position;
			AudioManager.PlaySound("Hide1");
		}
		else if (spotType == SpotType.CRAWL_SPACE)
		{
			//FindObjectOfType<AudioManager>().gameObject.transform.position = transform.position;
			AudioManager.PlaySound("Hide2");
		}
		if (!(iEntity is Player))
		{
			return;
		}
		Player lPlayer = (Player)iEntity;
		if (lPlayer.mHidden)
		{
			hidingSpotCamera.depth = -2;
			lPlayer.mHidden = false;
			lPlayer.mMovementDisabled = false;
			AudioManager.StopSound("Breathing");
			monsterShadow.gameObject.SetActive(false);
		}
		else
		{
			lPlayer.mHidden = true;
			hidingSpotCamera.depth = 1;
			lPlayer.transform.position = new Vector3(returnPosition.x, returnPosition.y, lPlayer.transform.position.z);
			lPlayer.ZeroMovement();
			lPlayer.mMovementDisabled = true;
			AudioManager.PlaySound("Breathing");
			monsterShadow.gameObject.SetActive(true);

		}
	}

	private void FixedUpdate()
	{
		if (Vector2.Distance(mPlayer.mPosition2D, transform.position) <= (mPlayer.mUseRadius * 4f))
		{
			monsterShadow.gameObject.SetActive(mEnemy.mSameFloorAsPlayer);
			monsterShadow.localPosition = new Vector3((mEnemy.mPosition.x - transform.position.x) * xOffsetMultiplier, monsterShadow.localPosition.y, monsterShadow.localPosition.z);

			monsterShadowSprite.sprite = mEnemy.mSprite.sprite;
			monsterShadowSprite.flipX = mEnemy.mSprite.flipX;
		}
	}
}
