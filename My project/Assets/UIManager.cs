using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class UIManager : MonoBehaviour
{
    // AI UI Elements
    public Slider aiHealthSlider;
    public Slider aiStaminaSlider;
    
    [SerializeField]
    public AIController aiController;

    // Player UI Elements
    public Slider playerHealthSlider;
    public Slider playerStaminaSlider;

    [SerializeField]
    public charController CharController; // Reference to Player Controller script

    void Start()
    {
        // Initialize AI Sliders
        if (aiController == null)
        {
            aiController = FindObjectOfType<AIController>();
        }
        aiHealthSlider.maxValue = aiController.maxHealth;
        aiStaminaSlider.maxValue = aiController.maxStamina;
        aiHealthSlider.value = aiController.health;
        aiStaminaSlider.value = aiController.stamina;

        // Initialize Player Sliders
        if (CharController == null)
        {
            CharController = FindObjectOfType<charController>(); // Ensure the PlayerController is assigned
        }
        playerHealthSlider.maxValue = CharController.maxHealth;
        playerStaminaSlider.maxValue = CharController.maxStamina;
        playerHealthSlider.value = CharController.health;
        playerStaminaSlider.value = CharController.stamina;
    }

    void Update()
    {
        // Update AI sliders
        aiHealthSlider.value = aiController.health;
        aiStaminaSlider.value = aiController.stamina;

        // Update Player sliders
        playerHealthSlider.value = CharController.health;
        playerStaminaSlider.value = CharController.stamina;
    }
}
