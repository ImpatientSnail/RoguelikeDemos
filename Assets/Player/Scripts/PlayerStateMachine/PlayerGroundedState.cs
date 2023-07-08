using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState() {}

    public override void UpdateState()
    {
        // Debug.Log("GROUNDED");
        // line for actual movement
        Ctx.Velocity = Ctx.CurrentMovementInput * Ctx.Speed;
        CheckSwitchStates();
        //Debug.Log("Switching to Jumping State from Grounded");
    }

    public override void ExitState() {}
    
    public override void CheckSwitchStates()
    {
        if (Ctx.IsJumpPressed || !Ctx.IsGrounded)
        {
            if (Ctx.IsJumpPressed)
            {
                Ctx.IsJumping = true;
            }
            SwitchState(Factory.Jump());
        }
    }
    
    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed)
        {
            //Debug.Log("Initializing Idle");
            SetSubState(Factory.Idle());
        } else if (Ctx.IsMovementPressed)
        {
            //Debug.Log("Initializing Move");
            SetSubState(Factory.Run());
        }
    }
}
