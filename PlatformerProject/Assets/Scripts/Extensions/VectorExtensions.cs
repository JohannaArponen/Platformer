using UnityEngine;
using Unity.Mathematics;

public static class VectorExtensions {

  // *********************** Vector3 *********************** //

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


  // *********************** float3 *********************** //

  public static float2 Fxy(this Vector3 v) => new float2(v.x, v.y);
  public static float2 Fxz(this Vector3 v) => new float2(v.x, v.z);
  public static float2 Fyz(this Vector3 v) => new float2(v.y, v.z);
  public static float2 Fyx(this Vector3 v) => new float2(v.y, v.x);
  public static float2 Fzx(this Vector3 v) => new float2(v.z, v.x);
  public static float2 Fzy(this Vector3 v) => new float2(v.z, v.y);

  public static float3 Fxyz(this Vector3 v) => new float3(v.x, v.y, v.z);
  public static float3 Fxzy(this Vector3 v) => new float3(v.x, v.z, v.y);
  public static float3 Fyzx(this Vector3 v) => new float3(v.y, v.z, v.x);
  public static float3 Fyxz(this Vector3 v) => new float3(v.y, v.x, v.z);
  public static float3 Fzxy(this Vector3 v) => new float3(v.z, v.x, v.y);
  public static float3 Fzyx(this Vector3 v) => new float3(v.z, v.y, v.x);



  public static float2 Fxo(this Vector3 v) => new float2(v.x, 0);
  public static float2 Fox(this Vector3 v) => new float2(0, v.x);
  public static float2 Foy(this Vector3 v) => new float2(0, v.y);
  public static float2 Fyo(this Vector3 v) => new float2(v.y, 0);
  public static float2 Fzo(this Vector3 v) => new float2(v.z, 0);
  public static float2 Foz(this Vector3 v) => new float2(0, v.z);
  public static float2 Foo(this Vector3 v) => new float2(0, 0);



  public static float3 Foxx(this Vector3 v) => new float3(0, v.x, v.x);
  public static float3 Fxox(this Vector3 v) => new float3(v.x, 0, v.x);
  public static float3 Fxxo(this Vector3 v) => new float3(v.x, v.x, 0);
  public static float3 Foxo(this Vector3 v) => new float3(0, v.x, 0);
  public static float3 Foox(this Vector3 v) => new float3(0, 0, v.x);
  public static float3 Fxoo(this Vector3 v) => new float3(v.x, 0, 0);
  public static float3 Fooo(this Vector3 v) => new float3(0, 0, 0);

  public static float3 Foyy(this Vector3 v) => new float3(0, v.y, v.y);
  public static float3 Fyoy(this Vector3 v) => new float3(v.y, 0, v.y);
  public static float3 Fyyo(this Vector3 v) => new float3(v.y, v.y, 0);
  public static float3 Foyo(this Vector3 v) => new float3(0, v.y, 0);
  public static float3 Fooy(this Vector3 v) => new float3(0, 0, v.y);
  public static float3 Fyoo(this Vector3 v) => new float3(v.y, 0, 0);

  public static float3 Fozz(this Vector3 v) => new float3(0, v.z, v.z);
  public static float3 Fzoz(this Vector3 v) => new float3(v.z, 0, v.z);
  public static float3 Fzzo(this Vector3 v) => new float3(v.z, v.z, 0);
  public static float3 Fozo(this Vector3 v) => new float3(0, v.z, 0);
  public static float3 Fooz(this Vector3 v) => new float3(0, 0, v.z);
  public static float3 Fzoo(this Vector3 v) => new float3(v.z, 0, 0);


  public static float3 Fxyo(this Vector3 v) => new float3(v.x, v.y, 0);
  public static float3 Foxy(this Vector3 v) => new float3(0, v.x, v.y);
  public static float3 Fyox(this Vector3 v) => new float3(v.y, 0, v.x);
  public static float3 Foyx(this Vector3 v) => new float3(0, v.y, v.x);
  public static float3 Fxoy(this Vector3 v) => new float3(v.x, 0, v.y);
  public static float3 Fyxo(this Vector3 v) => new float3(v.y, v.x, 0);

