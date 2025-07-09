using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class OrreryController : MonoBehaviour
    {
        [SerializeField]
        Transform arch;
        [SerializeField]
        Transform lampPivot;
        [SerializeField]
        Transform lampArm;

        protected void Start()
        {
            TerrariumController.Instance.SunDistance.On(OnSunDistanceChanged);
            TerrariumController.Instance.SunAngle.On(OnSunAngleChanged);
            TerrariumController.Instance.EnclosureAngle.On(OnEnclosureAngleChanged);
        }

        void OnSunDistanceChanged(float value)
        {
            lampArm.transform.localPosition = Vector3.up * Mathf.Lerp(0.63f, 0.9708f, value);
        }

        void OnSunAngleChanged(float value)
        {
            lampPivot.transform.localEulerAngles = Vector3.right * Mathf.Lerp(-90f, 0f, value);
        }

        void OnEnclosureAngleChanged(float value)
        {
            arch.transform.localEulerAngles = Vector3.up * -360f * value;
        }
    }
}
