using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health, IDamageable<int>
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}