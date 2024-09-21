using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerCharacterMovement : MonoBehaviour
{
    float horizontal, vertical, boost = 1, jumps = 0;
    Vector3 moveVector = new();
    Vector3 jumpVector = new();

    [SerializeField]
    float mspd = 25f;

    [SerializeField]
    CharacterController controller;

    [SerializeField]
    private GameObject me;

    private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 standScale = new Vector3(1f, 1f, 1f);

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        // print(jump);

        moveVector.Set(horizontal, 0, vertical);

        gravity();
        Move();
        // Debug.Log(jumps);
    }

    private void gravity()
    {
        if (controller.isGrounded) {
            jumps = 0;
        } else {
            jumpVector.y += Physics.gravity.y * Time.deltaTime;
            controller.Move(jumpVector * Time.deltaTime);
        }
    }

    private void Move()
    {
        // print("hello"        );
        moveVector.y = jumpVector.y;
        controller.Move(mspd * Time.deltaTime * moveVector * boost);
    }


    public void OnMove(InputAction.CallbackContext value){
        print(":')");
    }

    public void OnCrouch(InputAction.CallbackContext context){
        if(context.performed)
            me.transform.localScale = crouchScale;
        else if(context.canceled)
            me.transform.localScale = standScale;
    }


    public void OnDash(InputAction.CallbackContext context){
        if(context.performed)
            boost = 2f;
        else if(context.canceled)
            boost = 1f;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && jumps < 2){
            jumpVector.y = 3f; // Apply initial jump force
            jumps++;
        }
    }





}