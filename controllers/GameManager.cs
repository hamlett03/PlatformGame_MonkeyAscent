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

    public bool IsLoadingContinue { get; set; } = false;

    public void SaveGame(Vector3 playerPosition, AbilityManager abilityManager)
    {
        // save player position
        PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.SetInt("SavedFalls", fallCounter);

        // save camera position
        if (Camera.main != null)
        {
            PlayerPrefs.SetFloat("CamX", Camera.main.transform.position.x);
            PlayerPrefs.SetFloat("CamY", Camera.main.transform.position.y);
        }

        // save abilities
        string abilitiesString = "";

        if (abilityManager != null)
        {
            foreach (var ability in abilityManager.unlockedAbilities)
            {
                abilitiesString += ability.abilityName + ",";
            }
        }

        PlayerPrefs.SetString("SavedAbilities", abilitiesString);

        PlayerPrefs.Save();
    }

    // load player position
    public Vector3 GetSavedPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");

        return new Vector2(x, y);
    }

    public Vector3 GetSavedCameraPosition()
    {
        float x = PlayerPrefs.GetFloat("CamX", Camera.main.transform.position.x);
        float y = PlayerPrefs.GetFloat("CamY", Camera.main.transform.position.y);

        return new Vector3(x, y, Camera.main.transform.position.z);
    }

    // get saved fall count
    public void LoadFalls()
    {
        if (PlayerPrefs.HasKey("SavedFalls"))
        {
            fallCounter = PlayerPrefs.GetInt("SavedFalls");
            OnFallCountChanged?.Invoke(fallCounter);
        }
    }

    // get saved abilities
    public void LoadAbilities(AbilityManager abilityManager)
    {
        if (PlayerPrefs.HasKey("SavedAbilities"))
        {
            string savedData = PlayerPrefs.GetString("SavedAbilities");
            string[] abilityNames = savedData.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var abilityName in abilityNames)
            {
                abilityManager.RestoreAbilityByName(abilityName);
            }
        }
    }

    // erase saved game
    public void ClearSave()
    {
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("HasSavedGame");
        PlayerPrefs.DeleteKey("SavedFalls");
        fallCounter = 0;
    }

    // verify if the game has been saved
    public bool HasSaveFile()
    {
        return PlayerPrefs.HasKey("HasSavedGame");
    }

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