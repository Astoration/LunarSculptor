using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon",menuName ="WeaponData")]
public class Weapon : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite image;
    public int damge;
    public float damageRatio;
    public float lunarBoost;
}
