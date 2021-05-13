using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //Assingables
    public Transform attackPoint;
    public LayerMask enemyLayers;

    //Other
    private Rigidbody2D rb;
    private PlayerStates playerStates;

    //Attacking
    public float attackDamage;
    public float attackRange;

    //Input
    private bool attacking, healing;


    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
    }

    void FixedUpdate() {
        Movement();
    }

    void Update() {
        MyInput();
    }

    /// <summary>
    /// Find user input
    /// </summary>
    private void MyInput() {
        attacking = Input.GetMouseButton(0);
    }

    private void Movement() {
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

    public void Attack() {
        Collider2D[] hitResults = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayers);

        if (hitResults == null)
            return;
        foreach (Collider2D hit in hitResults)
        {
            var Damageable = hit.GetComponent<IDamageable>();
            if (Damageable == null) return;
            Damageable.TakeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

}