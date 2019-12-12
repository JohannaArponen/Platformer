using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MoveToClosestPointInShapes : MonoBehaviour {

  [Header("")]
  [Header("Alt: Drag overlapping points or snap to other points")]
  [Header("Control + Shift: Create new line starting from cursor position")]
  [Header("Shift: Create new lines or split lines")]
  [Header("Drag position handles to modify lines")]
  [Tooltip("Vertices that excactly at the same position are moved together")]
  public bool moveOverlappingVertices = true;
  [Tooltip("When moving vertices they try to snap to nearby vertices")]
  public float snapDistance = 0.1f;
  [Tooltip("Remvoes identical lines duh")]
  public bool removeIdentical = true;
  [Tooltip("Removes lines with identical start and end positions")]
  public bool removeZeroLength = true;
  [Tooltip("Calculate based on this transform instead of this.transform")]
  public Transform target;
  public Vector3 offset = Vector3.zero;
  public bool smoothDamp = true;
  [ConditionalField(nameof(smoothDamp))]
  public float dampSpeed = 1;
  [ConditionalField(nameof(smoothDamp))]
  public float dampMaxSpeed = 1;

  public List<Vector3> lines = new List<Vector3>();

  private Vector3 velocity = Vector3.zero;


  // Start is called before the first frame update
  void Start() {
    if (target == null) target = transform;
  }

  // Update is called once per frame
  void Update() {
    var minVector = Vector2.zero;
    var minVectorLength = float.PositiveInfinity;

    var pos = target.position;

    for (int i = 1; i < lines.Count; i += 2) {
      var line = (start: lines[i - 1], end: lines[i]);
      var dir = line.start - line.end;
      var res = ClosestPointOnLine(line.start, line.end, pos);
      if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
        minVector = res - pos.xy();
        minVectorLength = minVector.sqrMagnitude;
      }
    }

    if (smoothDamp) {
      transform.position = Vector3.SmoothDamp(transform.position, pos + minVector.xyo() + offset, ref velocity, dampSpeed, dampMaxSpeed, Time.deltaTime);
    } else transform.position = pos + minVector.xyo() + offset;
  }

  void OnDrawGizmos() {
    var minVector = Vector2.zero;
    var minVectorLength = float.PositiveInfinity;

    var pos = target == null ? transform.position : target.position;

    for (int i = 1; i < lines.Count; i += 2) {
      var line = (start: lines[i - 1], end: lines[i]);
      var dir = line.start - line.end;
      var res = ClosestPointOnLine(line.start, line.end, pos);
      if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
        minVector = res - pos.xy();
        minVectorLength = minVector.sqrMagnitude;
      }
    }
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(pos.AddXY(minVector).xyo(), 0.2f);
  }

  public static Vector2 ClosestPointOnLine(Vector2 A, Vector2 B, Vector2 P) {
    Vector2 AP = P - A; //Vector from A to P   
    Vector2 AB = B - A; //Vector from A to B  

    float magnitudeAB = AB.sqrMagnitude;  //Magnitude of AB vector (it's length squared)
    float ABAPproduct = Vector2.Dot(AP, AB);  //The DOT product of a_to_p and a_to_b
    float distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point

    if (distance < 0) return A;
    return (distance > 1 ? B : A + AB * distance);
  }
}
