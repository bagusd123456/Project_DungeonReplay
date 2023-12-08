using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public int medkit;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            PlayerInventory inventory = gameObject.GetComponent<PlayerInventory>();
            if (inventory.medkit > 0)
            {
                HealPlayer();
                inventory.medkit--;
            }
        }
    }

    public void HealPlayer()
    {
        PlayerHealth playerHealth = gameObject.GetComponent<PlayerHealth>();
        if (playerHealth.currentHealth < 100)
            playerHealth.currentHealth += 20;
    }
}
