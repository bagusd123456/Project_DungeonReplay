using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // environment variables
    [SerializeField] LayerMask _groundMask;

    // declare reference variables
    PlayerInput _playerInput;
    CharacterController _characterController;
    Rigidbody _rigidBody;
    Animator _animator;
    Camera _camera;

    // variables to store player input values
    Vector2 _currentMovementInput;
    Vector3 _appliedMovement;
    bool _isMovementPressed;
    bool _isAttackPressed = false;
    bool _isAttacking = false;
    bool _isDashPressed = false;
    bool _isDashing = false;
    bool _isAbleToDash = true;

    // variables responsible for player movements
    [Header("Movements Variables")]
    public float _moveSpeed;
    public float _dashSpeed;
    public float _attackDashSpeed;
    public float _rotationFactorPerFrame = 1.0f;
    [SerializeField] float _movementSmoothing = .2f;
    Vector3 cameraRelativeDirections;
    Vector2 _currentInputVector;
    Vector2 _smoothInputVelocity;

    // variables responsible for camera
    float _camRayLength = 100f;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // getters and setters
    public Animator Animator { get { return _animator; } }
    public Rigidbody Rigidbody { get { return _rigidBody; } set { _rigidBody = value; } }
    public Camera Camera { get { return _camera; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsAttackPressed { get { return _isAttackPressed; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsDashPressed { get { return _isDashPressed; } }
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
    public float DashSpeed { get { return _dashSpeed; } }
    public bool IsAbleToDash { get { return _isAbleToDash; } set { _isAbleToDash = value; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float CamRayLength { get { return _camRayLength; } set { _camRayLength = value; } }
    public LayerMask FloorMask { get { return _groundMask; } set { _groundMask = value; } }

    private void Awake()
    {
        // initially set reference variables
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;

        // setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        // set the player input callbacks
        _playerInput.Player.Movement.started += OnMovementInput;
        _playerInput.Player.Movement.canceled += OnMovementInput;
        _playerInput.Player.Movement.performed += OnMovementInput;
        _playerInput.Player.Attack.started += OnAttackInput;
        _playerInput.Player.Attack.canceled += OnAttackInput;
        _playerInput.Player.Dash.started += OnDashInput;
        _playerInput.Player.Dash.canceled += OnDashInput;

    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        HandleRotation();
        
    }

    private void FixedUpdate()
    {
        // Calls the Movement Logic
        HandleMovement();
    }

    // Handles Player Rotation
    void HandleRotation()
    {
        // Get Camera Relative Direction based on player input
        CameraRelativeControls(_currentMovementInput);

        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            // creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeDirections, Vector3.up);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // Handles player Movements
    void HandleMovement()
    {
        // Get Camera Relative Directions based on player input
        CameraRelativeControls(_appliedMovement);
       
        // IT IS THE WALKING BIT
        _rigidBody.MovePosition(transform.position + (cameraRelativeDirections * _moveSpeed * Time.deltaTime));
    }

    // Handles Camera Based Directions
    void CameraRelativeControls(Vector3 acceptedInput)
    {
        _currentInputVector = Vector2.SmoothDamp(_currentInputVector, acceptedInput, ref _smoothInputVelocity, _movementSmoothing);

        // Get Player Input Value
        float playerVecticalInput = _currentInputVector.y;
        float playerHorizontalInput = _currentInputVector.x;

        // Get Camera Directional Vectors
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        // Create Direction-Relative Input Vector
        Vector3 forwardRelativeVecticalInput = playerVecticalInput * forward;
        Vector3 rightRelativeVecticalInput = playerHorizontalInput * right;

        // Create Camera-Relative Movement
        cameraRelativeDirections = forwardRelativeVecticalInput + rightRelativeVecticalInput;
        
    }

    // Gives a short momentum boost when idle attacking
    void IdleAttackMomentum()
    {
        // Adds Force to Player
        _rigidBody.AddForce(transform.forward * _attackDashSpeed);

        // After done attacking, return to idle state
        _isAttacking = false;
    }

    // Gives a short cooldown on Dashes
    IEnumerator DashCooldown()
    {
        _isDashing = false;

        yield return new WaitForSeconds(.5f);

        _isAbleToDash = true;

    }


    #region Input System
    // below is Input Managers for the new Unity Input System
    // P.S: its actually really helpful but really complicated to set up
    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void OnDashInput(InputAction.CallbackContext context)
    {
        _isDashPressed = context.ReadValueAsButton();
    }

    void OnAttackInput(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        // enable the character controls action map
        _playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        // disable the character controls action map
        _playerInput?.Player.Disable();
    }
    #endregion
}
