using System;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    [Serializable]
    public class ChunkOpenings
    {
        [SerializeField]
        private bool _topOpen;
        [SerializeField]
        private bool _bottomOpen;
        [SerializeField]
        private bool _leftOpen;
        [SerializeField]
        private bool _rightOpen;
        //These properties tells te chunk whick openings are used
        public bool TopConnection { get; set; }
        public bool BottomConnetion { get; set; }
        public bool LeftConnection { get; set; }
        public bool RightConnection { get; set; }

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
    }
}