using System.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.SO
{
    public class ScriptableBaseEntityData : ScriptableConfig
    {
        [BoxGroup("IconBox", centerLabel: true, Order = -10)]
        [SerializeField, PreviewField(50)]
        [HorizontalGroup("IconBox/Horizontal", 50)]
        [LabelText("Icon")]
        private Sprite _icon;

        [BoxGroup("IconBox")]
        [VerticalGroup("IconBox/Horizontal/Vertical")]
        [LabelText("Entity name")] [SerializeField]
        private string _name;

        [BoxGroup("IconBox")]
        [VerticalGroup("IconBox/Horizontal/Vertical")]
        [LabelText("Entity description"), TextArea(3, 10)]
        [SerializeField]
        private string _description;
    }
}