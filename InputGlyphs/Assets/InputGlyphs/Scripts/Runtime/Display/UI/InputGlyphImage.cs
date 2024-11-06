#if INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using InputGlyphs.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InputGlyphs.Display
{
    public sealed class InputGlyphImage : UIBehaviour, ILayoutElement, IGlyphDisplay
    {
        public bool IsVisible => Image != null && Image.isActiveAndEnabled;

        public InputActionReference InputActionReference
        {
            get => inputActionReference;
            set
            {
                var shouldRefreshGlyph = inputActionReference == null && value != null || inputActionReference != null && value != null;
                inputActionReference = value;

                if (shouldRefreshGlyph)
                {
                    InputGlyphDisplayBridge.Unregister(this);
                    InputGlyphDisplayBridge.Register(this);
                }
            }
        } 

        [SerializeField]
        public Image Image = null;

        [FormerlySerializedAs("InputActionReference")] [SerializeField]
        private InputActionReference inputActionReference = null;

        [SerializeField]
        public GlyphsLayoutData GlyphsLayoutData = GlyphsLayoutData.Default;
        
        private PlayerInput _lastPlayerInput;
        private List<string> _pathBuffer = new List<string>();
        private Texture2D _texture;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            Image = GetComponent<Image>();
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            if (Image == null)
            {
                Image = GetComponent<Image>();
            }
            _texture = new Texture2D(2, 2);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (inputActionReference != null)
            {
                InputGlyphDisplayBridge.Register(this);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (inputActionReference != null)
            {
                InputGlyphDisplayBridge.Unregister(this);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(_texture);
            _texture = null;
            if (Image != null)
            {
                Image.sprite = null;
            }
        }

        public void UpdateGlyphs(ReadOnlyArray<InputDevice> devices, string controlScheme)
        {
            if (InputLayoutPathUtility.TryGetActionBindingPath(inputActionReference?.action, controlScheme, _pathBuffer))
            {
                if (DisplayGlyphTextureGenerator.GenerateGlyphTexture(_texture, devices, _pathBuffer, GlyphsLayoutData))
                {
                    Image.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), Mathf.Min(_texture.width, _texture.height));
                }
            }
        }

        //
        // ILayoutElement
        //

        [SerializeField]
        public bool EnableLayoutElement = true;

        [SerializeField]
        public int LayoutElementPriority = 1;

        [SerializeField]
        public float LayoutElementSize = 100f;

        public int layoutPriority => EnableLayoutElement ? LayoutElementPriority : -1;

        public void CalculateLayoutInputHorizontal() { }

        public void CalculateLayoutInputVertical() { }

        public float minWidth => -1;

        public float minHeight => -1;

        public float preferredWidth
        {
            get
            {
                if (Image == null || Image.sprite == null)
                {
                    return LayoutElementSize;
                }

                var ratio = (float)Image.sprite.rect.width / Image.sprite.rect.height;
                return LayoutElementSize * ratio;
            }
        }

        public float preferredHeight => LayoutElementSize;

        public float flexibleWidth => -1;

        public float flexibleHeight => -1;

        private void SetDirty()
        {
            if (!IsActive())
                return;
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }
#endif
    }
}
#endif
