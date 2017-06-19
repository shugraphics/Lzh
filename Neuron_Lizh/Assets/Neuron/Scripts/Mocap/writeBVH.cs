using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using UnityEngine;

using Neuron;


namespace NeuronBVH 
{
    class writeBVH
    {
        private bool recording = false;
        StreamWriter file;
        private bool initializing = false;
        public int intializingCounter = 0;
        string fileName;
        //Stopwatch sw = new Stopwatch(); 

        private int frameCounter = 0;
        private double avgFrameRate = 0;
        private double elapsedTimeSec = 0;

        BVHSkeleton bvhSkeleton = new BVHSkeleton();
        BVHSkeleton bvhSkeletonWritten = new BVHSkeleton();
        double[,] tempOffsetMatrix;
        double[] tempMotionVektor;

        Quaternion[] baseRotations;

        public writeBVH(string fileName)
        {
            fileName = fileName + ".bvh";
            this.fileName = fileName;
            NeuronSkeletonBVH.AddNeuronSkeleton(bvhSkeleton);
            initializing = true;
            tempOffsetMatrix = new double[3, bvhSkeleton.Bones.Count];
            tempMotionVektor = new double[bvhSkeleton.Channels];

            if (File.Exists(fileName))
                File.Delete(fileName);
            file = File.CreateText(fileName);
            file.WriteLine("HIERARCHY");
            recording = true; 
        }

        public void closeBVHFile()
        {
            //sw.Stop(); // Aufnahme beendet
            file.Flush();
            file.Close();
            string text = File.ReadAllText(fileName);
            text = text.Replace("PLATZHALTERFRAMES", frameCounter.ToString());
            File.WriteAllText(fileName, text);

            recording = false;
        }

        public bool isRecording
        {
            get { return recording; }
        }

        public bool isInitializing
        {
            get { return initializing; }
        }

