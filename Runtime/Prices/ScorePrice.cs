using Raccoons.Maths.Numbers;
using Raccoons.Scores;
using System;
using UnityEngine;

namespace Packages.Raccoons.Scores.Runtime.Prices
{
    public class ScorePrice : IDisposable
    {
        private bool _lastCanSpend = false;
        private bool _lastCanAcquire = false;
        public event EventHandler<bool> OnCanSpendChanged;
        public event EventHandler<bool> OnCanAcquireChanged;
        public event EventHandler<ScoreAcquisitionData> OnAcquired;
        public event EventHandler<ScoreAcquisitionData> OnSpent;
        public ScorePrice(AdvancedFloat value, IScoreBank scoreBank)
        {
            Value = value;
            ScoreBank = scoreBank;
            ScoreBank.OnScoreChanged += ScoreBank_OnScoreChanged;
        }

        public AdvancedFloat Value { get; private set; }
        public IScoreBank ScoreBank { get; private set; }
        public bool CanSpend => ScoreBank.CanSpend(Value.Value);
        public bool CanAcquire => ScoreBank.CanAcquire(Value.Value);

        private void ScoreBank_OnScoreChanged(object sender, ScoreChangeData e)
        {
            bool newCanSpend = CanSpend;
            if (newCanSpend != _lastCanSpend)
            {
                _lastCanSpend = newCanSpend;
                OnCanSpendChanged?.Invoke(this, _lastCanSpend);
            }
            bool newCanAcquire = CanAcquire;
            if (newCanAcquire != _lastCanAcquire)
            {
                _lastCanAcquire = newCanAcquire;
                OnCanAcquireChanged?.Invoke(this, _lastCanAcquire);
            }
        }

       /* public ScoreSpendingData Spend()
        {
            var data = ScoreBank.Spend(Value.Value);
        }

        public ScoreAcquisitionData Acquire()
        {
            var data = ScoreBank.Acquire(Value.Value);
        }*/

        public void Dispose()
        {
            ScoreBank.OnScoreChanged -= ScoreBank_OnScoreChanged;
        }
    }
}
