using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Terrarium.Components
{
    public class ToggleValue : MonoBehaviour
    {
        public class ToggleValueChangedEvent : UnityEvent<bool> { }

        [SerializeField]
        bool value = false;
        [SerializeField]
        readonly ToggleValueChangedEvent onChanged = new();

        public static ToggleValue Create(MonoBehaviour parent, bool value, UnityAction<bool> onChange = null)
        {
            var tv = parent.gameObject.AddComponent<ToggleValue>();
            tv.value = value;
            if (onChange != null)
            {
                tv.onChanged.AddListener(onChange);
            }
            tv.onChanged.Invoke(value);
            return tv;
        }

        public bool Value
        {
            get => value;
            set
            {
                this.value = value;
                onChanged.Invoke(this.value);
            }
        }

        public void On(UnityAction<bool> callback)
        {
            onChanged.AddListener(callback);
            callback(value);
        }

        public void Off(UnityAction<bool> callback)
        {
            onChanged.RemoveListener(callback);
        }

        public static implicit operator bool(ToggleValue tv) => tv != null && tv.Value;
    }
}
