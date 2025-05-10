using UnityEngine;


namespace ChUnity.Input
{
    /// <summary>
    /// マウスのクリックやスマートフォンなどのタッチした位置を管理するシングルトンクラス。
    /// マウスに関してはクリックしたボタンも管理する。
    /// instanceを取得し、Updateを行うことで情報を更新する。
    /// </summary>
    public class PointerManager
    {

        //SingleTon//
        public static PointerManager instance { get; private set; } = new PointerManager();

        PointerManager() { }

        //Define//

        public const int MOUSE_BUTTON_CHECK_COUNT = 20;

        //Enums//

        public enum NormalMouseButton
        {
            PrimaryButton,//(通常時左ボタン)//
            SecondryButton,//(通常時右ボタン)//
            MiddleButton
        }

        //Properties//
        public Vector2 startPoint { get; private set; } = Vector2.zero;

        public Vector2 beforePoint { get; private set; } = Vector2.zero;

        public Vector2 endPoint { get; private set; } = Vector2.zero;

        public Vector2 mousePoint { get; private set; } = Vector2.zero;

        public bool IsMouseButtonClick(NormalMouseButton _normalMouseButton)
        {
            return mouseButtonClickFlg[(int)_normalMouseButton];
        }

        public bool IsMouseButtonClick(int _buttonNo)
        {
            if (_buttonNo < 0 || _buttonNo >= MOUSE_BUTTON_CHECK_COUNT) return false;
            return mouseButtonClickFlg[_buttonNo];
        }

        // Update is called once per frame
        public void Update()
        {
            UpdateTouchPoint();
            UpdateMousePoint();
        }

        void UpdateTouchPoint()
        {
            if (UnityEngine.Input.touchCount <= 0) return;

            var touch = UnityEngine.Input.GetTouch(0);

            startPoint = touch.rawPosition;

            beforePoint = endPoint;

            endPoint = touch.position;

        }

        void UpdateMousePoint()
        {
            mousePoint = UnityEngine.Input.mousePosition;

            int beforeCount = mouseButtonClickCount;

            beforePoint = endPoint;

            for (int i = 0; i < MOUSE_BUTTON_CHECK_COUNT; i++)
            {
                if (UnityEngine.Input.GetMouseButtonDown(i))
                {
                    mouseButtonClickFlg[i] = true;
                    mouseButtonClickCount++;
                }

                if (UnityEngine.Input.GetMouseButtonUp(i) && mouseButtonClickFlg[i])
                {
                    mouseButtonClickFlg[i] = false;
                    mouseButtonClickCount--;
                }
            }

            if (beforeCount == 0 && mouseButtonClickCount > 0)
            {
                startPoint = mousePoint;
                beforePoint = mousePoint;
                endPoint = mousePoint;
            }

            if (mouseButtonClickCount > 0)
            {
                endPoint = mousePoint;
            }

        }

        bool[] mouseButtonClickFlg = new bool[MOUSE_BUTTON_CHECK_COUNT];

        int mouseButtonClickCount = 0;


    }

}
