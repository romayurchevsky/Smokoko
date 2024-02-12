using Scripts.CombatCode;
using Scripts.CaneraCode;
using System.Collections;
using UnityEngine;
using Scripts.CommonCode;

namespace Scripts.PlayerCode
{
    public class HeroController : BaseHero
    {
        [Header("Storages")]
        [SerializeField] private CharacterController characterController;

        [Header("Player Parameters")]
        [SerializeField] private float speedFactor = 1f;
        [SerializeField] private float rotationFactor = 1f;
        [SerializeField] private float enemyDetectRadius = 1f;
        [SerializeField] private float attackDelay = 1f;

        #region Variables
        private Joystick joystick;
        private Transform cameraTransform;
        private Vector3 moveVector;
        private bool canMove = false;
        private bool canAttack = false;
        private bool inAttack;
        private TargetStorage targetStorage;
        private ITarget currentEnemy;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            GameManager.LevelStartAction += SetParameters;
            Joystick.PointerDownAction += PointerDownReaction;
            Joystick.PointerUpAction += PointerUpReaction;
        }

        private void OnDestroy()
        {
            GameManager.LevelStartAction -= SetParameters;
            if (heroBody != null) heroBody.HeroAnimatorController.EndAttackAction -= EndAttackAnimation;
            Joystick.PointerDownAction -= PointerDownReaction;
            Joystick.PointerUpAction -= PointerUpReaction;
        }

        private void Update()
        {
            if (canMove) MovePlayer();
            if (!inAttack && canAttack) SelectEnemy();
        }

        public override void InitBody(HeroBody _heroBody)
        {
            base.InitBody(_heroBody);
            _heroBody.HeroAnimatorController.EndAttackAction += EndAttackAnimation;
        }

        private void SetParameters()
        {
            joystick = DependencyStorage.Joystick;
            cameraTransform = DependencyStorage.CameraController.GetCamera.transform;
            targetStorage = DependencyStorage.TargetStorage;
            CameraController.SetFollowAction?.Invoke(transform);
        }

        private void MovePlayer()
        {
            moveVector = Vector3.zero;
            if (joystick.Direction.sqrMagnitude > 0f)
            {
                moveVector = cameraTransform.TransformDirection(joystick.Direction);
                moveVector.y = 0f;
                moveVector.Normalize();
            }

            characterController.Move(moveVector * speedFactor * Time.deltaTime);
            heroBody.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(heroBody.transform.forward, moveVector, rotationFactor * Time.deltaTime, 0f), Vector3.up);
            SetRunAnimator();
        }

        private void SetRunAnimator()
        {
            if ((moveVector.x == 0f && moveVector.z == 0f) || !canMove)
            {
                heroBody.HeroAnimatorController.StopMove();
            }
            else
            {
                heroBody.HeroAnimatorController.SetSpeed(characterController.velocity.magnitude);
            }
        }

        private void SelectEnemy()
        {
            var tmpEnemy = targetStorage.GetNearestEnemy(transform.position, enemyDetectRadius);
            if (tmpEnemy == null && currentEnemy != null)
            {
                currentEnemy.SelectTarget(false);
                currentEnemy = null;
            }
            if (tmpEnemy != null && tmpEnemy != currentEnemy)
            {
                currentEnemy?.SelectTarget(false);
                currentEnemy = tmpEnemy;
                currentEnemy.SelectTarget(true);
            }

            TryToAttack();
        }

        private void TryToAttack()
        {
            if (currentEnemy != null && heroBody.ReadyToAttack())
            {
                heroBody.transform.LookAt(currentEnemy.TargetTransform);
                AttackReaction();
            }
        }

        private void AttackReaction()
        {
            inAttack = true;
            heroBody.HeroAnimatorController.Attack(0);
        }

        public void EndAttackAnimation()
        {
            heroBody.Attack(currentEnemy);
            StartCoroutine(AttackDelay());
        }

        private IEnumerator AttackDelay()
        {
            yield return new WaitForSeconds(attackDelay);
            inAttack = false;
        }

        private void PointerDownReaction()
        {
            canMove = true;
            canAttack = false;
            if (currentEnemy != null)
            {
                currentEnemy?.SelectTarget(false);
                currentEnemy = null;
                heroBody.HeroAnimatorController.FinishAttack();
                inAttack = false;
            }
        }

        private void PointerUpReaction()
        {
            canMove = false;
            if (heroBody.HasWeapon()) canAttack = true;
            SetRunAnimator();
        }
    }
}