  public static float3 Fxzo(this Vector3 v) => new float3(v.x, v.z, 0);
  public static float3 Foxz(this Vector3 v) => new float3(0, v.x, v.z);
  public static float3 Fzox(this Vector3 v) => new float3(v.z, 0, v.x);
  public static float3 Fozx(this Vector3 v) => new float3(0, v.z, v.x);
  public static float3 Fxoz(this Vector3 v) => new float3(v.x, 0, v.z);
  public static float3 Fzxo(this Vector3 v) => new float3(v.y, v.x, 0);

  public static float3 Fzyo(this Vector3 v) => new float3(v.z, v.y, 0);
  public static float3 Fozy(this Vector3 v) => new float3(0, v.z, v.y);
  public static float3 Fyoz(this Vector3 v) => new float3(v.y, 0, v.z);
  public static float3 Foyz(this Vector3 v) => new float3(0, v.y, v.z);
  public static float3 Fzoy(this Vector3 v) => new float3(v.z, 0, v.y);
  public static float3 Fyzo(this Vector3 v) => new float3(v.y, v.z, 0);



  public static float3 FAdd(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAdd(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z + b);


  public static float3 FAddX(this Vector3 v, int b) => new float3(v.x + b, v.y, v.z);
  public static float3 FAddY(this Vector3 v, int b) => new float3(v.x, v.y + b, v.z);
  public static float3 FAddZ(this Vector3 v, int b) => new float3(v.x, v.y, v.z + b);
  public static float3 FAddX(this Vector3 v, float b) => new float3(v.x + b, v.y, v.z);
  public static float3 FAddY(this Vector3 v, float b) => new float3(v.x, v.y + b, v.z);
  public static float3 FAddZ(this Vector3 v, float b) => new float3(v.x, v.y, v.z + b);


  public static float3 FAddXY(this Vector3 v, Vector2 b) => new float3(v.x + b.x, v.y + b.y, v.z);
  public static float3 FAddYX(this Vector3 v, Vector2 b) => new float3(v.x + b.y, v.y + b.x, v.z);
  public static float3 FAddYZ(this Vector3 v, Vector2 b) => new float3(v.x, v.y + b.x, v.z + b.y);
  public static float3 FAddZY(this Vector3 v, Vector2 b) => new float3(v.x, v.y + b.y, v.z + b.x);
  public static float3 FAddXZ(this Vector3 v, Vector2 b) => new float3(v.x + b.x, v.y, v.z + b.y);
  public static float3 FAddZX(this Vector3 v, Vector2 b) => new float3(v.x + b.y, v.y, v.z + b.x);

  public static float3 FAddXY(this Vector3 v, float2 b) => new float3(v.x + b.x, v.y + b.y, v.z);
  public static float3 FAddYX(this Vector3 v, float2 b) => new float3(v.x + b.y, v.y + b.x, v.z);
  public static float3 FAddYZ(this Vector3 v, float2 b) => new float3(v.x, v.y + b.x, v.z + b.y);
  public static float3 FAddZY(this Vector3 v, float2 b) => new float3(v.x, v.y + b.y, v.z + b.x);
  public static float3 FAddXZ(this Vector3 v, float2 b) => new float3(v.x + b.x, v.y, v.z + b.y);
  public static float3 FAddZX(this Vector3 v, float2 b) => new float3(v.x + b.y, v.y, v.z + b.x);

  public static float3 FAddXY(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z);
  public static float3 FAddYX(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z);
  public static float3 FAddYZ(this Vector3 v, float b) => new float3(v.x, v.y + b, v.z + b);
  public static float3 FAddZY(this Vector3 v, float b) => new float3(v.x, v.y + b, v.z + b);
  public static float3 FAddXZ(this Vector3 v, float b) => new float3(v.x + b, v.y, v.z + b);
  public static float3 FAddZX(this Vector3 v, float b) => new float3(v.x + b, v.y, v.z + b);

  public static float3 FAddXY(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z);
  public static float3 FAddYX(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z);
  public static float3 FAddYZ(this Vector3 v, int b) => new float3(v.x, v.y + b, v.z + b);
  public static float3 FAddZY(this Vector3 v, int b) => new float3(v.x, v.y + b, v.z + b);
  public static float3 FAddXZ(this Vector3 v, int b) => new float3(v.x + b, v.y, v.z + b);
  public static float3 FAddZX(this Vector3 v, int b) => new float3(v.x + b, v.y, v.z + b);


  public static float3 FAddXYZ(this Vector3 v, Vector3 b) => new float3(v.x + b.x, v.y + b.y, v.z + b.z);
  public static float3 FAddXZY(this Vector3 v, Vector3 b) => new float3(v.x + b.x, v.y + b.z, v.z + b.y);
  public static float3 FAddYZX(this Vector3 v, Vector3 b) => new float3(v.x + b.y, v.y + b.z, v.z + b.x);
  public static float3 FAddYXZ(this Vector3 v, Vector3 b) => new float3(v.x + b.y, v.y + b.x, v.z + b.z);
  public static float3 FAddZXY(this Vector3 v, Vector3 b) => new float3(v.x + b.z, v.y + b.x, v.z + b.y);
  public static float3 FAddZYX(this Vector3 v, Vector3 b) => new float3(v.x + b.z, v.y + b.y, v.z + b.x);

  public static float3 FAddXYZ(this Vector3 v, float3 b) => new float3(v.x + b.x, v.y + b.y, v.z + b.z);
  public static float3 FAddXZY(this Vector3 v, float3 b) => new float3(v.x + b.x, v.y + b.z, v.z + b.y);
  public static float3 FAddYZX(this Vector3 v, float3 b) => new float3(v.x + b.y, v.y + b.z, v.z + b.x);
  public static float3 FAddYXZ(this Vector3 v, float3 b) => new float3(v.x + b.y, v.y + b.x, v.z + b.z);
  public static float3 FAddZXY(this Vector3 v, float3 b) => new float3(v.x + b.z, v.y + b.x, v.z + b.y);
  public static float3 FAddZYX(this Vector3 v, float3 b) => new float3(v.x + b.z, v.y + b.y, v.z + b.x);

  public static float3 FAddXYZ(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddXZY(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddYZX(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddYXZ(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddZXY(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddZYX(this Vector3 v, float b) => new float3(v.x + b, v.y + b, v.z + b);

  public static float3 FAddXYZ(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddXZY(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddYZX(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddYXZ(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddZXY(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z + b);
  public static float3 FAddZYX(this Vector3 v, int b) => new float3(v.x + b, v.y + b, v.z + b);



  public static float3 FSetX(this Vector3 v, int b) => new float3(b, v.y, v.z);
  public static float3 FSetY(this Vector3 v, int b) => new float3(v.x, b, v.z);
  public static float3 FSetZ(this Vector3 v, int b) => new float3(v.x, v.y, b);
  public static float3 FSetX(this Vector3 v, float b) => new float3(b, v.y, v.z);
  public static float3 FSetY(this Vector3 v, float b) => new float3(v.x, b, v.z);
  public static float3 FSetZ(this Vector3 v, float b) => new float3(v.x, v.y, b);


  public static float3 FSetXY(this Vector3 v, Vector2 b) => new float3(b.x, b.y, v.z);
  public static float3 FSetYX(this Vector3 v, Vector2 b) => new float3(b.y, b.x, v.z);
  public static float3 FSetYZ(this Vector3 v, Vector2 b) => new float3(v.x, b.x, b.y);
  public static float3 FSetZY(this Vector3 v, Vector2 b) => new float3(v.x, b.y, b.x);
  public static float3 FSetXZ(this Vector3 v, Vector2 b) => new float3(b.x, v.y, b.y);
  public static float3 FSetZX(this Vector3 v, Vector2 b) => new float3(b.y, v.y, b.x);

  public static float3 FSetXY(this Vector3 v, float2 b) => new float3(b.x, b.y, v.z);
  public static float3 FSetYX(this Vector3 v, float2 b) => new float3(b.y, b.x, v.z);
  public static float3 FSetYZ(this Vector3 v, float2 b) => new float3(v.x, b.x, b.y);
  public static float3 FSetZY(this Vector3 v, float2 b) => new float3(v.x, b.y, b.x);
  public static float3 FSetXZ(this Vector3 v, float2 b) => new float3(b.x, v.y, b.y);
  public static float3 FSetZX(this Vector3 v, float2 b) => new float3(b.y, v.y, b.x);

  public static float3 FSetXY(this Vector3 v, float b) => new float3(b, b, v.z);
  public static float3 FSetYX(this Vector3 v, float b) => new float3(b, b, v.z);
  public static float3 FSetYZ(this Vector3 v, float b) => new float3(v.x, b, b);
  public static float3 FSetZY(this Vector3 v, float b) => new float3(v.x, b, b);
  public static float3 FSetXZ(this Vector3 v, float b) => new float3(b, v.y, b);
  public static float3 FSetZX(this Vector3 v, float b) => new float3(b, v.y, b);

  public static float3 FSetXY(this Vector3 v, int b) => new float3(b, b, v.z);
  public static float3 FSetYX(this Vector3 v, int b) => new float3(b, b, v.z);
  public static float3 FSetYZ(this Vector3 v, int b) => new float3(v.x, b, b);
  public static float3 FSetZY(this Vector3 v, int b) => new float3(v.x, b, b);
  public static float3 FSetXZ(this Vector3 v, int b) => new float3(b, v.y, b);
  public static float3 FSetZX(this Vector3 v, int b) => new float3(b, v.y, b);

  public static float3 FSetXY(this Vector3 v, int b, int c) => new float3(b, c, v.z);
  public static float3 FSetYX(this Vector3 v, int b, int c) => new float3(b, c, v.z);
  public static float3 FSetYZ(this Vector3 v, int b, int c) => new float3(v.x, b, c);
  public static float3 FSetZY(this Vector3 v, int b, int c) => new float3(v.x, b, c);
  public static float3 FSetXZ(this Vector3 v, int b, int c) => new float3(b, v.y, c);
  public static float3 FSetZX(this Vector3 v, int b, int c) => new float3(b, v.y, c);


  public static float3 FSetXYZ(this Vector3 v, Vector3 b) => new float3(b.x, b.y, b.z);
  public static float3 FSetXZY(this Vector3 v, Vector3 b) => new float3(b.x, b.z, b.y);
  public static float3 FSetYZX(this Vector3 v, Vector3 b) => new float3(b.y, b.z, b.x);
  public static float3 FSetYXZ(this Vector3 v, Vector3 b) => new float3(b.y, b.x, b.z);
  public static float3 FSetZXY(this Vector3 v, Vector3 b) => new float3(b.z, b.x, b.y);
  public static float3 FSetZYX(this Vector3 v, Vector3 b) => new float3(b.z, b.y, b.x);

  public static float3 FSetXYZ(this Vector3 v, float3 b) => new float3(b.x, b.y, b.z);
  public static float3 FSetXZY(this Vector3 v, float3 b) => new float3(b.x, b.z, b.y);
  public static float3 FSetYZX(this Vector3 v, float3 b) => new float3(b.y, b.z, b.x);
  public static float3 FSetYXZ(this Vector3 v, float3 b) => new float3(b.y, b.x, b.z);
  public static float3 FSetZXY(this Vector3 v, float3 b) => new float3(b.z, b.x, b.y);
  public static float3 FSetZYX(this Vector3 v, float3 b) => new float3(b.z, b.y, b.x);

  public static float3 FSetXYZ(this Vector3 v, float b) => new float3(b, b, b);
  public static float3 FSetXZY(this Vector3 v, float b) => new float3(b, b, b);
  public static float3 FSetYZX(this Vector3 v, float b) => new float3(b, b, b);
  public static float3 FSetYXZ(this Vector3 v, float b) => new float3(b, b, b);
  public static float3 FSetZXY(this Vector3 v, float b) => new float3(b, b, b);
  public static float3 FSetZYX(this Vector3 v, float b) => new float3(b, b, b);

  public static float3 FSetXYZ(this Vector3 v, int b) => new float3(b, b, b);
  public static float3 FSetXZY(this Vector3 v, int b) => new float3(b, b, b);
  public static float3 FSetYZX(this Vector3 v, int b) => new float3(b, b, b);
  public static float3 FSetYXZ(this Vector3 v, int b) => new float3(b, b, b);
  public static float3 FSetZXY(this Vector3 v, int b) => new float3(b, b, b);
  public static float3 FSetZYX(this Vector3 v, int b) => new float3(b, b, b);

  public static float3 FSetXYZ(this Vector3 v, float b, float c, float d) => new float3(b, c, d);
  public static float3 FSetXZY(this Vector3 v, float b, float c, float d) => new float3(b, c, d);
  public static float3 FSetYZX(this Vector3 v, float b, float c, float d) => new float3(b, c, d);
  public static float3 FSetYXZ(this Vector3 v, float b, float c, float d) => new float3(b, c, d);
  public static float3 FSetZXY(this Vector3 v, float b, float c, float d) => new float3(b, c, d);
  public static float3 FSetZYX(this Vector3 v, float b, float c, float d) => new float3(b, c, d);

  public static float3 FSetXYZ(this Vector3 v, int b, int c, int d) => new float3(b, c, d);
  public static float3 FSetXZY(this Vector3 v, int b, int c, int d) => new float3(b, c, d);
  public static float3 FSetYZX(this Vector3 v, int b, int c, int d) => new float3(b, c, d);
  public static float3 FSetYXZ(this Vector3 v, int b, int c, int d) => new float3(b, c, d);
  public static float3 FSetZXY(this Vector3 v, int b, int c, int d) => new float3(b, c, d);
  public static float3 FSetZYX(this Vector3 v, int b, int c, int d) => new float3(b, c, d);


  // *********************** Vector2 *********************** //

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



  // *********************** flaot2 *********************** //

  public static float2 Fxy(this Vector2 v) => new float2(v.x, v.y);
  public static float2 Fyx(this Vector2 v) => new float2(v.y, v.x);

  public static float3 Fxxx(this Vector2 v) => new float3(v.x, v.x, v.x);
  public static float3 Fyyy(this Vector2 v) => new float3(v.y, v.y, v.y);

  public static float3 Fxxy(this Vector2 v) => new float3(v.x, v.x, v.y);
  public static float3 Fxyy(this Vector2 v) => new float3(v.x, v.y, v.y);
  public static float3 Fyyx(this Vector2 v) => new float3(v.y, v.y, v.x);
  public static float3 Fxyx(this Vector2 v) => new float3(v.x, v.y, v.x);
  public static float3 Fyxx(this Vector2 v) => new float3(v.y, v.x, v.x);



  public static float2 Fxo(this Vector2 v) => new float2(v.x, 0);
  public static float2 Fox(this Vector2 v) => new float2(0, v.x);
  public static float2 Foy(this Vector2 v) => new float2(0, v.y);
  public static float2 Fyo(this Vector2 v) => new float2(v.y, 0);
  public static float2 Foo(this Vector2 v) => new float2(0, 0);


  public static float3 Foxx(this Vector2 v) => new float3(0, v.x, v.x);
  public static float3 Fxox(this Vector2 v) => new float3(v.x, 0, v.x);
  public static float3 Fxxo(this Vector2 v) => new float3(v.x, v.x, 0);
  public static float3 Foxo(this Vector2 v) => new float3(0, v.x, 0);
  public static float3 Foox(this Vector2 v) => new float3(0, 0, v.x);
  public static float3 Fxoo(this Vector2 v) => new float3(v.x, 0, 0);
  public static float3 Fooo(this Vector2 v) => new float3(0, 0, 0);

  public static float3 Foyy(this Vector2 v) => new float3(0, v.y, v.y);
  public static float3 Fyoy(this Vector2 v) => new float3(v.y, 0, v.y);
  public static float3 Fyyo(this Vector2 v) => new float3(v.y, v.y, 0);
  public static float3 Foyo(this Vector2 v) => new float3(0, v.y, 0);
  public static float3 Fooy(this Vector2 v) => new float3(0, 0, v.y);
  public static float3 Fyoo(this Vector2 v) => new float3(v.y, 0, 0);

  public static float3 Fxyo(this Vector2 v) => new float3(v.x, v.y, 0);
  public static float3 Foxy(this Vector2 v) => new float3(0, v.x, v.y);
  public static float3 Fyox(this Vector2 v) => new float3(v.y, 0, v.x);
  public static float3 Foyx(this Vector2 v) => new float3(0, v.y, v.x);
  public static float3 Fxoy(this Vector2 v) => new float3(v.x, 0, v.y);
  public static float3 Fyxo(this Vector2 v) => new float3(v.y, v.x, 0);



  public static float2 FAdd(this Vector2 v, int b) => new float2(v.x + b, v.y + b);
  public static float2 FAdd(this Vector2 v, float b) => new float2(v.x + b, v.y + b);

  public static float2 FAddX(this Vector2 v, int b) => new float2(v.x + b, v.y);
  public static float2 FAddY(this Vector2 v, int b) => new float2(v.x, v.y + b);

  public static float2 FAddX(this Vector2 v, float b) => new float2(v.x + b, v.y);
  public static float2 FAddY(this Vector2 v, float b) => new float2(v.x, v.y + b);


  public static float2 FAddXY(this Vector2 v, Vector2 b) => new float2(v.x + b.x, v.y + b.y);
  public static float2 FAddYX(this Vector2 v, Vector2 b) => new float2(v.x + b.y, v.y + b.x);

  public static float2 FAddXY(this Vector2 v, float2 b) => new float2(v.x + b.x, v.y + b.y);
  public static float2 FAddYX(this Vector2 v, float2 b) => new float2(v.x + b.y, v.y + b.x);


  public static float2 FAddXY(this Vector2 v, float b) => new float2(v.x + b, v.y + b);
  public static float2 FAddYX(this Vector2 v, float b) => new float2(v.x + b, v.y + b);

  public static float2 FAddXY(this Vector2 v, int b) => new float2(v.x + b, v.y + b);
  public static float2 FAddYX(this Vector2 v, int b) => new float2(v.x + b, v.y + b);



  public static float2 FSet(this Vector2 v, int b) => new float2(b, b);
  public static float2 FSet(this Vector2 v, float b) => new float2(b, b);

  public static float2 FSetX(this Vector2 v, int b) => new float2(b, v.y);
  public static float2 FSetY(this Vector2 v, int b) => new float2(v.x, b);

  public static float2 FSetX(this Vector2 v, float b) => new float2(b, v.y);
  public static float2 FSetY(this Vector2 v, float b) => new float2(v.x, b);


  public static float2 FSetXY(this Vector2 v, Vector2 b) => new float2(b.x, b.y);
  public static float2 FSetYX(this Vector2 v, Vector2 b) => new float2(b.y, b.x);

  public static float2 FSetXY(this Vector2 v, float2 b) => new float2(b.x, b.y);
  public static float2 FSetYX(this Vector2 v, float2 b) => new float2(b.y, b.x);


  public static float2 FSetXY(this Vector2 v, float b) => new float2(b, b);
  public static float2 FSetYX(this Vector2 v, float b) => new float2(b, b);

  public static float2 FSetXY(this Vector2 v, int b) => new float2(b, b);
  public static float2 FSetYX(this Vector2 v, int b) => new float2(b, b);


  public static float2 FSetXY(this Vector2 v, float b, float c) => new float2(b, c);
  public static float2 FSetYX(this Vector2 v, float b, float c) => new float2(b, c);

  public static float2 FSetXY(this Vector2 v, int b, int c) => new float2(b, c);
  public static float2 FSetYX(this Vector2 v, int b, int c) => new float2(b, c);
}