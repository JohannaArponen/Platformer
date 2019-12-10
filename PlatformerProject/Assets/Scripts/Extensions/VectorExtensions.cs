using UnityEngine;
using Unity.Mathematics;

public static class VectorExtensions {

  // *********************** float2 *********************** //

  public static bool LongerThan(this float2 v, float2 b) => math.lengthsq(v) > math.lengthsq(b);


  public static float2 SetLen(this float2 v, float length) => math.normalize(v) * length;
  public static float2 SetLenSafe(this float2 v, float length, float2 defaultValue) => math.normalize(v.x != 0 || v.y != 0 ? v : defaultValue) * length;
  public static float2 SetLenSafer(this float2 v, float length, float2 defaultValue = default(float2)) => math.normalizesafe(v.x != 0 || v.y != 0 ? v : defaultValue) * length;
  public static float2 AddLen(this float2 v, float addition) => math.normalize(v) * (math.length(v) + addition);
  public static float2 AddLenSafe(this float2 v, float addition) => math.normalizesafe(v) * (math.length(v) + addition);



  public static float2 SetDir(this float2 v, float2 d) => math.normalize(d * math.length(v));
  public static float2 SetDirSafe(this float2 v, float2 d) => math.normalizesafe(d * math.length(v));


  // *********************** float3 *********************** //

  public static bool LongerThan(this float3 v, float3 b) => math.lengthsq(v) > math.lengthsq(b);


  public static float3 SetLen(this float3 v, float length) => math.normalize(v) * length;
  public static float3 SetLenSafe(this float3 v, float length, float3 defaultValue) => math.normalize(v.x != 0 || v.y != 0 || v.z != 0 ? v : defaultValue) * length;
  public static float3 SetLenSafer(this float3 v, float length, float3 defaultValue = default(float3)) => math.normalizesafe(v.x != 0 || v.y != 0 || v.z != 0 ? v : defaultValue) * length;
  public static float3 AddLen(this float3 v, float addition) => math.normalize(v) * (math.length(v) + addition);
  public static float3 AddLenSafe(this float3 v, float addition) => math.normalizesafe(v) * (math.length(v) + addition);



  public static float3 SetDir(this float3 v, float3 d) => math.normalize(d * math.length(v));
  public static float3 SetDirSafe(this float3 v, float3 d) => math.normalizesafe(d * math.length(v));


  // *********************** Vector2 *********************** //

  public static Vector2 SetLen(this Vector2 v, float length) => v.normalized * length;
  public static Vector2 SetLenSafe(this Vector2 v, float length, Vector2 defaultValue) => math.normalize(v.x != 0 || v.y != 0 ? v : defaultValue) * length;
  public static Vector2 SetLenSafer(this Vector2 v, float length, Vector2 defaultValue = default(Vector2)) => math.normalizesafe(v.x != 0 || v.y != 0 ? v : defaultValue) * length;
  public static Vector2 AddLen(this Vector2 v, float addition) => v.normalized * (v.magnitude + addition);
  public static Vector2 AddLenSafe(this Vector2 v, float addition) => math.normalizesafe(v) * (v.magnitude + addition);



  public static Vector2 SetDir(this Vector2 v, Vector2 d) => d.normalized * v.magnitude;
  public static Vector2 SetDirSafe(this Vector2 v, Vector2 d) => math.normalizesafe(d * math.length(v));



  public static Vector2 xy(this Vector2 v) => new Vector2(v.x, v.y);
  public static Vector2 yx(this Vector2 v) => new Vector2(v.y, v.x);

  public static Vector3 xxx(this Vector2 v) => new Vector3(v.x, v.x, v.x);
  public static Vector3 yyy(this Vector2 v) => new Vector3(v.y, v.y, v.y);

