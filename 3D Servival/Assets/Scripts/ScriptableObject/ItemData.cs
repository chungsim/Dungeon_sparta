using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Hunger
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[Serializable]
public class ItemDataPassive
{
    public PassiveType type;

    public float value;

    public float time;
}
//메뉴창에 생성메뉴 추가
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Passive")]
    public ItemDataPassive[] passives;
}
