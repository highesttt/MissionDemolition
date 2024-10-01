using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLine : MonoBehaviour
{
    static List<ProjectileLine> PROJ_LINES = new List<ProjectileLine>();
    private const float DIM_MULT = 0.75f;
    private LineRenderer _line;
    private bool _drawing = true;
    private Projectile _projectile;

    void Start() {
        _line = this.GetComponent<LineRenderer>();
        _line.positionCount = 1;
        _line.SetPosition(0, this.transform.position);
        ADD_LINE(this);

        _projectile = this.GetComponent<Projectile>();
    }

    void FixedUpdate() {
        if (_drawing) {
            _line.positionCount++;
            _line.SetPosition(_line.positionCount - 1, this.transform.position);
            
            if (_projectile != null && !_projectile.awake) {
                _drawing = false;
                _projectile = null;
            }
        }
    }

    private void OnDestroy() {
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(ProjectileLine line) {
        Color col;

        foreach (ProjectileLine lineTemp in PROJ_LINES) {
            col = lineTemp._line.startColor;
            col = col * DIM_MULT;
            lineTemp._line.startColor = lineTemp._line.endColor = col;
        }

        PROJ_LINES.Add(line);
    }
}
