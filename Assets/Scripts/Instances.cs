using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instances : MonoBehaviour
{

    [SerializeField]
    private GameStatus Gamestatus_instance;
    [SerializeField]
    private SpawnManager spawnManager_instance;
    [SerializeField]
    private HUDManager HUDManager_instance;

    [SerializeField]
    private WavepointManager sheep_waveManager_instance;
    [SerializeField]
    private WavepointManager wolf_waveManager_instance;

    private static Instances instance;

    public void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one instance of Instances");
        }
        instance = this;

    }

    public static SpawnManager GetSpawnManager()
    {
        return instance.spawnManager_instance;
    }
    public static GameStatus GetGameStatus()
    {
        return instance.Gamestatus_instance;
    }
    public static HUDManager GetHUDManager()
    {
        return instance.HUDManager_instance;
    }
    public static WavepointManager GetSheepWaveManager()
    {
        return instance.sheep_waveManager_instance;
    }
    public static WavepointManager GetWolfWaveManager()
    {
        return instance.wolf_waveManager_instance;
    }
}
