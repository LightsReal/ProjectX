using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health, IDamageable
{
    public override void Damage()
    {
        base.Damage();
        
    }
    
    void IDamageable.TakeDamage(int damage)
    {
        health =- damage;
    }
}