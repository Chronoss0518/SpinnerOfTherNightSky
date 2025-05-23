using UnityEngine;

namespace ChUnity.Input
{
    /// <summary>
    /// マウスのクリックやスマートフォンなどのタッチした位置を管理するシングルトンクラス。
    /// マウスに関してはクリックしたボタンも管理する。
    /// instanceを取得し、Updateを行うことで情報を更新する。
    /// </summary>
    [System.Serializable]
    public class PointerManager
    {

        //SingleTon//
        public static PointerManager instance { get; private set; } = new PointerManager();

        PointerManager() { mouseButtonClickCount = 0; }

        //Define//

        public const int MOUSE_BUTTON_CHECK_COUNT = 3;

        //Enums//

        public enum NormalMouseButton
        {
            PrimaryButton,//(通常時左ボタン)//
            SecondryButton,//(通常時右ボタン)//
            MiddleButton
        }

        //Properties//
        public Vector2 startPoint { get; private set; } = Vector2.zero;

        public Vector2 startPointOnWindow { get{ return OnPanelPos(startPoint); } }

        public Vector2 beforePoint { get; private set; } = Vector2.zero;

        public Vector2 beforePointOnWindow { get { return OnPanelPos(beforePoint); } }

        public Vector2 endPoint { get; private set; } = Vector2.zero;

        public Vector2 endPointOnWindow { get { return OnPanelPos(endPoint); } }

        public Vector2 mousePoint { get; private set; } = Vector2.zero;

        public Vector2 mousePointOnWindow { get { return OnPanelPos(mousePoint); } }


        public Vector3 GetCameraToWorldPosition(Camera _camera,Vector2 _pos,float _len)
        {
            return _camera.ScreenToWorldPoint(new Vector3(_pos.x, _pos.y, _len));
        }

        public Vector2 GetWorldToCamera(Camera _camera, Vector3 _pos)
        {
            return _camera.WorldToScreenPoint(_pos);
        }

        public bool IsMouseButtonClick(NormalMouseButton _normalMouseButton)
        {
            return mouseButtonClickFlg[(int)_normalMouseButton];
        }

        public bool IsTouch() { return touchFlg; }

        public bool IsPushFlg()
        {
            bool res = touchFlg;

            for(int i = 0;i < MOUSE_BUTTON_CHECK_COUNT;i++)
            {
                res |= mouseButtonClickFlg[i];
            }

            return res;
        }

        // Update is called once per frame
        public void Update()
        {
            beforePoint = endPoint;
            UpdateMousePoint();
            UpdateTouchPoint();
        }

        void UpdateTouchPoint()
        {
            touchFlg = UnityEngine.Input.touchCount > 0;
            if (!touchFlg) return;

            var touch = UnityEngine.Input.GetTouch(0);

            startPoint = touch.rawPosition;

            endPoint = touch.position;

        }

        void UpdateMousePoint()
        {
            mousePoint = UnityEngine.Input.mousePosition;

            int beforeCount = mouseButtonClickCount;

            for (int i = 0; i < MOUSE_BUTTON_CHECK_COUNT; i++)
            {

                bool beforeFlg = mouseButtonClickFlg[i];
                mouseButtonClickFlg[i] = UnityEngine.Input.GetMouseButton(i);

                mouseButtonClickCount = beforeFlg == mouseButtonClickFlg[i] ? 
                    mouseButtonClickCount : mouseButtonClickFlg[i] ?
                        mouseButtonClickCount + 1 :
                        mouseButtonClickCount - 1;

            }

            if (mouseButtonClickCount > 0)
            {
                if(beforeCount <= 0)
                {
                    startPoint = mousePoint;
                    beforePoint = mousePoint;
                }
                endPoint = mousePoint;
            }

        }

        Vector2 OnPanelPos(Vector2 _pos){ return new Vector2(_pos.x / Screen.width * 2 - 1, _pos.y / Screen.height * -2 + 1); }

        bool[] mouseButtonClickFlg = new bool[MOUSE_BUTTON_CHECK_COUNT];

        bool touchFlg = false;

        int mouseButtonClickCount = 0;


    }

}
