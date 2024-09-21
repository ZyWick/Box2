using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 moveDir;
    Vector3 jumpVector;
    public InputActionReference move;
    public InputActionReference jump;
    bool  isJumping;

    public float mspd = 5f;

    public float rotspd = 20f;

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(move.action.ReadValue<Vector2>().x, 0f, move.action.ReadValue<Vector2>().y).normalized;
        JumpAndGravity();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) isJumping = true;
    }

    private void FixedUpdate()
    {
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotspd * Time.deltaTime);
        }
        //moveDir.y = jumpVector.y;

        controller.Move(mspd * Time.deltaTime * moveDir);
    }

    bool isGrounded = true;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print(hit.gameObject.name);
    }
    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            isJumping = false;
            jumpVector.y = Mathf.Sqrt(1f * -2f * Physics.gravity.y);
            isGrounded = false;
        }

        jumpVector.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(jumpVector * Time.deltaTime);

        if (jumpVector.y < 0f) jumpVector.y = 0f;
        //controller.Move(Mathf.Sqrt(2f * -2f * Physics.gravity.y) * Time.deltaTime * Vector3.up);
    }

    //void MovementJump()
    //{
    //    // Check if the player is grounded
    //    _groundedPlayer = _characterController.isGrounded;
    //    if (_groundedPlayer)
    //    {
    //        _playerVelocity.y = 0f; // If they're on the ground their vertical velocity is 0.
    //    }

    //    // Changes the height position of the player..
    //    if (_jumpPressed && _groundedPlayer)
    //    {
    //        _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue); // Move the player in a nice parabola for the jump
    //        _jumpPressed = false; // We've handled the jump so turn off the flag for the pressing of the button
    //    }

    //    _playerVelocity.y += _gravityValue * Time.deltaTime; // Update the player velocity (decreasing over time)
    //    _characterController.Move(_playerVelocity * Time.deltaTime); // Move the player
    //}
}
