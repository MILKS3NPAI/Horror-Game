using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	[SerializeField] GameObject dialogue;
	private PlayerControls playerControls;
	float direction;
	[SerializeField] bool _hidden = false;
	public bool mHidden { get { return _hidden; } set { if (_hidden == value) return; collider.isTrigger = value; physicsEnabled = !value; } }

	protected override void Awake()
	{
		base.Awake();
		playerControls = new PlayerControls();
		mGroundFilter = ConstantResources.sPlayerGroundMask;
		foreach (Transform t in dialogue.GetComponentInChildren<Transform>())
        {
			t.gameObject.AddComponent<Dialogue>();
			t.GetComponent<Dialogue>().dialogueOrder = Dialogue.currentDialogue;
		}
		Dialogue.currentDialogue = 0;
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

	private void Move(float iDirection)
	{
		direction = iDirection;
	}

	void Use()
	{

	}

    void ToggleFlashlight()
    {

    }

	void Interaction()
    {
		if (Dialogue.currentDialogue < dialogue.transform.childCount)
		{
			Move(0);
			playerControls._2Dmovement.Disable();
			dialogue.transform.GetChild(Dialogue.currentDialogue).gameObject.SetActive(true);
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
			dialogue.transform.GetChild(dialogue.transform.childCount - 1).gameObject.SetActive(false);
			Dialogue.currentDialogue = 0;
			playerControls._2Dmovement.Enable();
			dialogue.transform.GetChild(0).gameObject.SetActive(false);
		}
    }
}