  public static Vector3 xxy(this Vector2 v) => new Vector3(v.x, v.x, v.y);
  public static Vector3 xyy(this Vector2 v) => new Vector3(v.x, v.y, v.y);
  public static Vector3 yyx(this Vector2 v) => new Vector3(v.y, v.y, v.x);
  public static Vector3 xyx(this Vector2 v) => new Vector3(v.x, v.y, v.x);
  public static Vector3 yxx(this Vector2 v) => new Vector3(v.y, v.x, v.x);



  public static Vector2 xo(this Vector2 v) => new Vector2(v.x, 0);
  public static Vector2 ox(this Vector2 v) => new Vector2(0, v.x);
  public static Vector2 oy(this Vector2 v) => new Vector2(0, v.y);
  public static Vector2 yo(this Vector2 v) => new Vector2(v.y, 0);
  public static Vector2 oo(this Vector2 v) => new Vector2(0, 0);


  public static Vector3 oxx(this Vector2 v) => new Vector3(0, v.x, v.x);
  public static Vector3 xox(this Vector2 v) => new Vector3(v.x, 0, v.x);
  public static Vector3 xxo(this Vector2 v) => new Vector3(v.x, v.x, 0);
  public static Vector3 oxo(this Vector2 v) => new Vector3(0, v.x, 0);
  public static Vector3 oox(this Vector2 v) => new Vector3(0, 0, v.x);
  public static Vector3 xoo(this Vector2 v) => new Vector3(v.x, 0, 0);
  public static Vector3 ooo(this Vector2 v) => new Vector3(0, 0, 0);

  public static Vector3 oyy(this Vector2 v) => new Vector3(0, v.y, v.y);
  public static Vector3 yoy(this Vector2 v) => new Vector3(v.y, 0, v.y);
  public static Vector3 yyo(this Vector2 v) => new Vector3(v.y, v.y, 0);
  public static Vector3 oyo(this Vector2 v) => new Vector3(0, v.y, 0);
  public static Vector3 ooy(this Vector2 v) => new Vector3(0, 0, v.y);
  public static Vector3 yoo(this Vector2 v) => new Vector3(v.y, 0, 0);

  public static Vector3 xyo(this Vector2 v) => new Vector3(v.x, v.y, 0);
  public static Vector3 oxy(this Vector2 v) => new Vector3(0, v.x, v.y);
  public static Vector3 yox(this Vector2 v) => new Vector3(v.y, 0, v.x);
  public static Vector3 oyx(this Vector2 v) => new Vector3(0, v.y, v.x);
  public static Vector3 xoy(this Vector2 v) => new Vector3(v.x, 0, v.y);
  public static Vector3 yxo(this Vector2 v) => new Vector3(v.y, v.x, 0);



  public static Vector2 Add(this Vector2 v, int b) => new Vector2(v.x + b, v.y + b);
  public static Vector2 Add(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);

  public static Vector2 AddX(this Vector2 v, int b) => new Vector2(v.x + b, v.y);
  public static Vector2 AddY(this Vector2 v, int b) => new Vector2(v.x, v.y + b);

  public static Vector2 AddX(this Vector2 v, float b) => new Vector2(v.x + b, v.y);
  public static Vector2 AddY(this Vector2 v, float b) => new Vector2(v.x, v.y + b);


  public static Vector2 AddXY(this Vector2 v, Vector2 b) => new Vector2(v.x + b.x, v.y + b.y);
  public static Vector2 AddYX(this Vector2 v, Vector2 b) => new Vector2(v.x + b.y, v.y + b.x);

  public static Vector2 AddXY(this Vector2 v, float2 b) => new Vector2(v.x + b.x, v.y + b.y);
  public static Vector2 AddYX(this Vector2 v, float2 b) => new Vector2(v.x + b.y, v.y + b.x);

  public static Vector2 AddXY(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);
  public static Vector2 AddYX(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);

  public static Vector2 AddXY(this Vector2 v, int b) => new Vector2(v.x + b, v.y + b);
  public static Vector2 AddYX(this Vector2 v, int b) => new Vector2(v.x + b, v.y + b);



