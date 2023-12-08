using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState() {
        Ctx.Animator.SetBool("isWalking", false);
        Debug.Log("I am Standing.");
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementY = 0;
    }

    public override void UpdateState() {
        CheckSwitchState();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState() {
        if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Run());
        } else if (!Ctx.IsMovementPressed && Ctx.IsAttackPressed)
        {
            SwitchState(Factory.Attack());
        } else if (Ctx.IsDashPressed && Ctx.IsAbleToDash)
        {
            SwitchState(Factory.Dash());
        }
    }
}
