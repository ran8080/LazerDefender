using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] int score = 0;

    private void Awake()
    {
        SetUpSignleton();
    }

    private void SetUpSignleton() {
        if (FindObjectsOfType(GetType()).Length > 1) {
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore() {
        return score;
    }

    public void AddToScore(int amount) {
        score += amount;
    }

    public void ResetGameSession() {
        Destroy(gameObject);
    }
}
