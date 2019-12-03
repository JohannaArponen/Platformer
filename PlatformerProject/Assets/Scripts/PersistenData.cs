using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenData : MonoBehaviour{
    public int highScore = 0;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
   
}
