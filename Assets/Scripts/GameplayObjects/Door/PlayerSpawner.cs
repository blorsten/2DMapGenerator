using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(GameplayObject))]
    public class PlayerSpawner : MonoBehaviour
    {
        private bool _isInDoor;
        private GameplayObject _gameplayObject;

        void Awake()
        {
            _gameplayObject = GetComponent<GameplayObject>();
        }

        public void GrabPlayer(GameObject player)
        {
            if (player != null)
                player.transform.position = transform.position;
        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) || !_isInDoor)
                return;

            if (_gameplayObject.Owner.ChunkType == ChunkType.Start)
            {
                MapCycler.Instance.LoadPreviousMap();
            }
            else if (_gameplayObject.Owner.ChunkType == ChunkType.End)
            {
                MapCycler.Instance.LoadNextMap();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isInDoor = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isInDoor = false;
            }
        }
    }
}

