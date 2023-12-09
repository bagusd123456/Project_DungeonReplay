using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Ctx.Animator.SetTrigger("isDashing");
        Ctx.IsDashing = true;
        Ctx.IsAbleToDash = false;
        HandleDash();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsDashing && !Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (!Ctx.IsDashing && Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Run());
        }

        if (Ctx.IsSwitchingWeapon)
        {
            SwitchState(Factory.SwitchWeapon());
        }

        if (Ctx.PlayerHealth.damaged)
        {
            SwitchState(Factory.Hit());
        }
    }

    void HandleDash()
    {
        Ctx.Rigidbody.AddForce(Ctx.transform.forward * Ctx.DashSpeed, ForceMode.Impulse);
        //Ctx.IsDashing = false;
    }
}
