using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    public float attackDamage = 10f; // Damage dealt by this attack

    // Reference to the player or AI controller
    [SerializeField]
    private charController playerController;
    [SerializeField]
    private AIController aiController;

    private void Start()
    {
        playerController = GetComponentInParent<charController>();
        aiController = GetComponentInParent<AIController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("hello");
        print(collision.gameObject.tag);
        // Check if the collision is with the player or enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            print("hery");
            charController player = collision.gameObject.GetComponent<charController>();
            if (player != null && aiController != null) // Make sure AI is not hitting itself
            {
                player.TakeDamage(attackDamage);
                Debug.Log("Player hit by AI!");
            }
        }
        else if (collision.gameObject.CompareTag("Opponent"))
        {
            print("324W");
            AIController enemy = collision.gameObject.GetComponent<AIController>();
            if (enemy != null && playerController != null) // Make sure player is not hitting itself
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("AI hit by Player!");
            }
        }
    }
}
