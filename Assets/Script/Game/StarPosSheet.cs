using ChUnity.Input;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections;
using UnityEngine;

public class StarPosSheet : PanelPosBase
{
    PointerManager pointer = PointerManager.instance;

    [SerializeField]
    GameObject starPosPrefab = null;

    [SerializeField, ReadOnly]
    GameObject[][] starPosList = null;

    [SerializeField,ReadOnly]
    StoneBoardManager stoneBoard = null;

    [SerializeField,ReadOnly]
    bool movePanelFlg = false;

    Camera useCam = null;

    public void SetUseCamera(Camera _cam)
    {
        if (_cam == null) return;
        useCam = _cam;
    }

    public void PointDownGrip() { movePanelFlg = true; }
    
    public void PointUpGrip() { movePanelFlg = false; }


    override protected void InitHoryzontalList(int _nowCount)
    {
        starPosList[_nowCount] = new GameObject[PANEL_COUNT_X];
    }

    override protected void InitVerticalList()
    {
        starPosList = null;
        starPosList = new GameObject[PANEL_COUNT_Y][];
    }

    override protected Vector3 SetStartPos(Vector3 _startPos) 
    {
        _startPos.y = 0.0f;
        return _startPos;
    }

    public void SetStoneBoard(StoneBoardManager _stoneBoard)
    {
        stoneBoard = _stoneBoard;
        transform.localPosition = new Vector3(transform.localPosition.x, stoneBoard.stonePosTop, transform.localPosition.z);
    }

    public void AddStarPos(GameObject _obj, int _x, int _y)
    {
        if (!IsRange(_x, _y)) return;
        starPosList[_y][_x] = _obj;
    }

    public void SetStarPosFlg(int _x,int _y,bool _flg)
    {
        if (!IsRange(_x, _y)) return;
        starPosList[_y][_x].SetActive(_flg);
    }

    public bool IsRange(int _x, int _y)
    {
        return
            _x >= 0 && _x < PANEL_COUNT_X &&
            _y >= 0 && _y < PANEL_COUNT_Y;
    }

    public void ClearPos()
    {
        for (int i = 0; i < PANEL_COUNT_Y; i++)
        {
            for (int j = 0; j < PANEL_COUNT_X; j++)
            {
                starPosList[j][i].SetActive(false);
            }
        }
    }

    protected override void CreateOobject(float _vPos, Vector2Int _pos, GameObject _verticalPos, PanelPosManager _builder)
    {
        var starPos = Instantiate(starPosPrefab, _verticalPos.transform);

        starPos.name = $"X[{_pos.x}]Y[{_pos.y}]";
        starPos.transform.localPosition = new Vector3(_vPos, 0.0f, 0.0f);
        starPos.SetActive(false);

        starPosList[_pos.y][_pos.x] = starPos;
    }

    void Update()
    {
        if (stoneBoard == null) return;

        MoveTest();

        var pos = Vector3.zero;
        
        pos.x -= MinTest(transform.localPosition.x - size.x, stoneBoard.transform.localPosition.x - stoneBoard.size.x);
        pos.z -= MinTest(transform.localPosition.z - size.z, stoneBoard.transform.localPosition.z - stoneBoard.size.z);

        pos.x += MaxTest(transform.localPosition.x + size.x, stoneBoard.transform.localPosition.x + stoneBoard.size.x);
        pos.z += MaxTest(transform.localPosition.z + size.z, stoneBoard.transform.localPosition.z + stoneBoard.size.z);

        transform.localPosition += pos;
    }

    float MinTest(float _base, float _target)
    {
        float tmpSize = _base - _target;
        if (tmpSize < 0.0f)
            return tmpSize;
        return 0.0f;
    }
    float MaxTest(float _base, float _target)
    {
        float tmpSize = _target - _base;
        if (tmpSize < 0.0f)
            return tmpSize;
        return 0.0f;
    }

    void MoveTest()
    {
        if (useCam == null) return;
        if (!movePanelFlg) return;

        float cameraHeight = useCam.transform.position.y;

        Vector2 tmp = pointer.endPointOnWindow - pointer.beforePointOnWindow;

        Vector3 movePos = Vector3.zero;

        movePos.x += tmp.x;
        movePos.z += tmp.y;

        transform.localPosition += movePos;
    }


}
