using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neuron;

namespace NeuronBVH
{
    // 摘要:
    //     This contains all of the possible joint types.
    /*public enum JointType
    {
        Hips               = 0,
        RightUpLeg         = 1,
        RightLeg           = 2,
        RightFoot          = 3,
        LeftUpLeg          = 4,
        LeftLeg            = 5,
        LeftFoot           = 6,
        Spine              = 7,
        Spine1             = 8,
        Spine2             = 9,
        Spine3             = 10,
        Neck               = 11,
        Head               = 12,
        RightShoulder      = 13,
        RightArm           = 14,
        RightForeArm       = 15,
        RightHand          = 16,
        RightHandThumb1    = 17,
        RightHandThumb2    = 18,
        RightHandThumb3    = 19,
        RightInHandIndex   = 20,
        RightHandIndex1    = 21,
        RightHandIndex2    = 22,
        RightHandIndex3    = 23,
        RightInHandMiddle  = 24,
        RightHandMiddle1   = 25,
        RightHandMiddle2   = 26,
        RightHandMiddle3   = 27,
        RightInHandRing    = 28,
        RightHandRing1     = 29,
        RightHandRing2     = 30,
        RightHandRing3     = 31,
        RightInHandPinky   = 32,
        RightHandPinky1    = 33,
        RightHandPinky2    = 34,
        RightHandPinky3    = 35,
        LeftShoulder       = 36,
        LeftArm            = 37,
        LeftForeArm        = 38,
        LeftHand           = 39,
        LeftHandThumb1     = 40,
        LeftHandThumb2     = 41,
        LeftHandThumb3     = 42,
        LeftInHandIndex    = 43,
        LeftHandIndex1     = 44,
        LeftHandIndex2     = 45,
        LeftHandIndex3     = 46,
        LeftInHandMiddle   = 47,
        LeftHandMiddle1    = 48,
        LeftHandMiddle2    = 49,
        LeftHandMiddle3    = 50,
        LeftInHandRing     = 51,
        LeftHandRing1      = 52,
        LeftHandRing2      = 53,
        LeftHandRing3      = 54,
        LeftInHandPinky    = 55,
        LeftHandPinky1     = 56,
        LeftHandPinky2     = 57,
        LeftHandPinky3     = 58,
    }*/


    class NeuronSkeletonBVH
    {