  public static Vector2 Set(this Vector2 v, int b) => new Vector2(b, b);
  public static Vector2 Set(this Vector2 v, float b) => new Vector2(b, b);

  public static Vector2 SetX(this Vector2 v, int b) => new Vector2(b, v.y);
  public static Vector2 SetY(this Vector2 v, int b) => new Vector2(v.x, b);

  public static Vector2 SetX(this Vector2 v, float b) => new Vector2(b, v.y);
  public static Vector2 SetY(this Vector2 v, float b) => new Vector2(v.x, b);


  public static Vector2 SetXY(this Vector2 v, Vector2 b) => new Vector2(b.x, b.y);
  public static Vector2 SetYX(this Vector2 v, Vector2 b) => new Vector2(b.y, b.x);

  public static Vector2 SetXY(this Vector2 v, float2 b) => new Vector2(b.x, b.y);
  public static Vector2 SetYX(this Vector2 v, float2 b) => new Vector2(b.y, b.x);

  public static Vector2 SetXY(this Vector2 v, float b) => new Vector2(b, b);
  public static Vector2 SetYX(this Vector2 v, float b) => new Vector2(b, b);

  public static Vector2 SetXY(this Vector2 v, int b) => new Vector2(b, b);
  public static Vector2 SetYX(this Vector2 v, int b) => new Vector2(b, b);

  public static Vector2 SetXY(this Vector2 v, float b, float c) => new Vector2(b, c);
  public static Vector2 SetYX(this Vector2 v, float b, float c) => new Vector2(b, c);

  public static Vector2 SetXY(this Vector2 v, int b, int c) => new Vector2(b, c);
  public static Vector2 SetYX(this Vector2 v, int b, int c) => new Vector2(b, c);


  // *********************** Vector3 *********************** //

  public static bool LongerThan(this Vector3 v, Vector3 b) => v.sqrMagnitude > b.sqrMagnitude;


  public static Vector3 SetLen(this Vector3 v, float length) => v.normalized * length;
  public static Vector3 SetLenSafe(this Vector3 v, float length, Vector3 defaultValue) => math.normalize(v.x != 0 || v.y != 0 || v.z != 0 ? v : defaultValue) * length;
  public static Vector3 SetLenSafer(this Vector3 v, float length, Vector3 defaultValue = default(Vector3)) => math.normalizesafe(v.x != 0 || v.y != 0 || v.z != 0 ? v : defaultValue) * length;
  public static Vector3 AddLen(this Vector3 v, float addition) => v.normalized * (v.magnitude + addition);
  public static Vector3 AddLenSafe(this Vector3 v, float addition) => math.normalizesafe(v) * (v.magnitude + addition);



  public static Vector3 SetDir(this Vector3 v, Vector3 d) => d.normalized * v.magnitude;
  public static Vector3 SetDirSafe(this Vector3 v, Vector3 d) => math.normalizesafe(d * math.length(v));



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



  public static Vector2 xo(this Vector3 v) => new Vector2(v.x, 0);
  public static Vector2 ox(this Vector3 v) => new Vector2(0, v.x);
  public static Vector2 oy(this Vector3 v) => new Vector2(0, v.y);
  public static Vector2 yo(this Vector3 v) => new Vector2(v.y, 0);
  public static Vector2 zo(this Vector3 v) => new Vector2(v.z, 0);
  public static Vector2 oz(this Vector3 v) => new Vector2(0, v.z);
  public static Vector2 oo(this Vector3 v) => new Vector2(0, 0);


