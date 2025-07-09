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
        public class PlayerInsideEvent : UnityEvent<bool> { }

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
        [HideInInspector]
        public ChaseValue EnclosureAngle;
        [HideInInspector]
        public ToggleValue SunDistanceEnabled;
        [HideInInspector]
        public ToggleValue SunAngleEnabled;
        [HideInInspector]
        public ToggleValue HumidityEnabled;
        [HideInInspector]
        public ToggleValue AtmosphereEnabled;
        [HideInInspector]
        public ToggleValue EnclosureAngleEnabled;

        public readonly StateChangeEvent OnStateChanged = new();
        public readonly PlayerInsideEvent OnPlayerInside = new();

        TerrariumStateData currentStateData;
        bool wasInside;

        protected void Awake()
        {
            instance = this;
            SunDistance = ChaseValue.Create(this, 0.5f, OnSunDistanceChanged);
            SunAngle = ChaseValue.Create(this, 0.5f, OnSunAngleChanged);
            Humidity = ChaseValue.Create(this, 0.5f, OnHumidityChanged);
            Atmosphere = ChaseValue.Create(this, 0.5f, OnAtmosphereChanged);
            EnclosureAngle = ChaseValue.Create(this, 0.5f, OnEnclosureAngleChanged);
            SunDistanceEnabled = ToggleValue.Create(this, false, OnSunDistanceEnabledChanged);
            SunAngleEnabled = ToggleValue.Create(this, false, OnSunAngleEnabledChanged);
            HumidityEnabled = ToggleValue.Create(this, false, OnHumidityEnabledChanged);
            AtmosphereEnabled = ToggleValue.Create(this, false, OnAtmosphereEnabledChanged);
            EnclosureAngleEnabled = ToggleValue.Create(this, false, OnEnclosureAngleEnabledChanged);
        }

        public Vector3 GetSunRelativePosition()
        {
            var angleZ = 180f * SunAngle.Current;
            var angleY = 180f * EnclosureAngle.Current;
            var dist = SunDistance.Current;

            var dir = Vector3.up;
            dir = Quaternion.AngleAxis(angleZ, Vector3.forward) * dir;
            dir = Quaternion.AngleAxis(angleY, Vector3.up) * dir;

            return dir * dist;
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

        public bool IsPlayerInside() =>
            GetState() != null && 
            Vector3.Distance(Locator.GetPlayerTransform().position, GetTerrariumWorldPosition()) < GetTerrariumRadius();

        public TerrariumStateData GetState() => currentStateData;

        public void SetState(TerrariumStateData stateData)
        {
            if (currentStateData != stateData)
            {
                currentStateData = stateData;
                OnStateChanged.Invoke(stateData);
            }
        }

        protected void Update()
        {
            if (IsPlayerInside() != wasInside)
            {
                wasInside = !wasInside;
                OnPlayerInside.Invoke(wasInside);
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

        void OnEnclosureAngleChanged(float value)
        {

        }

        void OnSunDistanceEnabledChanged(bool value)
        {

        }

        void OnSunAngleEnabledChanged(bool value)
        {

        }

        void OnHumidityEnabledChanged(bool value)
        {

        }

        void OnAtmosphereEnabledChanged(bool value)
        {

        }

        void OnEnclosureAngleEnabledChanged(bool value)
        {

        }
    }
}
