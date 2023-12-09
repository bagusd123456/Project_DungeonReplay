using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Ctx.Animator.SetBool("isDead", true);
        Ctx.IsDead = true;
    }

    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() { }

    public override void InitializeSubState()
    {
        
    }

    public override void CheckSwitchState()
    {
    }
}
