using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class Player : Entity
{
	private Transform pTransfrom;
	private Animator animator;
	private Dialogue dialogue;
	private GameObject enemy;
	private PlayerControls playerControls;
	private bool canEscape = false;
	[SerializeField] private TextMeshProUGUI playerText;
	[SerializeField] private TextMeshProUGUI escapeText;
	public PlayerControls mPlayerControls { get { return playerControls; } }
	float direction;
	[SerializeField] bool _hidden = false;

	[Header ("ENABLE THIS (ONCE) FOR CUTSCENE")]
	[SerializeField] bool escaped = false;
	public bool mHidden { get { return _hidden; } set { if (_hidden == value) return; _hidden = value; collider.isTrigger = value; physicsEnabled = !value; } }
	[SerializeField] GameObject deathScreen, flashlight;
	[SerializeField] RuntimeAnimatorController enemyAnimator;
	[SerializeField] Sprite cutsceneSprite;
	//Get mouse poition
	Vector2 mousePos;
	Vector2 mouseAim;
	public Camera cam;
	private Transform flTransform;

	protected override void Awake()
	{
		base.Awake();
		playerControls = InputManager.GetInputActions();//new PlayerControls();
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
		AudioManager.SetVolume("Music1", 0.015f);
		AudioManager.SetVolume("Step1", 0.12f);
		AudioManager.SetVolume("Step2", 0.015f);
		AudioManager.SetVolume("TV", 0.005f);
		AudioManager.SetVolume("Fridge", 0.008f);
		AudioManager.SetVolume("Breathing", 0.030f);
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
		if (escaped)
			ShowCutscene();

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

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.name.Equals("Chandelier"))
		{
			collision.gameObject.GetComponent<Animator>().SetBool("PlayerInDiningRoom", true);
			AudioManager.PlaySound("Glass1");
		}
		else if (collision.transform.parent.gameObject.name.Equals("Kitchen"))
        {
			AudioManager.PlaySound("Fridge");
        }
		else if (collision.transform.parent.gameObject.name.Equals("Living Room"))
		{
			AudioManager.PlaySound("TV");
		}
		else if (collision.transform.gameObject.name.Equals("RazorTrigger"))
        {
			AudioManager.PlaySound("RazorTrigger");
			Destroy(collision.gameObject);
			Debug.Log("Razor");
        }
		else if(collision.transform.gameObject.name.Equals("Intro"))
        {
			playerText.gameObject.SetActive(true);
			playerText.enabled = true;
			playerText.text = "Turn on the fuse box";
        }
		else if (collision.transform.gameObject.name.Equals("Escape"))
		{
			escapeText.gameObject.SetActive(true);
			escapeText.enabled = true;
			escapeText.text = "Found Keys";
			canEscape = true;
			AudioManager.PlaySound("hell");
		}
		else if (collision.transform.gameObject.name.Equals("Exit") && canEscape == true)
		{
			escaped = true;
		}
	}

    public void OnTriggerExit2D(Collider2D collision)
    {

		if (collision.transform.parent.gameObject.name.Equals("Kitchen"))
		{
			AudioManager.StopSound("Fridge");
		}
		else if (collision.transform.name.Equals("Chandelier"))
		{
			//collision.gameObject.GetComponent<Animator>().SetBool("PlayerInDiningRoom", true);
			AudioManager.MuteSound("Glass1");
		}
		else if (collision.transform.parent.gameObject.name.Equals("Living Room"))
		{
			AudioManager.StopSound("TV");
		}
		else if (collision.transform.gameObject.name.Equals("Intro"))
		{
			playerText.gameObject.SetActive(false);
			playerText.enabled = false;
			AudioManager.PlaySound("Signal1");
			Destroy(collision.gameObject);
		}
		else if (collision.transform.gameObject.name.Equals("Escape"))
		{
			escapeText.gameObject.SetActive(false);
			escapeText.enabled = false;
			Destroy(collision.gameObject);
		}
	}
	private void ShowCutscene()
    {
		escaped = false;
		PauseMenu.PreventPause();
		AudioManager.StopSound("Music1");
		AudioManager.StopSound("Music2");
		AudioManager.MuteSound("TV");
		//enemy.GetComponent<Enemy>().enabled = false;
		VideoPlayer[] videoPlayers = FindObjectsOfType<VideoPlayer>();
		foreach (VideoPlayer v in videoPlayers){
			if (v.gameObject.transform.parent.gameObject.name.Equals("TV Static"))
            {
				v.gameObject.transform.parent.gameObject.SetActive(false);
			}
        }
		enemy.transform.position = new Vector3(-2f, -2f, -0.9161661f);
		enemy.AddComponent<Animator>();
		if (enemyAnimator != null)
			enemy.GetComponent<Animator>().runtimeAnimatorController = enemyAnimator;
		else
			Debug.Log("Enemy animator controller not set");
		enemy.gameObject.GetComponent<Animator>().SetBool("CutsceneActive", true);
		playerControls._2Dmovement.Move.Disable();
		playerControls._2Dmovement.Jump.Disable();
		transform.position = new Vector3(-7f, -2f, 0.1118393f);
		if (cutsceneSprite != null)
			transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = cutsceneSprite;
		else
			Debug.Log("Cutscene sprite not set");
		cam.GetComponent<CameraFollow>().enabled = false;
		cam.transform.position = transform.position + new Vector3(5, 1, -3);
	}

    public void Kill()
	{
		Debug.Log("I is dead", gameObject);
		if (deathScreen != null)
        {
			playerControls._2Dmovement.Disable();
			//deathScreen.SetActive(true);
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
