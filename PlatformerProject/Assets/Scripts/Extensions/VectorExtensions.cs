// Vector3Extensions.cs
// C#
using UnityEngine;
using Unity.Mathematics;

public static class VectorExtensions {
  // Vector3
  public static Vector2 xy(this Vector3 v) => new Vector2(v.x, v.y);
  public static Vector2 xz(this Vector3 v) => new Vector2(v.x, v.z);
  public static Vector2 yz(this Vector3 v) => new Vector2(v.y, v.z);
  public static Vector2 yx(this Vector3 v) => new Vector2(v.y, v.x);
  public static Vector2 zx(this Vector3 v) => new Vector2(v.z, v.x);
  public static Vector2 zy(this Vector3 v) => new Vector2(v.z, v.y);

  public static Vector3 xyz(this Vector3 v) => new Vector3(v.x, v.y, v.z);
  public static Vector3 xzy(this Vector3 v) => new Vector3(v.x, v.z, v.y);
  public static Vector3 yzx(this Vector3 v) => new Vector3(v.y, v.z, v.x);
  public static Vector3 yxz(this Vector3 v) => new Vector3(v.y, v.x, v.z);
  public static Vector3 zxy(this Vector3 v) => new Vector3(v.z, v.x, v.y);
  public static Vector3 zyx(this Vector3 v) => new Vector3(v.z, v.y, v.x);



  public static Vector3 Add(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);


  public static Vector3 Add2X(this Vector3 v, int b) => new Vector3(v.x + b, v.y, v.z);
  public static Vector3 Add2Y(this Vector3 v, int b) => new Vector3(v.x, v.y + b, v.z);
  public static Vector3 Add2Z(this Vector3 v, int b) => new Vector3(v.x, v.y, v.z + b);
  public static Vector3 Add2X(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z);
  public static Vector3 Add2Y(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z);
  public static Vector3 Add2Z(this Vector3 v, float b) => new Vector3(v.x, v.y, v.z + b);


