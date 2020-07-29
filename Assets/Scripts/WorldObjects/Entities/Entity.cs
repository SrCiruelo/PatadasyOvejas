using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : WorldObject
{
    [HideInInspector]
    public Rigidbody rb;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool MoveTo(Vector3 move_to_pos, float vel, float max_force, float max_dist, float verticality, ForceMode forceMode)
    {
        Vector3 real_dir = new Vector3(move_to_pos.x - transform.position.x, 0, move_to_pos.z - transform.position.z);
        if (real_dir.sqrMagnitude < max_dist * max_dist)
        {
            return true;
        }
        real_dir = real_dir.normalized;
        real_dir = new Vector3(real_dir.x, verticality, real_dir.z);
        real_dir = real_dir.normalized;

        Vector3 desired_vel = real_dir * vel;
        Vector3 steering_force = desired_vel - rb.velocity;

        if (steering_force.sqrMagnitude > max_force * max_force)
        {
            rb.AddForce(steering_force.normalized * max_force, forceMode);
        }
        else
        {
            rb.AddForce(steering_force, forceMode);
        }
        return false;
    }
    public virtual void PlayerInteraction()
    {

    }
}
