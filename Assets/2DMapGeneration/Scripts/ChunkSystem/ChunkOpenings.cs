using System;
using MapGeneration.Algorithm;
using UnityEngine;

namespace MapGeneration.ChunkSystem
{
    /// <summary>
    /// Purpose:
    /// To give each chunk some openings, to be used in any way
    /// Creator:
    /// MP
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

        [SerializeField] private bool _topOpen;
        [SerializeField] private bool _bottomOpen;
        [SerializeField] private bool _leftOpen;
        [SerializeField] private bool _rightOpen;

        [SerializeField, HideInInspector] private bool _topConnection;
        [SerializeField, HideInInspector] private bool _bottomConnetion;
        [SerializeField, HideInInspector] private bool _leftConnection;
        [SerializeField, HideInInspector] private bool _rightConnection;

        [SerializeField, HideInInspector] private ConnectionType _topConnectionType;
        [SerializeField, HideInInspector] private ConnectionType _bottomConnectionType;
        [SerializeField, HideInInspector] private ConnectionType _leftConnectionType;
        [SerializeField, HideInInspector] private ConnectionType _rightConnectionType;

        //These properties tells te chunk whick openings are used
        public bool TopConnection { get { return _topConnection; } private set { _topConnection = value; TopOpen = value; } }
        public bool BottomConnetion { get { return _bottomConnetion; } private set { _bottomConnetion = value; BottomOpen = value; } }
        public bool LeftConnection { get { return _leftConnection; } private set { _leftConnection = value; LeftOpen = value; } }
        public bool RightConnection { get { return _rightConnection; } private set { _rightConnection = value; RightOpen = value; } }

        public ConnectionType TopConnectionType { get { return _topConnectionType; } private set { _topConnectionType = value; } }
        public ConnectionType BottomConnectionType { get { return _bottomConnectionType; } private set { _bottomConnectionType = value; } }
        public ConnectionType LeftConnectionType { get { return _leftConnectionType; } private set { _leftConnectionType = value; } }
        public ConnectionType RightConnectionType { get { return _rightConnectionType; } private set { _rightConnectionType = value; } }

        //Properties for opennings
        public bool TopOpen {  get { return _topOpen; } set { _topOpen = value; } }
        public bool BottomOpen { get { return _bottomOpen; } set { _bottomOpen = value; } }
        public bool LeftOpen { get { return _leftOpen; } set { _leftOpen = value; } }
        public bool RightOpen { get { return _rightOpen; } set { _rightOpen = value; } }

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

        /// <summary>
        /// Checks if these chunk openings are all closed.
        /// </summary>
        /// <returns>Return true if all the openings are closed.</returns>
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

        /// <summary>
        /// Checks if these chunk openings match up with another one.
        /// </summary>
        /// <param name="b">Chunksopenings to check on.</param>
        /// <returns>If they got the completely same openings it returns true.</returns>
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

        /// <summary>
        /// Sets a connection between this chunkopening and another chunk holders chunk connections.
        /// </summary>
        /// <param name="dir">Which way is the connection.</param>
        /// <param name="next">What chunkholder is the connection going to.</param>
        /// <param name="type">What type of connection is it?</param>
        /// <param name="value">Is the connection closed or open?</param>
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

        /// <summary>
        /// Takes a direction and sets a connection state for that direction while setting a 
        /// specified connection type.
        /// </summary>
        /// <param name="dir">Which direction is the connection.</param>
        /// <param name="type">Type of connection.</param>
        /// <param name="value">Is the connection open or closed.</param>
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

        /// <summary>
        /// Returns true if the parameter input is open
        /// </summary>
        /// <param name="dir">The direction to check</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if the chunk is a dead end
        /// </summary>
        /// <returns></returns>
        public bool IsDeadEnd()
        {
            bool isValid = false;

            if (TopOpen && !RightOpen &&  !BottomOpen &&  !LeftOpen)
                isValid = true;
            
            if (!TopOpen && RightOpen && !BottomOpen && !LeftOpen)
                isValid = true;
            
            if (!TopOpen && !RightOpen && BottomOpen && !LeftOpen)
                isValid = true;
            
            if (!TopOpen && !RightOpen && !BottomOpen && LeftOpen)
                isValid = true;
            
            return isValid;
        }
    }
}