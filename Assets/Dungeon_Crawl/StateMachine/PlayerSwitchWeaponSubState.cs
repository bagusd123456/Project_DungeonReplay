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
        Debug.Log("Is switching weapon");
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

        if (Ctx.PlayerHealth.damaged)
        {
            SwitchState(Factory.Hit());
        }
    }

    void SwitchToWeapon()
    {
        if (Ctx.PlayerShootingScript.loadoutDataArray.Count > 1)
        {
            for (int i = 0; i <= Ctx.WeaponList.Count - 1;)
            {

                if (i == Ctx.CurrentWeapon - 1)
                {
                    Ctx.WeaponList[i].SetActive(true);
                    Ctx.PlayerShootingScript.WeaponSwitch(i);
                    WeaponData currentWeaponData = Ctx.PlayerShootingScript.loadoutDataArray[i].weaponData;
                }
                else
                {
                    Ctx.WeaponList[i].SetActive(false);
                }
                i++;
            }
        }
        
    }
}
