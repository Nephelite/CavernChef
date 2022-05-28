using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRTLookAtEnemy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Aiming script
        //This script aims at the only generated wisp for now
        if (GlobalVariables.enemyList.Count > 0)
        {
            //transform.Rotate(new Vector3(0, 0, 50));
            Transform target = GlobalVariables.enemyList[0].transform;
            //gameObject.transform.LookAt(target);
            float angle = 0;
            Vector3 relative = transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.Rotate(0, 0, -angle);
            Debug.Log(transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z);
        }
    }
}