        public static void AddNeuronSkeleton(BVHSkeleton Skeleton)
        {
            //Die Person steht falsch herum im Koordinatensystem der Kinect! Es wird erst beim Abspeichern korrigiert, weshalb die Verarbeitung noch mit umgekehrten Koordinaten erfolgt
            BVHBone Hips = new BVHBone(null, NeuronBones.Hips.ToString(), 6, TransAxis.None, true, 0);

            BVHBone RightUpLeg = new BVHBone(Hips, NeuronBones.RightUpLeg.ToString(), 3, TransAxis.None, true, 1);
            BVHBone RightLeg = new BVHBone(RightUpLeg, NeuronBones.RightLeg.ToString(), 3, TransAxis.None, true, 2);
            BVHBone RightFoot = new BVHBone(RightLeg, NeuronBones.RightFoot.ToString(), 0, TransAxis.None, true, 3);

            BVHBone LeftUpLeg = new BVHBone(Hips, NeuronBones.LeftUpLeg.ToString(), 3, TransAxis.None, true, 4);
            BVHBone LeftLeg = new BVHBone(LeftUpLeg, NeuronBones.LeftLeg.ToString(), 3, TransAxis.None, true, 5);
            BVHBone LeftFoot = new BVHBone(LeftLeg, NeuronBones.LeftFoot.ToString(), 0, TransAxis.None, true, 6);

            BVHBone Spine = new BVHBone(Hips, NeuronBones.Spine.ToString(), 3, TransAxis.None, true, 7);
            BVHBone Spine1 = new BVHBone(Spine, NeuronBones.Spine1.ToString(), 3, TransAxis.None, true, 8);
            BVHBone Spine2 = new BVHBone(Spine1, NeuronBones.Spine2.ToString(), 3, TransAxis.None, true, 9);
            BVHBone Spine3 = new BVHBone(Spine2, NeuronBones.Spine3.ToString(), 3, TransAxis.None, true, 10);
            BVHBone Neck = new BVHBone(Spine3, NeuronBones.Neck.ToString(), 3, TransAxis.None, true, 11);
            BVHBone Head = new BVHBone(Neck, NeuronBones.Head.ToString(), 0, TransAxis.None, true, 12);

            BVHBone RightShoulder = new BVHBone(Spine3, NeuronBones.RightShoulder.ToString(), 3, TransAxis.None, true, 13);
            BVHBone RightArm = new BVHBone(RightShoulder, NeuronBones.RightArm.ToString(), 3, TransAxis.None, true, 14);
            BVHBone RightForeArm = new BVHBone(RightArm, NeuronBones.RightForeArm.ToString(), 3, TransAxis.None, true, 15);
            BVHBone RightHand = new BVHBone(RightForeArm, NeuronBones.RightHand.ToString(), 3, TransAxis.None, true, 16);
            BVHBone RightHandThumb1 = new BVHBone(RightHand, NeuronBones.RightHandThumb1.ToString(), 3, TransAxis.None, true, 17);
            BVHBone RightHandThumb2 = new BVHBone(RightHandThumb1, NeuronBones.RightHandThumb2.ToString(), 3, TransAxis.None, true, 18);
            BVHBone RightHandThumb3 = new BVHBone(RightHandThumb2, NeuronBones.RightHandThumb3.ToString(), 0, TransAxis.None, true, 19);
            BVHBone RightInHandIndex = new BVHBone(RightHand, NeuronBones.RightInHandIndex.ToString(), 3, TransAxis.None, true, 20);
            BVHBone RightHandIndex1 = new BVHBone(RightInHandIndex, NeuronBones.RightHandIndex1.ToString(), 3, TransAxis.None, true, 21);
            BVHBone RightHandIndex2 = new BVHBone(RightHandIndex1, NeuronBones.RightHandIndex2.ToString(), 3, TransAxis.None, true, 22);
            BVHBone RightHandIndex3 = new BVHBone(RightHandIndex2, NeuronBones.RightHandIndex3.ToString(), 0, TransAxis.None, true, 23);
            BVHBone RightInHandMiddle = new BVHBone(RightHand, NeuronBones.RightInHandMiddle.ToString(), 3, TransAxis.None, true, 24);
            BVHBone RightHandMiddle1 = new BVHBone(RightInHandMiddle, NeuronBones.RightHandMiddle1.ToString(), 3, TransAxis.None, true, 25);
            BVHBone RightHandMiddle2 = new BVHBone(RightHandMiddle1, NeuronBones.RightHandMiddle2.ToString(), 3, TransAxis.None, true, 26);
            BVHBone RightHandMiddle3 = new BVHBone(RightHandMiddle2, NeuronBones.RightHandMiddle3.ToString(), 0, TransAxis.None, true, 27);
            BVHBone RightInHandRing = new BVHBone(RightHand, NeuronBones.RightInHandRing.ToString(), 3, TransAxis.None, true, 28);
            BVHBone RightHandRing1 = new BVHBone(RightInHandRing, NeuronBones.RightHandRing1.ToString(), 3, TransAxis.None, true, 29);
            BVHBone RightHandRing2 = new BVHBone(RightHandRing1, NeuronBones.RightHandRing2.ToString(), 3, TransAxis.None, true, 30);
            BVHBone RightHandRing3 = new BVHBone(RightHandRing2, NeuronBones.RightHandRing3.ToString(), 0, TransAxis.None, true, 31);
            BVHBone RightInHandPinky = new BVHBone(RightHand, NeuronBones.RightInHandPinky.ToString(), 3, TransAxis.None, true, 32);
            BVHBone RightHandPinky1 = new BVHBone(RightInHandPinky, NeuronBones.RightHandPinky1.ToString(), 3, TransAxis.None, true, 33);
            BVHBone RightHandPinky2 = new BVHBone(RightHandPinky1, NeuronBones.RightHandPinky2.ToString(), 3, TransAxis.None, true, 34);
            BVHBone RightHandPinky3 = new BVHBone(RightHandPinky2, NeuronBones.RightHandPinky3.ToString(), 0, TransAxis.None, true, 35);

            BVHBone LeftShoulder = new BVHBone(Spine3, NeuronBones.LeftShoulder.ToString(), 3, TransAxis.None, true, 36);
            BVHBone LeftArm = new BVHBone(LeftShoulder, NeuronBones.LeftArm.ToString(), 3, TransAxis.None, true, 37);
            BVHBone LeftForeArm = new BVHBone(LeftArm, NeuronBones.LeftForeArm.ToString(), 3, TransAxis.None, true, 38);
            BVHBone LeftHand = new BVHBone(LeftForeArm, NeuronBones.LeftHand.ToString(), 3, TransAxis.None, true, 39);
            BVHBone LeftHandThumb1 = new BVHBone(LeftHand, NeuronBones.LeftHandThumb1.ToString(), 3, TransAxis.None, true, 40);
            BVHBone LeftHandThumb2 = new BVHBone(LeftHandThumb1, NeuronBones.LeftHandThumb2.ToString(), 3, TransAxis.None, true, 41);
            BVHBone LeftHandThumb3 = new BVHBone(LeftHandThumb2, NeuronBones.LeftHandThumb3.ToString(), 0, TransAxis.None, true, 42);
            BVHBone LeftInHandIndex = new BVHBone(LeftHand, NeuronBones.LeftInHandIndex.ToString(), 3, TransAxis.None, true, 43);
            BVHBone LeftHandIndex1 = new BVHBone(LeftInHandIndex, NeuronBones.LeftHandIndex1.ToString(), 3, TransAxis.None, true, 44);
            BVHBone LeftHandIndex2 = new BVHBone(LeftHandIndex1, NeuronBones.LeftHandIndex2.ToString(), 3, TransAxis.None, true, 45);
            BVHBone LeftHandIndex3 = new BVHBone(LeftHandIndex2, NeuronBones.LeftHandIndex3.ToString(), 0, TransAxis.None, true, 46);
            BVHBone LeftInHandMiddle = new BVHBone(LeftHand, NeuronBones.LeftInHandMiddle.ToString(), 3, TransAxis.None, true, 47);
            BVHBone LeftHandMiddle1 = new BVHBone(LeftInHandMiddle, NeuronBones.LeftHandMiddle1.ToString(), 3, TransAxis.None, true, 48);
            BVHBone LeftHandMiddle2 = new BVHBone(LeftHandMiddle1, NeuronBones.LeftHandMiddle2.ToString(), 3, TransAxis.None, true, 49);
            BVHBone LeftHandMiddle3 = new BVHBone(LeftHandMiddle2, NeuronBones.LeftHandMiddle3.ToString(), 0, TransAxis.None, true, 50);
            BVHBone LeftInHandRing = new BVHBone(LeftHand, NeuronBones.LeftInHandRing.ToString(), 3, TransAxis.None, true, 51);
            BVHBone LeftHandRing1 = new BVHBone(LeftInHandRing, NeuronBones.LeftHandRing1.ToString(), 3, TransAxis.None, true, 52);
            BVHBone LeftHandRing2 = new BVHBone(LeftHandRing1, NeuronBones.LeftHandRing2.ToString(), 3, TransAxis.None, true, 53);
            BVHBone LeftHandRing3 = new BVHBone(LeftHandRing2, NeuronBones.LeftHandRing3.ToString(), 0, TransAxis.None, true, 54);
            BVHBone LeftInHandPinky = new BVHBone(LeftHand, NeuronBones.LeftInHandPinky.ToString(), 3, TransAxis.None, true, 55);
            BVHBone LeftHandPinky1 = new BVHBone(LeftInHandPinky, NeuronBones.LeftHandPinky1.ToString(), 3, TransAxis.None, true, 56);
            BVHBone LeftHandPinky2 = new BVHBone(LeftHandPinky1, NeuronBones.LeftHandPinky2.ToString(), 3, TransAxis.None, true, 57);
            BVHBone LeftHandPinky3 = new BVHBone(LeftHandPinky2, NeuronBones.LeftHandPinky3.ToString(), 0, TransAxis.None, true, 58);


            Skeleton.AddBone(Hips);             // 0
            Skeleton.AddBone(RightUpLeg);       // 1
            Skeleton.AddBone(RightLeg);         // 2
            Skeleton.AddBone(RightFoot);        // 3
            Skeleton.AddBone(LeftUpLeg);        // 4
            Skeleton.AddBone(LeftLeg);          // 5
            Skeleton.AddBone(LeftFoot);         // 6
            Skeleton.AddBone(Spine);            // 7
            Skeleton.AddBone(Spine1);           // 8
            Skeleton.AddBone(Spine2);           // 9
            Skeleton.AddBone(Spine3);           // 10
            Skeleton.AddBone(Neck);             // 11
            Skeleton.AddBone(Head);             // 12
            Skeleton.AddBone(RightShoulder);    // 13
            Skeleton.AddBone(RightArm);         // 14
            Skeleton.AddBone(RightForeArm);     // 15
            Skeleton.AddBone(RightHand);        // 16
            Skeleton.AddBone(RightHandThumb1);  // 17
            Skeleton.AddBone(RightHandThumb2);  // 18
            Skeleton.AddBone(RightHandThumb3);  // 19
            Skeleton.AddBone(RightInHandIndex); // 20
            Skeleton.AddBone(RightHandIndex1);  // 21
            Skeleton.AddBone(RightHandIndex2);  // 22
            Skeleton.AddBone(RightHandIndex3);  // 23
            Skeleton.AddBone(RightInHandMiddle);// 24
            Skeleton.AddBone(RightHandMiddle1); // 25
            Skeleton.AddBone(RightHandMiddle2); // 26
            Skeleton.AddBone(RightHandMiddle3); // 27
            Skeleton.AddBone(RightInHandRing);  // 28
            Skeleton.AddBone(RightHandRing1);   // 29
            Skeleton.AddBone(RightHandRing2);   // 30
            Skeleton.AddBone(RightHandRing3);   // 31
            Skeleton.AddBone(RightInHandPinky); // 32
            Skeleton.AddBone(RightHandPinky1);  // 33
            Skeleton.AddBone(RightHandPinky2);  // 34
            Skeleton.AddBone(RightHandPinky3);  // 35
            Skeleton.AddBone(LeftShoulder);     // 36
            Skeleton.AddBone(LeftArm);          // 37
            Skeleton.AddBone(LeftForeArm);      // 38
            Skeleton.AddBone(LeftHand);         // 39
            Skeleton.AddBone(LeftHandThumb1);   // 40
            Skeleton.AddBone(LeftHandThumb2);   // 41
            Skeleton.AddBone(LeftHandThumb3);   // 42
            Skeleton.AddBone(LeftInHandIndex);  // 43
            Skeleton.AddBone(LeftHandIndex1);   // 44
            Skeleton.AddBone(LeftHandIndex2);   // 45
            Skeleton.AddBone(LeftHandIndex3);   // 46
            Skeleton.AddBone(LeftInHandMiddle); // 47
            Skeleton.AddBone(LeftHandMiddle1);  // 48
            Skeleton.AddBone(LeftHandMiddle2);  // 49
            Skeleton.AddBone(LeftHandMiddle3);  // 50
            Skeleton.AddBone(LeftInHandRing);   // 51
            Skeleton.AddBone(LeftHandRing1);    // 52
            Skeleton.AddBone(LeftHandRing2);    // 53
            Skeleton.AddBone(LeftHandRing3);    // 54
            Skeleton.AddBone(LeftInHandPinky);  // 55
            Skeleton.AddBone(LeftHandPinky1);   // 56
            Skeleton.AddBone(LeftHandPinky2);   // 57
            Skeleton.AddBone(LeftHandPinky3);   // 58

            Skeleton.FinalizeBVHSkeleton();
        }

    }
}