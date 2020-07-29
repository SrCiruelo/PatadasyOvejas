using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep_StateVars : MonoBehaviour
{
    [Header("Sheep Eating Vars")]
    public float sheep_food_range;
    public LayerMask food_mask;
    [HideInInspector]
    public Food current_chasing_food = null;

    [Space(20)]
    [Header("Sheep Moving Vars")]
    public float sheep_vel;
    public float sheep_max_force;
    public float near_dest;
    public float rot_speed;
    public Vector2? current_obj;
    public float sheep_verticality;
    

    [Space(20)]
    [Header("Sheep Floating Vars")]
    public float floating_gravity_scale;
    public float sheep_floating_time;
    public float raycast_to_float_dist;


    [Space(20)]
    [Header("Sheep Fire Vars")]
    public GameObject fire_particles;
    public LayerMask sheep_mask;
    public float sheep_fire_time;
    public float sheep_fire_radius;
    public float sheep_fire_search_radius;
    public float sheep_fire_speed;
    public float sheep_fire_maxforce;
    [HideInInspector]
    public Sheep current_chasing_sheep = null;

    [Space(20)]
    [Header("Sheep Spheric Vars")]
    public float sheep_spheric_time;



    public Rigidbody rb { get; private set; }

    public Vector3 move_to_pos;
    private Sheep sheep;

#if DEBUG
    private Animator animator;

#endif
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sheep = GetComponent<Sheep>();
#if DEBUG
        animator = GetComponent<Animator>();
#endif
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sheep_food_range);
        
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * raycast_to_float_dist);

#if DEBUG
        if ( animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("EffectFire"))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sheep_fire_search_radius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sheep_fire_radius);


        }
        #endif
    }

public void jumpy_move()
    {
        Vector3 real_dir = new Vector3(move_to_pos.x - transform.position.x, 0, move_to_pos.z - transform.position.z);

        float vel = real_dir.magnitude / Time.deltaTime;
//        Debug.Log(vel + "  " + sheep_vel);
        vel = vel > sheep_vel ? sheep_vel : vel;

//        Debug.Log(vel + "  " + sheep_vel);

        real_dir = real_dir.normalized;
        real_dir = new Vector3(real_dir.x, sheep_verticality, real_dir.z);
        real_dir = real_dir.normalized;

        Vector3 desired_vel = real_dir * vel;
        Vector3 steering_force = desired_vel - rb.velocity;

        if (steering_force.sqrMagnitude > sheep_max_force * sheep_max_force)
        {
            rb.AddForce(steering_force.normalized * sheep_max_force, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(steering_force, ForceMode.Impulse);
        }



        sheep.MoveTo(move_to_pos, sheep_vel, sheep_max_force, near_dest, sheep_verticality, ForceMode.Impulse);
    }

    public void rotatetowardsvel()
    {
        Quaternion next_rotation = Quaternion.FromToRotation(Vector3.forward, rb.velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, next_rotation, rot_speed) * transform.rotation;
    }

}
