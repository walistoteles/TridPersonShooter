using MLSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    Rigidbody[] bones;
    BodyParts parts;
    public RagdollManager ragdollManager;

    public int maxHealth;
    private int _health;

    Animator anim;

    public Status status;

    public enum Status
    {
        Parado,
        Procurando,
        Atirando,
    };

    void Start()
    {
        anim = GetComponent<Animator>();
        ragdollManager = GetComponent<RagdollManager>();
        bones = GetComponentsInChildren<Rigidbody>();

        _health = maxHealth;
       
        foreach (var item in bones)
        {
            item.isKinematic = true;
        }
    }

    void Update()
    {
        if (_health <= 0)
        {
            Death();
        }
    }

    public void Hit(int[] parts, Vector3 vector3, int damage)
    {
        _health -= damage;
       

        //hit Reaction
        for (int i = 0; i < parts.Length; i++)
        {
            Debug.Log("Part " + parts[i].ToString());
        }
        ragdollManager.StartHitReaction(parts,vector3 * 1);

    }
    public void Death()
    {
        ragdollManager.StartRagdoll();
    }
}
