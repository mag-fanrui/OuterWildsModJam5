using System.Linq;
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
        public static INewHorizons NewHorizons => instance.newHorizons;
        public static bool DebugMode => instance.debugMode;

        INewHorizons newHorizons;
        bool debugMode;

        protected void Awake()
        {
            instance = this;
        }

        protected void Start()
        {
            ModHelper.Console.WriteLine($"My mod {nameof(Terrarium)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            newHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            newHorizons.LoadConfigs(this);

            newHorizons.GetBodyLoadedEvent().AddListener(bodyName =>
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
                else if (bodyName == "WW_TERRARIUM_WorldShip")
                {
                    SetUpWorldShipBody(body);
                }
            });

            new Harmony(ModHelper.Manifest.UniqueName).PatchAll(Assembly.GetExecutingAssembly());

            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen);
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        protected void OnGUI()
        {
            if (!DebugMode) return;

            var tc = TerrariumController.Instance;
            if (tc == null) return;

            GUILayout.Label($"Sun Angle: {tc.SunAngle.Target:F2} {(tc.SunAngleEnabled.Value ? "" : "(Locked)")}");
            GUILayout.Label($"Sun Distance: {tc.SunDistance.Target:F2} {(tc.SunDistanceEnabled.Value ? "" : "(Locked)")}");
            GUILayout.Label($"Humidity: {tc.Humidity.Target:F2} {(tc.HumidityEnabled.Value ? "" : "(Locked)")}");
            GUILayout.Label($"Atmosphere: {tc.Atmosphere.Target:F2} {(tc.AtmosphereEnabled.Value ? "" : "(Locked)")}");
            GUILayout.Label($"Enclosure Angle: {tc.EnclosureAngle.Target:F2} {(tc.EnclosureAngleEnabled.Value ? "" : "(Locked)")}");
        }

        public override void Configure(IModConfig config)
        {
            debugMode = config.GetSettingsValue<bool>("Debug Mode");
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;


        }

        void SetUpPlanetBody(GameObject body)
        {
            body.transform.Find("Sector/TerrariumComputer").gameObject.AddComponent<TerrariumComputerController>();

            var warpGO = body.transform.Find("Sector/WarpTransmitter").gameObject;
            foreach (Transform child in warpGO.transform)
            {
                if (child.name is not "WarpTransmitter_Streaming" and not "BlackHole" and not "WhiteHole")
                {
                    //child.gameObject.SetActive(false);
                }
            }
            warpGO.AddComponent<TerrariumWarpTransmitterController>();

            body.transform.Find("Sector/TerrariumInterface/EndingBlackHole").gameObject.AddComponent<EndingBlackHoleController>();

            SetUpNomaiCharacter(body);

            ReplaceMaterials(body);
        }

        void SetUpTerrariumBody(GameObject body)
        {
            body.AddComponent<TerrariumController>();

            body.transform.Find("GravityWell").gameObject.AddComponent<TerrariumVisibilityController>();
            body.transform.Find("Sector/Atmosphere").gameObject.AddComponent<AtmosphereLayerController>();
            body.transform.Find("Sector/Air").gameObject.AddComponent<OxygenLayerController>();
            body.transform.Find("Sector/Water").gameObject.AddComponent<WaterLayerController>();
            body.transform.Find("Sector/Clouds").gameObject.AddComponent<CloudLayerController>();
            body.transform.Find("Sector/Effects/RainEmitter").gameObject.AddComponent<RainLayerController>();

            var warpGO = body.transform.Find("Sector/WarpReceiver").gameObject;
            warpGO.AddComponent<TerrariumWarpReceiverController>();
            foreach (Transform child in warpGO.transform)
            {
                if (child.name is not "WarpReceiver_Streaming" and not "BlackHole" and not "WhiteHole" and not "Effects_NOM_ReverseWarpParticles" and not "ReturnActivationTrigger")
                {
                    child.gameObject.SetActive(false);
                }
            }
            var stopgapCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stopgapCollider.transform.SetParent(warpGO.transform, false);
            stopgapCollider.transform.localScale = Vector3.one * 2f;
            stopgapCollider.transform.localPosition = Vector3.down * 2f * 0.5f;
            Destroy(stopgapCollider.GetComponent<MeshRenderer>());
            Destroy(stopgapCollider.GetComponent<MeshFilter>());

            SetUpNomaiCharacter(body);

            ReplaceMaterials(body);
        }

        void SetUpSunBody(GameObject body)
        {
            var artificialSun = body.AddComponent<ArtificialSunController>();

            var lightSourceVolume = body.transform.Find("Volumes/Ruleset").gameObject.AddComponent<SingleLightSourceVolume>();
            lightSourceVolume.LinkLightSource(artificialSun);
        }

        void SetUpWorldShipBody(GameObject body)
        {
            body.AddComponent<EndingController>();

            SetUpNomaiCharacter(body);

            ReplaceMaterials(body);
        }

        void SetUpNomaiCharacter(GameObject body)
        {
            var jam5Mod = ModHelper.Interaction.TryGetMod("xen-42.ModJam5");
            var nomaiSuitTex = jam5Mod.ModHelper.Assets.GetTexture("planets/assets/Character_NOM_Nomai_v2_d 1.png");

            foreach (var solanumController in body.GetComponentsInChildren<SolanumAnimController>())
            {
                foreach (var renderer in solanumController.gameObject.GetComponentsInChildren<Renderer>())
                {
                    renderer.materials = [.. renderer.materials.Select(m =>
                    {
                        if (m.name.Contains("Character_NOM_Nomai_v2_mat"))
                        {
                            m.mainTexture = nomaiSuitTex;
                        }
                        return m;
                    })];
                }
            }
        }

        static readonly string[] MATERIALS_TO_REPLACE = [
            "Terrain_TH_CraterFloor_mat",
            "Terrain_TH_CraterCliffs_mat",
            "Terrain_TH_Grass_mat",
            "Terrain_TH_Cave_mat"
        ];

        void ReplaceMaterials(GameObject body)
        {
            var replacementMats = Resources.FindObjectsOfTypeAll<Material>().Where(m => MATERIALS_TO_REPLACE.Contains(m.name));
            foreach (var renderer in body.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer.sharedMaterials.Length > 0 && renderer.sharedMaterials.Any(m => m != null && MATERIALS_TO_REPLACE.Contains(m.name)))
                {
                    renderer.sharedMaterials = [.. renderer.sharedMaterials.Select(m =>
                    {
                        if (m == null) return null;
                        var originalMat = replacementMats.FirstOrDefault(o => o.name == m.name);
                        if (originalMat != null) return originalMat;
                        return m;
                    })];
                }
            }
        }
    }
}
