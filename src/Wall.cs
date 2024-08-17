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
    public float THRESHOLD_ALPHA;
    public const float THRESHOLD_MAX_TIMER = 0.5f;

    public bool isReseting = false;
    public float timer = 0f;
    public Coroutine timeCheckCoroutine;
    public Coroutine resetCoroutine;
    public Coroutine becomeTransparentCoroutine;
    void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }
    public void Transparent()
    {
        if (IsTransparent)
        {
            timer = 0f;
            return;
        }

        if (resetCoroutine != null && isReseting)
        {
            isReseting = false;
            IsTransparent = false;
            StopCoroutine(resetCoroutine);
        }

        IsTransparent = true;
        StartCoroutine(BecomeTransparent());
    }


    public void ResetOriginalTransparent()
    {
        // SetMaterialOpaque();
        resetCoroutine = StartCoroutine(ResetOriginalTransparentCoroutine());
    }

    public IEnumerator BecomeTransparent()
    {
        bool isComplete = true;
        for (int i = 0; i < renderers.Length; i++)
        {
            while (renderers[i].material.color.a >= THRESHOLD_ALPHA)
            {
                isComplete = false;
                Color color = renderers[i].material.color;
                color.a -= Time.deltaTime;
                renderers[i].material.color = color;
                yield return null;
                Debug.Log("renderers[i].material.color.a" + renderers[i].material.color.a);
                // timer += Time.deltaTime;
            }
        }
        if (isComplete)
        {
            CheckTimer();
        }

        // Debug.Log(isComplete);
    }

    private IEnumerator ResetOriginalTransparentCoroutine()
    {
        IsTransparent = false;

        bool isComplete = true;
        for (int i = 0; i < renderers.Length; i++)
        {
            while (renderers[i].material.color.a <= 1f)
            {
                isComplete = false;
                Color color = renderers[i].material.color;
                color.a += Time.deltaTime;
                renderers[i].material.color = color;
                yield return null;
                Debug.Log("renderers[i].material.color.a" + renderers[i].material.color.a);
                // timer += Time.deltaTime;
            }
        }
        if (isComplete)
        {
            CheckTimer();
            isReseting = false;
        }
    }

    public void CheckTimer()
    {
        if (timeCheckCoroutine != null)
        {
            StopCoroutine(timeCheckCoroutine);
            Debug.Log("sss" + timeCheckCoroutine);
        }
        timeCheckCoroutine = StartCoroutine(CheckTimerCouroutine());
    }

    private IEnumerator CheckTimerCouroutine()
    {
        Debug.Log("sss");
        timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > THRESHOLD_MAX_TIMER)
            {
                isReseting = true;
                // ResetOriginalTransparent();
                break;
            }

            yield return null;
        }
    }
}
