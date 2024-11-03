#if INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using InputGlyphs.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InputGlyphs.Display
{
    public class InputGlyphImage : UIBehaviour, ILayoutElement, IGlyphDisplay
    {
        public bool IsVisible => Image != null && Image.isActiveAndEnabled;

        [SerializeField]
        public Image Image = null;

        [SerializeField]
        public InputActionReference InputActionReference = null;

        [SerializeField]
        public GlyphsLayoutData GlyphsLayoutData = GlyphsLayoutData.Default;

        private Vector2 _defaultSizeDelta;
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
            _defaultSizeDelta = Image.rectTransform.sizeDelta;
            _texture = new Texture2D(2, 2);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            InputGlyphDisplayBridge.Register(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            InputGlyphDisplayBridge.Unregister(this);
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

        public void UpdateGlyphs(PlayerInput playerInput)
        {
            if (!playerInput.isActiveAndEnabled)
            {
                return;
            }

            var devices = playerInput.devices;
            if (devices.Count == 0)
            {
                Debug.LogWarning("No devices are connected.", this);
                return;
            }

            var controlScheme = playerInput.currentControlScheme;

            if (InputLayoutPathUtility.TryGetActionBindingPath(InputActionReference?.action, controlScheme, _pathBuffer))
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

        public virtual int layoutPriority => EnableLayoutElement ? LayoutElementPriority : -1;

        public virtual void CalculateLayoutInputHorizontal() { }

        public virtual void CalculateLayoutInputVertical() { }

        public virtual float minWidth => -1;

        public virtual float minHeight => -1;

        public virtual float preferredWidth
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

        public virtual float preferredHeight => LayoutElementSize;

        public virtual float flexibleWidth => -1;

        public virtual float flexibleHeight => -1;

        protected void SetDirty()
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
