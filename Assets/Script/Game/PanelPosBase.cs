using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public abstract class PanelPosBase : MonoBehaviour
{

    [SerializeField]
    protected Vector2Int PANEL_COUNT = Vector2Int.zero;

    public int PANEL_COUNT_X { get { return PANEL_COUNT.x - 1; } }

    public int PANEL_COUNT_Y { get { return PANEL_COUNT.y - 1; } }

    [SerializeField]
    MeshFilter meshFilter = null;

    public Vector3 size { get; private set; } = Vector3.zero;

    abstract protected void Init(Vector3 _startPos,Vector2 _interval);

    public class PanelPosManager
    {
        [SerializeField, ReadOnly]
        private Vector2 interval = Vector2.zero;

        [SerializeField, ReadOnly]
        private Vector3 startPos = Vector3.zero;


        public void CreatePanel(PanelPosBase _createPanel)
        {
            if (_createPanel == null) return;
            if (_createPanel.meshFilter == null) return;

            InitSize(_createPanel);

            _createPanel.Init(startPos,interval);

        }


        void MaxMinChangeTest(ref float _max, ref float _min)
        {
            if (_max >= _min) return;
            _max += _min;
            _min = _max - _min;
            _max = _max - _min;
        }


        void InitSize(PanelPosBase _createPanel)
        {
            if (_createPanel == null) return;
            if (_createPanel.meshFilter == null) return;

            var bounds = _createPanel.meshFilter.mesh.bounds;

            var tmp = bounds.max - bounds.min;

            tmp = _createPanel.meshFilter.transform.localToWorldMatrix.MultiplyPoint(tmp);

            tmp.x = Mathf.Abs(tmp.x);
            tmp.y = Mathf.Abs(tmp.y);
            tmp.z = Mathf.Abs(tmp.z);

            interval.x = tmp.x / _createPanel.PANEL_COUNT.x;
            interval.y = tmp.z / _createPanel.PANEL_COUNT.y;

            var minPos = _createPanel.meshFilter.transform.localToWorldMatrix.MultiplyPoint(bounds.min);
            var maxPos = _createPanel.meshFilter.transform.localToWorldMatrix.MultiplyPoint(bounds.max);

            MaxMinChangeTest(ref maxPos.x, ref minPos.x);
            MaxMinChangeTest(ref maxPos.y, ref minPos.y);
            MaxMinChangeTest(ref maxPos.z, ref minPos.z);

            startPos = minPos;

            startPos.x += interval.x;
            startPos.z += interval.y;
            startPos.y = maxPos.y;

            _createPanel.size = tmp;
        }

    }
}
