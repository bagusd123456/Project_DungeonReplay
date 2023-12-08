using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        FaceMouse();
        Ctx.Animator.SetTrigger("isAttacking");
        Ctx.IsAttacking = true;

    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsAttacking && !Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        } else if (!Ctx.IsAttacking && Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Run());

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
}
