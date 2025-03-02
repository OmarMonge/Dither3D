// using UnityEngine;
// using Lightbug.CharacterControllerPro.Core;
// using Lightbug.Utilities;
// using System;

// namespace Lightbug.CharacterControllerPro.Demo
// {

//     public class JumpPad1 : CharacterDetector
//     {
//         public Trigger trig;
//         public bool useLocalSpace = true;
//         public Vector3 direction = Vector3.up;
//         public float jumpPadVelocity = 10f;
//         float fixedTime;
//         public GameObject x;
//         public GameObject y;
//         protected bool camera1 = true;
//         protected bool camera2 = true;
//         // public void Update()
//         // {
//         //     // This prevents OnTrigger calls from updating the trigger more than once at the end of the simulation stage.
//         //     if (this.fixedTime == fixedTime)
//         //         return;

//         //     if(trig.firstContact == true){
//         //         Debug.Log("sdsad");
//         //                   x.SetActive(true);
//         //     y.SetActive(false);  
//         //     };
//         // }
//         // protected override void ProcessEnterAction(CharacterActor characterActor)
//         // {
//         //     if (characterActor.GroundObject != gameObject)
//         //         return;
//         //     Debug.Log("stuff");
//         //     //characterActor.ForceNotGrounded();
//         //     x.SetActive(true);
//         //     y.SetActive(false);
//         // }

//         // protected override void ProcessStayAction(CharacterActor characterActor)
//         // {
//         //     ProcessEnterAction(characterActor);
//         // }

//         // private void OnDrawGizmos()
//         // {
//         //     Vector3 direction = useLocalSpace ? transform.TransformDirection(this.direction) : this.direction;
//         //     //Gizmos.DrawRay(transform.position, direction * 2f);
//         //     CustomUtilities.DrawArrowGizmo(transform.position, transform.position + direction * 2f, Color.red);
//         // }
//         public void OnTriggerEnter(Collider other)
//         {
//             if(other.tag == "Player"){
//                 Debug.Log(camera1);
//                 camera1 = !camera1;
//             x.SetActive(camera1);
//             y.SetActive(!camera1);
//             }
//         }

//     }

// }

using UnityEngine;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System;

namespace Lightbug.CharacterControllerPro.Demo
{
    public class JumpPad1 : CharacterDetector
    {
        public CharacterStateController characterStateController;
        public Trigger trig;
        public bool useLocalSpace = true;
        public Vector3 direction = Vector3.up;
        public float jumpPadVelocity = 10f;
        private float fixedTime;

        public GameObject x; // Camera 1
        public GameObject y; // Camera 2

        private bool isCamera1Active = true; // Tracks which camera is active

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Safer way to check tag
            {
                isCamera1Active = !isCamera1Active; // Toggle camera state

                x.SetActive(isCamera1Active);  // Enable Camera 1 when true
                y.SetActive(!isCamera1Active); // Enable Camera 2 when false
                 characterStateController.SwapExternalReference(x.transform);
                Debug.Log("Camera Switched: " + (isCamera1Active ? "Camera 1 Active" : "Camera 2 Active"));
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) // Safer way to check tag
            {
                isCamera1Active = !isCamera1Active; // Toggle camera state

                x.SetActive(!isCamera1Active);  // Enable Camera 1 when true
                y.SetActive(isCamera1Active); // Enable Camera 2 when false
                characterStateController.SwapExternalReference(y.transform);
                Debug.Log("Camera Switched: " + (isCamera1Active ? "Camera 1 Active" : "Camera 2 Active"));
            }
        }
    }
}
