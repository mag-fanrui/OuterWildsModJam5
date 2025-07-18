using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class EndingFlowerController : MonoBehaviour
    {
        [SerializeField]
        protected float minDelay;
        [SerializeField]
        protected float maxDelay;

        protected void Start()
        {
            EndingController.Instance.OnEndingStart.AddListener(() =>
            {
                StartCoroutine(DoEnding());    
            });
        }

        IEnumerator DoEnding()
        {
            var delay = UnityEngine.Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            GetComponentInChildren<Animator>().SetBool("Open", true);
        }
    }
}
