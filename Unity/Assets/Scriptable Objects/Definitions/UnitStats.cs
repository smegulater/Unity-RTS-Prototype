using UnityEngine;
using UnityRTSCore;
using System.Collections;

[CreateAssetMenu(fileName ="New Unit", menuName ="UnityRTS/Unit")]
public class UnitStats : ScriptableObject
{
    [Header("General Unit Settings")]
    public string Name;
    public string description;
    public UnitType unitType;

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
    [Header("Unit Health Stats")]

    public float CurrentHealth;
    public float MaxHealth;
    [Range(0f, 1f)]
    public float CurrentStamina;
    public float MaxStamina;

    [Space(5f)]
    [Header("Unit Attack Stats")]
    public float bseAttackSpeed;
    public float bseAttackDamage;



}
