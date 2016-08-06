using UnityEngine;
using Klak.Math;
using Klak.Motion;
using System;

public class HelixController : MonoBehaviour
{
    #region Public properties

    public float helixFrequency { get; set; }
    public float cutoffLevel { get; set; }
    public float cutoffModulation { get; set; }
    public float noiseAmplitude { get; set; }
    public float noiseRoughness { get; set; }
    public float spikeAmplitude { get; set; }
    public float undulationAngle { get; set; }

    #endregion

    #region Public methods

    public void ChangeModel(bool random)
    {
        _modelSelector = random ? -1 : _modelSelector + 1;
        ResetModels();
    }

    #endregion

    #region Model settings

    [Serializable]
    struct Model
    {
        [SerializeField] public Mesh mesh;
        [SerializeField] public Material material;
        [SerializeField] public float scale;
        [SerializeField] public float offset;
    }

    [SerializeField]
    Model[] _models;

    #endregion

    #region Private members

    int _modelSelector;

    MaterialPropertyBlock _props;
    MeshRenderer[] _renderers;
    BrownianMotion[] _motions;

    Model SelectModel()
    {
        if (_modelSelector < 0)
            return _models[UnityEngine.Random.Range(0, _models.Length)];
        else
            return _models[_modelSelector % _models.Length];
    }

    void ResetModels()
    {
        foreach (var r in _renderers)
        {
            var model = SelectModel();
            r.GetComponent<MeshFilter>().sharedMesh = model.mesh;
            r.sharedMaterial = model.material;
            r.transform.localScale = Vector3.one * model.scale;
            r.transform.localPosition = Vector3.up * model.offset;
        }
    }

    #endregion

    #region MonoBehaviour functions

    void Start()
    {
        _props = new MaterialPropertyBlock();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _motions = GetComponentsInChildren<BrownianMotion>();
    }

    void Update()
    {
        var hash = new XXHash(1234);

        _props.SetFloat("_HelixFreq", helixFrequency);
        _props.SetFloat("_Cutoff", cutoffLevel);
        _props.SetFloat("_WaveAmp", cutoffModulation);
        _props.SetFloat("_NoiseRough", noiseRoughness);
        _props.SetFloat("_NoiseAmp", noiseAmplitude);
        _props.SetFloat("_SpikeAmp", spikeAmplitude);

        for (var i = 0; i < _renderers.Length; i++)
        {
            _props.SetFloat("_RandomSeed", hash.Value01(i) * 100);
            _renderers[i].SetPropertyBlock(_props);
            _motions[i].rotationAmplitude = undulationAngle;
        }
    }

    #endregion
}
