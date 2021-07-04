using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Base : MonoBehaviour
{
   public static bool CanDealDamage(StateManager CauserState,StateManager ReceiverState)
    {
        return CauserState.campType != ReceiverState.campType;
    }
}