  public static Vector3 oxx(this Vector3 v) => new Vector3(0, v.x, v.x);
  public static Vector3 xox(this Vector3 v) => new Vector3(v.x, 0, v.x);
  public static Vector3 xxo(this Vector3 v) => new Vector3(v.x, v.x, 0);
  public static Vector3 oxo(this Vector3 v) => new Vector3(0, v.x, 0);
  public static Vector3 oox(this Vector3 v) => new Vector3(0, 0, v.x);
  public static Vector3 xoo(this Vector3 v) => new Vector3(v.x, 0, 0);
  public static Vector3 ooo(this Vector3 v) => new Vector3(0, 0, 0);

  public static Vector3 oyy(this Vector3 v) => new Vector3(0, v.y, v.y);
  public static Vector3 yoy(this Vector3 v) => new Vector3(v.y, 0, v.y);
  public static Vector3 yyo(this Vector3 v) => new Vector3(v.y, v.y, 0);
  public static Vector3 oyo(this Vector3 v) => new Vector3(0, v.y, 0);
  public static Vector3 ooy(this Vector3 v) => new Vector3(0, 0, v.y);
  public static Vector3 yoo(this Vector3 v) => new Vector3(v.y, 0, 0);

  public static Vector3 ozz(this Vector3 v) => new Vector3(0, v.z, v.z);
  public static Vector3 zoz(this Vector3 v) => new Vector3(v.z, 0, v.z);
  public static Vector3 zzo(this Vector3 v) => new Vector3(v.z, v.z, 0);
  public static Vector3 ozo(this Vector3 v) => new Vector3(0, v.z, 0);
  public static Vector3 ooz(this Vector3 v) => new Vector3(0, 0, v.z);
  public static Vector3 zoo(this Vector3 v) => new Vector3(v.z, 0, 0);


  public static Vector3 xyo(this Vector3 v) => new Vector3(v.x, v.y, 0);
  public static Vector3 oxy(this Vector3 v) => new Vector3(0, v.x, v.y);
  public static Vector3 yox(this Vector3 v) => new Vector3(v.y, 0, v.x);
  public static Vector3 oyx(this Vector3 v) => new Vector3(0, v.y, v.x);
  public static Vector3 xoy(this Vector3 v) => new Vector3(v.x, 0, v.y);
  public static Vector3 yxo(this Vector3 v) => new Vector3(v.y, v.x, 0);

  public static Vector3 xzo(this Vector3 v) => new Vector3(v.x, v.z, 0);
  public static Vector3 oxz(this Vector3 v) => new Vector3(0, v.x, v.z);
  public static Vector3 zox(this Vector3 v) => new Vector3(v.z, 0, v.x);
  public static Vector3 ozx(this Vector3 v) => new Vector3(0, v.z, v.x);
  public static Vector3 xoz(this Vector3 v) => new Vector3(v.x, 0, v.z);
  public static Vector3 zxo(this Vector3 v) => new Vector3(v.y, v.x, 0);

  public static Vector3 zyo(this Vector3 v) => new Vector3(v.z, v.y, 0);
  public static Vector3 ozy(this Vector3 v) => new Vector3(0, v.z, v.y);
  public static Vector3 yoz(this Vector3 v) => new Vector3(v.y, 0, v.z);
  public static Vector3 oyz(this Vector3 v) => new Vector3(0, v.y, v.z);
  public static Vector3 zoy(this Vector3 v) => new Vector3(v.z, 0, v.y);
  public static Vector3 yzo(this Vector3 v) => new Vector3(v.y, v.z, 0);



