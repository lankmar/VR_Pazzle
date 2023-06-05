using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MagneticPoint
{
    public List<SpringJoint> jointList;
    public List<Rigidbody> rb;
    public List<ParticleSystem> highLight;
    public Transform blueObj;
    public Transform redObj;
    public Vector3 bluePos;
    public Vector3 redPos;
}

public class CharMagnetic : MonoBehaviour
{
    [SerializeField] private float _spellDistance = 20.0f;
    [SerializeField] private float _maxMagneticForce = 50.0f;
    [SerializeField] private MagneticPoint _magneticSpell;
    [SerializeField] private Transform _blueHolder;
    [SerializeField] private Transform _redHolder;
    [SerializeField] private Material _redMat;
    [SerializeField] private Material _blueMat;
    [SerializeField] private Material _yellowMat;
    [SerializeField] private ParticleSystem _hlReference;
    [SerializeField] private float _recolorDistance = 0.5f;

    public void SetBlue(Transform trans)
    {
        if (trans == _magneticSpell.redObj) EreaseSpell();

        _magneticSpell.blueObj = trans;
        _magneticSpell.bluePos = trans.position;
        Highlighting(true, trans);
        CheckToJoint();
    }

    public void SetRed(Transform trans)
    {
        if (trans == _magneticSpell.blueObj) EreaseSpell();

        _magneticSpell.redObj = trans;
        _magneticSpell.redPos = trans.position;
        Highlighting(false, trans);
        CheckToJoint();
    }

    public void SetBlue(Vector3 trans)
    {
        if (_magneticSpell.redObj != null && Vector3.Distance(_magneticSpell.redPos, trans) <= _recolorDistance) EreaseSpell();
        _magneticSpell.blueObj = _blueHolder;
        _magneticSpell.bluePos = trans;
        _blueHolder.position = trans;
        _blueHolder.GetChild(0).gameObject.SetActive(true);
        CheckToJoint();
    }

    public void SetRed(Vector3 trans)
    {
        if (_magneticSpell.blueObj != null && Vector3.Distance(_magneticSpell.bluePos, trans) <= _recolorDistance) EreaseSpell();
        _magneticSpell.redObj = _redHolder;
        _magneticSpell.redPos = trans;
        _redHolder.position = trans;
        _redHolder.GetChild(0).gameObject.SetActive(true);
        CheckToJoint();
    }

    private void CheckToJoint()
    {
        if (_magneticSpell.blueObj != null && _magneticSpell.redObj != null)
        {
            if (Vector3.Distance(_magneticSpell.redPos, _magneticSpell.bluePos) < _spellDistance) CreateJoint();
            else EreaseSpell();
        }
    }

    private void CreateJoint()
    {
        SpringJoint sp = _magneticSpell.blueObj.gameObject.AddComponent<SpringJoint>();
        sp.autoConfigureConnectedAnchor = false;
        sp.anchor = Vector3.zero;
        sp.connectedAnchor = Vector3.zero;
        sp.enableCollision = true;
        sp.enablePreprocessing = false;
        sp.connectedBody = _magneticSpell.redObj.GetComponent<Rigidbody>();

        EreaseSpell();
        _magneticSpell.jointList.Add(sp);
        Rigidbody rb = sp.GetComponent<Rigidbody>();
        _magneticSpell.rb.Add(rb);
        AddRG(sp.connectedBody);
    }

    private void AddRG(Rigidbody Rb)
    {
        if (_magneticSpell.rb == null)
        {
            return;
        }

        for (int i = 0; i < _magneticSpell.rb.Count; i++)
        {
            if (Rb == _magneticSpell.rb[i])
                break;
            if (i == _magneticSpell.rb.Count - 1)
            {
                _magneticSpell.rb.Add(Rb);
                break;
            }
        }
    }

    private void Highlighting(bool isBlue, Transform trans)
    {
        ParticleSystem ps = trans.GetComponent<ParticleSystem>();
        if (ps == null) ps = Instantiate(_hlReference, trans, false);

        Renderer renderer = ps.GetComponent<Renderer>();

        if (isBlue)
        {
            renderer.material = _blueMat;
        }
        else
        {
            renderer.material = _redMat;
        }

        _magneticSpell.highLight.Add(ps);
    }

    private void EreaseSpell()
    {
        _magneticSpell.blueObj = null;
        _magneticSpell.redObj = null;

        DestroyParticleSystems();
        DisableHolders();
    }

    private void DestroyParticleSystems()
    {
        for (int i = 0; i < _magneticSpell.highLight.Count; i++)
        {
            Destroy(_magneticSpell.highLight[i]);
        }
        _magneticSpell.highLight.Clear();
    }

    public void DestroyAllJoints()
    {
        for (int i = 0; i < _magneticSpell.jointList.Count; i++)
        {
            Destroy(_magneticSpell.jointList[i]);
        }

        for (int i = 0; i < _magneticSpell.rb.Count; i++)
        {
            _magneticSpell.rb[i].angularDrag = 0.05f;
            _magneticSpell.rb[i].drag = 0;
            _magneticSpell.rb[i].WakeUp();
        }

        _magneticSpell.jointList.Clear();
        _magneticSpell.rb.Clear();
        EreaseSpell();

        DestroyParticleSystems();

        DisableHolders();
    }

    private void DisableHolders()
    {
        _blueHolder.GetChild(0).gameObject.SetActive(false);
        _redHolder.GetChild(0).gameObject.SetActive(false);
    }

    public void ChangeSpringPower(float fNum)
    {
        if (_magneticSpell.jointList.Count > 0)
        {
            for (int i = 0; i < _magneticSpell.jointList.Count; i++)
            {
                _magneticSpell.jointList[i].spring += fNum;
                _magneticSpell.jointList[i].damper += fNum;
                _magneticSpell.jointList[i].damper += Mathf.Clamp(_magneticSpell.jointList[i].damper, 0, _maxMagneticForce);
                _magneticSpell.jointList[i].spring += Mathf.Clamp(_magneticSpell.jointList[i].spring, 0, _maxMagneticForce);
            }

            for (int i = 0; i < _magneticSpell.rb.Count; i++)
            {
                _magneticSpell.rb[i].WakeUp();
                _magneticSpell.rb[i].angularDrag += fNum;
                _magneticSpell.rb[i].drag += fNum;
                _magneticSpell.rb[i].angularDrag = Mathf.Clamp(_magneticSpell.rb[i].angularDrag, 0, _maxMagneticForce);
                _magneticSpell.rb[i].drag = Mathf.Clamp(_magneticSpell.rb[i].drag, 0, _maxMagneticForce);
            }
        }
    }
}