        //eigentliche Schreibarbeit:
        public void Entry(Transform[] trans)
        {
            /*Vector3[] initPos = new Vector3[(int)NeuronBones.NumOfBones];
             initPos[0].x = 0.34f;                  initPos[0].y = 1.11f;                  initPos[0].z = -0.01f;
             initPos[1].x = 0.1055315f;             initPos[1].y = -0.0880629f;            initPos[1].z = -0.008583521f;
             initPos[2].x =0.01200404f ;            initPos[2].y = -0.4565072f;            initPos[2].z =-0.01516209f ;
             initPos[3].x =-0.004437618f;           initPos[3].y =-0.4671162f ;            initPos[3].z =-0.002509034f;
             initPos[4].x = -0.1055315f;            initPos[4].y = -0.0880629f;            initPos[4].z = -0.008583521f; 
             initPos[5].x =-0.01200404f ;           initPos[5].y =-0.4565072f ;            initPos[5].z =-0.01516209f ;
             initPos[6].x =0.004437618f ;           initPos[6].y =-0.4671162f ;            initPos[6].z =-0.002509034f ;
             initPos[7].x =1.768458e-15f ;          initPos[7].y =0.09609298f ;            initPos[7].z =2.384186e-09f ;
             initPos[8].x =-1.984596e-15f ;         initPos[8].y =0.1125818f ;             initPos[8].z =0 ;
             initPos[9].x =4.052751e-16f ;          initPos[9].y =0.1203307f ;             initPos[9].z =0 ;
             initPos[10].x =-1.52742e-16f ;         initPos[10].y =0.1172307f ;            initPos[10].z =0.01248512f ;
             initPos[11].x =1.187939e-16f ;         initPos[11].y =0.1207016f ;            initPos[11].z =-0.01248512f ;
             initPos[12].x = -3.118321e-16f;        initPos[12].y = 0.1000406f;            initPos[12].z = 0;
             initPos[13].x =0.0490494f ;            initPos[13].y =0.04062805f ;           initPos[13].z =-0.0124851f ;
             initPos[14].x =0.1564886f ;            initPos[14].y =0 ;                     initPos[14].z =0 ;
             initPos[15].x =0.25876f;               initPos[15].y =0;                      initPos[15].z =0 ;
             initPos[16].x =0.2840199f ;            initPos[16].y =0 ;                     initPos[16].z =0 ;
             initPos[17].x =0.033787f ;             initPos[17].y =0.002580109f ;          initPos[17].z =0.042356f ;
             initPos[18].x =0.0499884f ;            initPos[18].y =-5.767822e-05f ;        initPos[18].z =-0.0002239871f ;
             initPos[19].x =0.03472366f ;           initPos[19].y =2.868652e-05f ;         initPos[19].z =-0.0002020526f ;
             initPos[20].x =0.04375305f ;           initPos[20].y =0.006900177f ;          initPos[20].z =0.0268534f ;
             initPos[21].x =0.07086907f ;           initPos[21].y =-0.001240082f ;         initPos[21].z =0.01356471f ;
             initPos[22].x =0.04906219f ;           initPos[22].y =-0.002389984f ;         initPos[22].z =8.839607e-05f ;
             initPos[23].x =0.02779404f ;           initPos[23].y =-0.001730041f ;         initPos[23].z =-1.251936e-05f ;
             initPos[24].x =0.04592308f ;           initPos[24].y =0.007020111f ;          initPos[24].z =0.01027932f ;
             initPos[25].x =0.07022797f ;           initPos[25].y =-0.001139831f ;         initPos[25].z =0.004263851f ;
             initPos[26].x =0.05348282f ;           initPos[26].y =-0.003639984f ;         initPos[26].z =-0.0001398747f ;
             initPos[27].x =0.03349342f ;           initPos[27].y =-0.002590179f ;         initPos[27].z =8.589432e-05f ;
             initPos[28].x =0.04567207f ;           initPos[28].y =0.007350158f ;          initPos[28].z =-0.0017598f ;
             initPos[29].x =0.06295501f ;           initPos[29].y =-0.0002999878f ;        initPos[29].z =-0.0065212f ;
             initPos[30].x =0.0465126f ;            initPos[30].y =-0.004377746f ;         initPos[30].z = 0.0001261163f;
             initPos[31].x =0.03228988f ;           initPos[31].y =-0.002809296f ;         initPos[31].z =-0.0001548386f ;
             initPos[32].x =0.04291298f ;           initPos[32].y =0.006370087f ;          initPos[32].z =-0.016307f ;
             initPos[33].x =0.05618301f ;           initPos[33].y =-0.0003599548f ;        initPos[33].z =-0.01479811f ;
             initPos[34].x =0.03736191f ;           initPos[34].y =-0.002039032f ;         initPos[34].z =-0.00017241f ;
             initPos[35].x =0.02357193f ;           initPos[35].y =-0.001777496f ;         initPos[35].z =9.027958e-05f ;
             initPos[36].x =-0.04904938f ;          initPos[36].y =0.0406308f ;            initPos[36].z =-0.01248512f ;
             initPos[37].x =-0.1564884f ;           initPos[37].y =0 ;                     initPos[37].z =0 ;
             initPos[38].x =-0.2587603f ;           initPos[38].y =0 ;                     initPos[38].z =0 ;
             initPos[39].x =-0.2840203f ;           initPos[39].y =0 ;                     initPos[39].z =0 ;
             initPos[40].x =-0.0337867f ;           initPos[40].y =0.002574768f ;          initPos[40].z =0.04235597f ;
             initPos[41].x =-0.04998795f ;          initPos[41].y =0.0001268005f ;         initPos[41].z =-0.0001256585f ;
             initPos[42].x =-0.03472481f ;          initPos[42].y =-7.965088e-05f ;        initPos[42].z =0.0001012206f ;
             initPos[43].x =-0.04375259f ;          initPos[43].y =0.006892395f ;          initPos[43].z =0.02685343f ;
             initPos[44].x =-0.07086868f ;          initPos[44].y =-0.001236572f ;         initPos[44].z =0.01356473f ;
             initPos[45].x =-0.04906235f ;          initPos[45].y =-0.002390747f ;         initPos[45].z =-0.0001231909f ;
             initPos[46].x =-0.02779373f ;          initPos[46].y =-0.001731262f ;         initPos[46].z =0.0001098132f ;
             initPos[47].x =-0.045923f ;            initPos[47].y =0.007020568f ;          initPos[47].z =0.01027935f ;
             initPos[48].x =-0.07022827f ;          initPos[48].y =-0.001141663f ;         initPos[48].z =0.004263849f ;
             initPos[49].x =-0.05348198f ;          initPos[49].y =-0.003643341f ;         initPos[49].z =-0.0001567896f ;
             initPos[50].x =-0.0334935f ;           initPos[50].y =-0.00258728f ;          initPos[50].z =0.0001428795f ;
             initPos[51].x =-0.04567215f ;          initPos[51].y =0.007352142f ;          initPos[51].z =-0.001759753f ;
             initPos[52].x =-0.06295448f ;          initPos[52].y =-0.0003025818f ;        initPos[52].z =-0.006521178f ;
             initPos[53].x =-0.04658203f ;          initPos[53].y =-0.003573151f ;         initPos[53].z =2.321959e-05f ;
             initPos[54].x =-0.03232788f ;          initPos[54].y =-0.002325134f ;         initPos[54].z =1.880646e-05f ;
             initPos[55].x =-0.04291321f ;          initPos[55].y =0.006371307f ;          initPos[55].z =-0.016307f ;
             initPos[56].x =-0.05618248f ;          initPos[56].y =-0.0003662109f ;        initPos[56].z =-0.01479807f ;
             initPos[57].x =-0.03736122f ;          initPos[57].y =-0.002046814f ;         initPos[57].z =0.0001068497f ;
             initPos[58].x = -0.02357231f;          initPos[58].y = -0.00177948f;          initPos[58].z = -0.0001187754f;*/

            /*string prefix = "Robot_";
            for (int k = 0; k < bvhSkeleton.Bones.Count; k++)
            {
                if (trans[k].name.StartsWith(prefix))
                {
                    string bone_name = trans[k].name.Substring(trans[k].name.IndexOf(prefix) + prefix.Length);
                    int index = NeuronHelper.GetBoneIndex(bone_name);
                    if (index >= 0)
                    {
                        bvhSkeleton.Bones[k].Index = index;
                        UnityEngine.Debug.Log(index);
                    }
                }
            }*/

                baseRotations = new Quaternion[bvhSkeleton.Bones.Count];//

                //for (int k = 0; k < bvhSkeleton.Bones.Count; k++)
                //{
                //    trans[k].rotation = Quaternion.identity;
                //}
                
                for (int k = 0; k < bvhSkeleton.Bones.Count; k++)
                {
                    baseRotations[k] = trans[k].localRotation;
                    //double[] bonevector = KinectSkeletonBVH.getBoneVectorOutofJointPosition(bvhSkeleton.Bones[k], skel);
                    //Vector3 bonevector = trans[k].localPosition;
                    //trans[k].tr

                    Vector3 bonevector;//myPos, parentPos, 

                    if (k == 0)
                    {
                        bonevector.x = 0.0f;
                        bonevector.y = 0.0f;
                        bonevector.z = 0.0f;
                    }
                    else 
                    {
                        //myPos = trans[k].position;
                        //parentPos = trans[bvhSkeleton.Bones[k].Parent.Index].position;
                        //bonevector = myPos - parentPos;

                        bonevector = trans[k].localPosition;

                        //myPos = initPos[k];
                        //parentPos = initPos[bvhSkeleton.Bones[k].Parent.Index];
                        //bonevector = myPos - parentPos;
                        //bonevector = initPos[k];
                    }

                    tempOffsetMatrix[0, k] = Math.Round(-bonevector.x * 30, 2);// Math.Round(bonevector.x * 100, 2);
                    tempOffsetMatrix[1, k] = Math.Round(bonevector.y * 30, 2);
                    tempOffsetMatrix[2, k] = Math.Round(bonevector.z * 30, 2);
                }     
        }

