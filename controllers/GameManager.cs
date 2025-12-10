using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static bool applicationIsQuitting = false;
    public static GameManager Instance
    {
        get
        {
            if (applicationIsQuitting)
                return null;

            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public int fallCounter = 0;

    // Event fired when the fall counter changes. Provides the new count.
    public event Action<int> OnFallCountChanged;

    private void Awake()
    {
        Application.targetFrameRate = 120;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    public void FallsCounted()
    {
        fallCounter++;
        // notify listeners (UI, analytics, etc.)
        OnFallCountChanged?.Invoke(fallCounter);
    }

    public int FallCount => fallCounter;
}