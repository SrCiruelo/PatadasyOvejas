using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_StateVars : MonoBehaviour
{
    public float sheep_detect_range;
    public float sheep_eat_range;
    public float raycast_to_float_dist;
    public LayerMask sheep_mask;
        
    [Header("Wolf Moving Vars")]
    public float wolf_vel;
    public float wolf_max_force;
    public float near_dest;
    public Vector2? current_obj;
    public float wolf_verticality;
    
    [HideInInspector]
    public Sheep current_chasing_sheep = null;
    
    public Rigidbody rb { get; private set; }

    public Vector3 move_to_pos;
    private Wolf wolf;

#if DEBUG
    private Animator animator;

#endif
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wolf = GetComponent<Wolf>();
#if DEBUG
        animator = GetComponent<Animator>();
#endif
    }


    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireSphere (transform.position, sheep_detect_range);
        
        Gizmos.DrawLine (transform.position, transform.position - Vector3.up * raycast_to_float_dist);

#if DEBUG
        if ( animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("EffectFire"))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sheep_detect_range);
        }
        #endif
    }

    public void move ()
    {
        wolf.MoveTo (move_to_pos, wolf_vel, wolf_max_force, near_dest, wolf_verticality, ForceMode.Force);
    }
}
