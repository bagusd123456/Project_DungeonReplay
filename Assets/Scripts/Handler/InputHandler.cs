using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    public PlayerInput playerInput;
    private bool isInteractPressed;
    private void Awake()
    {
        playerInput = new PlayerInput();

        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //playerInput.Player.Interact.started += OnCollectAction;
    }

    public void OnCollectAction(InputAction.CallbackContext ctx)
    {
        if (PlayerHealth.Instance.detectedItem != null)
        {
            PlayerHealth.Instance.CollectItem();
        }

        if (PlayerHealth.Instance.detectedWeapon != null)
        {
            PlayerHealth.Instance.CollectItem();
        }
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Interact.started += OnCollectAction;
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
        playerInput.Player.Interact.started -= OnCollectAction;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractPressed)
        {
            
        }
    }
}
