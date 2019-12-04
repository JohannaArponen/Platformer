
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveToClosestPointInShapes))]
public class MoveToClosestPointInShapesCustomEditor : Editor {

  private bool mouse = false;
  private bool blockCreate = false;
  private int createdAt;
  private MoveToClosestPointInShapes t { get => ((MoveToClosestPointInShapes)target); }

  protected virtual void OnSceneGUI() {
    var shift = Event.current.shift;
    var control = Event.current.control;


    switch (Event.current.type) {
      case EventType.Repaint:
        Handles.color = Handles.xAxisColor;
        if (t.lines.Count % 2 != 0) {
          var tail = t.lines[t.lines.Count - 1];
          t.lines.RemoveAt(t.lines.Count - 1);
          Handles.DrawLines(t.lines.ToArray());
          t.lines.Add(tail);
        } else
          Handles.DrawLines(t.lines.ToArray());
        break;
      case EventType.MouseDown:
        if (Event.current.button == 0) {
          Debug.Log("Left-Mouse Down");
          mouse = true;
        }
        break;
      case EventType.MouseUp:
        if (Event.current.button == 0) {
          Debug.Log("Left-Mouse Down");
          mouse = false;
          blockCreate = false;
        }
        break;
    }

    for (int i = 1; i < t.lines.Count; i += 2) {
      var start = t.lines[i - 1];
      var end = t.lines[i];

      EditorGUI.BeginChangeCheck();
      if (shift) {
        // Creating new lines from first line works but not from other lines
        if (blockCreate) {
          if (i == createdAt) {
            Vector3 newStart = Handles.PositionHandle(start, Quaternion.identity);
            Vector3 newEnd = Handles.PositionHandle(end, Quaternion.identity);
            if (EditorGUI.EndChangeCheck()) {
              Undo.RecordObject(t, "Change line position");
              t.lines[i - 1] = newStart;
              t.lines[i] = newEnd;
            }
          }
        } else {
          Vector3 newStart = Handles.PositionHandle(start, Quaternion.identity);
          Vector3 newEnd = Handles.PositionHandle(end, Quaternion.identity);
          if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(t, "Added new line");
            if (newStart == start) {
              t.lines.Add(newEnd);
              t.lines.Add(newEnd);
            } else {
              t.lines.Add(newStart);
              t.lines.Add(newStart);
            }
            createdAt = t.lines.Count - 1;
            blockCreate = true;
            if (!Application.isPlaying) {
              EditorUtility.SetDirty(t);
              UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            };
          }
        }
      } else {
        Vector3 newStart = Handles.PositionHandle(start, Quaternion.identity);
        Vector3 newEnd = Handles.PositionHandle(end, Quaternion.identity);
        if (EditorGUI.EndChangeCheck()) {
          Undo.RecordObject(t, "Change line position");
          t.lines[i - 1] = newStart;
          t.lines[i] = newEnd;
        }
      }
    }
  }
}