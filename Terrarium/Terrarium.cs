using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using Terrarium.Components;
using UnityEngine;

namespace Terrarium
{
    public class Terrarium : ModBehaviour
    {
        static Terrarium instance;

        public static Terrarium Instance => instance;

        public INewHorizons NewHorizons;

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            ModHelper.Console.WriteLine($"My mod {nameof(Terrarium)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);

            NewHorizons.GetBodyLoadedEvent().AddListener(bodyName =>
            {
                var body = GameObject.Find(bodyName.Replace(" ", "") + "_Body");

                if (bodyName == "WW_TERRARIUM_Planet")
                {
                    SetUpPlanetBody(body);
                }
                else if (bodyName == "WW_TERRARIUM_Terrarium")
                {
                    SetUpTerrariumBody(body);
                }
                else if (bodyName == "WW_TERRARIUM_Sun")
                {
                    SetUpSunBody(body);
                }
            });

            new Harmony(ModHelper.Manifest.UniqueName).PatchAll(Assembly.GetExecutingAssembly());

            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen);
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;


        }

        void SetUpPlanetBody(GameObject body)
        {
            var warpGO = body.transform.Find("Sector/WarpTransmitter").gameObject;
            warpGO.AddComponent<TerrariumWarpController>();
            foreach (Transform child in warpGO.transform)
            {
                if (child.name is not "WarpTransmitter_Streaming" and not "BlackHole" and not "WhiteHole")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        void SetUpTerrariumBody(GameObject body)
        {
            body.AddComponent<TerrariumController>();

            body.transform.Find("Sector/Water").gameObject.AddComponent<WaterLevelController>();

            foreach (var raft in body.GetComponentsInChildren<RaftController>())
            {
                foreach (Transform child in raft.transform.Find("Colliders"))
                {
                    if (child.name != "collider_base") child.gameObject.SetActive(false);
                }
                raft.transform.Find("LightSensorRoot").gameObject.SetActive(false);
            }
        }

        void SetUpSunBody(GameObject body)
        {
            var artificialSun = body.AddComponent<ArtificialSunController>();

            var lightSourceVolume = body.transform.Find("Volumes/Ruleset").gameObject.AddComponent<SingleLightSourceVolume>();
            lightSourceVolume.LinkLightSource(artificialSun);
        }
    }
}
