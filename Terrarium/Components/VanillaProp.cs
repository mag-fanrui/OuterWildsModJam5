using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class VanillaProp : MonoBehaviour
    {
        [SerializeField]
        string path;
        [SerializeField]
        List<string> childrenToRemove = [];

        protected void Awake()
        {
            var obj = Terrarium.Instance.NewHorizons.SpawnObject(Terrarium.Instance, transform.root.gameObject, GetComponentInParent<Sector>(), path, Vector3.zero, Vector3.zero, 1f, false);
            foreach (Transform child in obj.transform)
            {
                if (childrenToRemove.Contains(child.name))
                {
                    child.gameObject.SetActive(false);
                }
            }
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }
    }
}
