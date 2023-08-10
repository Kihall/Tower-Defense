using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TowerType")]
public class TowerTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Sprite sprite;
    public Projectile.ProjectileType projectileType;
    public int goldCost;
}
