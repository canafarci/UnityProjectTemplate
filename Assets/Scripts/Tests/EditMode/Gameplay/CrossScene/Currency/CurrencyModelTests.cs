using System.Collections.Generic;
using NUnit.Framework;
using ProjectTemplate.Runtime.CrossScene.Currency;
using ProjectTemplate.Runtime.CrossScene.Enums;
using ProjectTemplate.Runtime.CrossScene.Signals;
using ProjectTemplate.Runtime.Infrastructure.Signals;
using UnityEngine;
using VContainer;

namespace ProjectTemplate.Tests.EditMode.Gameplay.CrossScene.Currency
{
    public class CurrencyModelTests
    {
        private CurrencyConfig _currencyConfig;
        private ICurrencyModel _currencyModel;
        private SignalBus _signalBus;
        private Dictionary<CurrencyID, int> _originalLookup;
        private	const string PERSISTENT_DATA_PATH = "PERSISTENT_DATA";
        private	const string CURRENCY_KEY = "CURRENCY_KEY";
        
        [SetUp]
        public void Setup()
        {
            if (ES3.KeyExists(CURRENCY_KEY, PERSISTENT_DATA_PATH))
            {
                _originalLookup = ES3.Load<Dictionary<CurrencyID, int>>(CURRENCY_KEY, PERSISTENT_DATA_PATH);
                ES3.DeleteKey(CURRENCY_KEY, PERSISTENT_DATA_PATH);
            }
            
            ContainerBuilder builder = new();
            builder.RegisterSignalBus();
            builder.DeclareSignal<CurrencyChangedSignal>();
            _currencyConfig = ScriptableObject.CreateInstance<CurrencyConfig>();

            // Set initial values for testing
            var initialCurrencyValues = new Dictionary<CurrencyID, int>
            {
                { CurrencyID.Money, 100 },
                { CurrencyID.TemporaryGameplayMoney, 50 }
            };
            SetCurrencyDefaultValues(initialCurrencyValues);

            builder.RegisterInstance(_currencyConfig);
            builder.Register<ICurrencyModel, CurrencyModel>(Lifetime.Scoped);
            
            IObjectResolver container = builder.Build();
            _currencyModel = container.Resolve<ICurrencyModel>();
            _signalBus = container.Resolve<SignalBus>();
        }

        [TearDown]
        public void TearDown()
        {
            if (ES3.KeyExists(CURRENCY_KEY, PERSISTENT_DATA_PATH))
            {
                ES3.Save(CURRENCY_KEY, _originalLookup, PERSISTENT_DATA_PATH);
            }
        }

        private void SetCurrencyDefaultValues(Dictionary<CurrencyID, int> values)
        {
            typeof(CurrencyConfig)
                .GetField("CurrencyDefaultValues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_currencyConfig, values);
        }

        [Test]
        public void GetCurrencyValue_ReturnsInitialAmount()
        {
            Assert.AreEqual(100, _currencyModel.GetCurrencyValue(CurrencyID.Money));
            Assert.AreEqual(50, _currencyModel.GetCurrencyValue(CurrencyID.TemporaryGameplayMoney));
        }

        [Test]
        public void AddCurrency_IncreasesAmount()
        {
            _currencyModel.AddCurrency(CurrencyID.Money, 20);
            Assert.AreEqual(120, _currencyModel.GetCurrencyValue(CurrencyID.Money));
        }

        [Test]
        public void TryPayCost_SufficientFunds_DecreasesAmount()
        {
            bool result = _currencyModel.TryPayCost(CurrencyID.Money, 30);
            Assert.IsTrue(result);
            Assert.AreEqual(70, _currencyModel.GetCurrencyValue(CurrencyID.Money));
        }

        [Test]
        public void TryPayCost_InsufficientFunds_DoesNotChangeAmount()
        {
            bool result = _currencyModel.TryPayCost(CurrencyID.Money, 150);
            Assert.IsFalse(result);
            Assert.AreEqual(100, _currencyModel.GetCurrencyValue(CurrencyID.Money));
        }

        [Test]
        public void AddCurrency_NegativeAmount_ThrowsAssertion()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => _currencyModel.AddCurrency(CurrencyID.Money, -10));
        }

        [Test]
        public void TryPayCost_NegativeCost_ThrowsAssertion()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => _currencyModel.TryPayCost(CurrencyID.Money, -10));
        }

        [Test]
        public void AddCurrency_NonExistentCurrencyID_ThrowsAssertion()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => _currencyModel.AddCurrency((CurrencyID)999, 10));
        }

        [Test]
        public void TryPayCost_NonExistentCurrencyID_ThrowsAssertion()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => _currencyModel.TryPayCost((CurrencyID)999, 10));
        }

        [Test]
        public void AddingCurrency_FiresCurrencyChangedSignal()
        {
            bool signalFired = false;
            _signalBus.Subscribe<CurrencyChangedSignal>((signal) => signalFired = true);

            _currencyModel.AddCurrency(CurrencyID.TemporaryGameplayMoney, 10);

            Assert.IsTrue(signalFired);
        }

        [Test]
        public void TryPayCost_SufficientFunds_FiresCurrencyChangedSignal()
        {
            bool signalFired = false;
            _signalBus.Subscribe<CurrencyChangedSignal>((signal) => signalFired = true);

            _currencyModel.TryPayCost(CurrencyID.TemporaryGameplayMoney, 10);

            Assert.IsTrue(signalFired);
        }
    }
}
