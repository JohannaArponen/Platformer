using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClosestPointInShapes : MonoBehaviour {

  [Tooltip("Contains an ordered list of GameObjects where every first GameObject is the line start and every second is the line end")]
  public GameObject lineContainer;
  [Tooltip("Calculate based on this transform instead of this.transform")]
  public Transform target;
  [Tooltip("Delete gameObjects at start and save their positions as vectors only")]
  public bool deleteGameObjects = true;
  public Vector3 offset = Vector3.zero;
  public bool smoothDamp = true;
  public float dampSpeed = 1;
  public float dampMaxSpeed = 1;

  private Line[] lines = new Line[0];
  private Vector3 velocity = Vector3.zero;


  // Start is called before the first frame update
  void Start() {
    if (target == null) target = transform;
    if (deleteGameObjects) {
      var list = new List<Line> { };

      var flip = true;
      Vector2 child1 = Vector2.zero;
      Vector2 child2;
      foreach (Transform child in lineContainer.transform) {
        flip = !flip;
        if (flip) {
          child2 = child.position;
          list.Add(new Line(child1, child2));
        } else {
          child1 = child.position;
        }
        GameObject.Destroy(child.gameObject);
      }
      lines = list.ToArray();
    }
  }

  // Update is called once per frame
  void Update() {
    var minVector = Vector2.zero;
    var minVectorLength = float.PositiveInfinity;

    var pos = target.position + offset;
    if (lines.Length > 0) {
      foreach (var line in lines) {
        var dir = line.a - line.b;
        var res = ClosestPointOnLine(line.a, line.b, pos);
        if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
          minVector = res - pos.xy();
          minVectorLength = minVector.sqrMagnitude;
        }
      }
    } else {
      var flip = true;
      Transform child1 = transform; // Value only for initialization
      Transform child2;
      foreach (Transform child in lineContainer.transform) {
        flip = !flip;
        if (flip) {
          child2 = child;
          var dir = child1.transform.position - child2.transform.position;
          var res = ClosestPointOnLine(child1.transform.position, child2.transform.position, pos);
          if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
            minVector = res - pos.xy();
            minVectorLength = minVector.sqrMagnitude;
          }
        } else {
          child1 = child;
        }
      }
    }
    if (smoothDamp) {
      transform.position = Vector3.SmoothDamp(transform.position, pos + minVector.xyo(), ref velocity, dampSpeed, dampMaxSpeed, Time.deltaTime);
    } else transform.position = pos + minVector.xyo();
  }

  void OnDrawGizmos() {
    var minVector = Vector2.zero;
    var minVectorLength = float.PositiveInfinity;

    var pos = target.position + offset;
    if (lineContainer != null) {
      if (lines.Length > 0) {
        foreach (var line in lines) {
          var dir = line.a - line.b;
          var res = ClosestPointOnLine(line.a, line.b, pos);
          Gizmos.DrawLine(line.a, line.b);
          MyUtil.DrawCross(res.xyo(), 0.1f, Color.gray);
          if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
            minVector = res - pos.xy();
            minVectorLength = minVector.sqrMagnitude;
          }
        }
      } else {
        var flip = true;
        Transform child1 = transform; // Value only for initialization
        Transform child2;
        foreach (Transform child in lineContainer.transform) {
          flip = !flip;
          if (flip) {
            child2 = child;
            var dir = child1.transform.position - child2.transform.position;
            var res = ClosestPointOnLine(child1.transform.position, child2.transform.position, pos);
            Gizmos.DrawLine(child1.transform.position, child2.transform.position);
            MyUtil.DrawCross(res.xyo(), 0.1f, Color.gray);
            if (minVectorLength > (res - pos.xy()).sqrMagnitude) {
              minVector = res - pos.xy();
              minVectorLength = minVector.sqrMagnitude;
            }
          } else {
            child1 = child;
          }
        }
      }
    }
    MyUtil.DrawCross(pos.AddXY(minVector).xyo(), 0.2f, Color.green);
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

  struct Line {
    public Line(Vector3 a, Vector3 b) {
      this.a = a; this.b = b;
    }
    public Vector3 a;
    public Vector3 b;
  }
}
