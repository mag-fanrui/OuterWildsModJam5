using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class EndingBlackHoleController : MonoBehaviour
    {
        SingularityController singularityController;
        BlackHoleVolume blackHoleVolume;

        protected void Awake()
        {
            singularityController = transform.Find("BlackHoleRenderer").gameObject.AddComponent<SingularityController>();
            singularityController._owAmbientSource = transform.Find("BlackHoleAmbience").GetComponent<OWAudioSource>();
            singularityController._owOneShotSource = transform.Find("BlackHoleEmissionOneShot").GetComponent<OWAudioSource>();
            blackHoleVolume = transform.Find("BlackHoleVolume").GetComponent<BlackHoleVolume>();
            blackHoleVolume._singularityController = singularityController;
        }

        protected void Start()
        {
            blackHoleVolume._collider.enabled = false;
            singularityController._startActive = false;
            singularityController.CollapseImmediate();

            EndingController.Instance.OnEndingStart.AddListener(() =>
            {
                gameObject.SetActive(true);
                StartCoroutine(DoEnding());
            });

            gameObject.SetActive(false);
        }

        IEnumerator DoEnding()
        {
            singularityController.Create();
            yield return new WaitForSeconds(1f);
            blackHoleVolume._collider.enabled = true;
        }
    }
}
