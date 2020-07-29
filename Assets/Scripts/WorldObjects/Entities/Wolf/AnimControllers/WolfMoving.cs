using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WolfMoving : StateMachineBehaviour
{
    Sheep target;

    Wolf_StateVars wolf_vars;
    Wolf wolf;

    bool triggered;

    bool waypoint_set;
    
    /**
     * Updates sheep objective
     */
    public void update_sheep_obj(Animator animator)
    {
        
        if ((wolf_vars.move_to_pos - wolf.transform.position).sqrMagnitude < wolf_vars.near_dest * wolf_vars.near_dest)
        {
            //TODAVIA NO 
            if (wolf_vars.current_chasing_sheep != null)
            {
                wolf_vars.current_chasing_sheep = null;
            }
            else
            {
                wolf_vars.move_to_pos = Instances.GetSheepWaveManager().getRandomWavepoint();
            }
        }

        if (wolf_vars.current_chasing_sheep != null)
        {
            return;
        }


        /* Searches the nearest non-burning sheep */
        Collider[] colliders = Physics.OverlapSphere (wolf.transform.position, wolf_vars.sheep_detect_range, wolf_vars.sheep_mask);

        int i = 0;
        for (; i < colliders.Length;++i)
        {
            AnimatorStateInfo state_info = colliders[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            
            if (!state_info.IsName("EffectFire"))
            {
                break;
            }
        }

        /* If there's a sheep within range, chases it. If it's not, goes to a random waypoint */
        if (i < colliders.Length)
        {
            wolf_vars.move_to_pos = colliders[i].transform.position;
            wolf_vars.current_chasing_sheep = colliders[i].GetComponent<Sheep>();
        }
        else
        {
            if (!waypoint_set)
            {
                Debug.Log("Waypoint set");
                wolf_vars.move_to_pos = Instances.GetSheepWaveManager().getRandomClosest (wolf.transform.position);
                wolf_vars.current_chasing_sheep = null;
                waypoint_set = true;
            }
        }

        
    }

    public void check_near_sheeps()
    {
        Collider[] colliders = Physics.OverlapSphere (
            wolf.transform.position
          , wolf_vars.sheep_eat_range
          , wolf_vars.sheep_mask
        );

        foreach (var c in colliders)
        {
            Animator other_anim = c.GetComponent<Animator>();
            /* The sheep manages its own death */
            other_anim.SetTrigger ("Dead");
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wolf = animator.GetComponent<Wolf>();
        wolf_vars = wolf.GetComponent<Wolf_StateVars>();

        triggered = false;
        waypoint_set = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        update_sheep_obj (animator);
        check_near_sheeps ();

        wolf.MoveTo (
            wolf_vars.move_to_pos,
            wolf_vars.wolf_vel,
            wolf_vars.wolf_max_force,
            wolf_vars.near_dest,
            0,
            ForceMode.Acceleration
        );
    }

}
