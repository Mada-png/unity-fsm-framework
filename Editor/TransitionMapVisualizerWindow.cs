using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

public class TransitionMapVisualizerWindow : EditorWindow
{
    private TransitionSet selectedSet;
    private Dictionary<Type, Rect> nodeRects = new();
    private Vector2 scroll;
    private float nodeWidth = 150f;
    private float nodeHeight = 60f;
    private float spacing = 200f;
    private const float hoverThreshold = 5f;

    [MenuItem("Window/FSMDebug/Transition Map Visualizer")]
    public static void Open()
    {
        GetWindow<TransitionMapVisualizerWindow>("Transition Map Visualizer");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        selectedSet = (TransitionSet)EditorGUILayout.ObjectField("Transition Set", selectedSet, typeof(TransitionSet), false);

        if (selectedSet == null) return;

        scroll = EditorGUILayout.BeginScrollView(scroll);
        Rect canvas = GUILayoutUtility.GetRect(2000, 2000); // Large scrollable area

        float viewWidth = position.width;
        int columns = Mathf.Max(1, Mathf.FloorToInt(viewWidth / spacing));

        List<TransitionInfo> transitionInfo =  GetTransitionValue(selectedSet);

        BeginWindows();
        DrawNodes(transitionInfo, canvas, columns);
        EndWindows();

        Handles.BeginGUI();
        DrawTransitions(transitionInfo);
        Handles.EndGUI();

        EditorGUILayout.EndScrollView();
    }

    private void DrawNodes(List<TransitionInfo> info, Rect canvas, int columns)
    {
        nodeRects.Clear();

        var states = GetUniqueStates(info)
            .OrderBy(s => s.Name)
            .ToList();

        for (int i = 0; i < states.Count; i++)
        {
            var state = states[i];
            float x = 50 + (i % columns) * spacing;
            float y = 50 + (i / columns) * (nodeHeight + spacing);

            Rect rect = new(x, y, nodeWidth, nodeHeight);
            nodeRects[state] = rect;

            GUI.contentColor = Color.white;
            GUI.Box(rect, state.Name);
        }
    }

    //At this point, the code is really unreadable and probably need refactoring at some point.

    private void DrawTransitions(List<TransitionInfo> info)
    {
        Vector2 mousePos = Event.current.mousePosition;

        var outGoingGroup = info.GroupBy(t => t.FromStateType);
        float peelOffset = 10f;

        foreach (var group in outGoingGroup)
        {
            int exits = group.Count();
            int i = 0;

            foreach (var tr in group)
            {
                var fromRect = nodeRects[tr.FromStateType];
                var toRect = nodeRects[tr.ToStateType];

                // Calculate the start and end positions for the transition line
                float t = (++i) / (float)(exits + 1);
                float fromYOffset = Mathf.Lerp(fromRect.yMin, fromRect.yMax, t);

                Vector2 A = new Vector2(fromRect.xMax, fromYOffset);
                Vector2 B = A + Vector2.right * peelOffset * i;

                var incomingGroup = info.Where(x => x.ToStateType == tr.ToStateType).ToList();
                int entryIndex = incomingGroup.IndexOf(tr) + 1;
                float tIn = entryIndex / (float)(incomingGroup.Count + 1);
                float yIn = Mathf.Lerp(toRect.yMin, toRect.yMax, tIn);

                float fromYOffsetIn = Mathf.Lerp(toRect.yMin, toRect.yMax, tIn);

                Vector2 C = new Vector2(B.x, yIn);
                Vector2 D = new Vector2(toRect.xMin, yIn);

                Rect blockingRect = default;
                bool foundBlock = false;

                //Check if the line intersects with any other node rectangles (C and D)
                foreach (var rect in nodeRects)
                {
                    if (rect.Value == fromRect) continue;

                    if (LineIntersectDeflated(rect.Value, C, D))
                    {

                        blockingRect = rect.Value;
                        foundBlock = true;

                        Debug.Log(foundBlock);
                        break;
                    }
                }

                List<Vector3> linePoints = new List<Vector3>() { A, B };

                if (foundBlock)
                {
                    bool hopAbove = yIn > blockingRect.center.y;
                    float yHop = hopAbove ? blockingRect.yMax + 10 : blockingRect.yMin - 10;

                    C = D - Vector2.right * peelOffset * i;

                    Vector2 E = new Vector2(B.x, yHop);
                    Vector2 F = new Vector2(C.x, yHop);

                    linePoints.Add(E);
                    linePoints.Add(F);
                    linePoints.Add(C);
                    linePoints.Add(D);
                }
                else
                {
                    // No blocker: simple A→B→C→D
                    linePoints.Add(C);
                    linePoints.Add(D);
                }

                Handles.color = Color.red;
                Handles.DrawSolidDisc(A, Vector3.forward, 1f);

                Handles.color = Color.green;
                Handles.DrawSolidDisc(D, Vector3.forward, 1f);


                //Handles.Label(D, "D");

                bool isHover = false;
                for (int k = 0; k < linePoints.Count - 1; k++)
                {
                    if (HandleUtility.DistanceToLine(linePoints[k], linePoints[k + 1]) < hoverThreshold)
                    {
                        isHover = true;
                        break;
                    }
                }

                Handles.color = isHover ? Color.yellow : Color.white;
                float width = isHover ? 3f : 1f;
                Handles.DrawAAPolyLine(width, linePoints.ToArray());
            }
        }
    }

