using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Base : MonoBehaviour
{
    public bool StartSpawn;
    public GameObject PlayerPrefab;
    public Transform SpawnPoint;
    public static CharacterManager PlayerCharacter;
    private void Start()
    {
        if (StartSpawn)
        {
            SpawnPlayer();
        }
    }
    public void SpawnPlayer()
    {
        GameObject g = Instantiate(PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation);
        PlayerCharacter = g.GetComponent<CharacterManager>();
    }
   public static bool CanDealDamage(StateManager CauserState,StateManager ReceiverState)
    {
        return CauserState.campType != ReceiverState.campType;
    }
}
