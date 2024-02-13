using Scripts.CombatCode;
using Scripts.CommonCode;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, ITarget
{
    [Header("Components")]
    [SerializeField] private Transform cross;
    [SerializeField] private Slider healthSlider;

    [Header("PArameters")]
    [SerializeField] private int startingHP;

    #region Variables
    private TargetStorage targetStorage;
    private int currentHP;
    #endregion

    private void OnEnable()
    {
        targetStorage = DependencyStorage.TargetStorage;
        InitEnemy();
    }

    private void InitEnemy()
    {
        targetStorage.RegisterEnemy(this);
        currentHP = startingHP;
        SetUi();
        SelectTarget(false);
    }

    private void EnemyDie()
    {
        targetStorage.UnregisterEnemy(this);
        healthSlider.gameObject.SetActive(false);
    }

    private void DamageAnimation()
    {
        StartCoroutine(DamageProcess());
    }

    private IEnumerator DamageProcess()
    {
        var tnpScale = transform.localScale;
        transform.localScale += new Vector3(0.1f, 0, 0.1f);
        yield return new WaitForSeconds(0.2f);
        transform.localScale = tnpScale;
    }

    private void SetUi()
    {
        healthSlider.value = currentHP;
    }

    #region ITarget
    public Transform TargetTransform => transform;

    public bool IsAlive { get => currentHP > 0; } 

    public void GetDamage(int _damage, Vector3 _from)
    {
        currentHP -= _damage;
        SetUi();
        if (currentHP > 0)
        {
            DamageAnimation();
            return;
        }
        EnemyDie();
    }

    public void SelectTarget(bool _select)
    {
        cross.gameObject.SetActive(_select);
    }
    #endregion
}