  public static Vector3 Add2XY(this Vector3 v, Vector2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
  public static Vector3 Add2YX(this Vector3 v, Vector2 b) => new Vector3(v.x + b.y, v.y + b.x, v.z);
  public static Vector3 Add2YZ(this Vector3 v, Vector2 b) => new Vector3(v.x, v.y + b.x, v.z + b.y);
  public static Vector3 Add2ZY(this Vector3 v, Vector2 b) => new Vector3(v.x, v.y + b.y, v.z + b.x);
  public static Vector3 Add2XZ(this Vector3 v, Vector2 b) => new Vector3(v.x + b.x, v.y, v.z + b.y);
  public static Vector3 Add2ZX(this Vector3 v, Vector2 b) => new Vector3(v.x + b.y, v.y, v.z + b.x);

  public static Vector3 Add2XY(this Vector3 v, float2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
  public static Vector3 Add2YX(this Vector3 v, float2 b) => new Vector3(v.x + b.y, v.y + b.x, v.z);
  public static Vector3 Add2YZ(this Vector3 v, float2 b) => new Vector3(v.x, v.y + b.x, v.z + b.y);
  public static Vector3 Add2ZY(this Vector3 v, float2 b) => new Vector3(v.x, v.y + b.y, v.z + b.x);
  public static Vector3 Add2XZ(this Vector3 v, float2 b) => new Vector3(v.x + b.x, v.y, v.z + b.y);
  public static Vector3 Add2ZX(this Vector3 v, float2 b) => new Vector3(v.x + b.y, v.y, v.z + b.x);

  public static Vector3 Add2XY(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 Add2YX(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 Add2YZ(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 Add2ZY(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 Add2XZ(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z + b);
  public static Vector3 Add2ZX(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z + b);

  public static Vector3 Add2XY(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 Add2YX(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 Add2YZ(this Vector3 v, int b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 Add2ZY(this Vector3 v, int b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 Add2XZ(this Vector3 v, int b) => new Vector3(v.x + b, v.y, v.z + b);
  public static Vector3 Add2ZX(this Vector3 v, int b) => new Vector3(v.x + b, v.y, v.z + b);


  public static Vector3 Add2XYZ(this Vector3 v, Vector3 b) => new Vector3(v.x + b.x, v.y + b.y, v.z + b.z);
  public static Vector3 Add2XZY(this Vector3 v, Vector3 b) => new Vector3(v.x + b.x, v.y + b.z, v.z + b.y);
  public static Vector3 Add2YZX(this Vector3 v, Vector3 b) => new Vector3(v.x + b.y, v.y + b.z, v.z + b.x);
  public static Vector3 Add2YXZ(this Vector3 v, Vector3 b) => new Vector3(v.x + b.y, v.y + b.x, v.z + b.z);
  public static Vector3 Add2ZXY(this Vector3 v, Vector3 b) => new Vector3(v.x + b.z, v.y + b.x, v.z + b.y);
  public static Vector3 Add2ZYX(this Vector3 v, Vector3 b) => new Vector3(v.x + b.z, v.y + b.y, v.z + b.x);

  public static Vector3 Add2XYZ(this Vector3 v, float3 b) => new Vector3(v.x + b.x, v.y + b.y, v.z + b.z);
  public static Vector3 Add2XZY(this Vector3 v, float3 b) => new Vector3(v.x + b.x, v.y + b.z, v.z + b.y);
  public static Vector3 Add2YZX(this Vector3 v, float3 b) => new Vector3(v.x + b.y, v.y + b.z, v.z + b.x);
  public static Vector3 Add2YXZ(this Vector3 v, float3 b) => new Vector3(v.x + b.y, v.y + b.x, v.z + b.z);
  public static Vector3 Add2ZXY(this Vector3 v, float3 b) => new Vector3(v.x + b.z, v.y + b.x, v.z + b.y);
  public static Vector3 Add2ZYX(this Vector3 v, float3 b) => new Vector3(v.x + b.z, v.y + b.y, v.z + b.x);

  public static Vector3 Add2XYZ(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2XZY(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2YZX(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2YXZ(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2ZXY(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2ZYX(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);

  public static Vector3 Add2XYZ(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2XZY(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2YZX(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2YXZ(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2ZXY(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add2ZYX(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);



  public static Vector3 Set2X(this Vector3 v, int b) => new Vector3(b, v.y, v.z);
  public static Vector3 Set2Y(this Vector3 v, int b) => new Vector3(v.x, b, v.z);
  public static Vector3 Set2Z(this Vector3 v, int b) => new Vector3(v.x, v.y, b);
  public static Vector3 Set2X(this Vector3 v, float b) => new Vector3(b, v.y, v.z);
  public static Vector3 Set2Y(this Vector3 v, float b) => new Vector3(v.x, b, v.z);
  public static Vector3 Set2Z(this Vector3 v, float b) => new Vector3(v.x, v.y, b);


  public static Vector3 Set2XY(this Vector3 v, Vector2 b) => new Vector3(b.x, b.y, v.z);
  public static Vector3 Set2YX(this Vector3 v, Vector2 b) => new Vector3(b.y, b.x, v.z);
  public static Vector3 Set2YZ(this Vector3 v, Vector2 b) => new Vector3(v.x, b.x, b.y);
  public static Vector3 Set2ZY(this Vector3 v, Vector2 b) => new Vector3(v.x, b.y, b.x);
  public static Vector3 Set2XZ(this Vector3 v, Vector2 b) => new Vector3(b.x, v.y, b.y);
  public static Vector3 Set2ZX(this Vector3 v, Vector2 b) => new Vector3(b.y, v.y, b.x);

  public static Vector3 Set2XY(this Vector3 v, float2 b) => new Vector3(b.x, b.y, v.z);
  public static Vector3 Set2YX(this Vector3 v, float2 b) => new Vector3(b.y, b.x, v.z);
  public static Vector3 Set2YZ(this Vector3 v, float2 b) => new Vector3(v.x, b.x, b.y);
  public static Vector3 Set2ZY(this Vector3 v, float2 b) => new Vector3(v.x, b.y, b.x);
  public static Vector3 Set2XZ(this Vector3 v, float2 b) => new Vector3(b.x, v.y, b.y);
  public static Vector3 Set2ZX(this Vector3 v, float2 b) => new Vector3(b.y, v.y, b.x);

  public static Vector3 Set2XY(this Vector3 v, float b) => new Vector3(b, b, v.z);
  public static Vector3 Set2YX(this Vector3 v, float b) => new Vector3(b, b, v.z);
  public static Vector3 Set2YZ(this Vector3 v, float b) => new Vector3(v.x, b, b);
  public static Vector3 Set2ZY(this Vector3 v, float b) => new Vector3(v.x, b, b);
  public static Vector3 Set2XZ(this Vector3 v, float b) => new Vector3(b, v.y, b);
  public static Vector3 Set2ZX(this Vector3 v, float b) => new Vector3(b, v.y, b);

  public static Vector3 Set2XY(this Vector3 v, int b) => new Vector3(b, b, v.z);
  public static Vector3 Set2YX(this Vector3 v, int b) => new Vector3(b, b, v.z);
  public static Vector3 Set2YZ(this Vector3 v, int b) => new Vector3(v.x, b, b);
  public static Vector3 Set2ZY(this Vector3 v, int b) => new Vector3(v.x, b, b);
  public static Vector3 Set2XZ(this Vector3 v, int b) => new Vector3(b, v.y, b);
  public static Vector3 Set2ZX(this Vector3 v, int b) => new Vector3(b, v.y, b);


  public static Vector3 Set2XYZ(this Vector3 v, Vector3 b) => new Vector3(b.x, b.y, b.z);
  public static Vector3 Set2XZY(this Vector3 v, Vector3 b) => new Vector3(b.x, b.z, b.y);
  public static Vector3 Set2YZX(this Vector3 v, Vector3 b) => new Vector3(b.y, b.z, b.x);
  public static Vector3 Set2YXZ(this Vector3 v, Vector3 b) => new Vector3(b.y, b.x, b.z);
  public static Vector3 Set2ZXY(this Vector3 v, Vector3 b) => new Vector3(b.z, b.x, b.y);
  public static Vector3 Set2ZYX(this Vector3 v, Vector3 b) => new Vector3(b.z, b.y, b.x);

  public static Vector3 Set2XYZ(this Vector3 v, float3 b) => new Vector3(b.x, b.y, b.z);
  public static Vector3 Set2XZY(this Vector3 v, float3 b) => new Vector3(b.x, b.z, b.y);
  public static Vector3 Set2YZX(this Vector3 v, float3 b) => new Vector3(b.y, b.z, b.x);
  public static Vector3 Set2YXZ(this Vector3 v, float3 b) => new Vector3(b.y, b.x, b.z);
  public static Vector3 Set2ZXY(this Vector3 v, float3 b) => new Vector3(b.z, b.x, b.y);
  public static Vector3 Set2ZYX(this Vector3 v, float3 b) => new Vector3(b.z, b.y, b.x);

  public static Vector3 Set2XYZ(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 Set2XZY(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 Set2YZX(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 Set2YXZ(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 Set2ZXY(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 Set2ZYX(this Vector3 v, float b) => new Vector3(b, b, b);

  public static Vector3 Set2XYZ(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 Set2XZY(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 Set2YZX(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 Set2YXZ(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 Set2ZXY(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 Set2ZYX(this Vector3 v, int b) => new Vector3(b, b, b);



  // Vector2
  public static Vector2 xy(this Vector2 v) => new Vector2(v.x, v.y);
  public static Vector2 yx(this Vector2 v) => new Vector2(v.y, v.x);


  public static Vector2 Add(this Vector2 v, int b) => new Vector2(v.x + b, v.y + b);
  public static Vector2 Add(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);

  public static Vector2 Add2X(this Vector2 v, int b) => new Vector2(v.x + b, v.y);
  public static Vector2 Add2Y(this Vector2 v, int b) => new Vector2(v.x, v.y + b);

  public static Vector2 Add2X(this Vector2 v, float b) => new Vector2(v.x + b, v.y);
  public static Vector2 Add2Y(this Vector2 v, float b) => new Vector2(v.x, v.y + b);


  public static Vector2 Add2XY(this Vector2 v, Vector2 b) => new Vector2(v.x + b.x, v.y + b.y);
  public static Vector2 Add2YX(this Vector2 v, Vector2 b) => new Vector2(v.x + b.y, v.y + b.x);

  public static Vector2 Add2XY(this Vector2 v, float2 b) => new Vector2(v.x + b.x, v.y + b.y);
  public static Vector2 Add2YX(this Vector2 v, float2 b) => new Vector2(v.x + b.y, v.y + b.x);


  public static Vector2 Add2XY(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);
  public static Vector2 Add2YX(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);

  public static Vector2 Add2XY(this Vector2 v, int b) => new Vector2(v.x + b, v.y + b);
  public static Vector2 Add2YX(this Vector2 v, int b) => new Vector2(v.x + b, v.y + b);

}