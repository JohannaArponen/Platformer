using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClosestPointInShapes : MonoBehaviour {

  [Tooltip("Calculate based on this transform instead of this.transform")]
  public Transform target;
  public Vector3 offset = Vector3.zero;
  public bool smoothDamp = true;
  public float dampSpeed = 1;
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

    var flip = true;
    Vector3 pos1 = Vector3.zero; // Value only for initialization
    Vector3 pos2;
    foreach (var point in lines) {
      flip = !flip;
      if (flip) {
        pos2 = point;
        var dir = pos1 - pos2;
        var res = ClosestPointOnLine(pos1, pos2, pos);
        if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
          minVector = res - pos.xy();
          minVectorLength = minVector.sqrMagnitude;
        }
      } else {
        pos1 = point;
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
    var flip = true;
    Vector3 pos1 = Vector3.zero; // Value only for initialization
    Vector3 pos2;
    foreach (var point in lines) {
      flip = !flip;
      if (flip) {
        pos2 = point;
        var dir = pos1 - pos2;
        var res = ClosestPointOnLine(pos1, pos2, pos);
        Gizmos.color = new Color(0.85f, 0.85f, 0.85f, 1);
        Gizmos.DrawSphere(res.xyo(), 0.1f);
        if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
          minVector = res - pos.xy();
          minVectorLength = minVector.sqrMagnitude;
        }
      } else {
        pos1 = point;
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
