using System.Collections;
using UnityEngine;

namespace Ardenfall.Animation
{
    public class RagdollCuller : MonoBehaviour
    {
        private Rigidbody[] rigidbodies;
        private SkinnedMeshRenderer[] renderers;

        private bool isSleeping;
        private MeshFilter[] bakedMeshFilters;
        private MeshRenderer[] bakedMeshRenderers;

        private void OnEnable()
        {
            Initiate();
        }

        /// <summary>
        /// Performs scan on existing skinned mesh renderers and rigidbodies,
        /// And creates their normal mesh object counterparts
        /// </summary>
        public void Initiate()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            bakedMeshFilters = new MeshFilter[renderers.Length];
            bakedMeshRenderers = new MeshRenderer[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                //Ensure skinned mesh always renders when enabled
                renderers[i].updateWhenOffscreen = true;

                GameObject bakedMeshObject = new GameObject("Baked Mesh");
                bakedMeshObject.transform.parent = renderers[i].transform;
                bakedMeshObject.transform.localPosition = Vector3.zero;
                bakedMeshObject.transform.localRotation = Quaternion.identity;

                bakedMeshRenderers[i] = bakedMeshObject.AddComponent<MeshRenderer>();
                bakedMeshRenderers[i].sharedMaterial = renderers[i].sharedMaterial;

                bakedMeshFilters[i] = bakedMeshObject.AddComponent<MeshFilter>();
                bakedMeshFilters[i].sharedMesh = new Mesh();
            }

            SetIsSleeping(false);
        }

        private void Update()
        {
            bool toggle = !isSleeping;

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                if (isSleeping && !rigidbodies[i].IsSleeping())
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

            if (toggle)
                SetIsSleeping(!isSleeping);
        }

        private void SetIsSleeping(bool sleeping)
        {
            if (isSleeping == sleeping)
                return;

            isSleeping = sleeping;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = !isSleeping;
                bakedMeshRenderers[i].enabled = isSleeping;

                if (isSleeping)
                {
                    renderers[i].BakeMesh(bakedMeshFilters[i].sharedMesh);
                    bakedMeshFilters[i].sharedMesh.RecalculateBounds();
                }

            }

        }
    }
}
