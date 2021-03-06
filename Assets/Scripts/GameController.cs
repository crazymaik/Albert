﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public delegate void OnScoreChanged(int oldScore, int newScore);

    public event OnScoreChanged OnScoreChangedEvent;

    [SerializeField]
    private string nextLevel;
    [SerializeField]
    private AudioClip gameOverAudio;

    private AudioSource audioSource;
    private SceneFade sceneFade;
    private int fuelCollected;
    private bool isGameOver;

    void Start () {
        fuelCollected = 0;
        audioSource = GetComponent<AudioSource>();
        sceneFade = GetComponent<SceneFade>();
        sceneFade.StartFade(SceneFade.Direction.In, 0.5f);
    }

    void Update() {
    }

    public void OnFuelCollected() {
        fuelCollected++;
        OnScoreChangedEvent.Invoke(fuelCollected-1, fuelCollected);
    }

    public void LevelCompleted() {
        if (isGameOver) {
            return;
        }
        isGameOver = true;
        StartCoroutine(LoadLevel(nextLevel, 0.5f));
    }

    public void GameOver() {
        if (isGameOver) {
            return;
        }
        isGameOver = true;
        StartCoroutine(ReloadLevel());
    }

    private IEnumerator ReloadLevel() {
        audioSource.PlayOneShot(gameOverAudio);
        yield return new WaitForSeconds(gameOverAudio.length);
        yield return LoadLevel(SceneManager.GetActiveScene().name, 1.0f);
    }

    private IEnumerator LoadLevel(string level, float fadeDuration) {
        sceneFade.StartFade(SceneFade.Direction.Out, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(level);
    }
}
