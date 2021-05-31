using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //Assingables
    public Transform attackPoint;

    //Other
    private Rigidbody2D rb;
    private PlayerStates playerStates;

    //Attacking
    public int attackDamage;
    public float attackRange;

    //Input
    private bool attacking;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        //Attacking
        if (Input.GetMouseButton(0) && attacking == false) { 
            StartCoroutine(AttackOrder());
        }
    }

    private IEnumerator AttackOrder() 
    {
        if (attacking == true) yield break;

        attacking = true;

        rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        playerStates.movementState = PlayerStates.MovementStates.Attacking;

        yield return new WaitForSeconds(0.41f); //For animation

        Attack();

        attacking = false;  

        new WaitForSeconds(0.6f); //For CoolDown 

        yield break;
    }

    public void Attack()
    {
        Collider2D[] hitResults = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange);

        if (hitResults == null)
            return;
        foreach (Collider2D hit in hitResults)
        {
            var Damageable = hit.GetComponent<IDamageable<int>>();
            if (Damageable == null) 
                return;
            else
            {
                Damageable.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

}