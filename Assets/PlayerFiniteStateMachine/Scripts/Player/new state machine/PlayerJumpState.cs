using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
        InitializeSubState();
    }
    
    public override void EnterState()
    {
        HandleJump();
    }
    
    public override void UpdateState() { }
    
    public override void ExitState() { }
    
    public override void CheckSwitchStates() { }
    
    public override void InitializeSubState() { }
    
    void HandleJump()
    {
        /* WORKING ON THIS
        _ctx.IsJumping = true;
        _ctx.CurrentMovementInputY = _ctx.InitialJumpVelocity;
        */
    }
}
