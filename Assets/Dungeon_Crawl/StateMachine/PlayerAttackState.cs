using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        Ctx.IsAttacking = true;

        Ctx.Animator.SetBool("isWalking", false);

        Debug.Log("Iam attacking!");
        Ctx.Animator.SetTrigger("isAttacking");
    }

    public override void UpdateState()
    {
        //FaceMouse();
        
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() {
        
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsAttackPressed)
        {
            Ctx.Animator.ResetTrigger("isAttacking");

            if (Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Run());
                Debug.Log("Iam exiting attack");
            }
            else if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
                Debug.Log("Iam exiting attack");
            }
        } else
        {
            SwitchState(Factory.Attack());
        }

        if (Ctx.PlayerHealth.damaged)
        {
            SwitchState(Factory.Hit());
        }

    }

    // Logic for Player faces the mouse when clicking
    void FaceMouse()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, Ctx.CamRayLength, Ctx.FloorMask))
        {
            Vector3 playerToMouse = floorHit.point - Ctx.transform.position;
            playerToMouse.y = 0;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            Ctx.Rigidbody.MoveRotation(newRotation);
        }
    }

    // logic to shoot projectile from weapon
    void ShootWeapon()
    {
        
    }
}
