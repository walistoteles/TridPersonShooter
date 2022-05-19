using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    Rigidbody[] bones;
    void Start()
    {
        bones = GetComponentsInChildren<Rigidbody>();

        foreach (var item in bones)
        {
            item.isKinematic = true;
        }
    }

    void Update()
    {
        
    }


    public void Death()
    {

    }
}
