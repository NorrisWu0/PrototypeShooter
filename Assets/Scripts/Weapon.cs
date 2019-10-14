using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public bool isShotgun;
    [Range(0, 2)]
    public float fireRate;
    [Range(0, 45)]
    public float disperseRate;
    public int shotgunPelletsCount;
    public AudioClip weaponSFX;
}
