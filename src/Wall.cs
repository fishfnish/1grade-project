using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool IsTransparent = false;

    public MeshRenderer[] renderers;
    public WaitForSeconds delay = new WaitForSeconds(0.001f);
    public WaitForSeconds resetDelay = new WaitForSeconds(0.005f);
    public float MinAlpha;
    public float alphaValue;
    public float MaxTIMER;

    public bool isReseting = false;
    public float timer = 0f;
    public Coroutine timeCheckCoroutine;
    public Coroutine resetCoroutine;
    public Coroutine becomeTransparentCoroutine;

    void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        alphaValue = 1.0f;
    }

    public void BecomeTransparent()
    {
        for(int i = 0; i< renderers.Length; i++)
        {
            foreach(Material material in renderers[i].materials)
            {
                material.renderQueue = 3000;
            }
        }
        
        // if (IsTransparent)
        // {
        //     timer = 0f;
        //     return;
        // }

        // if (resetCoroutine != null && isReseting)
        // {
        //     isReseting = false;
        //     IsTransparent = false;
        //     StopCoroutine(resetCoroutine);
        // }

        // IsTransparent = true;
        // StartCoroutine(BecomeTransparentCoroutine());
    }
    public void ResetOriginalTransparent()
    {
        resetCoroutine = StartCoroutine(ResetOriginalTransparentCoroutine());
    }

    private IEnumerator BecomeTransparentCoroutine()
    {
        while (true)
        {
            bool isComplete = true;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (alphaValue > MinAlpha)
                    isComplete = false;
                
                renderers[i].material.SetFloat("_objTrans", alphaValue);
                alphaValue -= Time.deltaTime;
                Debug.Log(alphaValue);
            }

            if (isComplete)
            {
                // Debug.Log("sss");
                CheckTimer();
                break;
            }

            yield return delay;
        }
    }

    private IEnumerator ResetOriginalTransparentCoroutine()
    {
        IsTransparent = false;

        while (true)
        {
            bool isComplete = true;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (alphaValue < 1.0f)
                    isComplete = false;
                
                renderers[i].material.SetFloat("_objTrans", alphaValue);
                alphaValue += Time.deltaTime;
                Debug.Log(alphaValue);
            }

            if (isComplete)
            {
                isReseting = false;
                break;
            }

            yield return resetDelay;
        }
    }

    public void CheckTimer()
    {
        // Debug.Log("aaa");
        if (timeCheckCoroutine != null)
            StopCoroutine(timeCheckCoroutine);
        timeCheckCoroutine = StartCoroutine(CheckTimerCouroutine());
    }

    private IEnumerator CheckTimerCouroutine()
    {
        timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > MaxTIMER)
            {
                isReseting = true;
                ResetOriginalTransparent();
                break;
            }

            yield return null;
        }
    }
}
