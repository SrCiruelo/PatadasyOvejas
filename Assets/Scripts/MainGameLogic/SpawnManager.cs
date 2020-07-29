using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnManager : MonoBehaviour
{
    /**
     * Variable used to calculate probabilities.
     */
    private Random _rand;

    /**
     * This object holds all the information about the world
     */
    private GameStatus _commBus;
    
    /**
     * Maximum number of sheep allowed outside of the pen at once
     */
    public int maxSheepOutside;
    
    /* ----------------------------------------- */
    /* Variables to reference the world entities */
    /* ----------------------------------------- */

    /* ==== Types of entities ==== */
    public Sheep sheepPrefab;
    
    public Wolf wolfPrefab;
    
    public Food [] foodPrefabs;

    /* ==== Spawn points for the different entities ==== */
    public GameObject [] wolfSpawnPoints;

    public GameObject [] sheepEscapePoints;

    public GameObject [] foodSpawnPoints;
    
    /**
     * Counter to reset the probability once a sheep spawned
     */
    private long _sheepTime = 0L;
   
     /**
      * Counter to reset the probability once a wolf spawned
      */
     private long _wolfTime = 0L;

    /* ------------------------------------- */
    /* FUNCTIONS TO CALCULATE PROBABILITIES  */
    /* ------------------------------------- */

    /**
     * Function that returns the probability at any given time for a wolf to spawn.
     * 
     * Spawns just a wolf iff there are no other wolves and at least 2 sheep
     */
    private readonly Func<int, int, double, double> _wolfSpawnThreshold =
        (int wolves, int sheep, double time) => (wolves == 0 && sheep >= 2) ? 0 : 101;
    
    /**
      * Function that returns the probability at any given time for a sheep to escape the pen.
     */
    private readonly Func<int, int, int, double, double> _sheepEscapeThreshold =
        (int sheep_out, int sheep_in, int maxOutside, double time) =>
        {
            if (sheep_out >= maxOutside)
            {
                return 101;
            }

            /* We want an exponential function, f(t) = 3^t. To calculate the probability of a sheep escaping, we have
             to integrate the function: (3^t)/log(3)
             Then, we have to convert it into a value between 0 and 100, being '100' anything bigger than
             'maxOutside' */
            double prob = 100 - (Math.Pow (5.0, time) / Math.Log (5.0));

            return prob > 101? 101 : prob;
        }
    ;
    
     /**
      * Spawns all the entities across the map
      */
     private void Start ()
     {
         _rand = new Random (); 
         /* Discards the first non-random value (yeah... you suck, .NET) */
         _rand.Next ();

         _commBus = Instances.GetGameStatus ();
         
         /* Adds random food types until all the food points are filled */
         foreach (GameObject point in foodSpawnPoints)
         {
             int idx = _rand.Next (foodPrefabs.Length);
             Food foodItem = foodPrefabs[idx];
             Instantiate (foodItem, point.transform);
         }

         StartCoroutine (calcSpawmns ());
     }


     /**
      * Calculates if the determined object should be instantiated randomly at one of the defined points.
      *
      * <param name="entity">
      * Either a <see cref="WolfPrefab"/> or a <see cref="SheepPrefab"/>, which should be created
      * </param>
      *
      * <param name="spawnPoints">
      * A list with all the available spawn points.
      * </param>
      */
     private void _spawnEntity (Entity entity, GameObject [] spawnPoints)
     {
          double value = _rand.Next (100);
          double threshold = 0;

          /* It's a different threshold depending on the entity being spawned */
          if (entity.GetType () == typeof (Wolf))
          {
              threshold = _wolfSpawnThreshold (_commBus.wolves, _commBus.sheep_out, _wolfTime);
              
          } else if (entity.GetType () == typeof (Sheep))
          {
              threshold = _sheepEscapeThreshold (_commBus.sheep_out, _commBus.sheep_in, maxSheepOutside, _sheepTime);
          }

          if (value < threshold)
          {
              return;
          }

          /* Gets a random spawn point. It doesn't matter if the previous entity has already been spawned here, as
          they should be walking around and chasing some food */
          int idx = _rand.Next (spawnPoints.Length);
          GameObject point = spawnPoints [idx];
 
          /* The entity is spawned at one of the predefined points */
          if (entity.GetType () == typeof (Sheep))
          {
              Instantiate (wolfPrefab, point.transform.position, Quaternion.identity);
              _wolfTime = 0;
              /* Notifies the change */
              _commBus.sheepEscaped ();

          } else if (entity.GetType () == typeof (Wolf))
          {
              Instantiate (sheepPrefab, point.transform.position, Quaternion.identity);
              _sheepTime = 0;
              /* Notifies the change */
              _commBus.wolfAdded ();
          }
     }

     /**
      * Checks if any spawn event should happen
      */
     IEnumerator calcSpawmns ()
     {
         while (true)
         {
             yield return new WaitForSeconds (1);
             _spawnEntity(wolfPrefab, wolfSpawnPoints);
             _spawnEntity(sheepPrefab, sheepEscapePoints);

             _sheepTime++;
             _wolfTime++;
         }
     }


     /**
      * Notifies this <see cref="SpawnManager"/> about a food that has been eaten.
      *
      * The manager acknowledges the notification and starts a coroutine to spawn a new kind of food.
      *
      * <param name="position">
      * Position of the destroyed food item.
      * </param>
      */
     public void NotifyFoodEaten (Transform position)
     {
         StartCoroutine (_spawnFood (position));
     }

     /**
      * Waits a random time between 30 and 90 seconds, and creates a new food item in the designated position.
      * 
      * 
      * <param name="position">
      * Position of the destroyed food item.
      * </param>
      */
     private IEnumerator _spawnFood (Transform position)
     {
         while (true)
         {
             yield return new WaitForSeconds(_rand.Next(30, 90));

             int idx = _rand.Next(foodPrefabs.Length);
             Food newItem = foodPrefabs[idx];

             Instantiate(newItem, position);
         }
     }

}
