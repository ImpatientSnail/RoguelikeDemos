using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // new
    PlayerInput playerInput;
    Rigidbody2D rigidBody2D;
    Animator animator;

    // input variables
    Vector2 currentMovementInput;
    bool isMovementPressed;
    bool isFacingLeft = true;

    //move variables
    float speed = 50.0f;

    // jump variables
    bool _isJumpPressed = false;
    float initialJumpVelocity = 3.0f;
    bool isJumping = false;

    //end new

    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // setters and getters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public float InitialJumpVelocity { get { return initialJumpVelocity; } }
    public Vector2 CurrentMovementInput { get { return currentMovementInput; } set { currentMovementInput = value; } }
    public float CurrentMovementInputY { get { return currentMovementInput.y; } set { currentMovementInput.y = value; } }
    public bool IsJumping { set { isJumping = value; }  }

    void Awake()
    {
        // movement state
        playerInput = new PlayerInput();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //

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
        //
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
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        Debug.Log(_isJumpPressed);
    }

    void handleAnimation()
    {
        bool isMove = animator.GetBool("isMove");

        if (isMovementPressed)
        {
            animator.SetBool("isMove", true);
        }
        else if (!isMovementPressed)
        {
            animator.SetBool("isMove", false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //movement
        handleFlip();
        //Debug.Log(currentMovementInput * Time.deltaTime * 50);
        handleAnimation();
        rigidBody2D.AddForce(currentMovementInput * speed - rigidBody2D.velocity);
        //rigidBody2D.MovePosition(rigidBody2D.position + currentMovementInput * Time.deltaTime * 50);
        //
        _currentState.UpdateStates();
        _currentState.UpdateState();
        // jump for now
        /*if(isJumping)
        {
            Debug.Log("JUMPING");
            rigidBody2D.AddForce(new Vector2(0f, 1f), ForceMode2D.Impulse);
        }*/
    }

    void OnEnable()
    {
        playerInput.Controls.Enable();
    }

    void OnDisable()
    {
        playerInput.Controls.Disable();
    }
}