        public void startWritingEntry()
        {          
            for (int k = 0; k < bvhSkeleton.Bones.Count; k++)
            {
                //double length = Math.Sqrt(Math.Pow(Math.Round(tempOffsetMatrix[0, k] , 5),2) + Math.Pow(Math.Round(tempOffsetMatrix[1, k] , 5),2) + Math.Pow(Math.Round(tempOffsetMatrix[2, k] , 5),2));  
                double length = Math.Max(Math.Abs(tempOffsetMatrix[0, k]), Math.Abs(tempOffsetMatrix[1, k]));
                length = Math.Max(length, Math.Abs(tempOffsetMatrix[2, k]));
                length = Math.Round(length, 2);

                switch(bvhSkeleton.Bones[k].Axis)
                {
                        case TransAxis.X :
                        bvhSkeleton.Bones[k].setTransOffset(length, 0, 0);
                        break;
                        case TransAxis.Y :
                        bvhSkeleton.Bones[k].setTransOffset(0, length, 0);
                        break;
                        case TransAxis.Z :
                        bvhSkeleton.Bones[k].setTransOffset(0, 0, length);
                        break;
                        case TransAxis.nX :
                        bvhSkeleton.Bones[k].setTransOffset(-length, 0, 0);
                        break;
                        case TransAxis.nY :
                        bvhSkeleton.Bones[k].setTransOffset(0, -length, 0);
                        break;
                        case TransAxis.nZ :
                        bvhSkeleton.Bones[k].setTransOffset(0, 0, -length);
                        break;

                    default :
                        bvhSkeleton.Bones[k].setTransOffset(tempOffsetMatrix[0, k], tempOffsetMatrix[1, k], tempOffsetMatrix[2, k]);
                        break;
                

                }      
            }      

            this.initializing = false;
            writeEntry();
            file.Flush();
        }

