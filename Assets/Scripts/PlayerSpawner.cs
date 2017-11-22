using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject __player;

        public void Start()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
                Instantiate(__player, transform.position, Quaternion.identity).transform.position = transform.position;
            else
            {
                player.transform.position = transform.position;
            }
        }
    }
}

