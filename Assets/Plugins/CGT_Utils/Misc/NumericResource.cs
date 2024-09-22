using UnityEngine;

namespace RPG
{
    public class NumericResource : MonoBehaviour, IHasNotes
    {
        [TextArea(3, 6)]
        [SerializeField] protected string notes = string.Empty;
        [SerializeField] protected double minValue = 0, maxValue = 30, currentValue = 30;

        public virtual string Notes { get { return notes; } }

        public virtual double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public virtual double CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        public virtual void ProcessAlteration(float value)
        {
            ProcessAlteration((double)value);
        }

        public virtual void ProcessAlteration(double value)
        {
            if (currentValue <= minValue)
                return;

            double newValue = currentValue + value;
            
        }

    }
}