        private void writeEntry()
        {
            List<List<BVHBone>> bonesListList = new List<List<BVHBone>>();
            List<BVHBone> resultList;

            while (bvhSkeleton.Bones.Count != 0)
            {
                if (bvhSkeletonWritten.Bones.Count == 0)
                {
                    resultList = bvhSkeleton.Bones.FindAll(i => i.Root == true);
                    bonesListList.Add(resultList);
                }
                else
                {
                    if (bvhSkeletonWritten.Bones.Last().End == false)
                    {
                        for (int k = 1; k <= bvhSkeletonWritten.Bones.Count; k++)
                        {
                            resultList = bvhSkeletonWritten.Bones[bvhSkeletonWritten.Bones.Count - k].Children;
                            if (resultList.Count != 0)
                            {
                                bonesListList.Add(resultList);
                                break;
                            }
                        }
                    }
                }

                BVHBone currentBone = bonesListList.Last().First();
                string tabs = calcTabs(currentBone);
                if (currentBone.Root == true)
                    file.WriteLine("ROOT " + currentBone.Name);
                else if (currentBone.End == true)
                    file.WriteLine(tabs + "End Site");
                else
                    file.WriteLine(tabs + "JOINT " + currentBone.Name);

                file.WriteLine(tabs + "{");
                file.WriteLine(tabs + "\tOFFSET " + currentBone.translOffset[0].ToString().Replace(",", ".") +
                    " " + currentBone.translOffset[1].ToString().Replace(",", ".") +
                    " " + currentBone.translOffset[2].ToString().Replace(",", "."));

                if (currentBone.End == true)
                {
                    while (bonesListList.Count != 0 && bonesListList.Last().Count == 1)
                    {
                        tabs = calcTabs(bonesListList.Last()[0]);
                        foreach (List<BVHBone> liste in bonesListList)
                        {
                            if (liste.Contains(bonesListList.Last()[0]))
                            {
                                liste.Remove(bonesListList.Last()[0]);
                            }
                        }
                        bonesListList.Remove(bonesListList.Last());
                        file.WriteLine(tabs + "}");
                    }

                    if (bonesListList.Count != 0)
                    {
                        if (bonesListList.Last().Count != 0)
                        {
                            bonesListList.Last().Remove(bonesListList.Last()[0]);
                        }
                        else
                        {
                            bonesListList.Remove(bonesListList.Last());
                        }
                        tabs = calcTabs(bonesListList.Last()[0]);
                        file.WriteLine(tabs + "}");
                    }
                }
                else
                {
                    file.WriteLine(tabs + "\t" + writeChannels(currentBone));
                }
                bvhSkeleton.Bones.Remove(currentBone);
                bvhSkeletonWritten.AddBone(currentBone);
            }
            bvhSkeletonWritten.copyParameters(bvhSkeleton);
        }

