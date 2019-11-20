using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class MyUtil {

  public static void DrawCross(float3 pos, float radius = 0.1f, Color color = new Color(), float duration = 0) {
    Debug.DrawLine(pos + new float3(0, -radius, 0), pos + new float3(0, radius, 0), color, duration);
    Debug.DrawLine(pos + new float3(-radius, 0, 0), pos + new float3(radius, 0, 0), color, duration);
    Debug.DrawLine(pos + new float3(0, 0, -radius), pos + new float3(0, 0, radius), color, duration);
  }

  public static void DrawBox(float2 pos, Color color = new Color(), float duration = 0) {
    Debug.DrawLine(new float3(pos.x, 0, pos.y), new float3(pos.x + 1, 0, pos.y), color, duration);
    Debug.DrawLine(new float3(pos.x, 0, pos.y), new float3(pos.x, 0, pos.y + 1), color, duration);
    Debug.DrawLine(new float3(pos.x + 1, 0, pos.y + 1), new float3(pos.x + 1, 0, pos.y), color, duration);
    Debug.DrawLine(new float3(pos.x + 1, 0, pos.y + 1), new float3(pos.x, 0, pos.y + 1), color, duration);
  }

  public static void DrawCube(float3 pos, Color color = new Color(), float duration = 0) {
    Debug.DrawLine(pos, pos + new float3(1, 0, 0), color, duration);
    Debug.DrawLine(pos, pos + new float3(0, 0, 1), color, duration);
    Debug.DrawLine(pos + new float3(1, 0, 1), pos + new float3(0, 0, 1), color, duration);
    Debug.DrawLine(pos + new float3(1, 0, 1), pos + new float3(1, 0, 0), color, duration);

    Debug.DrawLine(pos + new float3(0, 1, 0), pos + new float3(1, 1, 0), color, duration);
    Debug.DrawLine(pos + new float3(0, 1, 0), pos + new float3(0, 1, 1), color, duration);
    Debug.DrawLine(pos + new float3(1, 1, 1), pos + new float3(1, 1, 0), color, duration);
    Debug.DrawLine(pos + new float3(1, 1, 1), pos + new float3(0, 1, 1), color, duration);

    Debug.DrawLine(pos + new float3(0, 0, 0), pos + new float3(0, 1, 0), color, duration);
    Debug.DrawLine(pos + new float3(1, 0, 0), pos + new float3(1, 1, 0), color, duration);
    Debug.DrawLine(pos + new float3(1, 0, 1), pos + new float3(1, 1, 1), color, duration);
    Debug.DrawLine(pos + new float3(0, 0, 1), pos + new float3(0, 1, 1), color, duration);
  }
}
