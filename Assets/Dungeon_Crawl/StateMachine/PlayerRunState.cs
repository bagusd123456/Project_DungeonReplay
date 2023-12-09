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
        //AdaptiveLegMovement();
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() {

        //AdaptiveLegMovement();
        

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

    /*void AdaptiveLegMovement()
    {
        // Get Player Input Value
        float playerVecticalInput = Ctx.CurrentMovementInput.y;
        float playerHorizontalInput = Ctx.CurrentMovementInput.x;

        // Get Camera Directional Vectors
        Vector3 forward = Ctx.transform.forward;
        Vector3 right = Ctx.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        // Create Direction-Relative Input Vector
        Vector3 forwardRelativeVecticalInput = playerVecticalInput * forward;
        Vector3 rightRelativeVecticalInput = playerHorizontalInput * right;

        Vector3 playerRelativeDirections = forwardRelativeVecticalInput + rightRelativeVecticalInput;

        //Debug.Log(Ctx.Rigidbody.velocity.magnitude);
        Ctx.Animator.SetFloat("Velocity X", Mathf.RoundToInt(playerRelativeDirections.x));
        Ctx.Animator.SetFloat("Velocity Z", Mathf.RoundToInt(playerRelativeDirections.z));
    }*/


}