        public void Motion(Transform[] trans, Quaternion[] transInit)
        {
            //sw.Start(); //Aufnahme der Bewegungen beginnt

            for (int k = 0; k < bvhSkeletonWritten.Bones.Count; k++)
            {
                //UnityEngine.Debug.Log(string.Format("{0}-{1}-{2}", k, trans[k].name, bvhSkeletonWritten.Bones[k].Name));

                if (bvhSkeletonWritten.Bones[k].End == false)
                {
                    Quaternion rotTmp = trans[k].localRotation;// *Quaternion.Inverse(baseRotations[k]);
                    rotTmp.w *= -1;
                    Vector3 degVec = rotTmp.eulerAngles;//Vector3 degVec = trans[k].localRotation.eulerAngles;

                    int indexOffset = 0;
                    if (bvhSkeletonWritten.Bones[k].Root == true)
                    {
                        indexOffset = 3;
                    }

                    tempMotionVektor[bvhSkeletonWritten.Bones[k].MotionSpace + indexOffset] = degVec.z;//degVec.x;
                    tempMotionVektor[bvhSkeletonWritten.Bones[k].MotionSpace + 1 + indexOffset] = -degVec.x;//degVec.y;
                    tempMotionVektor[bvhSkeletonWritten.Bones[k].MotionSpace + 2 + indexOffset] = degVec.y;//degVec.z;
                }

            }
            //UnityEngine.Debug.Log(string.Format("transInit[1] {0}: {1}: {2}", transInit[1].eulerAngles.x, transInit[1].eulerAngles.y, transInit[1].eulerAngles.z));
            //UnityEngine.Debug.Log(string.Format("trans[1]- {0}: {1}: {2}", trans[1].localRotation.eulerAngles.x, trans[1].localRotation.eulerAngles.y, trans[1].localRotation.eulerAngles.z));
            //Root Bewegung
            tempMotionVektor[0] = -trans[0].position.x * 30;//trans[0].localPosition.x;
            tempMotionVektor[1] = trans[0].position.y * 30;//trans[0].localPosition.y;
            tempMotionVektor[2] = trans[0].position.z * 30;//trans[0].localPosition.z;

            writeMotion(tempMotionVektor);
            file.Flush();

            //elapsedTimeSec =  Math.Round(Convert.ToDouble(sw.ElapsedMilliseconds) / 1000,2);
        }

        private void writeMotion(double[] tempMotionVektor)
        {
            string motionStringValues = "";

            if (frameCounter == 0)
            {
                file.WriteLine("MOTION");
                file.WriteLine("Frames: PLATZHALTERFRAMES");
                file.WriteLine("Frame Time: 0.0333333");
            }
            foreach (var i in tempMotionVektor)
            {
                motionStringValues += (Math.Round(i, 4).ToString().Replace(",", ".") + " ");
            }

            file.WriteLine(motionStringValues);

            frameCounter++;
        }

        private string writeChannels(BVHBone bone)
        {
            string output = "CHANNELS " + bone.Channels.Length.ToString() + " ";

            for (int k = 0; k < bone.Channels.Length; k++)
            {
                output += bone.Channels[k].ToString() + " ";

            }
            return output;
        }

        private string calcTabs(BVHBone currentBone)
        {
            int depth = currentBone.Depth;
            string tabs = "";
            for (int k = 0; k < currentBone.Depth; k++)
            {
                tabs += "\t";
            }
            return tabs;
        }
    }
}
