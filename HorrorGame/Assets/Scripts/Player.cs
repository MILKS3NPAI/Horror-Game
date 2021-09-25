using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	private PlayerControls playerControls;
	float direction;
	[SerializeField] bool _hidden = false;
	public bool mHidden { get { return _hidden; } set { if (_hidden == value) return; collider.isTrigger = value; physicsEnabled = !value; } }

	protected override void Awake()
	{
		base.Awake();
		playerControls = new PlayerControls();
		mGroundFilter = ConstantResources.sPlayerGroundMask;
	}

	protected override void Start()
	{
		playerControls._2Dmovement.Jump.performed += _ => Jump();
		playerControls._2Dmovement.Move.performed += cxt => Move(cxt.ReadValue<float>());
        playerControls.UI.Interact.performed += _ => Hide();
    }

	private void OnEnable()
	{
		playerControls.Enable();
	}

	private void OnDisable()
	{
		playerControls.Disable();
	}

	protected override void FixedUpdate()
	{

		//physicsMovement = Vector2.zero;
		Vector2 lMovement = new Vector2(direction, 0);
		/*
        if (direction == 1)
        {
            //Debug.Log("movement right");
            lMovement.x = 1f;
        }
        else if (direction == -1)
        {
            lMovement.x = -1f;
        }*/

		MoveRelative(lMovement.normalized);
		base.FixedUpdate();
	}

	private void Move(float iDirection)
	{
		direction = iDirection;
	}

	void Hide()
	{
		Debug.Log("Hiding");
	}
}
