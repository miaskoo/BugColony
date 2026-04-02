using System;
using BugColony.Core;
using BugColony.Environment;
using BugColony.Spawning;
using BugColony.Strategies;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace BugColony.Bugs
{
    public class BugObject : SceneEntity
    {
        [SerializeField] private BoxCollider _col;
        public override Collider EntityCollider => _col;
        [SerializeField] private BugViewController _view;
        public BugType Type { get; private set; }
        private IBugStrategy _strategy;
        private Tween _lastMoveAction;
        private CompositeDisposable _lifeDisposable;
        private SerialDisposable _lifeTimerDisposable;
        private SerialDisposable _lifeCountdownDisposable;

        private BugStrategyController _strategyController;

        public ReactiveProperty<int> StomachPoints { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<float> LifeTimeRemaining { get; } = new ReactiveProperty<float>(-1f);
        public bool IsInvulnerable { get; private set; }

        [Inject]
        private void Construct(BugStrategyController strategyController)
        {
            _strategyController = strategyController;
        }

        private void OnDestroy()
        {
            Deactivate();
        }

        public void StartLife(BugType typeValue)
        {
            StomachPoints.Value = 0;
            LifeTimeRemaining.Value = -1f;
            Type = typeValue;
            _strategy = _strategyController.GetStrategy(Type);
            _view.Init(this);
            _view.UpdateView(_strategy.GetConfig().BugMaterial);


            _lifeDisposable?.Dispose();
            _lifeDisposable = new CompositeDisposable();
            _lifeTimerDisposable = new SerialDisposable();
            _lifeCountdownDisposable = new SerialDisposable();
            _lifeDisposable.Add(_lifeTimerDisposable);
            _lifeDisposable.Add(_lifeCountdownDisposable);

            _strategy.OnBugActivated(this);

            ApplyInvulnerability();

            this.OnCollisionEnterAsObservable()
                .Subscribe(col => _strategy.TryEatThis(this, col.gameObject))
                .AddTo(_lifeDisposable);

            GoToNextPoint();
        }

        public void ActivateLife(Vector3 position)
        {
            Activate(position);
        }

        public override void Deactivate()
        {
            if (_strategy == null)
                return;

            _lifeDisposable?.Dispose();
            _lifeDisposable = null;
            _lastMoveAction?.Kill();
            _strategy = null;
            base.Deactivate();
        }

        public void GoToNextPoint()
        {
            ReplaceMoveAction(_strategy.CreateMoveAction(this));
        }

        public void IncrementPoint()
        {
            StomachPoints.Value++;
            if (_strategy.TryMutate(this))
                StomachPoints.Value = 0;
        }

        public void ReplaceMoveAction(Tween newAction)
        {
            if (newAction == null)
                return;

            _lastMoveAction?.Kill();
            _lastMoveAction = newAction;
        }

        public void ApplyInvulnerability()
        {
            var config = _strategy.GetConfig();
            if (config.InvulnerabilityTime <= 0)
                return;

            IsInvulnerable = true;
            _view.UpdateView(config.InvulnerabilityMaterial);
            Observable.Timer(TimeSpan.FromSeconds(config.InvulnerabilityTime))
                .Subscribe(_ =>
                {
                    IsInvulnerable = false;
                    _view.UpdateView(config.BugMaterial);
                })
                .AddTo(_lifeDisposable);
        }

        public void SetLifeTimer(IDisposable disposable, float duration)
        {
            LifeTimeRemaining.Value = duration;
            _lifeCountdownDisposable.Disposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    LifeTimeRemaining.Value -= Time.deltaTime;
                });
            _lifeTimerDisposable.Disposable = disposable;
        }

    }
}
