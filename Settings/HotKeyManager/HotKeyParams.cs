using System.Collections;
using UnityEngine;

namespace HotKeys
{
    public class HotKeyParams
    {

        public bool isDown;
        public bool isUp;
        public bool isPressed;

        public HotKeyParams()
        {
            HotKeyManager.instance.StartCoroutine(Update());
        }

        public void Set(EventType eventType)
        {
            if (eventType == EventType.KeyDown)
            {
                isUp = false;
                if (isPressed == false)
                    isDown = true;
                isPressed = true;
            }
            else if (eventType == EventType.KeyUp)
            {
                isUp = true;
                isDown = false;
                isPressed = false;
            }
            else
            {
                isUp = false;
                isDown = false;
                isPressed = false;
            }
        }

        private IEnumerator Update()
        {
            while (true)
            {
                yield return null;
                if (isDown)
                    isDown = false;
                if (isUp)
                    isUp = false;
            }
        }

    }
}
