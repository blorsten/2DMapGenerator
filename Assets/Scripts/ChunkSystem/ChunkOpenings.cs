using System;
using MapGeneration.Algorithm;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    [Serializable]
#pragma warning disable 660,661
    public class ChunkOpenings
#pragma warning restore 660,661
    {
        public enum ConnectionType
        {
            Default,
            Critical
        }

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
            private set
            {
                _topConnection = value;
                TopOpen = value;
            }
        }

        public bool BottomConnetion
        {
            get { return _bottomConnetion; }
            private set
            {
                _bottomConnetion = value;
                BottomOpen = value;
            }
        }

        public bool LeftConnection
        {
            get { return _leftConnection; }
            private set
            {
                _leftConnection = value;
                LeftOpen = value;
            }
        }

        public bool RightConnection
        {
            get { return _rightConnection; }
            private set
            {
                _rightConnection = value;
                RightOpen = value;
            }
        }

        public ConnectionType TopConnectionType { get; private set; }
        public ConnectionType BottomConnectionType { get; private set; }
        public ConnectionType LeftConnectionType { get; private set; }
        public ConnectionType RightConnectionType { get; private set; }

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

        public bool IsMatching(ChunkOpenings b)
        {
            bool isValid = true;

            if (TopOpen && !b.TopOpen)
                isValid = false;

            if (RightOpen && !b.RightOpen)
                isValid = false;

            if (BottomOpen && !b.BottomOpen)
                isValid = false;

            if (LeftOpen && !b.LeftOpen)
                isValid = false;

            return isValid;
        }

        public void SetConnectionAuto(PathAlgorithm.CardinalDirections dir, ChunkHolder next,
            ConnectionType type = ConnectionType.Default, bool value = true)
        {
            switch (dir)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    SetConnection(PathAlgorithm.CardinalDirections.Top, type);
                    next.ChunkOpenings.SetConnection(PathAlgorithm.CardinalDirections.Bottom, type);
                    break;
                case PathAlgorithm.CardinalDirections.Bottom:
                    SetConnection(PathAlgorithm.CardinalDirections.Bottom, type);
                    next.ChunkOpenings.SetConnection(PathAlgorithm.CardinalDirections.Top, type);
                    break;
                case PathAlgorithm.CardinalDirections.Left:
                    SetConnection(PathAlgorithm.CardinalDirections.Left, type);
                    next.ChunkOpenings.SetConnection(PathAlgorithm.CardinalDirections.Right, type);
                    break;
                case PathAlgorithm.CardinalDirections.Right:
                    SetConnection(PathAlgorithm.CardinalDirections.Right, type);
                    next.ChunkOpenings.SetConnection(PathAlgorithm.CardinalDirections.Left, type);
                    break;
            }
        }

        public void SetConnection(PathAlgorithm.CardinalDirections dir, ConnectionType type = ConnectionType.Default, bool value = true)
        {
            switch (dir)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    TopConnection = value;
                    TopConnectionType = type;
                    break;
                case PathAlgorithm.CardinalDirections.Bottom:
                    BottomConnetion = value;
                    BottomConnectionType = type;
                    break;
                case PathAlgorithm.CardinalDirections.Left:
                    LeftConnection = value;
                    LeftConnectionType = type;
                    break;
                case PathAlgorithm.CardinalDirections.Right:
                    RightConnection = value;
                    RightConnectionType = type;
                    break;
            }
        }

        public bool IsOpen(PathAlgorithm.CardinalDirections dir)
        {
            switch (dir)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    return TopOpen;
                case PathAlgorithm.CardinalDirections.Bottom:
                    return BottomOpen;
                case PathAlgorithm.CardinalDirections.Left:
                    return LeftOpen;
                case PathAlgorithm.CardinalDirections.Right:
                    return RightOpen;
            }

            return false;
        }
    }
}