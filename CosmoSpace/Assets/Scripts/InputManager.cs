using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CS
{
    public class MouseInput
    {
        public UnityEvent leftDown = new UnityEvent();
        public UnityEvent midDown = new UnityEvent();
        public UnityEvent rightDown = new UnityEvent();
        public UnityEvent leftUp = new UnityEvent();
        public UnityEvent midUp = new UnityEvent();
        public UnityEvent rightUp = new UnityEvent();
    
        public void OnUpdate ()
        {
            if (Input.GetMouseButtonDown(0)) this.leftDown.Invoke();
            if (Input.GetMouseButtonDown(1)) this.midDown.Invoke();
            if (Input.GetMouseButtonDown(2)) this.rightDown.Invoke();
            if (Input.GetMouseButtonUp(0)) this.leftUp.Invoke();
            if (Input.GetMouseButtonUp(1)) this.midUp.Invoke();
            if (Input.GetMouseButtonUp(2)) this.rightUp.Invoke();
        }
    }

    public class KeyboardHotInput
    {
        public UnityEvent escapeDown = new UnityEvent();
        public UnityEvent enterDown = new UnityEvent();
        public UnityEvent ctrlDown = new UnityEvent();
        public UnityEvent shiftDown = new UnityEvent();
        public UnityEvent altDown = new UnityEvent();        
        public UnityEvent escapeUp = new UnityEvent();
        public UnityEvent enterUp = new UnityEvent();
        public UnityEvent ctrlUp = new UnityEvent();
        public UnityEvent shiftUp = new UnityEvent();
        public UnityEvent altUp = new UnityEvent();

        public float timeDown = 0;

        public void OnUpdate ()
        {
            this.timeDown += Time.deltaTime;
            
            if (Input.GetKeyDown(KeyCode.Escape)) this._Invoke(this.escapeDown);
            if (Input.GetKeyDown(KeyCode.KeypadEnter)) this._Invoke(this.enterDown);
            if (Input.GetKeyDown(KeyCode.LeftControl)) this._Invoke(this.ctrlDown);
            if (Input.GetKeyDown(KeyCode.LeftShift)) this._Invoke(this.shiftDown);
            if (Input.GetKeyDown(KeyCode.LeftAlt)) this._Invoke(this.altDown);           
            if (Input.GetKeyUp(KeyCode.Escape)) this._Invoke(this.escapeUp);
            if (Input.GetKeyUp(KeyCode.KeypadEnter)) this._Invoke(this.enterUp);
            if (Input.GetKeyUp(KeyCode.LeftControl)) this._Invoke(this.ctrlUp);
            if (Input.GetKeyUp(KeyCode.LeftShift)) this._Invoke(this.shiftUp);
            if (Input.GetKeyUp(KeyCode.LeftAlt)) this._Invoke(this.altUp);
        }

        private void _Invoke (UnityEvent unityEvent)
        {
            unityEvent.Invoke();
            this.timeDown = 0;
        }
    }

    public class InputManager : MonoBehaviour
    {
        private static MouseInput _mouse;
        private static KeyboardHotInput _keyboard;

        public static MouseInput mouse => _mouse;
        public static KeyboardHotInput keyboard => _keyboard;

        public InputManager ()
        {
            _mouse = new MouseInput();
            _keyboard = new KeyboardHotInput();
        }

        private void Update()
        {
            _mouse.OnUpdate();
            _keyboard.OnUpdate();
        }
    }
}    