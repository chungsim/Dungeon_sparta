using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    public float jumpForce;
    public Vector3 direction;
    public float time;

    private Coroutine coroutine;
    private List<PlayerController> launchables = new List<PlayerController>();

    [SerializeField] Image image;

    [SerializeField] Image arrowImage;


    void Start()
    {
        RotateArrow();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            launchables.Add(pc);
            Debug.Log("launcher on");
        }
        if(coroutine == null)
        {
            coroutine = StartCoroutine(Launch());  
        }       
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            if (launchables.Contains(pc))
            {
                launchables.Remove(pc);
            }
        }

    }

    void FillImage(float amount)
    {
        image.fillAmount = amount;
    }
    
    void RotateArrow()
    {
        arrowImage.rectTransform.rotation = Quaternion.LookRotation(direction);
        arrowImage.rectTransform.Rotate(90, 0, 0);
    }

    IEnumerator Launch()
    {
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            FillImage(t / time);

            if (launchables.Count < 1)
            {
                break;
            }

            yield return null;
        }
        
        if(t >= time)
        {
            foreach(PlayerController pc in launchables)
            {
                pc.OnLauncher(jumpForce, direction);
            }
        }

        yield return new WaitForSeconds(0.25f);

        while (t > 0)
        {
            t -= Time.deltaTime;

            FillImage(t / time);

            yield return null;
        }

        FillImage(0);

        yield return null;
        coroutine = null;
    }
}
