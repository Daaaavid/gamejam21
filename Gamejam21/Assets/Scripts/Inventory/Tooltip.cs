using Interaction;
using TMPro;
using UnityEngine;

namespace Inventory
{
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI TmpTooltip;
        
        public void Show(InteractableObject obj)
        {
            TmpTooltip.text = obj.TextOnHover;

        }
    }
}