using BugColony.Bugs;
using BugColony.Infrastructure;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace BugColony.UI
{
    public class DeadBugUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _deadWorkersLabel;
        [SerializeField] private TextMeshProUGUI _deadPredatorsLabel;
        [SerializeField] private string _deadWorkersText;
        [SerializeField] private string _deadPredatorsText;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            var kills = _signalBus.GetStream<BugKilledSignal>();

            kills
                .Where(bug => bug.Type == BugType.Worker)
                .Scan(0, (count, _) => count + 1)
                .StartWith(0)
                .Subscribe(c => _deadWorkersLabel.text = string.Format(_deadWorkersText, c.ToString()))
                .AddTo(this);

            kills
                .Where(bug => bug.Type == BugType.Predator)
                .Scan(0, (count, _) => count + 1)
                .StartWith(0)
                .Subscribe(c => _deadPredatorsLabel.text = string.Format(_deadPredatorsText, c.ToString()))
                .AddTo(this);
        }
    }
}
