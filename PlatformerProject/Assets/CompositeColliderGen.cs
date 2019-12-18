using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class CompositeColliderGen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        GetComponent<CompositeCollider2D>().GenerateGeometry();
    }
}
