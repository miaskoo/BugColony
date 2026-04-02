using TMPro;
using UniRx;
using UnityEngine;

namespace BugColony.Bugs
{
    public class BugViewController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _modelMesh;
        [SerializeField] private TextMeshPro _modelLabel;
        private string _id;
        private CompositeDisposable _disposable;

        public void Init(BugObject bug)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            _id = bug.Type.ToString() + bug.name;

            bug.StomachPoints
                .CombineLatest(bug.LifeTimeRemaining, (points, lifeTime) => (points, lifeTime))
                .Subscribe(pair => UpdateLabel(pair.points, pair.lifeTime))
                .AddTo(_disposable);
        }

        public void UpdateView(Material material)
        {
            _modelMesh.material = material;
        }

        private void UpdateLabel(int points, float lifeTime)
        {
            if (lifeTime >= 0)
                _modelLabel.text = $"ID:{_id}\nStomach:{points}\nLife:{lifeTime:F1}s";
            else
                _modelLabel.text = $"ID:{_id}\nStomach:{points}";
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}
