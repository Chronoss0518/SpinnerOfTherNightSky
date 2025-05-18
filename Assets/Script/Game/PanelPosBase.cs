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

    abstract protected void InitHoryzontalList(int _nowCount);
    abstract protected void InitVerticalList();

    protected virtual Vector3 SetStartPos(Vector3 _startPos) { return _startPos; }

    public Vector2Int GetPlayerPositionPos(int _x,int _y,GameManager.PlayerPosition _playerPosition)
    {
        if (_playerPosition == GameManager.PlayerPosition.Right)
            return new Vector2Int(_y, PANEL_COUNT_Y - _x);
        if (_playerPosition == GameManager.PlayerPosition.Back)
            return new Vector2Int(PANEL_COUNT_X - _x, PANEL_COUNT_Y - _y);
        if (_playerPosition == GameManager.PlayerPosition.Left)
            return new Vector2Int(PANEL_COUNT_X - _y, _x);
        return new Vector2Int(_x, _y);
    }

    protected abstract void CreateOobject(float _vPos, Vector2Int _pos, GameObject _verticalPos,PanelPosManager _builder);

    public class PanelPosManager
    {
        public Vector2 interval { get; private set; } = Vector2.zero;

        public Vector3 startPos { get; private set; } = Vector2.zero;


        public void CreatePanel(PanelPosBase _createPanel)
        {
            if (_createPanel == null) return;
            if (_createPanel.meshFilter == null) return;

            InitSize(_createPanel);

            InitPanel(_createPanel);

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

            var tmpInterval = Vector2.zero;

            tmpInterval.x = tmp.x / _createPanel.PANEL_COUNT.x;
            tmpInterval.y = tmp.z / _createPanel.PANEL_COUNT.y;

            var minPos = _createPanel.meshFilter.transform.localToWorldMatrix.MultiplyPoint(bounds.min);
            var maxPos = _createPanel.meshFilter.transform.localToWorldMatrix.MultiplyPoint(bounds.max);

            MaxMinChangeTest(ref maxPos.x, ref minPos.x);
            MaxMinChangeTest(ref maxPos.y, ref minPos.y);
            MaxMinChangeTest(ref maxPos.z, ref minPos.z);

            var tmpStartPos = minPos;

            tmpStartPos.x += tmpInterval.x;
            tmpStartPos.z = maxPos.z - tmpInterval.y;
            tmpStartPos.y = maxPos.y;

            startPos = tmpStartPos;

            interval = tmpInterval;
            _createPanel.size = tmp;
        }

        void InitPanel(PanelPosBase _createPanel)
        {
            Vector3 useStartPos = _createPanel.SetStartPos(startPos);

            Vector3 pos = useStartPos;

            _createPanel.InitVerticalList();

            for (int i = 0; i < _createPanel.PANEL_COUNT_Y; i++)
            {
                pos.x = useStartPos.x;

                _createPanel.InitHoryzontalList(i);

                var verticalPos = new GameObject($"VerticalStonePos[{i + 1}]");
                verticalPos.transform.SetParent(_createPanel.transform);
                verticalPos.transform.localPosition = pos;
                float tmpVPos = 0.0f;
                for (int j = 0; j < _createPanel.PANEL_COUNT_X; j++)
                {
                    var tmpPos = new Vector2Int(j, i);

                    _createPanel.CreateOobject(tmpVPos, tmpPos, verticalPos,this);

                    tmpVPos += interval.x;
                }

                pos.z -= interval.y;
            }
        }

    }
}
