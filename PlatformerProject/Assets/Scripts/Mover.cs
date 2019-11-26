using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
  public Vector3 movement;
  // Update is called once per frame
  void Update() {
    transform.position += movement * Time.deltaTime;
  }
}
