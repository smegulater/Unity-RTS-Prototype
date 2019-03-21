using UnityEngine;
using UnityRTSCore;
using System.Collections;

[CreateAssetMenu(fileName = "New Building", menuName = "UnityRTS/Building")]
public class BuildingStats : ScriptableObject
{
    [Header("General Building Settings")]
    public string Name;
    public string description;
    public BuildingType unitType;

    [Space(5f)]
    public bool MissionCritical;
    [Space(5f)]
    public bool CanTraversGround;
    public bool CanTraverseSea;
    public bool CanTraverseAir;
    [Space(5f)]
    public bool CanAttackGround;
    public bool CanAttackAir;
    public bool CanAttackSea;
    public bool CanAttackBuilding;

    [Space(5f)]
    [Header("Building Health Stats")]

    public float CurrentHealth;
    public float MaxHealth;

    [Space(5f)]
    [Header("Building Attack Stats")]
    public float bseAttackSpeed;
    public float bseAttackDamage;

}
