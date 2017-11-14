using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class ChunkOpenings : MonoBehaviour
    {
        //These properties tells te chunk whick openings are used
        public bool TopConnection { get; set; }
        public bool BottomConnetion { get; set; }
        public bool LeftConnection { get; set; }
        public bool RightConnection { get; set; }

        //Properties for opennings
        public bool TopOpen { get; set; } //{ get { return _topOpen; } set { _topOpen = value; } }
        public bool BottomOpen { get; set; } //{ get { return _bottomOpen; } set { _bottomOpen = value; } }
        public bool LeftOpen { get; set; } //{ get { return _leftOpen; } set { _leftOpen = value; } }
        public bool RightOpen { get; set; } //{ get { return _rightOpen; } set { _rightOpen = value; } }
    }
}