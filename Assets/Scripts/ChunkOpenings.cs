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

        private bool _topConnection;
        private bool _bottomConnetion;
        private bool _leftConnection;
        private bool _rightConnection;

        //These properties tells te chunk whick openings are used
        public bool TopConnection
        {
            get { return _topConnection; }
            set
            {
                _topConnection = value;
                TopOpen = value;
            }
        }

        public bool BottomConnetion
        {
            get { return _bottomConnetion; }
            set
            {
                _bottomConnetion = value;
                BottomOpen = value;
            }
        }

        public bool LeftConnection
        {
            get { return _leftConnection; }
            set
            {
                _leftConnection = value;
                LeftOpen = value;
            }
        }

        public bool RightConnection
        {
            get { return _rightConnection; }
            set
            {
                _rightConnection = value;
                RightOpen = value;
            }
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

        public static bool operator == (ChunkOpenings a, ChunkOpenings b)
        {
            bool isValid = true;
            
            if (a.TopOpen != b.TopOpen)
                isValid = false;
            if (a.RightOpen != b.RightOpen)
                isValid = false;
            if (a.BottomOpen != b.BottomOpen)
                isValid = false;
            if (a.LeftOpen != b.LeftOpen)
                isValid = false;

            return isValid;
        }

        public static bool operator !=(ChunkOpenings a, ChunkOpenings b)
        {
            return !(a == b);
        }

        public bool IsEmpty()
        {
            bool isValid = true;

            if (TopOpen)
                isValid = false;
            if (RightOpen)
                isValid = false;
            if (BottomOpen)
                isValid = false;
            if (LeftOpen)
                isValid = false;

            return isValid;
        }
    }
}