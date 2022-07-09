using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TENTATIVE TOWER IDEAS

Earth - nuke style attacks with slight stun
Snow - aoe slow (50%) for <duration> (aoe might be implemented later instead of now)
Water - enemies become wet for <duration>, slightly slowed, stacks with snow
Electric - Chain up to <max>, slight stun, extra dmg to wet enemies (chain might be implemented later instead of now)
Fire - aoe dmg, if hits snowed/watered enemy will remove snow/water effect but deal extra dmg (aoe might be implemented later instead of now)
        logic for water dealing extra dmg I mean boiling water idk LMAO why not 
        Actually maybe just make it have a minor dmg reduction idk can just tweak values in `Enemy` class
Light - the line dmg tower (this would be a fcking pain so def not now)




(Just theorizing, not yet for now at least)
Earth tower upgrades:
    Significant dmg increase, stun only minimal
Snow tower upgrades:
    increase aoe radius
    maybe minimal dmg increase ig
    increase snowed duration
    (Increasing slow effect would require convoluted logic so maybe fixing it at 50% slow is better)
Water tower upgrades:
    minimal dmg increase, water tower is meant more to complement the other towers
    increase wet duration


Plasma tower could also work as the "Basic TRT"
*/


public abstract class AtkTower : TRT
{
    // Set in Unity, contains a reference to the projectile of the tower
    // public GameObject Projectile;
    // Moved back to indiv towers


    // Set these 3 in Unity
    // Cost of the turret
    public int cost;
    // Time between attacks
    public float tBetAtks;
    // Range
    public float range;

    // Position of the turret
    internal Vector2 towerPos;

    // CD till next atk
    internal float cooldown;

    // Orients the TRT to face the enemy
    protected void LookAtEnemy()
    {
        Vector2 towerPos = gameObject.transform.position;
        Enemy targetToLookAt = GlobalVariables.enemyList.findTarget(towerPos, range);

        if (targetToLookAt != null)
        {
            //transform.Rotate(new Vector3(0, 0, 50));
            // Transform target = GlobalVariables.enemyList[0].transform;
            //gameObject.transform.LookAt(target);

            Transform target = targetToLookAt.transform;
            float angle = 0;
            Vector3 relative = transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.Rotate(0, 0, -angle);
        }
    }
}
