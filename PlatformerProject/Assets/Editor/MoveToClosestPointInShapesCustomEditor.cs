using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveToClosestPointInShapes))]
public class MoveToClosestPointInShapesCustomEditor : Editor {

  private bool mouse = false;
  private bool shift = false;
  private bool control { get => controlRight || controlLeft; }
  private bool controlRight = false;
  private bool controlLeft = false;
  private bool alt { get => altRight || altLeft; }
  private bool altRight = false;
  private bool altLeft = false;
  private bool moving = false;
  private bool blockCreate = false;
  private List<int> moveWithCurrent = new List<int>();
  private bool creatingFirst;
  private bool blockSplitting;
  private bool splitting;
  private Vector3 splitPos;
  private int splitIndex;
  private Vector3 creatingFirstPos;
  private Vector3 newA;
  private Vector3 newB;
  private Vector3 oldMousePos;
  private Vector3 actualNewA;
  private Vector3 actualNewB;
  private bool forceStart = false;
  private Vector3 forceStartPos;
  private int currentUndo;

  private MoveToClosestPointInShapes t { get => ((MoveToClosestPointInShapes)target); }

  protected virtual void OnSceneGUI() {
    shift = Event.current.shift;

    switch (Event.current.type) {
      case EventType.Repaint:
        Handles.color = Handles.xAxisColor;
        if (t.lines.Count % 2 != 0) {
          var tail = t.lines[t.lines.Count - 1];
          t.lines.RemoveAt(t.lines.Count - 1);
          Handles.DrawLines(t.lines.ToArray());
          t.lines.Add(tail);
        } else {
          Handles.DrawLines(t.lines.ToArray());
        }
        if (shift && !moving) {
          if (splitting) {
            if (!blockSplitting) {
              Handles.color = Handles.yAxisColor;
              Handles.DrawSolidDisc(splitPos, Vector3.forward, 0.1f);
            }
          } else {
            if (t.snapDistance > 0) {
              var minDist = float.PositiveInfinity;
              var minPos = Vector3.zero;
              for (int j = 0; j < t.lines.Count; j++) {
                if (minDist > (newB - t.lines[j]).sqrMagnitude) {
                  minDist = (newB - t.lines[j]).sqrMagnitude;
                  minPos = t.lines[j];
                }
              }
              if (minDist <= t.snapDistance) newB = minPos;
            }

            Handles.color = Handles.yAxisColor;
            Handles.DrawLine(newA, newB);
            Handles.Button(newA, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap);
            actualNewA = newA;
            actualNewB = newB;
          }
        }
        foreach (var point in t.lines) {
          Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);
        }
        break;
      case EventType.MouseDown:
        if (Event.current.button == 0) {
          currentUndo = Undo.GetCurrentGroup();
          mouse = true;
        }
        break;
      case EventType.MouseUp:
        if (Event.current.button == 0) mouse = false;
        break;
      case EventType.KeyDown:
        if (Event.current.keyCode == KeyCode.RightAlt) altRight = true;
        else if (Event.current.keyCode == KeyCode.LeftAlt) altLeft = true;

        if (Event.current.keyCode == KeyCode.RightControl) controlRight = true;
        else if (Event.current.keyCode == KeyCode.LeftControl) controlLeft = true;
        break;
      case EventType.KeyUp:
        if (Event.current.keyCode == KeyCode.RightAlt) altRight = false;
        else if (Event.current.keyCode == KeyCode.LeftAlt) altLeft = false;

        if (Event.current.keyCode == KeyCode.RightControl) controlRight = false;
        else if (Event.current.keyCode == KeyCode.LeftControl) controlLeft = false;
        break;
    }
    HandleAll();
  }

  void HandleAll() {
    newA = forceStart ? forceStartPos.xy() : Vector2.zero;
    oldMousePos = newB;
    newB = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.xyo();
    if (!shift || (t.lines.Count > 0 && shift && !control)) creatingFirst = false;
    if (!mouse) {
      blockSplitting = false;
      splitting = false;
      if (moving) {
        Clean();
        Dirty();
        Undo.RegisterCompleteObjectUndo(t, "Modify line");
      }
      moving = false;
    } else if (blockSplitting) { // Allow moving split position
      t.lines[splitIndex] += newB - oldMousePos;
      t.lines[splitIndex + 1] += newB - oldMousePos;
    }
    if (shift && (t.lines.Count == 0 || control)) {
      if (!creatingFirst) {
        creatingFirst = true;
        creatingFirstPos = newB;

      }
      newA = creatingFirstPos;
      Handles.Button(newB, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap);
      if (mouse && !blockCreate) {
        Undo.RecordObject(t, "Create new line");
        blockCreate = true;
        t.lines.Add(actualNewA);
        t.lines.Add(actualNewB);
        forceStart = true;
        forceStartPos = newB;
        Clean();
        Dirty();
      }
    } else {
      if (!blockSplitting && !moving && shift && !forceStart && t.snapDistance > 0) {
        var minVector = Vector2.zero;
        var minVectorLength = float.PositiveInfinity;

        for (int i = 1; i < t.lines.Count; i += 2) {
          var line = (start: t.lines[i - 1], end: t.lines[i]);
          var dir = line.start - line.end;
          var res = MoveToClosestPointInShapes.ClosestPointOnLine(line.start, line.end, newB);
          if (minVectorLength > (res - newB.xy()).sqrMagnitude) {
            minVector = res - newB.xy();
            minVectorLength = minVector.sqrMagnitude;
            splitIndex = i;
          }
        }
        if (minVectorLength <= t.snapDistance) {
          splitting = true;
          splitPos = newB.AddXY(minVector);
          Handles.Button(newB, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap); // Prevent deselect
          if (mouse) {
            Undo.RecordObject(t, "Splitted line");
            blockSplitting = true;
            var splitStart = t.lines[splitIndex - 1];
            var splitEnd = t.lines[splitIndex];
            t.lines[splitIndex - 1] = splitStart;
            t.lines[splitIndex] = splitPos;
            t.lines.InsertRange(splitIndex + 1, new Vector3[] { splitPos, splitEnd });
            Dirty();
          }
          return;
        }
      }
    }
    if (blockSplitting) return;
    splitting = false;

    var minDistance = float.PositiveInfinity;
    for (int i = 1; i < t.lines.Count; i += 2) {
      var line = (start: t.lines[i - 1], end: t.lines[i]);

      EditorGUI.BeginChangeCheck();
      if (control) {
        if (!shift) {
          var center = line.start + (line.end - line.start) / 2;
          if (Handles.Button(center, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap)) {
            Undo.RecordObject(t, "Delete line");
            t.lines.RemoveRange(i - 1, 2);
            Dirty();
          }
        }
      } else {
        if (shift && !moving) {
          Handles.Button(newB, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap); // Prevent deselect
          if (!forceStart) {
            if (minDistance > (line.start - newB).xy().sqrMagnitude) {
              newA = line.start;
              minDistance = (line.start - newB).xy().sqrMagnitude;
            }
            if (minDistance > (line.end - newB).xy().sqrMagnitude) {
              newA = line.end;
              minDistance = (line.end - newB).xy().sqrMagnitude;
            }
          }
        } else {
          forceStart = false;
          Vector3 newStart = Handles.FreeMoveHandle(line.start, Quaternion.identity, 0.5f, Vector3.zero, Handles.RectangleHandleCap);
          Vector3 newEnd = Handles.FreeMoveHandle(line.end, Quaternion.identity, 0.5f, Vector3.zero, Handles.RectangleHandleCap);
          if (EditorGUI.EndChangeCheck()) {
            bool startMoved = newStart != line.start;
            Undo.RecordObject(t, "Modify line");
            if (!moving) {
              moveWithCurrent = FindOverlapping(newStart == line.start ? line.end : line.start, i, i - 1);
            }
            moving = true;

            var minDist = float.PositiveInfinity;
            var minPos = Vector3.zero;
            if (t.snapDistance > 0) {
              if (t.moveOverlappingVertices) {
                for (int j = 0; j < t.lines.Count; j++) {
                  if (j == i || j == i - 1 || moveWithCurrent.Contains(j)) continue;
                  if (startMoved) {
                    if (minDist > (newStart - t.lines[j]).sqrMagnitude) {
                      minDist = (newStart - t.lines[j]).sqrMagnitude;
                      minPos = t.lines[j];
                    }
                  } else {
                    if (minDist > (newEnd - t.lines[j]).sqrMagnitude) {
                      minDist = (newEnd - t.lines[j]).sqrMagnitude;
                      minPos = t.lines[j];
                    }
                  }
                }
              }
              if (minDist <= t.snapDistance) {
                if (startMoved) newStart = minPos;
                else newEnd = minPos;
              }
            }

            if (t.moveOverlappingVertices) {
              if (minDist <= t.snapDistance)
                foreach (var index in moveWithCurrent)
                  t.lines[index] = minPos;

              foreach (var index in moveWithCurrent)
                t.lines[index] = startMoved ? newStart : newEnd;

            }
            t.lines[i - 1] = newStart;
            t.lines[i] = newEnd;
          }
          continue;
        }
        if (mouse && shift) {
          moving = false;
          if (!blockCreate) {
            Undo.RecordObject(t, "Create new line");
            blockCreate = true;
            t.lines.Add(actualNewA);
            t.lines.Add(actualNewB);
            forceStart = true;
            forceStartPos = actualNewB;
            Dirty();
          }
        } else {
          blockCreate = false;
        }
      }
    }
  }

  void Clean() {
    if (t.removeIdentical) {
      var hashList = new HashSet<(Vector3, Vector3)>();
      for (int i = 1; i < t.lines.Count; i += 2) {
        var line = (t.lines[i - 1], t.lines[i]);
        if (hashList.Contains(line)) {
          t.lines.RemoveRange(i - 1, 2);
          i -= 2;
          Dirty();
        } else
          hashList.Add(line);
      }
    }
    if (t.removeZeroLength) {
      for (int i = 1; i < t.lines.Count; i += 2) {
        if (t.lines[i - 1] == t.lines[i]) {
          t.lines.RemoveRange(i - 1, 2);
          i -= 2;
          Dirty();
        }
      }
    }
  }

  bool FindNearest(Vector3 pos, out Vector3 nearest, params int[] ignoreIndexes) {
    bool found = false;

    var minDist = float.PositiveInfinity;
    var minPos = Vector3.zero;
    for (int i = 0; i < t.lines.Count; i++) {
      if (!ignoreIndexes.Includes(i)) continue;
      var dist = (pos - t.lines[i]).sqrMagnitude;
      if (minDist > dist) {
        found = true;
        minDist = dist;
        minPos = t.lines[i];
      }
    }
    nearest = minPos;
    return found;
  }

  List<int> FindOverlapping(Vector3 pos, params int[] ignoreIndexes) {
    var res = new List<int>();
    for (int i = 0; i < t.lines.Count; i++) {
      if (t.lines[i] == pos && !ignoreIndexes.Includes(i)) res.Add(i);
    }
    return res;
  }

  void Dirty() {
    if (!Application.isPlaying) {
      EditorUtility.SetDirty(t);
      UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
  }
}

public static class Ext {
    /// <summary> Returns true if `array` has `value` </summary>
    public static bool Includes<T>(this T[] array, T value) {
        int length = array.Length;
        for (int i = 0; i < length; i++)
            if (array[i].Equals(value)) return true;
        return false;
    }
}
