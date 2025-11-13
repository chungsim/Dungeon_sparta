using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition playerCondition;

    public PlayerAnimationController playerAnimationController;

    public PlayerPassive playerPassive;

    public ItemData itemData;
    public Action addItem;

    public Transform dropItemPos;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerPassive = GetComponent<PlayerPassive>();
    }
}
