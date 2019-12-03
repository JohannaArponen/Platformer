using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    public int score = 0;
    public Text scoreText;
    public Text centerText;
    public GameObject persistenDataPrefab;
    PersistenData pData;

    void UpdateScoreText() {
        int visibleHighscore = score > pData.highScore ? score : pData.highScore;
        scoreText.text = "Score: " + score + " High: " + visibleHighscore;
    }
    
    void Start(){
        pData = FindObjectOfType<PersistenData>();
        if(pData == null) {
            pData = Instantiate(persistenDataPrefab).GetComponent<PersistenData>();
        }
        UpdateScoreText();
        centerText.text = "Start!";
        Invoke("ClearCenterText", 2);
    }

    public void ClearCenterText() {
        centerText.text = " ";
    }

    
    void Update(){
        
    }

    public void EnemyDestroy() {
        score++;
        UpdateScoreText();
        if(score > pData.highScore) {
            print("New HighScore");
            pData.highScore = score;
        }
    }
}
