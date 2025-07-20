using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using Terrarium.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Terrarium.Components
{
    public class TerrariumController : MonoBehaviour
    {
        public class StateChangeEvent : UnityEvent<TerrariumStateData> { }
        public class ParameterChangeEvent : UnityEvent<TerrariumParamType, float> { }
        public class PlayerInsideEvent : UnityEvent<bool> { }
        public class EmergencyWarpEvent : UnityEvent<Transform> { }

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
        public readonly ParameterChangeEvent OnParameterChanged = new();
        public readonly PlayerInsideEvent OnPlayerInside = new();
        public readonly EmergencyWarpEvent OnEmergencyWarp = new();

        protected TerrariumStateData currentStateData;
        protected bool wasInside;
        protected List<TerrariumStateController> stateControllers = [];

        protected void Awake()
        {
            instance = this;
            SunDistance = ChaseValue.Create(0.5f, OnSunDistanceChanged);
            SunAngle = ChaseValue.Create(0.5f, OnSunAngleChanged);
            Humidity = ChaseValue.Create(0.5f, OnHumidityChanged);
            Atmosphere = ChaseValue.Create(0.5f, OnAtmosphereChanged);
            EnclosureAngle = ChaseValue.Create(0.5f, OnEnclosureAngleChanged);
            SunDistanceEnabled = ToggleValue.Create(false);
            SunAngleEnabled = ToggleValue.Create(false);
            HumidityEnabled = ToggleValue.Create(false);
            AtmosphereEnabled = ToggleValue.Create(false);
            EnclosureAngleEnabled = ToggleValue.Create(false);
        }

        public Vector3 GetSunWorldPosition()
        {
            var angleX = 90f * (1f - SunAngle.Current);

            var rotation = Quaternion.Euler(angleX, 0f, 0f);
            var dir = rotation * Vector3.up;

            var sunPos = dir * GetSunOrbitDistance();

            return transform.root.TransformPoint(sunPos);
        }

        public float GetSunOrbitDistance() => GetTerrariumRadius() + 150f + SunDistance.Current * 200f;

        public Vector3 GetTerrariumWorldPosition()
        {
            return transform.root.position;
        }

        public float GetTerrariumRadius() => 75f;

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

        public ChaseValue GetParameter(TerrariumParamType paramType) => paramType switch
        {
            TerrariumParamType.SunDistance => SunDistance,
            TerrariumParamType.SunAngle => SunAngle,
            TerrariumParamType.Humidity => Humidity,
            TerrariumParamType.Atmosphere => Atmosphere,
            TerrariumParamType.EnclosureAngle => EnclosureAngle,
            _ => throw new ArgumentOutOfRangeException(nameof(paramType)),
        };

        public ToggleValue GetParameterEnabled(TerrariumParamType paramType) => paramType switch
        {
            TerrariumParamType.SunDistance => SunDistanceEnabled,
            TerrariumParamType.SunAngle => SunAngleEnabled,
            TerrariumParamType.Humidity => HumidityEnabled,
            TerrariumParamType.Atmosphere => AtmosphereEnabled,
            TerrariumParamType.EnclosureAngle => EnclosureAngleEnabled,
            _ => throw new ArgumentOutOfRangeException(nameof(paramType)),
        };

        public TerrariumStateController GetStateController(TerrariumStateData stateData)
            => stateControllers.FirstOrDefault(c => c.StateData == stateData);

        public void RegisterStateController(TerrariumStateController controller)
            => stateControllers.Add(controller);

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
            OnParameterChanged.Invoke(TerrariumParamType.SunDistance, value);
        }

        void OnSunAngleChanged(float value)
        {
            OnParameterChanged.Invoke(TerrariumParamType.SunAngle, value);
        }

        void OnHumidityChanged(float value)
        {
            OnParameterChanged.Invoke(TerrariumParamType.Humidity, value);
        }

        void OnAtmosphereChanged(float value)
        {
            OnParameterChanged.Invoke(TerrariumParamType.Atmosphere, value);
        }

        void OnEnclosureAngleChanged(float value)
        {
            OnParameterChanged.Invoke(TerrariumParamType.EnclosureAngle, value);
        }
    }
}
