using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitSubState : PlayerBaseState
{
    public PlayerHitSubState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("I got hit!!");
        Ctx.Animator.SetTrigger("gotHit");
    }

    public override void UpdateState()
    {
        //FaceMouse();
        Ctx.PlayerHealth.damaged = false;

        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if (Ctx.PlayerHealth.currentHealth <= 0)
        {
            SwitchState(Factory.Death());
        }else if (!Ctx.PlayerHealth.damaged)
        {
            if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Run());
            }
        }
    }
}
