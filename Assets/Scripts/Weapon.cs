using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string name;
    public bool isRailgun;
    public GameObject projectilePrefab;
    [Range(0, 2)]
    public float fireRate;
    [Range(0, 45)]
    public float disperseRate;

    [Header("Shotgun Setting")]
    public bool isShotgun;
    public int shotgunPelletsCount;
    public AudioClip weaponSFX;
}
