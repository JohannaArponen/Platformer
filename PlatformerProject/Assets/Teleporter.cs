using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Teleporter : MonoBehaviour {
  public Transform target;
  public string tagFilter = "";
  private Collider2D col;

  // Start is called before the first frame update
  void Start() {
    col = GetComponent<Collider2D>();
  }

  // Update is called once per frame
  void Update() {
    var results = new List<Collider2D>();
    col.OverlapCollider(default(ContactFilter2D), results);

    foreach (var col in results) {
      if (col.tag == tagFilter) {
        col.transform.position = target.position;
      }
    }
  }
}
