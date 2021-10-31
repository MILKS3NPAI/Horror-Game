using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
	private Transform pTransfrom;
	private Animator animator;
	private Dialogue dialogue;
	private GameObject enemy;
	private PlayerControls playerControls;
	public PlayerControls mPlayerControls { get { return playerControls; } }
	float direction;
	[SerializeField] bool _hidden = false;
	[SerializeField] float useRadius = 3f;
	public bool mHidden { get { return _hidden; } set { if (_hidden == value) return; _hidden = value; collider.isTrigger = value; physicsEnabled = !value; } }
	[SerializeField] GameObject deathScreen, flashlight;
	//Get mouse poition
	Vector2 mousePos;
	Vector2 mouseAim;
	public Camera cam;
	private Transform flTransform;

	protected override void Awake()
	{
		base.Awake();
		playerControls = new PlayerControls();
		ConstantResources.Initialize();
		mGroundFilter = ConstantResources.sPlayerGroundMask;
		if (dialogue == null)
		{
			Dialogue lDialog = FindObjectOfType<Dialogue>();
			if (lDialog != null) { dialogue = lDialog; }
		}
		Enemy lEnemy = FindObjectOfType<Enemy>();
		if (lEnemy != null)
		{
			enemy = FindObjectOfType<Enemy>().gameObject;
		}
		else
		{
			Debug.LogWarning("Enemy does not exist in scene, or could not be found.");
		}
		flashlight = this.gameObject.transform.Find("Flashlight").gameObject;
		flTransform = flashlight.GetComponent<Transform>();
		animator = this.gameObject.GetComponentInChildren<Animator>();
		pTransfrom = this.gameObject.GetComponent<Transform>();
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
		animator.SetFloat("Horizontal", lMovement.x);
		animator.SetFloat("Speed", lMovement.sqrMagnitude);
		if (lMovement.x < 0)
		{
			this.transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
		else if (lMovement.x > 0)
		{
			this.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
		base.FixedUpdate();
	}
	protected override void Update()
	{
			AudioManager.MuteSound("Music2");
			AudioManager.UnmuteSound("Music1");

		//Flashlight
		mouseAim = playerControls._2Dmovement.Aim.ReadValue<Vector2>();

		mousePos = cam.ScreenToWorldPoint(mouseAim);

		Vector2 lookDir = mousePos - new Vector2(flTransform.position.x, flTransform.position.y);
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
		flTransform.rotation = Quaternion.Euler(0, 0, angle);
	}

	private void Move(float iDirection)
	{
		direction = iDirection;

		if (direction != 0 && mGroundDetected && !AudioManager.GetSound("Step1").source.isPlaying)
		{
			AudioManager.PlaySound("Step1");
		}
		else
		{
			AudioManager.StopSound("Step1");
		}
	}

	void Use()
	{
		Collider2D[] lUseables = new Collider2D[1];
		if (Physics2D.OverlapCircle(mPosition2D, useRadius, ConstantResources.sUseableMask, lUseables) > 0)
		{
			foreach (Collider2D lCollider in lUseables)
			{
				foreach (Useable lUseable in lCollider.GetComponents<Useable>())
				{
					if (lUseable == null)
					{
						continue;
					}
					lUseable.Use(this);
				}
			}
		}

	}

	void ToggleFlashlight()
	{

	}


	void Interaction()
	{


		/*
		if (dialogue == null)
		{
			return;
		}
		if (Dialogue.currentDialogue < Dialogue.dialogueSequence.Count)
		{
			Move(0);
			playerControls._2Dmovement.Disable();
			dialogue.ShowDialogue();
		}
		else
		{
			playerControls._2Dmovement.Enable();
			dialogue.HideDialogue();
		}
        */
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
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.parent != null && collision.transform.parent.gameObject.name.Equals("Dining Room"))
		{
			Animator[] anim = FindObjectsOfType<Animator>();
			for (int i = 0; i < anim.Length; i++)
			{
				if (anim[i].gameObject.transform.parent.gameObject.name.Equals("Dining Room"))
				{
					anim[i].SetBool("PlayerInDiningRoom", true);
				}
			}
		}
		else
		{
			//Debug.Log("Player hit " + collision);
		}
	}

	public void Kill()
	{
		Debug.Log("I is dead", gameObject);
		if (deathScreen != null)
        {
			deathScreen.SetActive(true);
        }
        else
        {
			Debug.Log("Death Screen not set");
        }
	}


	IEnumerator wait()
	{
		yield return new WaitForSeconds(0.2f);
	}
}
