using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // Layer mask for checking what is considered grounded (Ground layer)
    [SerializeField] LayerMask groundLayerMask;

    // Componenets
    PlayerInput playerInput;
    Rigidbody2D rigidBody2D;
    Animator animator;
    BoxCollider2D boxCollider2D;

    // input variables
    Vector2 currentMovementInput;
    bool isMovementPressed;
    bool isFacingLeft = true;

    // Move variables
    [SerializeField] float speed = 8f;

    // jump variables
    bool isJumpPressed = false;
    [SerializeField] float initialJumpVelocityY = 20f;
    [SerializeField] float initialJumpVelocityX = 8f;
    bool isJumping = false;
    [SerializeField] float gravityScale = 9.8f;
    float extraHeightText = .1f;

    // Animator hash for optimization
    int isMoveHash;
    int isJumpHash;

    // State context
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // Setters and Getters
    public Animator Animator { get { return animator; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsJumpPressed { get { return isJumpPressed; } }
    public float InitialJumpVelocityY { get { return initialJumpVelocityY; } }
    public float InitialJumpVelocityX { get { return initialJumpVelocityX; } }
    public Vector2 CurrentMovementInput { get { return currentMovementInput; } set { currentMovementInput = value; } }
    public float CurrentMovementInputY { get { return currentMovementInput.y; } set { currentMovementInput.y = value; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; }  }
    public bool IsGrounded { get { return isGrounded(); } }
    public Vector2 Velocity { set { rigidBody2D.velocity = value; } }
    public float Speed { get { return speed; } }
    public bool IsMovementPressed { get { return isMovementPressed; } }
    public int IsMoveHash { get { return isMoveHash; } }
    public int IsJumpHash { get { return isJumpHash; } }

    // Universal functions
    bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, groundLayerMask);
        
        // only for debugging. Make sure gizmos are on in the game tab
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2D.bounds.extents.x) * 2, rayColor);
        //Debug.Log(raycastHit.collider != null);
        // end of debugging code

        return raycastHit.collider != null;
    }

    void handleFlip()
    {
        if (isFacingLeft == true && currentMovementInput.x > 0 || isFacingLeft == false && currentMovementInput.x < 0)
        {
            isFacingLeft = !isFacingLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Input reading
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    // MonoBehavior Functions
    void Awake()
    {
        // component initialization
        playerInput = new PlayerInput();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        // animator hash initialization
        isMoveHash = Animator.StringToHash("isMove");
        isJumpHash = Animator.StringToHash("isJump");

        // input callbacks
        playerInput.Controls.Movement.started += OnMovementInput;
        playerInput.Controls.Movement.canceled += OnMovementInput;
        playerInput.Controls.Movement.performed += OnMovementInput;
        playerInput.Controls.Jump.started += OnJump;
        playerInput.Controls.Jump.canceled += OnJump;

        // state machine
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        // component initialization
        rigidBody2D.gravityScale = gravityScale;
        //
    }

    void Start()
    {

    }

    void Update()
    {
        handleFlip();
        _currentState.UpdateStates();
    }

    // New Unity Input Functions
    void OnEnable()
    {
        playerInput.Controls.Enable();
    }

    void OnDisable()
    {
        playerInput.Controls.Disable();
    }
}
