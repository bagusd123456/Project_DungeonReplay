using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) {
            IsRootState = true;
        }
    public override void EnterState()
    {
        InitializeSubState();
        Debug.Log("Iam currently grounded");
    }

    public override void UpdateState() { 
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() { }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Idle());
        } else if (Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Run());
        } else if (Ctx.IsDashPressed && Ctx.IsAbleToDash)
        {
            SetSubState(Factory.Dash());
        }

        if (Ctx.IsSwitchingWeapon)
        {
            SetSubState(Factory.SwitchWeapon());
        }

        if (Ctx.IsAttackPressed)
        {
            SetSubState(Factory.Attack());
        }

        if (Ctx.PlayerHealth.damaged)
        {
            SetSubState(Factory.Hit());
        }
    }

    public override void CheckSwitchState()
    {
    }
}
