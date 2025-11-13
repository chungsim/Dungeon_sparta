using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UIConditions uIConditions;

    Condition health { get { return uIConditions.health; } }
    Condition hunger { get { return uIConditions.hunger; } }
    Condition stamina { get { return uIConditions.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    // Update is called once per frame
    void Update()
    {
        hunger.Substract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Substract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }


    public void Heal(float v)
    {
        health.Add(v);
    }

    public void Eat(float v)
    {
        hunger.Add(v);
    }

    void Die()
    {
        Debug.Log("Player died");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Substract(damage);
        onTakeDamage?.Invoke();
    }
}
