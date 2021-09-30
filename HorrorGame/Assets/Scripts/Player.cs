using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	private GameObject dialogue, enemy;
	private PlayerControls playerControls;
	public PlayerControls mPlayerControls { get { return playerControls; } }
	float direction;
	[SerializeField] bool _hidden = false;
	float useRadius = 3f;
	//[SerializeField] GameObject dialogue;
	public bool mHidden { get { return _hidden; } set { if (_hidden == value) return; _hidden = value; collider.isTrigger = value; physicsEnabled = !value; } }

	protected override void Awake()
	{
		base.Awake();
		playerControls = new PlayerControls();
		mGroundFilter = ConstantResources.sPlayerGroundMask;
		if (dialogue == null)
		{
			Dialogue lDialog = FindObjectOfType<Dialogue>();
			if (lDialog != null) { dialogue = lDialog.gameObject; }
		}
		enemy = FindObjectOfType<Enemy>().gameObject;
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
		if (dialogue == null)
		{
			return;
		}
		if (Dialogue.currentDialogue < dialogue.transform.childCount)
		{
			Move(0);
			playerControls._2Dmovement.Disable();
			dialogue.transform.GetChild(Dialogue.currentDialogue).gameObject.SetActive(true);
			if (Dialogue.dialogueSpeakers[Dialogue.currentDialogue].Equals("p"))
			{
				dialogue.transform.position = transform.position + new Vector3(0, 3, 0);
			}
			else
			{
				dialogue.transform.position = enemy.transform.position + new Vector3(0, 3, 0);
			}
			try
			{
				dialogue.transform.GetChild(Dialogue.currentDialogue - 1).gameObject.SetActive(false);
			}
			catch (System.Exception ex)
			{
				Debug.Log(ex);
			}
			Dialogue.currentDialogue++;
		}
		else
		{
			playerControls._2Dmovement.Enable();
			dialogue.transform.GetChild(dialogue.transform.childCount - 1).gameObject.SetActive(false);
			Dialogue.currentDialogue = 0;
		}
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
