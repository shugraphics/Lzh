using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class BVHExportMenu : MonoBehaviour
{

    [MenuItem("Assets/Export BVH Animation...")]
    static void ExportBVHAnimation()
    {
        Transform character = Selection.activeTransform;

        String filename = EditorUtility.SaveFilePanel("Export skeleton to BVH", "", character.root.name + ".bvh", "bvh");

        if (filename.Length > 0)
        {
            CharacterToFile(Selection.activeTransform, filename);
        }
    }

    // Validate the menu item defined by the function above.
    // The menu item will be disabled if this function returns false.
    [MenuItem("Assets/Export BVH Animation...", true)]
    static bool ValidateExportBVHAnimation()
    {
        // Return false if no transform is selected.
        return Selection.activeTransform != null;
    }

    public static float globalScale = 1f;
    private static Dictionary<String, Quaternion> baseRotations;

    private static String ValidString(String original)
    {
        return original;
    }

    private static String BoneToString(Transform bone, int inset)
    {
        baseRotations.Add(bone.name, bone.localRotation);

        StringBuilder sb = new StringBuilder();
        String tabs = String.Empty;
        for (int i = 0; i < inset; i++)
        {
            tabs = tabs + "\t";
        }

        sb.Append(tabs); sb.Append("JOINT "); sb.AppendLine(ValidString(bone.name));
        sb.Append(tabs); sb.AppendLine("{");

#if (POSE_CORRECT)
        // This worked for the original bad edit, good pose
        Vector3 offset = bone.localPosition * globalScale;
#else
        // This works for edit mode, but breaks poses
        Vector3 offset = (bone.position - bone.parent.position) * globalScale;
#endif

        sb.Append(tabs); sb.AppendFormat("\tOFFSET {0:0.000000} {1:0.000000} {2:0.000000}", -offset.x, offset.y, offset.z); sb.AppendLine();
        sb.Append(tabs); sb.AppendLine("\tCHANNELS 3 Zrotation Xrotation Yrotation");
        foreach (Transform child in bone)
        {
            sb.Append(BoneToString(child, inset + 1));
        }
        // Attach a minimal end site so the skeleton stays tidy
        if (bone.childCount == 0)
        {
#if (POSE_CORRECT)
            // This worked for the original bad edit, good pose
            Vector3 end = Vector3.left * bone.localPosition.magnitude * globalScale * 0.5f;
#else
            // This works for edit mode, but breaks poses
            Vector3 end = bone.rotation * Vector3.left * bone.localPosition.magnitude * 0.5f * globalScale;
#endif
            sb.Append(tabs); sb.AppendLine("\tEnd Site");
            sb.Append(tabs); sb.AppendLine("\t{");
            sb.Append(tabs); sb.AppendFormat("\t\tOFFSET {0:0.000000} {1:0.000000} {2:0.000000}", -end.x, end.y, end.z); sb.AppendLine();
            sb.Append(tabs); sb.AppendLine("\t}");
        }
        sb.Append(tabs); sb.AppendLine("}");

        return sb.ToString();
    }

    public static String SkeletonToString(Transform skeleton)
    {
        ZeroBones(skeleton);

        baseRotations = new Dictionary<String, Quaternion>();
        baseRotations.Add(skeleton.name, skeleton.localRotation);

        StringBuilder sb = new StringBuilder();

        sb.Append("ROOT "); sb.AppendLine(ValidString(skeleton.name));
        sb.AppendLine("{");

#if (POSE_CORRECT)
        // This worked for the original bad edit, good pose
        Vector3 offset = skeleton.position * globalScale;
#else
        // This works for edit mode, but breaks poses
        Vector3 offset = (skeleton.position - skeleton.parent.position) * globalScale;
#endif
        sb.AppendFormat("\tOFFSET {0:0.000000} {1:0.000000} {2:0.000000}", -offset.x, offset.y, offset.z); sb.AppendLine();
        sb.AppendLine("\tCHANNELS 6 Xposition Yposition Zposition Zrotation Xrotation Yrotation");

        foreach (Transform child in skeleton)
        {
            sb.Append(BoneToString(child, 1));
        }
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static String BoneRotationToString(Transform bone)
    {
        StringBuilder sb = new StringBuilder();

#if (POSE_CORRECT)
        // This worked for the original bad edit, good pose
        Quaternion rotation = bone.localRotation;
        rotation.w *= -1;
        sb.AppendFormat("{0:0.000000} {1:0.000000} {2:0.000000} ", rotation.eulerAngles.z, -rotation.eulerAngles.x, rotation.eulerAngles.y);
#else
        // This is broken
        Quaternion rotation = bone.localRotation * Quaternion.Inverse(baseRotations[bone.name]);
        rotation.w *= -1;
        sb.AppendFormat("{0:0.000000} {1:0.000000} {2:0.000000} ", rotation.eulerAngles.z, -rotation.eulerAngles.x, rotation.eulerAngles.y);
#endif

        foreach (Transform child in bone)
        {
            sb.Append(BoneRotationToString(child));
        }

        return sb.ToString();
    }

    public static String PoseToString(Transform skeleton)
    {
        StringBuilder sb = new StringBuilder();

#if (POSE_CORRECT)
        // This worked for the original bad edit, good pose
        Vector3 position = skeleton.localPosition * globalScale;
#else
        // This works for edit mode, but breaks poses
        Vector3 position = (skeleton.position - skeleton.parent.position) * globalScale;
#endif

        sb.AppendFormat("{0:0.000000} {1:0.000000} {2:0.000000} ", -position.x, position.y, position.z);
        sb.Append(BoneRotationToString(skeleton));
        sb.AppendLine();

        return sb.ToString();
    }

    public static String CharacterToString(Transform skeleton)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("HIERARCHY");
        sb.Append(SkeletonToString(skeleton));

        sb.AppendLine("MOTION");
        Animation anim = skeleton.root.GetComponent<Animation>();
        AnimationClip clip = anim.clip;
        float step = 1f / clip.frameRate;
        int frames = Mathf.CeilToInt(clip.length / step);
        anim[clip.name].enabled = true;
        anim[clip.name].weight = 1f;
        anim[clip.name].time = 0f;
        anim.Sample();

        sb.Append("Frames: "); sb.Append(frames); sb.AppendLine();
        sb.AppendFormat("Frame Time: {0:0.000000}", step); sb.AppendLine();
        for (int frame = 0; frame < frames; frame++)
        {
            sb.Append(PoseToString(skeleton));
            anim[clip.name].time += step;
            anim.Sample();
        }

        return sb.ToString();
    }

    public static void CharacterToFile(Transform skeleton, String filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(CharacterToString(skeleton));
        }
    }

    static void ZeroBones(Transform bone)
    {
        bone.transform.rotation = Quaternion.identity;

        foreach (Transform child in bone)
        {
            if (child.GetComponents<Component>().Length == 1) // only affect bones (they will only have a transform component)
            {
                ZeroBones(child);
            }
        }
    }
}
