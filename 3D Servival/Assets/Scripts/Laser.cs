using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Transform startNode;
    [SerializeField] private Transform endNode;
    [SerializeField] private Transform laser;

    [Header("Damage")]
    
    public int damage;
    public int damageRate;
    List<IDamagable> things = new List<IDamagable>();

    Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        AdjustLaser();
    }

    void AdjustLaser()
    {
        Vector3 dir = endNode.position - startNode.position;

        // 벡터 차이에서 거리구하기
        float distance = dir.magnitude;
        // 중앙 레이저 위치
        laser.position = startNode.position + dir / 2f;
        // 레이저 회전 값
        laser.rotation = Quaternion.LookRotation(dir);
        // 레이저 눕히기
        laser.Rotate(90, 0, 0);

        Vector3 scale = laser.localScale;
        scale.y = distance / 2f;
        laser.localScale = scale;
    }



    IEnumerator DealDamage()
    {
        while(things.Count > 0)
        {
            for (int i = 0; i < things.Count; i++)
            {
                things[i].TakePhysicalDamage(damage);
            }

            yield return new WaitForSeconds(damageRate);
        }       
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }

        coroutine = StartCoroutine(DealDamage());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }

        if(things.Count > 1)
        {
            StopCoroutine(coroutine);
        }
    }
}
