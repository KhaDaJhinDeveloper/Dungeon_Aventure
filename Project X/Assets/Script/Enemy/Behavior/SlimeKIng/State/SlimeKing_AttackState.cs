using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing_AttackState : IState
{
    private Vector3 target;
    private bool isJumping;
    private SlimeKingController controller;
    private float delayBetweenJumps;
    public SlimeKing_AttackState(SlimeKingController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        this.isJumping = false;
        this.delayBetweenJumps = this.controller.DelayBetweenJumps;
    }

    public void Execute()
    {
        if (!this.isJumping) 
            this.controller.StartCoroutine(MultilJumpCoroutine());
    }

    public void Exit()
    {
        
    }
    private IEnumerator MultilJumpCoroutine()
    {
        for (int i = 0; i < this.controller.NumbersOfJump; i++)
        {
            this.target = this.controller.Player.transform.position;
            this.controller.JumpUpDuration -= this.controller.JumpUpDuration * 0.25f;
            //this.controller.JumpHeight += this.controller.JumpHeight * 0.25f;
            this.delayBetweenJumps -= this.delayBetweenJumps * 0.25f;
            yield return SingleJumpCoroutine(this.target);
            if(i < this.controller.NumbersOfJump - 1)
            {
                yield return new WaitForSeconds(delayBetweenJumps);
            }                
        }
        isJumping = true;
        this.controller.Ani.SetBool("falldown", false);
        this.controller.SetUp();
        this.controller.EnabledCollider2D();
    }
    private IEnumerator SingleJumpCoroutine(Vector3 targetPosition)
    {
        isJumping = true;
        this.controller.Ani.SetBool("falldown", false);
        this.controller.Ani.SetBool("jumpup", true);
        this.controller.DisabledCollider2D();
        Vector3 startPos = this.controller.transform.position;
        Vector3 aboveTargetPos = new Vector3(targetPosition.x,
                                             targetPosition.y + this.controller.JumpHeight,
                                             targetPosition.z);
        float elapsedTime = 0f;
        while (elapsedTime < this.controller.JumpUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / this.controller.JumpUpDuration;
            float smoothTime = 1f - Mathf.Pow(1f - t, 2f);
            this.controller.transform.position = Vector3.Lerp(startPos, aboveTargetPos, smoothTime);
            yield return null;
        }
        this.controller.transform.position = aboveTargetPos;
        yield return new WaitForSeconds(this.controller.HoverDuration);

        elapsedTime = 0f;
        this.controller.Ani.SetBool("jumpup", false);
        this.controller.Ani.SetBool("falldown", true);
        while (elapsedTime < this.controller.FallDownDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / this.controller.FallDownDuration;
            float smoothTime = Mathf.Pow(t, 2f);
            this.controller.transform.position = Vector3.Lerp(aboveTargetPos, targetPosition, smoothTime);
            yield return null;
        }
        this.controller.transform.position = targetPosition;
        this.controller.EnabledCollider2D();
        Onlaned();
    }
    void Onlaned()
    {
        GameObject crevice = ObjectPooling.ObjectPooling_Instance.GetPool(KeyPool.KEY_VFX_CREVICE);
        crevice.transform.transform.position = this.controller.transform.position;
    }
}
