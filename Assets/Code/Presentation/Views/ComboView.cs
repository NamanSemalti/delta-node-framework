using UnityEngine;
using TMPro;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;

namespace CardMatch.Presentation.Views
{
    public sealed class ComboView : MonoBehaviour
    {
        [SerializeField] private TMP_Text comboText;

        private IEventBus _eventBus;

        public void Initialize(IEventBus eventBus)
        {
            _eventBus = eventBus;
            comboText.gameObject.SetActive(false);

            _eventBus.Subscribe<ComboChanged>(OnComboChanged);
        }

        private void OnDestroy()
        {
            if (_eventBus != null)
                _eventBus.Unsubscribe<ComboChanged>(OnComboChanged);
        }

        private void OnComboChanged(ComboChanged evt)
        {
            if (evt.Combo > 1)
            {
                comboText.text = $"COMBO x{evt.Combo}";
                comboText.gameObject.SetActive(true);
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
    }
}