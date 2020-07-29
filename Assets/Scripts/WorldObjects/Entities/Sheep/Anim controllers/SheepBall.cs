using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBall : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    float time_to_pass;
    float time_passed;
    Sheep sheep;
    Sheep_StateVars sheep_vars;

    /**
     * To avoid triggering the end of the animation multiple times
     */
    private bool animationFinished;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.GetComponent<Sheep>();
        sheep_vars = sheep.GetComponent<Sheep_StateVars>();
        time_to_pass = sheep_vars.sheep_spheric_time;

        sheep.SwapToSpheric ();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationFinished)
        {
            return;
        }

        if (time_passed > time_to_pass)
        {
            Debug.Log ("Effect is done");
            animationFinished = true;
            animator.SetTrigger("FruitFinished");
            sheep.SwapToNormal ();
        }
        time_passed += Time.deltaTime;
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
