using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }


    Vector3 previousVelocity;
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

    public override void ExitState() {
        Ctx.Rigidbody.velocity = previousVelocity;

    }

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
        previousVelocity = Ctx.Rigidbody.velocity;

        Ctx.Rigidbody.velocity = Ctx.CameraRelativeDirections * Ctx.DashSpeed;
        //Ctx.IsDashing = false;
    }
}
