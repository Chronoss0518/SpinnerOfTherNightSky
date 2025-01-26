using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Common
{
    public class ApplicationEnd : MonoBehaviour
    {
        public void ShutDownApplication(int _num)
        {
            Application.Quit(_num);
        }
    }
}
