using UnityEngine;

namespace Outlines
{
    [RequireComponent(typeof(Outline))]
    public class Highlight : MonoBehaviour
    {
        public HighlightBus Bus;
        private Outline _target;

        private void Awake()
        {
            _target = GetComponent<Outline>();
            _target.enabled = false;
            Bus.OnActivate.AddListener(t => ActivateOutline());
            Bus.OnDeactivate.AddListener(t => DeactivateOutline());
        }

        private void ActivateOutline()
        {
            _target.enabled = true;
        }
    
        private void DeactivateOutline()
        {
            _target.enabled = false;
        }
    }
}