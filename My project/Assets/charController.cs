using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class charController : MonoBehaviour
{
    float boost = 1;

    [SerializeField]
    float mspd = 25f;

    [SerializeField]
    CharacterController controller;

    private Animator anim;
    private Vector2 moveInput; // Store input values

    public float health = 100f; // Current health
    public float maxHealth = 100f; // Maximum health
    public float stamina = 100f; // Current stamina
    public float maxStamina = 100f; // Maximum stamina
    
    public float staminaCostPerAttack = 20f;
    public float staminaRegenRate = 5f; // Stamina regeneration rate

    void Start()
    {
        anim = GetComponent<Animator>();
    }

        private void Update()
    {
        RegenerateStamina();
        Move();
        Animate();
    }

     private void RegenerateStamina()
    {
        if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina); // Keep stamina within bounds
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            Die(); // Handle player death
        }
    }

    private void Die()
    {
        // Add logic for player death (e.g., respawn or game over)
        Debug.Log("Player has died.");
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        // Capture move input from Input Action (expected as Vector2)
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        // Calculate movement direction based on input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction to the character's local space
        move = transform.TransformDirection(move);

        // Apply movement
        controller.Move(move * mspd * Time.deltaTime * boost);
    }

    private void Animate()
    {
        // Pass the input values to the animator to control blend tree
        anim.SetFloat("x", moveInput.x);
        anim.SetFloat("y", moveInput.y);
    }

    public void OnCrouch(InputAction.CallbackContext context){
        // if(context.performed)
        //     me.transform.localScale = crouchScale;
        // else if(context.canceled)
        //     me.transform.localScale = standScale;
    }


    public void OnDash(InputAction.CallbackContext context){
        if(context.performed){ 
            print("dash");
            boost = 2f;
        }
        else if(context.canceled)
            boost = 1f;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void onJab(InputAction.CallbackContext context)
    {
        if (context.performed && anim != null && stamina >= staminaCostPerAttack)
            {
                stamina -= staminaCostPerAttack;
                anim.SetTrigger("jab");
            }
    }

        public void onCross(InputAction.CallbackContext context)
    {
        if (context.performed && anim != null && stamina >= staminaCostPerAttack)  
            {
                stamina -= staminaCostPerAttack;
                anim.SetTrigger("cross");
            }
    }



}