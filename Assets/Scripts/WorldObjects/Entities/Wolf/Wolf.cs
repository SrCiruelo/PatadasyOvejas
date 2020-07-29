using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Entity
{
    public Animator animator { get; private set; }

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    /**
     * Removes this sheep from the game
     */
    public void Die ()
    {
        /* We assume that sheep inside of the pen are safe and cannot die */
        Instances.GetGameStatus ().wolfRemoved ();
        Destroy (this.gameObject);
    }

    public override void PlayerInteraction()
    {
        throw new System.NotImplementedException();
        base.PlayerInteraction();
    }

    public override bool Interact (WorldObject other_object)
    {
        return false;
    }


}
