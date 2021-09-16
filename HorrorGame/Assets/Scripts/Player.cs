using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private PlayerControls playerControls;
    float direction;
    protected override void Awake()
    
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    protected override void Start()
    {
        playerControls._2Dmovement.Jump.performed += _ => Jump();
        playerControls._2Dmovement.Move.performed += cxt => Move(cxt.ReadValue<float>());

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

        totalMovement = Vector2.zero;
        Vector2 lMovement = new Vector2();

        if (direction == 1)
        {
            Debug.Log("movement right");
            lMovement.x = 1f;
        }
        else if (direction == -1)
        {
            lMovement.x = -1f;
        }

        MoveRelative(lMovement.normalized);



        base.FixedUpdate();

        
        /*
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

		MoveRelative(lMovement.normalized);
		base.FixedUpdate();

        */
        //RunPhysicsStep();
        //body.MovePosition(body.position + (totalMovement * Time.fixedDeltaTime));
    }

    private void Move(float d )
    {
        direction = d;
    }
}
