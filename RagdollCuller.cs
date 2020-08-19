using System.Collections;
using UnityEngine;

namespace Ardenfall.Animation
{
    public class RagdollCuller : MonoBehaviour
    {
        private Rigidbody[] rigidbodies;
        private SkinnedMeshRenderer[] renderers;

        private bool isSleeping;

        private void OnEnable()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            SetIsSleeping(false);
            StartCoroutine(SlowUpdateLoop());
        }

        private IEnumerator SlowUpdateLoop()
        {
            while(true)
            {
                bool toggle = !isSleeping;

                for(int i = 0; i < rigidbodies.Length; i++)
                {
                    if(isSleeping && !rigidbodies[i].IsSleeping())
                    {
                        toggle = true;
                        break;
                    }

                    if (!isSleeping && !rigidbodies[i].IsSleeping())
                    {
                        toggle = false;
                        break;
                    }
                }

                if(toggle)
                    SetIsSleeping(!isSleeping);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void SetIsSleeping(bool sleeping)
        {
            isSleeping = sleeping;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].updateWhenOffscreen = !isSleeping;

                if (isSleeping)
                {
                    renderers[i].sharedMesh.RecalculateBounds();
                    renderers[i].localBounds = renderers[i].sharedMesh.bounds;
                }
            }

        }
    }
}
