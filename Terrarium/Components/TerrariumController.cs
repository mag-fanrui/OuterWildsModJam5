using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Terrarium.Components
{
    public class TerrariumController : MonoBehaviour
    {
        public class StateChangeEvent : UnityEvent<TerrariumStateData> { }

        static TerrariumController instance;

        public static TerrariumController Instance => instance;

        [HideInInspector]
        public ChaseValue SunDistance;
        [HideInInspector]
        public ChaseValue SunAngle;
        [HideInInspector]
        public ChaseValue Humidity;
        [HideInInspector]
        public ChaseValue Atmosphere;

        public readonly StateChangeEvent OnStateChanged = new();

        TerrariumStateData currentStateData;

        protected void Awake()
        {
            instance = this;
            SunDistance = ChaseValue.Create(this, 0.5f, OnSunDistanceChanged);
            SunAngle = ChaseValue.Create(this, 0.5f, OnSunAngleChanged);
            Humidity = ChaseValue.Create(this, 0.5f, OnHumidityChanged);
            Atmosphere = ChaseValue.Create(this, 0.5f, OnAtmosphereChanged);
        }

        public Vector3 GetSunRelativePosition()
        {
            var angle = 180f * SunAngle.Current;
            var dist = SunDistance.Current;
            return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * dist;
        }

        public Vector3 GetSunWorldPosition()
        {
            var sunPos = GetSunRelativePosition().normalized;
            sunPos *= GetSunOrbitDistance();
            return transform.root.TransformPoint(sunPos);
        }

        public float GetSunOrbitDistance() => GetTerrariumRadius() + 100f + SunDistance.Current * 200f;

        public Vector3 GetTerrariumWorldPosition()
        {
            return transform.root.position;
        }

        public float GetTerrariumRadius() => 350f;

        public TerrariumStateData GetState() => currentStateData;

        public void SetState(TerrariumStateData stateData)
        {
            if (currentStateData != stateData)
            {
                currentStateData = stateData;
                OnStateChanged.Invoke(stateData);
            }
        }

        void OnSunDistanceChanged(float value)
        {

        }

        void OnSunAngleChanged(float value)
        {

        }

        void OnHumidityChanged(float value)
        {

        }

        void OnAtmosphereChanged(float value)
        {

        }
    }
}
