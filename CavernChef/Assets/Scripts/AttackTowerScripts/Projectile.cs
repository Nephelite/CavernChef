using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Access modifiers for c#: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers

public - all scripts can access and can edit in unity
       - use for fields that are initialized by the programmer in unity
internal - all scripts should(?) be able to acees but cannot edit in unity
         - use for fields that are initialized by the script instead to declutter the unity UI
protected - all subclasses
          - for functions that get called by children
*/

/* Current projectile behavior if target dies while en route (2022-6-15 night/2022-6-16 morning)
1.) Continue going to where the target died
2.) If the projectile has an aoe effect, produce the aoe effect on the spot of death
*/


// Abstract class for projectiles produced by atk towers 
// (5 eleemtan ns trts as of 2022-6-15-)
public abstract class Projectile : MonoBehaviour
{
    // Speed of the projectile (SET IN UNITY)
    public float centi_speed;   // This is in centiunits per frame
    // Dmg of the projectile (SET IN UNITY)
    public float dmg;
    // Effect duration, if any (snow, wet)
    public int effect_frames;

    // True speed
    internal float speed;
    // Target of a projectile (set by the TRT firing this)
    internal Enemy target;
    // Position of the target; so that a reference still exists after the target dies
    internal Vector2 targetPos;

    // Set the speed
    protected void Setup() {
        speed = centi_speed / 100;   // Convert to true speed
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
