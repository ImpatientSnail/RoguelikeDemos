using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsMoveHash, false);
    }
    public override void UpdateState()
    {
        // Debug.Log("IDLE");
        CheckSwitchStates();
    }
    public override void ExitState() {}
    public override void CheckSwitchStates()
    {
        if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Run());
        }
    }
    public override void InitializeSubState() { }
}
