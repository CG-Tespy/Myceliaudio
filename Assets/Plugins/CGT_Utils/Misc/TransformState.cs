using UnityEngine;

namespace CGT
{
    public class TransformState
    {
        public Transform BasedOn { get; protected set; }
        public Vector3 Position { get; protected set; }
        public Vector3 LocalPosition { get; protected set; }
        public Transform Parent { get; protected set; }
        public int SiblingIndex { get; protected set; }

        public static TransformState From(Transform tForm)
        {
            TransformState result = new TransformState();
            result.SetFrom(tForm);
            return result;
        }

        public virtual void SetFrom(Transform tForm)
        {
            BasedOn = tForm;
            Position = tForm.position;
            LocalPosition = tForm.localPosition;
            Parent = tForm.parent;
            SiblingIndex = tForm.GetSiblingIndex();
        }

        public static TransformState From(TransformState other)
        {
            // Best not just call the SetFrom(Transform) here, since the state of the transform
            // might be different from what the snapshot passed to us suggests
            TransformState result = new TransformState();
            result.BasedOn = other.BasedOn;
            result.Position = other.Position;
            result.LocalPosition = other.LocalPosition;
            result.Parent = other.Parent;
            result.SiblingIndex = other.SiblingIndex;
            return result;
        }

    }
}