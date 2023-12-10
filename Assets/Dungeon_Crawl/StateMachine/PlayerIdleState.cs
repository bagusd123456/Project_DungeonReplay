using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState() {
        Ctx.Animator.SetBool("isWalking", false);
        Ctx.Animator.SetFloat("Forward", 0);
        Ctx.Animator.SetFloat("Turn", 0);
        Debug.Log("I am Standing.");
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementY = 0;
        Ctx.Move(Ctx.MoveAnimation);
    }

    public override void UpdateState() {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState() {
        if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Run());
        } else if (Ctx.IsDashPressed && Ctx.IsAbleToDash)
        {
            SwitchState(Factory.Dash());
        }

        if (Ctx.IsSwitchingWeapon)
        {
            SwitchState(Factory.SwitchWeapon());
        }

        if (Ctx.IsAttackPressed)
        {
            SwitchState(Factory.Attack());
        }

        if (Ctx.PlayerHealth.damaged)
        {
            SwitchState(Factory.Hit());
        }
    }
}
