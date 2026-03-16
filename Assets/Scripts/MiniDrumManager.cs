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
    [SerializeField] private AudioClip cowbellClip;

    [Header("自動生成鼓組視覺設定")]
    [SerializeField] private bool autoCreateVisualCubes = true;
    [SerializeField] private Vector3 basePosition = Vector3.zero;
    [Tooltip("爵士鼓相對位置與大小可在此微調")]
    [SerializeField] private float drumLayoutScale = 1f;
    [SerializeField] private Color kickColor = new Color(0.6f, 0.25f, 0.2f);
    [SerializeField] private Color snareColor = new Color(0.35f, 0.35f, 0.4f);
    [SerializeField] private Color closedHiHatColor = new Color(0.75f, 0.7f, 0.5f);
    [SerializeField] private Color cowbellColor = new Color(0.9f, 0.8f, 0.5f);

    [Header("對應的可視方塊 (若 autoCreateVisualCubes 為 false，可手動指定)")]
    [SerializeField] private Transform kickVisual;
    [SerializeField] private Transform snareVisual;
    [SerializeField] private Transform closedHiHatVisual;
    [SerializeField] private Transform cowbellVisual;

    [Header("動畫參數")]
    [SerializeField] private float hitScaleMultiplier = 1.2f;
    [SerializeField] private float scaleDuration = 0.08f;

    private AudioSource kickSource;
    private AudioSource snareSource;
    private AudioSource closedHiHatSource;
    private AudioSource cowbellSource;

    private Vector3 kickOriginalScale;
    private Vector3 snareOriginalScale;
    private Vector3 closedHiHatOriginalScale;
    private Vector3 cowbellOriginalScale;

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

        if (cowbellClip == null)
        {
            cowbellClip = TryLoadClip("Cowbell");
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

            if (cowbellVisual == null)
            {
                cowbellVisual = transform;
            }
        }

        // 為每個鼓自動建立 AudioSource（若不存在）
        kickSource = CreateAudioSource("KickAudioSource", kickClip);
        snareSource = CreateAudioSource("SnareAudioSource", snareClip);
        closedHiHatSource = CreateAudioSource("ClosedHiHatAudioSource", closedHiHatClip);
        cowbellSource = CreateAudioSource("CowbellAudioSource", cowbellClip);

        kickOriginalScale = kickVisual != null ? kickVisual.localScale : Vector3.one;
        snareOriginalScale = snareVisual != null ? snareVisual.localScale : Vector3.one;
        closedHiHatOriginalScale = closedHiHatVisual != null ? closedHiHatVisual.localScale : Vector3.one;
        cowbellOriginalScale = cowbellVisual != null ? cowbellVisual.localScale : Vector3.one;
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

        // L -> Cowbell
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayDrum(cowbellSource, cowbellVisual, cowbellOriginalScale);
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
        Vector3 origin = transform.position + basePosition;
        float s = drumLayoutScale;

        // 爵士鼓擺放：大鼓橫放、略右略下不壓到小鼓，小鼓在大鼓左側，開合鈸再更左、更高
        if (kickVisual == null)
        {
            Vector3 pos = origin + new Vector3(0.5f * s, -0.5f * s, 0.8f * s);
            Vector3 kickScale = new Vector3(1.2f * s, 0.4f * s, 1.2f * s);
            kickVisual = CreateDrumVisual("KickDrum", pos, kickColor, kickScale, PrimitiveType.Cylinder, Quaternion.Euler(0f, 90f, 90f));
        }

        if (snareVisual == null)
        {
            Vector3 pos = origin + new Vector3(-0.38f * s, 0.22f * s, 0.1f * s);
            Vector3 snareScale = new Vector3(0.5f * s, 0.15f * s, 0.5f * s);
            snareVisual = CreateDrumVisual("SnareDrum", pos, snareColor, snareScale, PrimitiveType.Cylinder, Quaternion.identity);
        }

        if (closedHiHatVisual == null)
        {
            Vector3 pos = origin + new Vector3(-0.85f * s, 0.7f * s, 0.65f * s);
            closedHiHatVisual = CreateDrumVisual("ClosedHiHat", pos, closedHiHatColor, new Vector3(0.65f * s, 0.06f * s, 0.65f * s), PrimitiveType.Cylinder, Quaternion.identity);
        }

        if (cowbellVisual == null)
        {
            Vector3 pos = origin + new Vector3(0.95f * s, 0.65f * s, 0.4f * s);
            Vector3 cowbellScale = new Vector3(0.35f * s, 0.18f * s, 0.35f * s);
            cowbellVisual = CreateDrumVisual("Cowbell", pos, cowbellColor, cowbellScale, PrimitiveType.Cube, Quaternion.identity);
        }
    }

    private Transform CreateDrumVisual(string name, Vector3 position, Color color, Vector3 scale, PrimitiveType primitiveType, Quaternion rotation = default)
    {
        if (rotation == default)
            rotation = Quaternion.identity;

        GameObject drum = GameObject.CreatePrimitive(primitiveType);
        drum.name = name;
        drum.transform.SetParent(transform);
        drum.transform.position = position;
        drum.transform.rotation = rotation;
        drum.transform.localScale = scale;

        var renderer = drum.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(renderer.sharedMaterial);
            mat.color = color;
            renderer.material = mat;
        }

        return drum.transform;
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

