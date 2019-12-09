using System.Collections;
using System.Collections.Generic;

static class ArrayExtensions {
  /// <summary> Returns true if the array  </summary>
  public static bool Includes<T>(this T[] array, T value) {
    int length = array.Length;
    for (int i = 0; i < length; i++)
      if (array[i].Equals(value)) return true;
    return false;
  }

  /// <summary> Reverses the array or part of it in place </summary>
  public static void Reverse<T>(this T[] array, int index, int length) => System.Array.Reverse(array, index, length);
  public static void Reverse<T>(this T[] array) => System.Array.Reverse(array);

}
