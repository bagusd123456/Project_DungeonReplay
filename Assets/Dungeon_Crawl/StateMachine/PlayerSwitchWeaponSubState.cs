using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchWeaponSubState : PlayerBaseState
{
    public PlayerSwitchWeaponSubState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Ctx.Animator.SetTrigger("isSwitching");
        SwitchToWeapon();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsSwitchingWeapon)
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

    void SwitchToWeapon()
    {
        for (int i = 0; i <= Ctx.WeaponList.Count - 1;)
        {

            if (i == Ctx.CurrentWeapon - 1)
            {
                Ctx.WeaponList[i].SetActive(true);
            } else
            {
                Ctx.WeaponList[i].SetActive(false);
            }
            i++;
        }
    }
}
