using CGT.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CGT.Myceliaudio
{
    public abstract class AudioSliderComponent : MonoBehaviour
    {
        [SerializeField] protected Slider _slider;
        [SerializeField] protected TrackGroup _trackGroup;

        [Tooltip("If this is assigned, this only does its thing when steps are done")]
        [SerializeField] protected SliderStep _sliderStep;
        [Tooltip("Whether or not this should do its thing based on the SliderStep component")]
        [SerializeField] protected bool _useSliderStep;

        [Tooltip("Whether this should do its thing right away")]
        [SerializeField] protected bool _applyOnStart = false;
        [SerializeField] protected float _delayBeforeApply = 0f;

        public virtual TrackGroup TrackGroup { get { return _trackGroup; } }

        protected virtual void Start()
        {
            AlignToSystemVol();
            _prevStepVal = _slider.value;
            // ^To prevent things from activating twice in quick succession in
            // response to a single step

            if (_applyOnStart)
            {
                if (_delayBeforeApply <= 0)
                {
                    InitApply();
                }
                else
                {
                    Invoke(nameof(InitApply), _delayBeforeApply);
                }
            }
        }

        protected virtual void AlignToSystemVol()
        {
            float vol = AudioSystem.S.GetTrackGroupVol(_trackGroup);
            SyncWith(vol);
        }

        protected virtual void SyncWith(float newVol)
        {
            float percentage = newVol / 100f;
            // The formula for lerp is:
            // lerpedValue = startValue + (range * percentage)
            // range = (endValue - startValue)
            float result = Mathf.Lerp(_slider.minValue, _slider.maxValue, percentage);

            _slider.SetValueWithoutNotify(result); // To avoid stack overflows
        }

        protected float _prevStepVal;

        protected virtual void InitApply()
        {

        }

        protected virtual bool ShouldUseSliderStep { get { return _useSliderStep && _sliderStep != null; } }

        protected virtual bool IsDifferentStepValue(float newValue)
        {
            return newValue != _prevStepVal;
        }



    }
}