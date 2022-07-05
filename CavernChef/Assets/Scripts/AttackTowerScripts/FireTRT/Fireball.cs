using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Add AoE (probably small radius) (comment added 2022-6-16)

public class Fireball : Projectile
{
    /* Stats as of 2022-6-16
    centi_speed = 20
    dmg = 6
    effect_frames = 0
    */

    // Start is called before the first frame update
    void Start()
    {
        // Set speed
        base.Setup();
        FindObjectOfType<AudioManager>().Play("Fire");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 projectilePos = gameObject.transform.position;   // Bullet position
        if (target != null) {                                    // If target is not yet dead,
            targetPos = target.transform.position;               // Update targetPos
        }
        Vector2 traj = targetPos - projectilePos;                // Trajectory to target
        float dist = traj.magnitude;                             // Dist to target

        if (dist < speed)   // Hit
        {
            // Get list of enemies hit by AoE
            List<Enemy> hit = GlobalVariables.enemyList.AoECasualties(targetPos, AoeRadius);
            foreach (Enemy enemy in hit) {
                // Deal dmg
                enemy.status.fireDmg(dmg);
            }

            // DEBUG
            // Debug.Log(hit.Count);   Note to self actually double check that I typed the right variables


            /*
            // If target not yet dead, deal dmg
            if (target != null) {
                target.status.fireDmg(dmg);
            }
            // If the projectile is AoE, do AoE stuff
            */

            // Destroy the bullet itself
            Destroy(gameObject);
        }
        else   // Not yet hit
        {
            // Move towards the target
            Vector2 delta = traj * speed / dist;
            gameObject.transform.position += (Vector3) delta;   //Type cast needed since .trans.pos is 3-dim
            // Explanation for need of type cast aboce
            // https://gamedev.stackexchange.com/questions/98715/in-unity-why-is-adding-a-vector2-and-a-vector3-ambiguous-but-assigning-isnt
        }
    }
}

// Projectile outline (as of 2022-6-16)
/*

public class projectile_name : Projectile
{
    /* Stats as of 2022-6-
    centi_speed = 
    dmg = 
    effect_frames = 
    * /

    // Start is called before the first frame update
    void Start()
    {
        // Set speed
        base.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 projectilePos = gameObject.transform.position;   // Bullet position
        if (target != null) {                                    // If target is not yet dead,
            targetPos = target.transform.position;               // Update targetPos
        }
        Vector2 traj = targetPos - projectilePos;                // Trajectory to target
        float dist = traj.magnitude;                             // Dist to target

        if (dist < speed)   // If target is hit
        {
            if (target != null) {   // if target is not yet dead
                // Deal damage (possibly AoE)
                // Apply status effect (if any; possibly AoE)
            }
            Destroy(gameObject);   // Destroy the bullet itself
        }
        else   // Elif not yet hit
        {
            Vector2 delta = traj * speed / dist;                // Movement
            gameObject.transform.position += (Vector3) delta;   // Update coordinates
        }
    }
}

*/







// Before 2022-6-15 edit (adding the abstract class Projectile and
// letting this extend(? I forgot the word) from that)


/*
public class Fireball : MonoBehaviour
{
    // Speed of the projectile; set here or in Unity
    public float speed = 9.0f;
    // Dmg of the projectile
    public float dmg = 6.0f;

    public Enemy target;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* Code flow
        if enemy DNE:
            self destruct
        elif enemy exists:
            get vector to enemy
            if near enough:
                deal dmg
                if enemy ded:
                    delete ref
                    destroy enemy
                self destruct (do last in case update() terminates upon self desturctiosn)
            elif not near enough:
                move closer by speed
        * /

        // Wispy target = targetButGameObject.GetComponent<Wispy>();
        Vector2 bulletPos = gameObject.transform.position;   // Bullet position
        Vector2 enemyPos = target.transform.position;        // Target position
        Vector2 traj = enemyPos - bulletPos;                 // Trajectory
        float dist = traj.magnitude;                         // Dist to target

        if (dist < speed)   // Hit
        {
            // target.hp -= dmg;
            target.status.fireDmg(dmg);

            // If target dead logic moved to the target itself
            /*
            if (target.hp <= 0)   // If target dead
            {
                    
                // Destroy(targetButGameObject);            // Moved to the enemy itself also
                // Destroy(target);                         // Destroy the enemy game object
                // Destroy in enemy itself
                
            }
            * /
            Destroy(gameObject);                     // Destroy the bullet itself
        }
        else   // Not yet hit
        {
            // Move towards the target
            Vector2 delta = traj * speed / dist;
            gameObject.transform.position += (Vector3) delta;//Type cast needed since .trans.pos is 3-dim
        }
    }
}
*/





