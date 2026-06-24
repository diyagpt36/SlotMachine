using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a single reel: spinning animation, stopping on a target symbol,
/// and displaying the visible strip of symbols.
/// Attach to each Reel GameObject in the scene.
/// </summary>
public class ReelController : MonoBehaviour
{
    // ── Inspector ──────────────────────────────────────────────────────────
    [Header("Symbol Strip")]
    [Tooltip("All symbols that can appear on this reel (defines probability).")]
    [SerializeField] private SymbolData[] symbolPool;

    [Header("Display Slots")]
    [Tooltip("The three Image components shown top/middle/bottom. Middle is the payline.")]
    [SerializeField] private Image[] symbolSlots; // index 0=top, 1=middle, 2=bottom

    [Header("Config")]
    [SerializeField] private SlotConfiguration config;

    // ── State ──────────────────────────────────────────────────────────────
    private int _currentSymbolIndex;   // index into symbolPool that is on the payline
    private bool _isSpinning;

    // ── Public API ─────────────────────────────────────────────────────────

    /// <summary>The symbol currently sitting on the payline (centre slot).</summary>
    public SymbolData CurrentSymbol => symbolPool[_currentSymbolIndex];

    /// <summary>
    /// Spin this reel for <paramref name="duration"/> seconds then stop
    /// on a randomly chosen symbol. Calls <paramref name="onComplete"/> when done.
    /// </summary>
    public void Spin(float duration, Action onComplete)
    {
        if (_isSpinning) return;
        StartCoroutine(SpinRoutine(duration, onComplete));
    }

    // ── Private helpers ────────────────────────────────────────────────────

    private IEnumerator SpinRoutine(float duration, Action onComplete)
    {
        _isSpinning = true;

        // Pick result before animating so we can line up the strip correctly.
        int targetIndex = UnityEngine.Random.Range(0, symbolPool.Length);

        // Fast-scroll phase: rapidly cycle through symbols.
        float elapsed = 0f;
        float symbolHeight = GetSymbolHeight();
        float scrollAccumulator = 0f;
        int displayIndex = _currentSymbolIndex;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scrollAccumulator += config.spinSpeed * Time.deltaTime;

            // Each time we've scrolled one symbol height, advance the strip.
            while (scrollAccumulator >= symbolHeight)
            {
                scrollAccumulator -= symbolHeight;
                displayIndex = (displayIndex + 1) % symbolPool.Length;
                RefreshDisplay(displayIndex);
            }

            yield return null;
        }

        // Land on the chosen target.
        RefreshDisplay(targetIndex);
        _currentSymbolIndex = targetIndex;

        // Bounce effect for game-feel.
        yield return StartCoroutine(BounceRoutine());

        _isSpinning = false;
        onComplete?.Invoke();
    }

    /// <summary>
    /// Simple punch-scale bounce on the middle (payline) slot.
    /// </summary>
    private IEnumerator BounceRoutine()
    {
        if (symbolSlots == null || symbolSlots.Length < 2) yield break;

        RectTransform rt = symbolSlots[1].rectTransform;
        Vector2 original = rt.anchoredPosition;
        Vector2 overshot = original + Vector2.down * config.bounceOvershoot;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (config.bounceDuration * 0.5f);
            rt.anchoredPosition = Vector2.Lerp(original, overshot, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (config.bounceDuration * 0.5f);
            rt.anchoredPosition = Vector2.Lerp(overshot, original, t);
            yield return null;
        }

        rt.anchoredPosition = original;
    }

    /// <summary>
    /// Updates the three visible slots around the given centre index.
    /// </summary>
    private void RefreshDisplay(int centreIndex)
    {
        int len = symbolPool.Length;
        int top    = (centreIndex - 1 + len) % len;
        int bottom = (centreIndex + 1) % len;

        if (symbolSlots.Length >= 3)
        {
            symbolSlots[0].sprite = symbolPool[top].sprite;
            symbolSlots[1].sprite = symbolPool[centreIndex].sprite;
            symbolSlots[2].sprite = symbolPool[bottom].sprite;
        }
    }

    /// <summary>
    /// Returns the height of one symbol slot in pixels.
    /// Falls back to 100 if slots are not set up yet.
    /// </summary>
    private float GetSymbolHeight()
    {
        if (symbolSlots != null && symbolSlots.Length > 0)
            return symbolSlots[0].rectTransform.rect.height;
        return 100f;
    }

    // ── Unity lifecycle ────────────────────────────────────────────────────

    private void Start()
    {
        // Initialise display to the first symbol.
        _currentSymbolIndex = 0;
        RefreshDisplay(_currentSymbolIndex);
    }
}
