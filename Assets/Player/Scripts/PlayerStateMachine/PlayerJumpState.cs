using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    
    public override void EnterState()
    {
        if (Ctx.IsJumping)
        {
            Ctx.Animator.SetBool(Ctx.IsJumpHash, true);
            HandleJump();
            Ctx.IsJumping = false;
        }
    }
    
    public override void UpdateState()
    {
        Debug.Log("Jumping");
        CheckSwitchStates();
    }
    
    public override void ExitState()
    {
        // quick fix to make it so moving animation isn't being done when landing and switching to idle state
        Ctx.Animator.SetBool(Ctx.IsJumpHash, false);
        if (!Ctx.IsMovementPressed)
        {
            Ctx.Animator.SetBool(Ctx.IsMoveHash, false);
        } else if (Ctx.IsMovementPressed) // quick fix to make it so falling off a ledge and landing while moving doesn't cause idle animation to stick
        {
            Ctx.Animator.SetBool(Ctx.IsMoveHash, true);
        }
    }
    
    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded)
        {
            // Debug.Log("Switching to Grounded State from Jumping");
            SwitchState(Factory.Grounded());
        }
    }
    
    public override void InitializeSubState() { }
    
    void HandleJump()
    {
        Ctx.Velocity = Vector2.up * Ctx.InitialJumpVelocityY;
        if (Ctx.CurrentMovementInput != null)
        {
            Ctx.Velocity = Ctx.CurrentMovementInput * Ctx.InitialJumpVelocityX + Vector2.up * Ctx.InitialJumpVelocityY;
        }
        //Ctx.CurrentMovementInputY = Ctx.InitialJumpVelocity;
    }
}
