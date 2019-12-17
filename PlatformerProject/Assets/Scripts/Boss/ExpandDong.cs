using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandDong : MonoBehaviour {
  [Header("Moves this GameObject along the curve for a duration")]
  public Transform start;
  public Transform end;
  private Vector3 dif { get => end.position - start.position; }
  public AnimationCurve curve = new AnimationCurve();
  public float duration = 1;

  private BossManager man;
  private bool expanded = false;
  private float expandStart = float.NegativeInfinity;


  // Start is called before the first frame update
  void Start() {
    man = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossManager>();
  }

  void Update() {
    if (expandStart > Time.time - duration) {
      var fraction = (Time.time - expandStart) / duration;
      transform.position = start.position + dif * curve.Evaluate(fraction);
    } else if (expanded) {
      expanded = false;
      transform.position = start.position + dif * curve.Evaluate(1);
    }
  }

  [MyBox.ButtonMethod]
  public bool Expand() {
    if (expanded) return false;
    expanded = true;
    expandStart = Time.time;
    transform.position = start.position + dif * curve.Evaluate(0);

    return true;
  }
}