  public static Vector3 Add(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 Add(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);


  public static Vector3 AddX(this Vector3 v, int b) => new Vector3(v.x + b, v.y, v.z);
  public static Vector3 AddY(this Vector3 v, int b) => new Vector3(v.x, v.y + b, v.z);
  public static Vector3 AddZ(this Vector3 v, int b) => new Vector3(v.x, v.y, v.z + b);
  public static Vector3 AddX(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z);
  public static Vector3 AddY(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z);
  public static Vector3 AddZ(this Vector3 v, float b) => new Vector3(v.x, v.y, v.z + b);


  public static Vector3 AddXY(this Vector3 v, Vector2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
  public static Vector3 AddYX(this Vector3 v, Vector2 b) => new Vector3(v.x + b.y, v.y + b.x, v.z);
  public static Vector3 AddYZ(this Vector3 v, Vector2 b) => new Vector3(v.x, v.y + b.x, v.z + b.y);
  public static Vector3 AddZY(this Vector3 v, Vector2 b) => new Vector3(v.x, v.y + b.y, v.z + b.x);
  public static Vector3 AddXZ(this Vector3 v, Vector2 b) => new Vector3(v.x + b.x, v.y, v.z + b.y);
  public static Vector3 AddZX(this Vector3 v, Vector2 b) => new Vector3(v.x + b.y, v.y, v.z + b.x);

  public static Vector3 AddXY(this Vector3 v, float2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
  public static Vector3 AddYX(this Vector3 v, float2 b) => new Vector3(v.x + b.y, v.y + b.x, v.z);
  public static Vector3 AddYZ(this Vector3 v, float2 b) => new Vector3(v.x, v.y + b.x, v.z + b.y);
  public static Vector3 AddZY(this Vector3 v, float2 b) => new Vector3(v.x, v.y + b.y, v.z + b.x);
  public static Vector3 AddXZ(this Vector3 v, float2 b) => new Vector3(v.x + b.x, v.y, v.z + b.y);
  public static Vector3 AddZX(this Vector3 v, float2 b) => new Vector3(v.x + b.y, v.y, v.z + b.x);

  public static Vector3 AddXY(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 AddYX(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 AddYZ(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 AddZY(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 AddXZ(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z + b);
  public static Vector3 AddZX(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z + b);

  public static Vector3 AddXY(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 AddYX(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z);
  public static Vector3 AddYZ(this Vector3 v, int b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 AddZY(this Vector3 v, int b) => new Vector3(v.x, v.y + b, v.z + b);
  public static Vector3 AddXZ(this Vector3 v, int b) => new Vector3(v.x + b, v.y, v.z + b);
  public static Vector3 AddZX(this Vector3 v, int b) => new Vector3(v.x + b, v.y, v.z + b);


  public static Vector3 AddXYZ(this Vector3 v, Vector3 b) => new Vector3(v.x + b.x, v.y + b.y, v.z + b.z);
  public static Vector3 AddXZY(this Vector3 v, Vector3 b) => new Vector3(v.x + b.x, v.y + b.z, v.z + b.y);
  public static Vector3 AddYZX(this Vector3 v, Vector3 b) => new Vector3(v.x + b.y, v.y + b.z, v.z + b.x);
  public static Vector3 AddYXZ(this Vector3 v, Vector3 b) => new Vector3(v.x + b.y, v.y + b.x, v.z + b.z);
  public static Vector3 AddZXY(this Vector3 v, Vector3 b) => new Vector3(v.x + b.z, v.y + b.x, v.z + b.y);
  public static Vector3 AddZYX(this Vector3 v, Vector3 b) => new Vector3(v.x + b.z, v.y + b.y, v.z + b.x);

  public static Vector3 AddXYZ(this Vector3 v, float3 b) => new Vector3(v.x + b.x, v.y + b.y, v.z + b.z);
  public static Vector3 AddXZY(this Vector3 v, float3 b) => new Vector3(v.x + b.x, v.y + b.z, v.z + b.y);
  public static Vector3 AddYZX(this Vector3 v, float3 b) => new Vector3(v.x + b.y, v.y + b.z, v.z + b.x);
  public static Vector3 AddYXZ(this Vector3 v, float3 b) => new Vector3(v.x + b.y, v.y + b.x, v.z + b.z);
  public static Vector3 AddZXY(this Vector3 v, float3 b) => new Vector3(v.x + b.z, v.y + b.x, v.z + b.y);
  public static Vector3 AddZYX(this Vector3 v, float3 b) => new Vector3(v.x + b.z, v.y + b.y, v.z + b.x);

  public static Vector3 AddXYZ(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddXZY(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddYZX(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddYXZ(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddZXY(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddZYX(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);

  public static Vector3 AddXYZ(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddXZY(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddYZX(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddYXZ(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddZXY(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);
  public static Vector3 AddZYX(this Vector3 v, int b) => new Vector3(v.x + b, v.y + b, v.z + b);



  public static Vector3 SetX(this Vector3 v, int b) => new Vector3(b, v.y, v.z);
  public static Vector3 SetY(this Vector3 v, int b) => new Vector3(v.x, b, v.z);
  public static Vector3 SetZ(this Vector3 v, int b) => new Vector3(v.x, v.y, b);
  public static Vector3 SetX(this Vector3 v, float b) => new Vector3(b, v.y, v.z);
  public static Vector3 SetY(this Vector3 v, float b) => new Vector3(v.x, b, v.z);
  public static Vector3 SetZ(this Vector3 v, float b) => new Vector3(v.x, v.y, b);


  public static Vector3 SetXY(this Vector3 v, Vector2 b) => new Vector3(b.x, b.y, v.z);
  public static Vector3 SetYX(this Vector3 v, Vector2 b) => new Vector3(b.y, b.x, v.z);
  public static Vector3 SetYZ(this Vector3 v, Vector2 b) => new Vector3(v.x, b.x, b.y);
  public static Vector3 SetZY(this Vector3 v, Vector2 b) => new Vector3(v.x, b.y, b.x);
  public static Vector3 SetXZ(this Vector3 v, Vector2 b) => new Vector3(b.x, v.y, b.y);
  public static Vector3 SetZX(this Vector3 v, Vector2 b) => new Vector3(b.y, v.y, b.x);

  public static Vector3 SetXY(this Vector3 v, float2 b) => new Vector3(b.x, b.y, v.z);
  public static Vector3 SetYX(this Vector3 v, float2 b) => new Vector3(b.y, b.x, v.z);
  public static Vector3 SetYZ(this Vector3 v, float2 b) => new Vector3(v.x, b.x, b.y);
  public static Vector3 SetZY(this Vector3 v, float2 b) => new Vector3(v.x, b.y, b.x);
  public static Vector3 SetXZ(this Vector3 v, float2 b) => new Vector3(b.x, v.y, b.y);
  public static Vector3 SetZX(this Vector3 v, float2 b) => new Vector3(b.y, v.y, b.x);

  public static Vector3 SetXY(this Vector3 v, float b) => new Vector3(b, b, v.z);
  public static Vector3 SetYX(this Vector3 v, float b) => new Vector3(b, b, v.z);
  public static Vector3 SetYZ(this Vector3 v, float b) => new Vector3(v.x, b, b);
  public static Vector3 SetZY(this Vector3 v, float b) => new Vector3(v.x, b, b);
  public static Vector3 SetXZ(this Vector3 v, float b) => new Vector3(b, v.y, b);
  public static Vector3 SetZX(this Vector3 v, float b) => new Vector3(b, v.y, b);

  public static Vector3 SetXY(this Vector3 v, int b) => new Vector3(b, b, v.z);
  public static Vector3 SetYX(this Vector3 v, int b) => new Vector3(b, b, v.z);
  public static Vector3 SetYZ(this Vector3 v, int b) => new Vector3(v.x, b, b);
  public static Vector3 SetZY(this Vector3 v, int b) => new Vector3(v.x, b, b);
  public static Vector3 SetXZ(this Vector3 v, int b) => new Vector3(b, v.y, b);
  public static Vector3 SetZX(this Vector3 v, int b) => new Vector3(b, v.y, b);

  public static Vector3 SetXY(this Vector3 v, int b, int c) => new Vector3(b, c, v.z);
  public static Vector3 SetYX(this Vector3 v, int b, int c) => new Vector3(b, c, v.z);
  public static Vector3 SetYZ(this Vector3 v, int b, int c) => new Vector3(v.x, b, c);
  public static Vector3 SetZY(this Vector3 v, int b, int c) => new Vector3(v.x, b, c);
  public static Vector3 SetXZ(this Vector3 v, int b, int c) => new Vector3(b, v.y, c);
  public static Vector3 SetZX(this Vector3 v, int b, int c) => new Vector3(b, v.y, c);


  public static Vector3 SetXYZ(this Vector3 v, Vector3 b) => new Vector3(b.x, b.y, b.z);
  public static Vector3 SetXZY(this Vector3 v, Vector3 b) => new Vector3(b.x, b.z, b.y);
  public static Vector3 SetYZX(this Vector3 v, Vector3 b) => new Vector3(b.y, b.z, b.x);
  public static Vector3 SetYXZ(this Vector3 v, Vector3 b) => new Vector3(b.y, b.x, b.z);
  public static Vector3 SetZXY(this Vector3 v, Vector3 b) => new Vector3(b.z, b.x, b.y);
  public static Vector3 SetZYX(this Vector3 v, Vector3 b) => new Vector3(b.z, b.y, b.x);

  public static Vector3 SetXYZ(this Vector3 v, float3 b) => new Vector3(b.x, b.y, b.z);
  public static Vector3 SetXZY(this Vector3 v, float3 b) => new Vector3(b.x, b.z, b.y);
  public static Vector3 SetYZX(this Vector3 v, float3 b) => new Vector3(b.y, b.z, b.x);
  public static Vector3 SetYXZ(this Vector3 v, float3 b) => new Vector3(b.y, b.x, b.z);
  public static Vector3 SetZXY(this Vector3 v, float3 b) => new Vector3(b.z, b.x, b.y);
  public static Vector3 SetZYX(this Vector3 v, float3 b) => new Vector3(b.z, b.y, b.x);

  public static Vector3 SetXYZ(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 SetXZY(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 SetYZX(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 SetYXZ(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 SetZXY(this Vector3 v, float b) => new Vector3(b, b, b);
  public static Vector3 SetZYX(this Vector3 v, float b) => new Vector3(b, b, b);

  public static Vector3 SetXYZ(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 SetXZY(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 SetYZX(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 SetYXZ(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 SetZXY(this Vector3 v, int b) => new Vector3(b, b, b);
  public static Vector3 SetZYX(this Vector3 v, int b) => new Vector3(b, b, b);

  public static Vector3 SetXYZ(this Vector3 v, float b, float c, float d) => new Vector3(b, c, d);
  public static Vector3 SetXZY(this Vector3 v, float b, float c, float d) => new Vector3(b, c, d);
  public static Vector3 SetYZX(this Vector3 v, float b, float c, float d) => new Vector3(b, c, d);
  public static Vector3 SetYXZ(this Vector3 v, float b, float c, float d) => new Vector3(b, c, d);
  public static Vector3 SetZXY(this Vector3 v, float b, float c, float d) => new Vector3(b, c, d);
  public static Vector3 SetZYX(this Vector3 v, float b, float c, float d) => new Vector3(b, c, d);

  public static Vector3 SetXYZ(this Vector3 v, int b, int c, int d) => new Vector3(b, c, d);
  public static Vector3 SetXZY(this Vector3 v, int b, int c, int d) => new Vector3(b, c, d);
  public static Vector3 SetYZX(this Vector3 v, int b, int c, int d) => new Vector3(b, c, d);
  public static Vector3 SetYXZ(this Vector3 v, int b, int c, int d) => new Vector3(b, c, d);
  public static Vector3 SetZXY(this Vector3 v, int b, int c, int d) => new Vector3(b, c, d);
  public static Vector3 SetZYX(this Vector3 v, int b, int c, int d) => new Vector3(b, c, d);
}