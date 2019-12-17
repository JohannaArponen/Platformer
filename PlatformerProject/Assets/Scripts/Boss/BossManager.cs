using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

  [Header("Rectangle which represents the ends of the room")]
  public Rect room;

  void OnDrawGizmosSelected() {
    if (room == null) return;
    Gizmos.DrawCube(room.center, room.size);
  }

  public ExpandDong dongRight;
  public ExpandDong dongLeft;
  public ExpandDong dongUp1;
  public ExpandDong dongUp2;
  public ExpandDong dongUp3;

  private ExpandDong[] dongs;

  // Start is called before the first frame update
  void Start() {
    dongs = GetComponentsInChildren<ExpandDong>();
  }

  // Update is called once per frame
  void Update() {

  }

  bool Expand(ExpandDong dong) {
    return dong.Expand();
  }

  [MyBox.ButtonMethod]
  void ExpandDongs() {
    foreach (var dong in dongs) {
      dong.Expand();
    }
  }
}
