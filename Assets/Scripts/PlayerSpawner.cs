using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class PlayerSpawner : MonoBehaviour
    {
        public void GrabPlayer(GameObject player)
        {
            if (player != null)
                player.transform.position = transform.position;
        }
    }
}

