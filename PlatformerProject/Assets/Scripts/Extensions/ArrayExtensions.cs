using System.Collections;
using System.Collections.Generic;

static class ArrayExtensions {
  /// <summary> Returns true if `array` has `value` </summary>
  public static bool Includes<T>(this T[] array, T value) {
    int length = array.Length;
    for (int i = 0; i < length; i++)
      if (array[i].Equals(value)) return true;
    return false;
  }
  /// <summary> Returns true if `array` has `value` and passes the found position with `index` which is -1 when not found </summary>
  public static bool Includes<T>(this T[] array, T value, out int index) {
    index = -1;
    int length = array.Length;
    for (int i = 0; i < length; i++)
      if (array[i].Equals(value)) {
        index = i;
        return true;
      }
    return false;
  }

  /// <summary> Reverses the array or part of it in place </summary>
  public static void Reverse<T>(this T[] array, int index, int length) => System.Array.Reverse(array, index, length);
  public static void Reverse<T>(this T[] array) => System.Array.Reverse(array);

}
