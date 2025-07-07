using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Terrarium.Components
{
    public class ChaseValue : MonoBehaviour
    {
        public class ChaseValueChangedEvent : UnityEvent<float> { }

        [SerializeField]
        float current = 0f;
        [SerializeField]
        float target = 0f;
        [SerializeField]
        float rate = 0.1f;
        [SerializeField]
        readonly ChaseValueChangedEvent onChanged = new();

        public static ChaseValue Create(MonoBehaviour parent, float value, UnityAction<float> onChange, float rate = 0.1f)
        {
            var cv = parent.gameObject.AddComponent<ChaseValue>();
            cv.current = value;
            cv.target = value;
            cv.rate = rate;
            cv.enabled = false;
            if (onChange != null)
            {
                cv.onChanged.AddListener(onChange);
            }
            cv.onChanged.Invoke(value);
            return cv;
        }

        public float Current
        {
            get => current;
            set
            {
                current = value;
                onChanged.Invoke(current);
                enabled = IsChasing;
            }
        }
        public float Target
        {
            get => target;
            set
            {
                target = value;
                enabled = IsChasing;
            }
        }

        public float Rate
        {
            get => rate;
            set => rate = value;
        }

        public bool IsChasing => current != target;

        public void On(UnityAction<float> callback)
        {
            onChanged.AddListener(callback);
            callback(current);
        }

        public void Off(UnityAction<float> callback)
        {
            onChanged.RemoveListener(callback);
        }

        protected void Update()
        {
            if (IsChasing)
            {
                current = Mathf.MoveTowards(current, target, Time.deltaTime * rate);
                onChanged.Invoke(current);
            }
            if (!IsChasing)
            {
                enabled = false;
                return;
            }
        }

        public static implicit operator float(ChaseValue cv) => cv.Current;
    }
}
