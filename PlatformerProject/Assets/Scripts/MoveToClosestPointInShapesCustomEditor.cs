
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
  private bool blockCreate = false;
  private bool blockDelete = false;
  private bool creatingFirst;
  private Vector3 creatingFirstPos;
  private Vector3 newA;
  private Vector3 newB;
  private Vector3 actualNewA;
  private Vector3 actualNewB;
  private bool forceStart = false;
  private Vector3 forceStartPos;

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
        if (shift) {
          Handles.color = Handles.yAxisColor;
          Handles.DrawLine(newA, newB);
          actualNewA = newA;
          actualNewB = newB;
        }
        foreach (var point in t.lines) {
          Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);
        }
        break;
      case EventType.MouseDown:
        if (Event.current.button == 0) mouse = true;
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
    newB = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.xyo();
    if (!shift || (t.lines.Count > 0 && shift && !control)) {
      creatingFirst = false;
    }
    if (shift && (t.lines.Count == 0 || control)) {
      if (!creatingFirst) {
        creatingFirst = true;
        creatingFirstPos = newB;
      }
      newA = creatingFirstPos;
      Handles.Button(newB, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap); // Prevent deselect
      if (mouse) {
        if (!blockCreate) {
          Undo.RecordObject(t, "Create new line");
          blockCreate = true;
          t.lines.Add(actualNewA);
          t.lines.Add(actualNewB);
          forceStart = true;
          forceStartPos = newB;
          if (!Application.isPlaying) {
            EditorUtility.SetDirty(t);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
          };
        }
      }
    }
    var minDistance = float.PositiveInfinity;
    for (int i = 1; i < t.lines.Count; i += 2) {
      var start = t.lines[i - 1];
      var end = t.lines[i];

      EditorGUI.BeginChangeCheck();
      if (control) {
        var center = start + (end - start) / 2;
        if (Handles.Button(center, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap)) {
          t.lines.RemoveRange(i - 1, 2);
          if (!Application.isPlaying) {
            Undo.RecordObject(t, "Delete line");
            EditorUtility.SetDirty(t);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
          };
        }
      } else {
        if (shift) {
          Handles.Button(newB, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap); // Prevent deselect
          if (!forceStart) {
            if (minDistance > (start - newB).xy().sqrMagnitude) {
              newA = start;
              minDistance = (start - newB).xy().sqrMagnitude;
            }
            if (minDistance > (end - newB).xy().sqrMagnitude) {
              newA = end;
              minDistance = (end - newB).xy().sqrMagnitude;
            }
          }
        } else {
          forceStart = false;
          Vector3 newStart = Handles.PositionHandle(start, Quaternion.identity);
          Vector3 newEnd = Handles.PositionHandle(end, Quaternion.identity);
          if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(t, "Modify line");
            t.lines[i - 1] = newStart;
            t.lines[i] = newEnd;
            if (!alt) {
              for (int j = 0; j < t.lines.Count; j++) {
                // !!! DOESNT WORK
                if (t.lines[j].Equals(newStart)) {
                  t.lines[j] = newStart;
                } else if (t.lines[j].Equals(newEnd)) {
                  t.lines[j] = newEnd;
                }
              }
            }
          }
        }
        if (mouse && shift) {
          if (!blockCreate) {
            Undo.RecordObject(t, "Create new line");
            blockCreate = true;
            t.lines.Add(actualNewA);
            t.lines.Add(actualNewB);
            forceStart = true;
            forceStartPos = newB;
            if (!Application.isPlaying) {
              EditorUtility.SetDirty(t);
              UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            };
          }
        } else {
          blockCreate = false;
        }
      }
    }
  }
}
