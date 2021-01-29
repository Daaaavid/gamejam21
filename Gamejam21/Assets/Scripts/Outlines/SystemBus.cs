using UnityEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Outlines
{
      public abstract class SystemBus : ScriptableObject
    {
        public abstract void Invoke();
        public abstract bool IsInitialized { get; protected set; }
        public abstract void Reset();
        public abstract void Init();

        protected virtual bool InvokeWithIdenticalValue => false;
    }
    
    public abstract class SystemBusWithValue<T, TY> : SystemBus where TY : UnityEvent<T>, new()
    {
        public TY OnChange = new TY();
        public T InitialValue;

        public override bool IsInitialized { get; protected set; }

        [SerializeField] private T _value;

        public T Value
        {
            get
            {
                if (!IsInitialized) Init();
                return _value;
            }
        }

        public virtual void SetValue(T x)
        {
            if (!IsInitialized) IsInitialized = true;

            if (!InvokeWithIdenticalValue)
            {
                if (x == null && _value == null) return;
                if (x != null && x.Equals(_value)) return;
            }

            _value = x;
            OnChange.Invoke(x);
        }

        public override void Init()
        {
            _value = InitialValue;
            IsInitialized = true;
        }

        public override void Invoke()
        {
            OnChange.Invoke(Value);
        }
        
        public override void Reset()
        {
            _value = default;
            IsInitialized = false;
        }
        
        public virtual T GetValueAndAddListener(UnityAction<T> action)
        {
            if (!IsInitialized) Init();
            OnChange.AddListener(action);
            return Value;
        }

        public virtual T GetValue()
        {
            if (!IsInitialized) Init();
            return _value;
        }

        public void RemoveListener(UnityAction<T> action)
        {
            OnChange.RemoveListener(action);
        }

        // In the editor, reset IsInitialized value when changing playmode.
#if UNITY_EDITOR
        void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeStateChange;
        }

        void OnDisable()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeStateChange;

        }

        private void PlayModeStateChange(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode) Reset();
        }
#endif
    }
}