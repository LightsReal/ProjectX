using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayIdleAnim()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isCrouching", false);
    }

    public void PlayRunningAnim()
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("isJumping", false);
        animator.SetBool("isCrouching", false);
    }

    public void PlayJumpingAnim()
    {
        animator.SetBool("isJumping", true);
        animator.SetBool("isCrouching", false);
    }

        public void PlayCrouchingAnim()
    {
        animator.SetBool("isCrouching", true);
        animator.SetBool("isJumping", false);
    }

    public void StopRunningAnim()
    {
        animator.SetBool("isRunning", false);
    }

    public void StopJumpingAnim()
    {
        animator.SetBool("isJumping", false);
    }

    public void StopCrouchingAnim()
    {
        animator.SetBool("isCrouching", false);
    }

    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attacking");
    }

    public void TriggerHealAnimation()
    {
        animator.SetTrigger("Healing");
    }

}