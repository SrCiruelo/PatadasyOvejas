using Stateless.Graph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMoving : StateMachineBehaviour
{


    Sheep_StateVars sheep_vars;
    WavepointManager sheep_manager;
    Sheep sheep;
    
    /**
     * Prevents triggering the transition multiple times.
     */
    private bool changeTriggered;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep_vars = animator.GetComponent<Sheep_StateVars>();
        sheep_manager = Instances.GetSheepWaveManager();
        sheep = sheep_vars.GetComponent<Sheep>();
        sheep_vars.move_to_pos = sheep_manager.getRandomClosest(sheep.transform.position);

        changeTriggered = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
         TODO: moverse hacia el wavepoint marcado. Si en el camino se encuentra con otra oveja que estaba en llamas,
         cambia al estado [onFire] (usando el trigger "caughtFire").
         */

        /* A transition change is already pending, so we're done here */
        sheep_vars.rotatetowardsvel();
        if (changeTriggered)
        {
            return;
        }

        Vector3 sheep_position = sheep.transform.position;
        
        /* The sheep arrived to its destination */
        if((sheep_vars.move_to_pos - sheep_position).sqrMagnitude < sheep_vars.near_dest * sheep_vars.near_dest)
        {
            if (sheep_vars.current_chasing_food != null)
            {
                /* Eat food */
                animator.SetTrigger("CanEatFood");
                changeTriggered = true;
            }
            else
            {
                /* Change target */
                sheep_vars.move_to_pos = Instances.GetSheepWaveManager ().getRandomClosest (sheep.transform.position);
            }
        }
          

        if (sheep_vars.current_chasing_food != null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(sheep.transform.position, sheep_vars.sheep_food_range,sheep_vars.food_mask);
//        Debug.Log("SheepMoving.OnStateUpdate: Colliders.length =" + colliders.Length);

        if(colliders.Length != 0)
        {
            sheep_vars.current_chasing_food = colliders[0].GetComponent<Food>();
            sheep_vars.move_to_pos = sheep_vars.current_chasing_food.transform.position;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
