using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public enum ChunkType
    {
        Default, DeadEnd, Reward, Secret
    }


    /// <summary>
    /// Purpose:
    /// To Store the chunk's data.
    /// Creator:
    /// Mikkel Nielsen
    /// </summary>
    public class Chunk : MonoBehaviour
    {
        //This sections is for generel properties 
        [Header("Properties"), SerializeField]
        private int _width;//Tells the width of the chunk

        [SerializeField]
        private int _height;//Tells the height of the chunk

        [SerializeField]
        private ChunkType _chunkType;//Tells the type of the chunk

        //These fields tells what openings are open on the chunk
        [Header("Openings"), SerializeField]
        private bool _topOpen;

        [SerializeField]
        private bool _bottomOpen;

        [SerializeField]
        private bool _leftOpen;

        [SerializeField]
        private bool _rightOpen;

        //This section if for refernces
        [Header("Refernces"), SerializeField]
        private ChunkBehavior _chunkBehavior;

        [SerializeField]
        private Tilemap _enviorment;

        //Properties for generel properties
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public ChunkType ChunkType
        {
            get { return _chunkType; }
            set { _chunkType = value; }
        }

        //Properties for opennings
        public bool TopOpen
        {
            get { return _topOpen; }
            set { _topOpen = value; }
        }
        public bool BottomOpen
        {
            get { return _bottomOpen; }
            set { _bottomOpen = value; }
        }
        public bool LeftOpen
        {
            get { return _leftOpen; }
            set { _leftOpen = value; }
        }
        public bool RightOpen
        {
            get { return _rightOpen; }
            set { _rightOpen = value; }
        }

        //Properties for references
        public ChunkBehavior ChunkBehavior
        {
            get { return _chunkBehavior; }
            set { _chunkBehavior = value; }
        }
        public Tilemap Enviorment
        {
            get { return _enviorment; }
            set { _enviorment = value; }
        }

        public string ID { get; set; }

        //These properties tells te chunk whick openings are used
        public bool TopConnection { get; set; }
        public bool BottomConnetion { get; set; }
        public bool LeftConnection { get; set; }
        public bool RightConnection { get; set; }

        //A list for the items in the chunk
        public List<GameObject> Items { get; set; }
    }
}