    private bool IsHover(List<Vector2> linePoints)
    {
        bool hover = false;

        for (int i = 0; i < linePoints.Count - 1; i++)
        {
            if (HandleUtility.DistanceToLine(linePoints[i], linePoints[i + 1]) < hoverThreshold)
            {
                return true;
            }
        }

        return hover;
    }

    // Checks if the line segment from p1 to p2 intersects the rectangle r
    private bool IsIntersectRect(Rect rect, Vector2 startPoint, Vector2 endPoint)
    {
        // If either endpoint lies inside the rectangle, we have an intersection
        if (rect.Contains(startPoint) || rect.Contains(endPoint))
            return true;

        // Otherwise, check each of the four edges of the rectangle:
        // Bottom edge: from (xMin, yMin) to (xMax, yMin)
        // Right edge:  from (xMax, yMin) to (xMax, yMax)
        // Top edge:    from (xMax, yMax) to (xMin, yMax)
        // Left edge:   from (xMin, yMax) to (xMin, yMin)
        return LineSegmentIntersect(startPoint, endPoint, new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMin))
            || LineSegmentIntersect(startPoint, endPoint, new Vector2(rect.xMax, rect.yMin), new Vector2(rect.xMax, rect.yMax))
            || LineSegmentIntersect(startPoint, endPoint, new Vector2(rect.xMax, rect.yMax), new Vector2(rect.xMin, rect.yMax))
            || LineSegmentIntersect(startPoint, endPoint, new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMin, rect.yMin));
    }

    private bool LineIntersectDeflated(Rect rect, Vector2 startPoint, Vector2 endPoint)
    {
        Rect shrunk = new Rect(rect.xMin+1, rect.yMin+1, rect.width-2, rect.height-2);

        return IsIntersectRect(shrunk, startPoint, endPoint);
    }

    private Rect DeflateRect(Rect rect)
    {
        return new Rect(rect.xMin + 1, rect.yMin + 1, rect.width - 2, rect.height - 2);
    }

    private bool LineSegmentIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        var r = a2 - a1;
        var s = b2 - b1;

        float Cross(Vector2 u, Vector2 v) => u.x * v.y - u.y * v.x;
        var denom = Cross(r, s);
        if (Mathf.Approximately(denom, 0)) return false; // parallel

        var u = Cross(b1 - a1, r) / denom;
        var t = Cross(b1 - a1, s) / denom;

        return (t >= 0 && t <= 1) && (u >= 0 && u <= 1);
    }

    private HashSet<Type> GetUniqueStates(List<TransitionInfo> info)
    {
        var uniqueStates = new HashSet<Type>();

        foreach (var transition in info)
        {
            if (transition == null) continue;

            var fromStateType = transition.FromStateType;
            var toStateType = transition.ToStateType;

            if (fromStateType != null) uniqueStates.Add(fromStateType);
            if (toStateType != null) uniqueStates.Add(toStateType);
        }

        return uniqueStates;
    }

    private List<TransitionInfo> GetTransitionValue(TransitionSet set)
    {
        var info = new List<TransitionInfo>();

        foreach (StateTransition transition in set.Transitions)
        {
            if (transition == null) continue;

            var type = transition.GetType();

            var fromProperty = type.GetProperty("FromStateDefinition");
            var toProperty = type.GetProperty("NextStateDefinition");
            var triggerProperty = type.GetProperty("Trigger");

            var from = fromProperty?.GetValue(transition);
            var to = toProperty?.GetValue(transition);
            var trigger = triggerProperty?.GetValue(transition);

            var fromStateType = from?.GetType().GetProperty("StateType")?.GetValue(from) as Type;
            var toStateType = to?.GetType().GetProperty("StateType")?.GetValue(to) as Type;

            info.Add(new TransitionInfo
            {
                TransitionType = type,
                FromStateType = fromStateType,
                ToStateType = toStateType,
                TriggerValue = trigger
            });
        }

        if (info.Count == 0)
        {
            Debug.LogWarning("No transitions found in the selected TransitionSet.");
        }

        return info;
    }
}

public class TransitionInfo
{
    public Type TransitionType { get; set; }
    public Type FromStateType { get; set; }
    public Type ToStateType { get; set; }
    public object TriggerValue { get; set; }
}

public static class HandleDraw
{
    public static void DrawRectBorder(Rect rect, Color borderColor, float padding = 0)
    {
        Vector2 a = new Vector2(rect.xMin + padding, rect.yMin + padding);
        Vector2 b = new Vector2(rect.xMax - padding, rect.yMin + padding);
        Vector2 c = new Vector2(rect.xMax - padding, rect.yMax - padding);
        Vector2 d = new Vector2(rect.xMin + padding, rect.yMax - padding);

        Handles.color = borderColor;
        Handles.DrawPolyLine(a, b, c, d, a);
    }

    public static void DrawRect(Rect rect, Color color)
    {
        Handles.color = color;
        Handles.DrawSolidRectangleWithOutline(rect, color, Color.clear);
    }

    public static void DrawDestinationPoints(Vector2 start, Vector2 end, Color startColor, Color endColor)
    {
        Handles.color = startColor;
        Handles.DrawSolidDisc(start, Vector3.forward, 3f);

        Handles.color = endColor;
        Handles.DrawSolidDisc(end, Vector3.forward, 3f);
    }
}
