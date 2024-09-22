using UnityEngine;
using Cinemachine;

namespace RPG
{
    public class FaceCurrentVCam : MonoBehaviour
    {
        [SerializeField] protected Transform[] whatWillFace = new Transform[0];

        protected virtual void Awake()
        {
            cineBrain = FindObjectOfType<CinemachineBrain>();
        }

        protected CinemachineBrain cineBrain;

        protected virtual void Update()
        {
            foreach (var elem in whatWillFace)
            {
                elem.transform.LookAt(VCamTrans);
            }
        }

        protected virtual Transform VCamTrans
        {
            get
            {
                var vCam = cineBrain.ActiveVirtualCamera;
                var vCamGO = vCam.VirtualCameraGameObject;
                return vCamGO.transform;
            }
        }
    }
}