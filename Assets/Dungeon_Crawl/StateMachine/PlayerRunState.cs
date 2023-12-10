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
        FaceMouse();
        AdaptiveLegMovement();
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() {


        AdaptiveLegMovement();
        

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

    void AdaptiveLegMovement()
    {
        Ctx.Move(Ctx.MoveAnimation);
    }


}
