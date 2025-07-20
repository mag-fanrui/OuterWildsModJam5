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
        protected string path;
        [SerializeField]
        protected List<string> childrenToRemove = [];
        [SerializeField]
        protected bool removeColliders;

        protected void Awake()
        {
            var obj = Terrarium.NewHorizons.SpawnObject(Terrarium.Instance, transform.root.gameObject, GetComponentInParent<Sector>(), path, Vector3.zero, Vector3.zero, 1f, false);
            foreach (string childPath in childrenToRemove)
            {
                var child = obj.transform.Find(childPath);
                if (child == null)
                {
                    Terrarium.Instance.ModHelper.Console.WriteLine($"Invalid child path: \"{childPath}\" on prop with path \"{path}\"");
                }
                Destroy(child.gameObject);
            }
            if (removeColliders)
            {
                foreach (OWCollider owCollider in obj.GetComponentsInChildren<OWCollider>())
                {
                    Destroy(owCollider);
                }
                foreach (Collider collider in obj.GetComponentsInChildren<Collider>())
                {
                    Destroy(collider);
                }
            }
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
        }
    }
}
