using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState() { 
        Ctx.Animator.SetBool("isWalking", true);
        Debug.Log("I am Running!");
    }

    public override void UpdateState() {
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementY = Ctx.CurrentMovementInput.y;
        CheckSwitchState();
    }

    public override void ExitState() {
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementY = 0;
        

    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState() {
        if (Ctx.IsAttackPressed)
        {
            SwitchState(Factory.Attack());
            Ctx.Animator.SetBool("isWalking", false);

        }

        if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        } else if (Ctx.IsDashPressed && Ctx.IsAbleToDash)
        {
            SwitchState(Factory.Dash());
        }

        if (Ctx.IsSwitchingWeapon)
        {
            SwitchState(Factory.SwitchWeapon());
        }

        if (Ctx.PlayerHealth.damaged)
        {
            SwitchState(Factory.Hit());
            Ctx.Animator.SetBool("isWalking", false);
        }


    }
}
