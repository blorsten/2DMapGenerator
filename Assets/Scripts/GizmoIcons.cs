using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GizmoIcons", menuName = "MapGeneration/Gizmos/GizmoIcons")]
public class GizmoIcons : ScriptableObject
{
    [SerializeField] private string _trapIcon;
    [SerializeField] private string _treasureIcon;
    [SerializeField] private string _groundIcon;
    [SerializeField] private string _flyingIcon;

    public string TrapIcon{get { return _trapIcon; }}
    public string TreasureIcon{get { return _treasureIcon; }}
    public string GroundIcon{get { return _groundIcon; }}
    public string FlyingIcon{get { return _flyingIcon; }}
}
