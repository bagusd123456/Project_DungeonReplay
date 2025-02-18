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
    PlayerShooting _playerShootingScript;
    PlayerHealth _playerHealth;

    // variables to store player input values
    Vector2 _currentMovementInput;
    Vector3 _appliedMovement;
    bool _isMovementPressed;
    bool _isAttackPressed = false;
    bool _isAttacking = false;
    bool _isDashPressed = false;
    bool _isDashing = false;
    bool _isAbleToDash = true;
    bool _isSwitchingWeapon = false;
    float _currentWeapon = 1;
    bool isMovementPerformed = false;
    bool _isDead = false;


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
    Vector3 position;

    Vector3 _camForward;
    Vector3 _move;
    Vector3 _moveInput;
    float _forwardAmount;
    float _turnAmount;



    [Header("Weapon List")]
    public List<GameObject> _weaponList;
    float _currentWeaponFirerate;

    // variables responsible for camera
    float _camRayLength = 100f;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // getters and setters
    #region getters and setters
    
    public Animator Animator { get { return _animator; } }
    public Rigidbody Rigidbody { get { return _rigidBody; } set { _rigidBody = value; } }
    public Camera Camera { get { return _camera; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Vector3 CameraRelativeDirections { get { return cameraRelativeDirections; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } set { _isMovementPressed = value; } }
    public bool IsAttackPressed { get { return _isAttackPressed; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsDashPressed { get { return _isDashPressed; } }
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
    public float DashSpeed { get { return _dashSpeed; } }
    public bool IsAbleToDash { get { return _isAbleToDash; } set { _isAbleToDash = value; } }
    public List<GameObject> WeaponList { get { return _weaponList; } }
    public bool IsSwitchingWeapon { get { return _isSwitchingWeapon; } }
    public float CurrentWeapon { get { return _currentWeapon; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public PlayerShooting PlayerShootingScript { get { return _playerShootingScript; } }
    public PlayerHealth PlayerHealth { get { return _playerHealth; } set { _playerHealth = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float CamRayLength { get { return _camRayLength; } set { _camRayLength = value; } }
    public LayerMask FloorMask { get { return _groundMask; } set { _groundMask = value; } }
    public Vector3 MoveAnimation { get { return _move; } }
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    #endregion
    private void Awake()
    {
        // initially set reference variables
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _playerShootingScript = GetComponent<PlayerShooting>();
        _playerHealth = GetComponent<PlayerHealth>();

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
        _playerInput.Player.SwitchWeapon.started += OnWeaponSwitchInput;
        _playerInput.Player.SwitchWeapon.started += OnSwitchWeaponCheck;
        _playerInput.Player.SwitchWeapon.canceled += OnSwitchWeaponCheck;
        _playerInput.Player.Interact.started += OnCollectAction;
        _playerInput.Player.Interact.canceled += OnCollectAction;
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        if (!_isDead)
        {
            //HandleRotation();
            _isMovementPressed = _playerInput.Player.Movement.inProgress;
        }
        
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
        if (!_isDead)
        {
            // Calls the Movement Logic
            HandleMovement();
        }

        // Handles Local Relative Input for Blend Tree
        if (_camera != null)
        {
            _camForward = Vector3.Scale(_camera.transform.up, new Vector3(1, 0, 1)).normalized;
            _move = _appliedMovement.y * _camForward + _appliedMovement.x * _camera.transform.right;

        } else
        {
            _move = _appliedMovement.y * Vector3.forward + _appliedMovement.x * Vector3.right;
        }

        if (_move.magnitude >   1)
        {
            _move.Normalize();
        }
    }

    public void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this._moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(_moveInput);

        _turnAmount = localMove.x;
        _forwardAmount = localMove.z;
    }

    void UpdateAnimator()
    {
        _animator.SetFloat("Forward", _forwardAmount, .1f, Time.deltaTime);
        _animator.SetFloat("Turn", _turnAmount, .1f, Time.deltaTime);
    }

    // Handles Player Rotation
    void HandleRotation()
    {
        if (!_isAttackPressed)
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
        } else
        {
            FaceMouse();
        }
        
    }

    public void FaceMouse()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, Mathf.Infinity))
        {
            /*Vector3 playerToMouse = floorHit.point - Ctx.transform.position;
            playerToMouse.y = 0;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            Ctx.Rigidbody.MoveRotation(newRotation);*/

            position = new Vector3(floorHit.point.x, 0f, floorHit.point.z);
            position.y = 0;

        }

        Quaternion transRot = Quaternion.LookRotation(position - transform.position);
        transform.rotation = Quaternion.Lerp(transRot, transform.rotation, 0f);
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
    void CheckMovementIsPressed()
    {
        _isAttacking = false;
    }

    // Trigger Spawn Projectile Event
    void SpawnProjectile()
    {
        PlayerShooting.Instance.TriggerShoot();
    }

    // Gives a short cooldown on Dashes
    IEnumerator DashCooldown()
    {
        _isDashing = false;

        yield return new WaitForSeconds(.5f);

        _isAbleToDash = true;
    }

    // below is Input Managers for the new Unity Input System
    // P.S: its actually really helpful but really complicated to set up
    #region Input System

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

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
    }

    void OnDashInput(InputAction.CallbackContext context)
    {
        _isDashPressed = context.ReadValueAsButton();
    }

    void OnAttackInput(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
    }

    #region Weapon Switching Input Logic
    void OnSwitchWeaponCheck(InputAction.CallbackContext context)
    {
        _isSwitchingWeapon = context.ReadValueAsButton();
    }

    void OnWeaponSwitchInput(InputAction.CallbackContext context)
    {
        _currentWeapon = context.ReadValue<float>();
    }
    #endregion 

    private void OnEnable()
    {
        // enable the character controls action map
        _playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        // disable the character controls action map
        _playerInput.Player.Disable();
    }
    #endregion
}
