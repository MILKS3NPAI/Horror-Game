﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
	private GameObject dialogue, enemy;
	private PlayerControls playerControls;
	public PlayerControls mPlayerControls { get { return playerControls; } }
	float direction;
	[SerializeField] bool _hidden = false;
	float useRadius = 3f;
	public bool mHidden { get { return _hidden; } set { if (_hidden == value) return; _hidden = value; collider.isTrigger = value; physicsEnabled = !value; } }
    [SerializeField] GameObject flashlight;
    //Get mouse poition
    Vector2 mousePos;
    Vector2 mouseAim;
    public Camera cam;
    private Transform flTransform;

    protected override void Awake()
	{
		base.Awake();
		playerControls = new PlayerControls();
		mGroundFilter = ConstantResources.sPlayerGroundMask;
		if (dialogue == null)
		{
			Dialogue lDialog = FindObjectOfType<Dialogue>();
			if (lDialog != null) { dialogue = lDialog.gameObject.transform.GetChild(0).gameObject; }
		}
		enemy = FindObjectOfType<Enemy>().gameObject;

        flashlight = this.gameObject.transform.Find("Flashlight").gameObject;
        flTransform = flashlight.GetComponent<Transform>();
	}

	protected override void Start()
	{
		playerControls._2Dmovement.Jump.performed += _ => Jump();
		playerControls._2Dmovement.Move.performed += cxt => Move(cxt.ReadValue<float>());
		playerControls.UI.Interact.performed += _ => Use();
		playerControls.UI.Interact.performed += _ => Interaction();
		playerControls.UI.Flashlight_Toggle.performed += _ => ToggleFlashlight();
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
	protected override void Update()
	{
		if (PlayerRoom().Equals("right"))
		{
			AudioManager.MuteSound("Music2");
			AudioManager.UnmuteSound("Music1");
		}
		else if (PlayerRoom().Equals("left"))
		{
			AudioManager.MuteSound("Music1");
			AudioManager.UnmuteSound("Music2");
		}
		else
		{
			Debug.Log("Player is not in a room.");
		}

        //Flashlight
        mouseAim = playerControls._2Dmovement.Aim.ReadValue<Vector2>();

        mousePos = cam.ScreenToWorldPoint(mouseAim);

        Vector2 lookDir = mousePos - new Vector2 (flTransform.position.x, flTransform.position.y);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        flTransform.rotation = Quaternion.Euler(0,0,angle);
    }

	private void Move(float iDirection)
	{
		direction = iDirection;
		if (iDirection != 0 && mGroundDetected)
		{
			if (PlayerRoom().Equals("right"))
			{
				AudioManager.PlaySound("Step1");
				AudioManager.StopSound("Step2");
			}
			else if (PlayerRoom().Equals("left"))
			{
				AudioManager.StopSound("Step1");
				AudioManager.PlaySound("Step2");
			}
		}
		else
		{
			AudioManager.StopSound("Step1");
			AudioManager.StopSound("Step2");
		}
	}

	void Use()
	{
		Collider2D[] lUseables = new Collider2D[1];
		if (Physics2D.OverlapCircle(mPosition2D, useRadius, ConstantResources.sUseableMask, lUseables) > 0)
		{
			foreach (Collider2D lCollider in lUseables) {
				Useable lUseable = lCollider.GetComponent<Useable>();
				if (lUseable == null)
				{
					continue;
				}
				lUseable.Use(this);
				break;
			}
		}

	}

	void ToggleFlashlight()
	{

	}

    
	void Interaction()
	{


        //*
		if (dialogue == null)
		{
			return;
		}
		if (Dialogue.currentDialogue < Dialogue.dialogue.Count)
		{
			Move(0);
			playerControls._2Dmovement.Disable();
			dialogue.SetActive(true);
			dialogue.transform.GetChild(0).GetComponent<Text>().text =
				Dialogue.dialogue[Dialogue.currentDialogue].Substring(3);
			if (Dialogue.dialogue[Dialogue.currentDialogue].Substring(0, 1).Equals("P"))
            {
				dialogue.transform.position = transform.position + new Vector3(0, 2, 0);
			}
            else if (Dialogue.dialogue[Dialogue.currentDialogue].Substring(0, 1).Equals("E"))
			{
				dialogue.transform.position = enemy.transform.position + new Vector3(0, 2, 0);
			}
			Dialogue.currentDialogue++;
		}
		else
		{
			playerControls._2Dmovement.Enable();
			dialogue.SetActive(false);
			Dialogue.currentDialogue = 0;
		}
        //*/
	}

	// Determines what room the player is in
	private string PlayerRoom()
	{
		if (transform.position.x >= 0)
		{
			return "right";
		}
		else
		{
			return "left";
		}
	}
}
