using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PassiveType { Speed, Health, Hunger }

public class Passive
{
    public PassiveType Type;
    public float Value;
    public float RemainTime;

    public Passive(PassiveType type, float value, float time)
    {
        Type = type;
        Value = value;
        RemainTime = time;
    }
}

public class PlayerPassive : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private List<Passive> passiveList = new List<Passive>();
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void GetPassive(PassiveType pt, float v, float t)
    {
        Passive newPassive = new Passive(pt, v, t);
        passiveList.Add(newPassive);
        ApplyEffect(newPassive);
        StartCoroutine(Timer(newPassive));
        Debug.Log($"Passive On! you got {passiveList.Count} passives");
    }

    IEnumerator Timer(Passive p)
    {
        while (p.RemainTime > 0)
        {
            p.RemainTime -= Time.deltaTime;
            yield return null;
        }

        passiveList.Remove(p);
        unApplyEffect(p);
        Debug.Log($"Passive Over, you got {passiveList.Count} passives");
        p = null;
        if(passiveList.Count == 0)
        {
            ClearAllPassive();
        }
    }

    public void ApplyEffect(Passive p)
    {
        switch (p.Type)
        {
            case PassiveType.Speed:
                playerController.passiveSpeed += p.Value;
                break;

            case PassiveType.Hunger:
                break;

            case PassiveType.Health:
                break;
        }
        
    }

    public void unApplyEffect(Passive p)
    {
        switch (p.Type)
        {
            case PassiveType.Speed:
                playerController.passiveSpeed -= p.Value;
                break;

            case PassiveType.Hunger:
                break;

            case PassiveType.Health:
                break;
        }
    }

    void ClearAllPassive()
    {
        passiveList = null;
        playerController.passiveSpeed = 0f;
    }

}
