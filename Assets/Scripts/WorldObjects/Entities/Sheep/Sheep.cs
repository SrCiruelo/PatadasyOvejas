using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Entity
{
    public Animator animator { get; private set; }



    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    /**
     * Gets converted to a spherical body
     */
    public void SwapToSpheric()
    {
        /* TODO: Añadir animación */
        print("Swaping to spheric");
    }
    
    /**
     * Gets converted back to a normal sheep
     */
    public void SwapToNormal()
    {
        /* TODO: Añadir animación */
        print("Swaping to normal");
    }
    
    /**
     * Starts burning and dies, trying to expand the fire among the rest of the sheep
     */
    public void Burn ()
    { 
        this.animator.SetTrigger ("FireFruit");
    }

    /**
     * Gets converted into a wolf
     */
    public void MorphIntoWolf()
    {
        this.animator.SetTrigger ("WolfFruit");
    }

    /**
     * Gets converted into a ball
     */
    public void MorphIntoBall ()
    {
        this.animator.SetTrigger ("BallFruit");
    }

    /**
     * Gets converted into a balloon and starts floating
     */
    public void MorphIntoBalloon ()
    {
        this.animator.SetTrigger ("BalloonFruit");
    }


    /**
     * Removes this sheep from the game
     */
    public void Die ()
    {
        /* We assume that sheep inside of the pen are safe and cannot die */
        Instances.GetGameStatus ().sheepRemoved (false);
        Destroy (this.gameObject);
    }

    public void rotatetowardsvel()
    {
        Quaternion next_rotation = Quaternion.FromToRotation(Vector3.forward, rb.velocity);
//        transform.rotation = Quaternion.Slerp(transform.rotation, next_rotation);
    }


    public override void PlayerInteraction()
    {
        throw new System.NotImplementedException();
        base.PlayerInteraction();
    }

    public override bool Interact(WorldObject other_object)
    {
        throw new System.NotImplementedException();
        return base.Interact(other_object);
    }


}