// Before comment cleaning on 2022-6-14 night/2022-6-15 morning

/*
public class Fireball : MonoBehaviour
{

    // Speed of the projectile; set here or in Unity
    public float speed = 9.0f; // (Wispy speed is 7 rn) (now 5)
    // Dmg of the projectile
    public float dmg = 6.0f; // (Wispy hp is 10 rn)

    public Enemy target;

    /*
    public TRTBasicBullet(Enemy target) {
        this.target = target;
    }
    * /


    // Start is called before the first frame update
    void Start()
    {
        // GameObject target
        // target could die and need retargetting, do in Update instead
    }

    // Update is called once per frame
    void Update()
    {
        /* Plan
        1.) Get the vector from bullet to the front enemy
        2.) Travel `speed` dist along it (speed here is in units/frame)
            
        3.) If dist to enemy is < epsilon 
             - deal dmg
             - self-destruct
             - delete reference in global enemy list
            (Need to use < e rather than = 0 due to rounding errors 
            and the possibility of overshooting by e also)
            *NOTE: To ensure that overshooting doesn't happen, set
                epsilon >= speed
            for a decently wide margin of error. (*should* work, 
            diameter = 2*speed) 
            e < speed/2 is asking for trouble


        WAIT what if targetted enemy dies
        Case 1: There exists other enemies
            Retarget the next frontmost enemy (for this turret at least)
            (No need to consider where the retargetted enemy is, most TDs 
            just let it retarget to anywhere on screen iirc)
        Case 2: No other enemy exists
            3 options
            1.) Destroy the projectile (visually disappear or fly straight off the screen)
            2.) Let the projectile just complete it's course and destroy then
            3.) Spin in place and retarget once an enemy exists again (Boomerang tower in
                The Creeps) (unlikely to be used but good to keep in the back of our 
                head ig??? idk in case there's a tower where this makes sense)
            Currently will do 1.) for simplicity
        


        Code flow
        if enemy DNE:
            self destruct
        elif enemy exists:
            get vector to enemy
            if near enough:
                deal dmg
                if enemy ded:
                    delete ref
                    destroy enemy
                self destruct (do last in case update() terminates upon self desturctiosn)
            elif not near enough:
                move closer by speed
        
        ~~~HOW TO USE UNITY API~~~
        To get the vector of a gameobject, do
            GameObject.Find("Your_Name_Here").transform.position;
            GameObject.transform.position
        To get the magnitude of a Vector2
            float Vector2.magnitude
        Vector2 constructor
            public Vector2(float x, float y);
        Unity just uses a constant number for conversion
            Mathf.Deg2Rad = pi/180
        To get the distance bet GameObjects (No need for manual)
            Vector2.Distance(a,b)


        TODOS IN OTHER SCRIPTS: 
         - If enemy HP <= 0, commit unexist (done here instead)
         - Delete the killed enemy from GlobalVariables.enemyList since the ref still exists
           (not sure which script should do this) (use `RemoveAt(ind)`) (done here instead)
         - Haven't done instantiate yet in `TRTBasicOffense.cs`
        * /

        if (target == null)   // If target is dead
        {
            Destroy(gameObject);   // Destroy the bullet
        }
        else   // If target is not dead
        {
            // GameObject targetButGameObject = GlobalVariables.enemyList[0];
            /* IMPORTANT TODO
            Get an abstract Enemy class or else hp retrieving will be pain
            * /
            // Wispy target = targetButGameObject.GetComponent<Wispy>();
            Vector2 bulletPos = gameObject.transform.position;   // Bullet position
            Vector2 enemyPos = target.transform.position;        // Target position
            Vector2 traj = enemyPos - bulletPos;                 // Trajectory
            float dist = traj.magnitude;                         // Dist to target

            if (dist < speed)   // Hit
            {
                // target.hp -= dmg;
                target.status.fireDmg(dmg);

                // If target dead logic moved to the target itself
                /*
                if (target.hp <= 0)   // If target dead
                {
                    
                    // Destroy(targetButGameObject);            // Moved to the enemy itself also
                    // Destroy(target);                         // Destroy the enemy game object
                    // Destroy in enemy itself
                    
                }
                * /
                Destroy(gameObject);                     // Destroy the bullet itself
            }
            else   // Not yet hit
            {
                // Move towards the target
                Vector2 delta = traj * speed / dist;
                gameObject.transform.position += (Vector3) delta;//Type cast needed since .trans.pos is 3-dim
            }
        }
    }
}
*/
