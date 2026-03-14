using System.Collections;
using UnityEngine;

/// <summary>
/// 簡易迷你鼓機管理器。
/// 將此腳本掛在場景中的一個空物件或可視方塊上。
/// - Space：Kick
/// - J：Snare
/// - K：Close Hi‑Hat
///
/// 音效來源：
/// - 會優先嘗試從 Resources/Audio 資料夾載入 Kick / Snare / ClosedHiHat
/// - 若未找到，則可在 Inspector 中手動指定 AudioClip
/// </summary>
public class MiniDrumManager : MonoBehaviour
{
    [Header("Drum Clips (若為空會嘗試自動載入)")]
    [SerializeField] private AudioClip kickClip;
    [SerializeField] private AudioClip snareClip;
    [SerializeField] private AudioClip closedHiHatClip;

    [Header("自動生成鼓方塊設定")]
    [SerializeField] private bool autoCreateVisualCubes = true;
    [SerializeField] private float cubeSize = 1f;
    [SerializeField] private float cubeSpacing = 1.6f;
    [SerializeField] private Vector3 basePosition = Vector3.zero;
    [SerializeField] private Color kickColor = new Color(1f, 0.4f, 0.4f);
    [SerializeField] private Color snareColor = new Color(0.4f, 0.8f, 1f);
    [SerializeField] private Color closedHiHatColor = new Color(1f, 1f, 0.4f);

    [Header("對應的可視方塊 (若 autoCreateVisualCubes 為 false，可手動指定)")]
    [SerializeField] private Transform kickVisual;
    [SerializeField] private Transform snareVisual;
    [SerializeField] private Transform closedHiHatVisual;

    [Header("動畫參數")]
    [SerializeField] private float hitScaleMultiplier = 1.2f;
    [SerializeField] private float scaleDuration = 0.08f;

    private AudioSource kickSource;
    private AudioSource snareSource;
    private AudioSource closedHiHatSource;

    private Vector3 kickOriginalScale;
    private Vector3 snareOriginalScale;
    private Vector3 closedHiHatOriginalScale;

    private void Awake()
    {
        // 嘗試從 Resources/Audio 自動載入音效（若尚未在 Inspector 指定）
        // 注意：要讓自動偵測生效，請將音效放在 Assets/Resources/Audio 資料夾中
        if (kickClip == null)
        {
            kickClip = TryLoadClip("Kick");
        }

        if (snareClip == null)
        {
            snareClip = TryLoadClip("Snare");
        }

        if (closedHiHatClip == null)
        {
            closedHiHatClip = TryLoadClip("ClosedHiHat");
        }

        // 自動或手動建立可視方塊
        if (autoCreateVisualCubes)
        {
            CreateDefaultVisualCubes();
        }
        else
        {
            // 若未指定可視方塊，預設使用當前物件
            if (kickVisual == null)
            {
                kickVisual = transform;
            }

            if (snareVisual == null)
            {
                snareVisual = transform;
            }

            if (closedHiHatVisual == null)
            {
                closedHiHatVisual = transform;
            }
        }

        // 為每個鼓自動建立 AudioSource（若不存在）
        kickSource = CreateAudioSource("KickAudioSource", kickClip);
        snareSource = CreateAudioSource("SnareAudioSource", snareClip);
        closedHiHatSource = CreateAudioSource("ClosedHiHatAudioSource", closedHiHatClip);

        kickOriginalScale = kickVisual != null ? kickVisual.localScale : Vector3.one;
        snareOriginalScale = snareVisual != null ? snareVisual.localScale : Vector3.one;
        closedHiHatOriginalScale = closedHiHatVisual != null ? closedHiHatVisual.localScale : Vector3.one;
    }

    private void Update()
    {
        // Space -> Kick
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayDrum(kickSource, kickVisual, kickOriginalScale);
        }

        // J -> Snare
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayDrum(snareSource, snareVisual, snareOriginalScale);
        }

        // K -> Close Hi‑Hat
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayDrum(closedHiHatSource, closedHiHatVisual, closedHiHatOriginalScale);
        }
    }

    private AudioClip TryLoadClip(string clipName)
    {
        // 嘗試兩種常見路徑：Resources/clipName 以及 Resources/Audio/clipName
        var clip = Resources.Load<AudioClip>(clipName);
        if (clip != null)
        {
            return clip;
        }

        clip = Resources.Load<AudioClip>("Audio/" + clipName);
        return clip;
    }

    private AudioSource CreateAudioSource(string childName, AudioClip clip)
    {
        Transform child = transform.Find(childName);
        AudioSource source;

        if (child == null)
        {
            var go = new GameObject(childName);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            source = go.AddComponent<AudioSource>();
        }
        else
        {
            source = child.GetComponent<AudioSource>();
            if (source == null)
            {
                source = child.gameObject.AddComponent<AudioSource>();
            }
        }

        source.playOnAwake = false;
        source.clip = clip;
        return source;
    }

    private void CreateDefaultVisualCubes()
    {
        // 以管理器所在位置為基準
        Vector3 origin = transform.position + basePosition;

        // Kick 在左
        if (kickVisual == null)
        {
            kickVisual = CreateCubeVisual("KickCube", origin + Vector3.left * cubeSpacing, kickColor);
        }

        // Snare 在中間
        if (snareVisual == null)
        {
            snareVisual = CreateCubeVisual("SnareCube", origin, snareColor);
        }

        // HiHat 在右
        if (closedHiHatVisual == null)
        {
            closedHiHatVisual = CreateCubeVisual("ClosedHiHatCube", origin + Vector3.right * cubeSpacing, closedHiHatColor);
        }
    }

    private Transform CreateCubeVisual(string name, Vector3 position, Color color)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = name;
        cube.transform.SetParent(transform);
        cube.transform.position = position;
        cube.transform.localScale = Vector3.one * cubeSize;

        var renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            // 使用一個新的材質，避免改到內建材質
            Material mat = new Material(renderer.sharedMaterial);
            mat.color = color;
            renderer.material = mat;
        }

        return cube.transform;
    }

    private void PlayDrum(AudioSource source, Transform visual, Vector3 originalScale)
    {
        if (source != null && source.clip != null)
        {
            source.PlayOneShot(source.clip);
        }

        if (visual != null)
        {
            StopAllCoroutines();
            StartCoroutine(ScalePunch(visual, originalScale));
        }
    }

    private IEnumerator ScalePunch(Transform target, Vector3 originalScale)
    {
        if (target == null)
        {
            yield break;
        }

        float elapsed = 0f;
        Vector3 hitScale = originalScale * hitScaleMultiplier;

        while (elapsed < scaleDuration)
        {
            float t = elapsed / scaleDuration;
            // 先放大再回到原始大小的簡單 Lerp
            float curve = 1f - Mathf.Abs(2f * t - 1f); // 0 -> 1 -> 0
            target.localScale = Vector3.Lerp(originalScale, hitScale, curve);

            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = originalScale;
    }
}

