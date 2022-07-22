using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Access modifiers for c#: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers

public - all scripts can access and can edit in unity
internal - all scripts should(?) be able to acees but cannot edit in unity
protected - all subclasses
*/

/* Current projectile behavior if target dies while en route (2022-6-15 night/2022-6-16 morning)
1.) Continue going to where the target died
2.) If the projectile has an aoe effect, produce the aoe effect on the spot of death
*/


// Abstract class for projectiles produced by atk towers 
// (5 eleemtan ns trts as of 2022-6-15-)
public abstract class Projectile : MonoBehaviour
{
    // internal float centi_speed;   // TO REMOVE

    // Speed of the projectile (SET IN UNITY)
    internal float speed;
    // Dmg of the projectile (SET IN UNITY)
    internal float dmg;
    // AoE radius, if any
    internal float AoeRadius;
    // Effect duration, if any (snow, water)
    internal int effectFrames;

    // Argument of the trajectory of the projectile (SET IN UNITY to og arg of sprite)
    // This is in degrees
    internal float arg;

    // Target of a projectile (set by the TRT firing this)
    internal Enemy target;
    // Position of the target; so that a reference still exists after the target dies
    internal Vector2 targetPos;

    // For animations, if any
    public Animator animator;
    // For telling the projectile to do nothing 
    internal bool ended;


    // For passing as a parameter in Invoke for delayed destroy for animation
    internal void Unexist() {
        Destroy(gameObject);
    }
    

    // Angles the projectile towards the target (Does not apply to lazer or lightning)
    internal void AngleTowardsTarget() {
        // If target is dead, no change needed
        if (target != null)
        {
            // If target is alive, 
            this.AngleTowardsPosition(target.transform.position);
        }
    }

    // Angles the projectile towards a specified coordinate
    internal void AngleTowardsPosition(Vector2 destination) {
        // Get the current trajectory
        Vector2 projPos = gameObject.transform.position;
        // Vector2 targetPos = target.transform.position;
        Vector2 traj = destination - projPos;

        // Get the argument of the current trajectory
        float x = traj.x;
        float y = traj.y;
        float new_arg;
        // Handle corner cases
        if (x != 0) {   // Standard case
            new_arg = Mathf.Atan(y/x) * Mathf.Rad2Deg;   // Only returns in the range (-90,90)
            if (x < 0) {                 // Face the other way
                new_arg += 180;
            }
        } else if (y != 0) {   // x=0, y!=0
            if (y > 0) {
                new_arg = 90;
            } else {
                new_arg = -90;
            }
        } else {   // If x=y=0, do nothing
            return;
        }

        // Update the field arg and rotate accordingly
        transform.Rotate(0,0,new_arg - arg);
        arg = new_arg;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
