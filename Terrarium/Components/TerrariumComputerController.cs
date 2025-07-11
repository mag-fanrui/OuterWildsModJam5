using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Terrarium.Data;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumComputerController : MonoBehaviour
    {
        const int PARAMETER_COUNT = 5;
        const float TARGET_SUN_ANGLE = 0.75f;
        const float TARGET_SUN_DISTANCE = 0.25f;
        const float TARGET_HUMIDITY = 0.25f;
        const float TARGET_ATMOSPHERE = 0.5f;
        const float TARGET_ENCLOSURE_ANGLE = 0.25f;

        static TerrariumComputerController instance;

        public static TerrariumComputerController Instance => instance;

        NomaiVesselComputer computer;
        string xmlSrc;
        XmlDocument xmlDoc;

        protected void Awake()
        {
            instance = this;
            computer = GetComponent<NomaiVesselComputer>();
            var xmlPath = Terrarium.Instance.ModHelper.Manifest.ModFolderPath + "/planets/text/TERRARIUM_COMPUTER.xml";
            xmlSrc = System.IO.File.ReadAllText(xmlPath);
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlSrc);
        }

        protected void Start()
        {
            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
            TerrariumController.Instance.OnParameterChanged.AddListener(OnParameterChanged);
            computer.TurnOff();
        }

        public string GetSystemCountString() => $"{GetSystemCount():N0}";
        public string GetCandidateCountString() => $"{GetCandidateCount():N0}";

        int GetTotalParameterCount() => PARAMETER_COUNT;

        int GetUnsolvedParameterCount()
        {
            var count = 0;
            var tc = TerrariumController.Instance;
            if (tc.SunAngle.Target != TARGET_SUN_ANGLE) count++;
            if (tc.SunDistance.Target != TARGET_SUN_DISTANCE) count++;
            if (tc.Humidity.Target != TARGET_HUMIDITY) count++;
            if (tc.Atmosphere.Target != TARGET_ATMOSPHERE) count++;
            if (tc.EnclosureAngle.Target != TARGET_ENCLOSURE_ANGLE) count++;
            return count;
        }

        int GetSystemCount() => (int)Mathf.Pow(3f, 3f * GetTotalParameterCount());

        int GetCandidateCount() => (int)Mathf.Pow(3f, 3f * GetUnsolvedParameterCount());

        void OnStateChanged(TerrariumStateData state)
        {
            if (state != null)
            {
                computer.TurnOn();
            }
            else
            {
                computer.TurnOff();
            }
        }

        void OnParameterChanged(TerrariumParamType _, float __)
        {
            if (GetUnsolvedParameterCount() == 0)
            {
                DialogueConditionManager.SharedInstance.SetConditionState("WW_TERRARIUM_AllParametersSolved", true);
            }
            else
            {
                DialogueConditionManager.SharedInstance.SetConditionState("WW_TERRARIUM_AllParametersSolved", false);
            }
        }
    }
}
