using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SheepOnFire : StateMachineBehaviour
{
    Sheep target;

    /* ================================================================== */
    /* TODO: Esto se puede reutilizar para el comportamiento de los lobos */
    /* ================================================================== */

    Sheep_StateVars sheep_vars;
    Sheep sheep;

    float time_to_pass;
    float time_passed;

    bool triggered;

    GameObject particles;

    #region FireChecks 
    bool waypoint_set;
    /**
     * Updates sheep objective
     */
    public void update_sheep_obj(Animator animator)
    {
        
        if ((sheep_vars.move_to_pos - sheep.transform.position).sqrMagnitude < sheep_vars.near_dest * sheep_vars.near_dest)
        {
            //TODAVIA NO 
            if (sheep_vars.current_chasing_sheep != null)
            {
                sheep_vars.current_chasing_sheep = null;
            }
            else
            {
                sheep_vars.move_to_pos = Instances.GetSheepWaveManager().getRandomWavepoint();
            }
        }

        if (sheep_vars.current_chasing_sheep != null)
        {
            return;
        }


        Collider[] colliders = Physics.OverlapSphere(sheep.transform.position, sheep_vars.sheep_fire_search_radius, sheep_vars.sheep_mask);

        int i = 0;


        for (; i < colliders.Length;++i)
        {
            AnimatorStateInfo state_info = colliders[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            
            if (!state_info.IsName("EffectFire"))
            {
                
                break;
            }
        }

        if (i < colliders.Length)
        {
            sheep_vars.move_to_pos = colliders[i].transform.position;
            sheep_vars.current_chasing_sheep = colliders[i].GetComponent<Sheep>();
        }
        else
        {
            if(!waypoint_set)
            {
                Debug.Log("Waypoint set");
                sheep_vars.move_to_pos = Instances.GetSheepWaveManager().getRandomWavepoint();
                sheep_vars.current_chasing_sheep = null;
                waypoint_set = true;
            }
        }

        
    }

    public void check_near_sheeps()
    {
        Collider[] colliders = Physics.OverlapSphere(sheep.transform.position, sheep_vars.sheep_fire_radius, sheep_vars.sheep_mask);

        int i = 0;
        for (; i < colliders.Length; ++i)
        {
            Animator other_anim = colliders[i].GetComponent<Animator>();
            AnimatorStateInfo state_info = other_anim.GetCurrentAnimatorStateInfo(0);

            if (!state_info.IsName("EffectFire"))
            {
                other_anim.SetTrigger("FireFruit");
            }
        }


    }

    #endregion

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /* TODO: Busca la oveja más cercana y la establece como objetivo */
        sheep = animator.GetComponent<Sheep>();
        sheep_vars = sheep.GetComponent<Sheep_StateVars>();

        //sheep_vars.rb.freezeRotation 
        time_to_pass = sheep_vars.sheep_fire_time;
        time_passed = 0;

        triggered = false;
        waypoint_set = false;

        particles = Instantiate(sheep_vars.fire_particles, animator.transform.position, Quaternion.identity, animator.transform);
    }




    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /* TODO: Se mueve hacia la oveja objetivo. Si toca a otra oveja, ella se vuelve también en llamas.
         Si alcanza a la oveja objetivo, la prende fuego y busca otro objetivo.
         
         Tras X segundos (por ejemplo, 20s), muere. Al morir, hay que notificar a GameStatus.sheepRemoved(false) (o
          GameStatus.sheepRemoved(true), si estaba dentro del redil)
          */

        update_sheep_obj(animator);
        check_near_sheeps();

        sheep.MoveTo(sheep_vars.move_to_pos, sheep_vars.sheep_fire_speed, sheep_vars.sheep_fire_maxforce,sheep_vars.near_dest, 0, ForceMode.Acceleration);

        if (time_passed > time_to_pass && !triggered)
        {
            animator.SetTrigger("Dead");
            triggered = true;
            
        }

        time_passed += Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(particles);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
