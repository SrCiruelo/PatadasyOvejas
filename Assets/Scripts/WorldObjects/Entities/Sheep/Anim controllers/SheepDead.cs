using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepDead : StateMachineBehaviour
{
     
    /**
     * To avoid triggering the end of the animation multiple times
     */
    private bool animationFinished;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Sheep sheep = animator.GetComponent<Sheep>();
        sheep.Die ();
    }

}
