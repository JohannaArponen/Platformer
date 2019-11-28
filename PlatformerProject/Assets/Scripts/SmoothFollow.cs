using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SmoothFollow : MonoBehaviour {

  [Space(10)]
  [Header("Not smooth")]

  [Tooltip("Value used in SmoothDamp")]
  public float speed = 0.1f;
  [Tooltip("Value used in SmoothDamp")]
  public float maxSpeed = float.PositiveInfinity;
  [Tooltip("Target Transform. Parent is used if not defined")]
  public Transform target;
  [Tooltip("Look at target position")]
  public bool look = false;
  [Tooltip("Offset from target position")]
  public Vector3 offset = Vector3.zero;
  [Tooltip("Enable following target's x position")]
  public bool x = true;
  [Tooltip("Enable following target's y position")]
  public bool y = true;
  [Tooltip("Enable following target's z position")]
  public bool z = true;

  private Vector3 velocity = Vector3.zero;

  // Start is called before the first frame update
  void Start() {
    if (!target) {
      target = gameObject.GetComponentInParent<Transform>();
    }
    var _x = x ? target.transform.position.x : transform.position.x;
    var _y = y ? target.transform.position.y : transform.position.y;
    var _z = z ? target.transform.position.z : transform.position.z;
    transform.position = new Vector3(_x, _y, _z) + offset;
  }

  // Update is called once per frame
  void Update() {
    var current = transform.position;
    var dampTarget = target.position;
    if (!x) {
      current.x = 0;
      dampTarget.x = 0;
    }
    if (!y) {
      current.y = 0;
      dampTarget.y = 0;
    }
    if (!z) {
      current.z = 0;
      dampTarget.z = 0;
    }
    var dampPos = Vector3.SmoothDamp(current, dampTarget, ref velocity, speed, maxSpeed, Time.deltaTime);

    if (!x)
      dampPos.x = transform.position.x;
    if (!y)
      dampPos.y = transform.position.y;
    if (!z)
      dampPos.z = transform.position.z;

    transform.position = dampPos + offset;

    if (look)
      transform.LookAt(target);
  }
}
