using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class GlassWallTextController : MonoBehaviour
    {
        [SerializeField]
        protected string xmlName;
        [SerializeField]
        protected int seed;

        protected void Awake()
        {
            if (string.IsNullOrEmpty(xmlName))
            {
                Terrarium.Instance.ModHelper.Console.WriteLine("Glass wall text with no XML file specified", OWML.Common.MessageType.Error);
                return;
            }

            var textXml = File.ReadAllText($"{Terrarium.Instance.ModHelper.Manifest.ModFolderPath}/planets/text/{xmlName}.xml");
            var textJson = @$"{{""type"": ""wall"",""seed"": {seed}}}";
            var textObj = Terrarium.NewHorizons.CreateNomaiText(textXml, textJson, transform.root.gameObject);
            textObj.transform.SetParent(transform, false);
            textObj.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }
}
