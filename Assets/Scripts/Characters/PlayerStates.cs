using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public MovementStates movementState;
    public FacingDirection facingDirection;
    public Position position;
    
    private Rigidbody2D rb;   
    private SpriteRenderer sprite;
    private PlayerAnimation playerAnimation;

    public enum MovementStates
    {
        Idle,
        Running,
        Jumping,
        Dashing,
        Crouching,
        Attacking,
        Healing
    }

    public enum FacingDirection 
    {
        Right,
        Left
    }

    public enum Position
    {
        InAir,
        OnGround    
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }    
    
    private void FixedUpdate() 
    {
        if (rb.velocity.x > 0)
            facingDirection = FacingDirection.Right;
        if (rb.velocity.x < 0)
            facingDirection = FacingDirection.Left;

        if (rb.velocity.y == 0)
            position = Position.OnGround;
        if (rb.velocity.y != 0)
            position = Position.InAir;
    }

    private void Update() 
    {
        SetPlayerState();
    }

    public void SetPlayerState()
    {
        if (position == Position.OnGround && rb.velocity.y == 0)
        {
            if (movementState != MovementStates.Crouching)
            {
                if (rb.velocity.x == 0)
                {
                movementState = MovementStates.Idle;
                }
                else if (rb.velocity.x > 0)
                {
                facingDirection = FacingDirection.Right;
                movementState = MovementStates.Running;
                }
                else if (rb.velocity.x < 0)
                {
                facingDirection = FacingDirection.Left;
                movementState = MovementStates.Running;
                }
            }
            if (movementState == MovementStates.Crouching && rb.velocity.x == 0)
            {
                movementState = MovementStates.Crouching;
            }
        }
        
        if (position == Position.InAir)
        {
            movementState = MovementStates.Jumping;

            if (rb.velocity.x < 0)
            {
                facingDirection = FacingDirection.Left;
            }
            else if (rb.velocity.x > 0)
            {
                facingDirection = FacingDirection.Right;
            }
        }
    }

    private void PlayAnimationsBasedOnState() 
    {
        switch (movementState)
        {
            case MovementStates.Idle:
                playerAnimation.PlayIdleAnim();
                break;
            case MovementStates.Running:
                playerAnimation.PlayRunningAnim();
                break;
            case MovementStates.Jumping:
                playerAnimation.PlayJumpingAnim();
                break;
            case MovementStates.Attacking: 
                playerAnimation.TriggerAttackAnimation();
                break;
            case MovementStates.Healing:
                break;
            default:
                break;
        }
    }

    private void SetCharacterDirection()
    {
        switch (facingDirection)
        {
            case FacingDirection.Right:
                sprite.flipX = false;
                break;
            case FacingDirection.Left:
                sprite.flipX = true;
                break;
        }
    }
}