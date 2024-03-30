using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace SimpleVRHand.Tests
{
    /// <summary>
    /// Unit tests for <see cref="VrHandDriver"/>
    /// </summary>
    [TestFixture(Category = "XR")]
    public class VrHandDriverTests
    {
        
        [Test]
        public void UpdateHand_MissingFingerState_DoesNotThrowAndUpdatesExisingStates()
        {
            var hand = new MockHand()
            {
                MockFingers =  new ()
                {
                    new MockFinger() { Finger = HandFinger.Index },
                    new MockFinger() { Finger = HandFinger.Middle },
                    new MockFinger() { Finger = HandFinger.Pinky },
                    new MockFinger() { Finger = HandFinger.Ring },
                    new MockFinger() { Finger = HandFinger.Thumb },
                }
            };
            var defaultProvider = new MockStateProvider() 
            {                 
                Profile =  new MockHandProfile()
                {
                    FingerStates = new Dictionary<HandFinger, VrFingerState>()
                    {
                        { HandFinger.Index, new VrFingerState(){ Tilt = 1.1f }},
                        { HandFinger.Middle, new VrFingerState(){ Tilt = 1.2f }},
                        { HandFinger.Pinky, new VrFingerState(){ Tilt = 1.3f }},
                        { HandFinger.Ring, new VrFingerState(){ Tilt = 1.4f }},
                        { HandFinger.Thumb, new VrFingerState(){ Tilt = 1.5f }},
                    }
                } 
            };
            var driver = new VrHandDriver(defaultProvider);
            
            driver.UpdateHand(hand);
        }
        
        [Test]
        public void UpdateHand_MissingFingerState_UpdatesExisingStates()
        {
            var hand = new MockHand()
            {
                MockFingers =  new ()
                {
                    new MockFinger() { Finger = HandFinger.Index },
                    new MockFinger() { Finger = HandFinger.Middle },
                    new MockFinger() { Finger = HandFinger.Pinky },
                    new MockFinger() { Finger = HandFinger.Ring },
                    new MockFinger() { Finger = HandFinger.Thumb },
                }
            };
            var defaultProvider = new MockStateProvider() 
            {                 
                Profile =  new MockHandProfile()
                {
                    FingerStates = new Dictionary<HandFinger, VrFingerState>()
                    {
                        { HandFinger.Index, new VrFingerState(){ Tilt = 0.1f }},
                        { HandFinger.Pinky, new VrFingerState(){ Tilt = 0.3f }},
                        { HandFinger.Thumb, new VrFingerState(){ Tilt = 0.5f }},
                    }
                } 
            };
            var driver = new VrHandDriver(defaultProvider);
            driver.UpdateHand(hand);
            
            foreach (var finger in hand.MockFingers)
            {
                var hasState = defaultProvider.Profile.FingerStates.ContainsKey(finger.Finger);
                Assert.AreEqual(hasState, finger.UpdatedState.HasValue, $"The state of the {finger.Finger} has not been set!");
                
                if(hasState)
                    Assert.AreEqual(defaultProvider.Profile.FingerStates[finger.Finger].Tilt, finger.Tilt );
            }
        }
        
        [Test]
        public void UpdateHand_WithNewProvider_UsesCorrectFingerState()
        {
            var hand = new MockHand()
            {
                MockFingers = new ()
                {
                    new MockFinger() { Finger = HandFinger.Index },
                    new MockFinger() { Finger = HandFinger.Middle },
                    new MockFinger() { Finger = HandFinger.Pinky },
                    new MockFinger() { Finger = HandFinger.Ring },
                    new MockFinger() { Finger = HandFinger.Thumb },
                }
            };
            var defaultProvider = new MockStateProvider() { Profile =  new MockHandProfile() };
            var provider = new MockStateProvider()
            {
                Profile =  new MockHandProfile()
                {
                    FingerStates = new Dictionary<HandFinger, VrFingerState>()
                    {
                        { HandFinger.Index, new VrFingerState(){ Tilt = 1.1f }},
                        { HandFinger.Middle, new VrFingerState(){ Tilt = 1.2f }},
                        { HandFinger.Pinky, new VrFingerState(){ Tilt = 1.3f }},
                        { HandFinger.Ring, new VrFingerState(){ Tilt = 1.4f }},
                        { HandFinger.Thumb, new VrFingerState(){ Tilt = 1.5f }},
                    }
                }
            };
            var driver = new VrHandDriver(defaultProvider);
            
            driver.SetProvider(provider);
            driver.UpdateHand(hand);
            foreach (var finger in hand.MockFingers)
            {
                Assert.IsTrue(finger.UpdatedState.HasValue, $"The state of the {finger.Finger} has not been set!");
                Assert.AreEqual(provider.Profile.FingerStates[finger.Finger].Tilt, finger.Tilt );
            }
        }
        
        
        [Test]
        public void UpdateHand_CurrentProviderDoesNotHaveActiveFinger_UsesDefaultProfile()
        {
            var hand = new MockHand()
            {
                MockFingers =  new ()
                {
                    new MockFinger() { Finger = HandFinger.Index },
                    new MockFinger() { Finger = HandFinger.Middle },
                    new MockFinger() { Finger = HandFinger.Pinky },
                    new MockFinger() { Finger = HandFinger.Ring },
                    new MockFinger() { Finger = HandFinger.Thumb },
                }
            };
            var defaultProvider = new MockStateProvider()
            {
                Profile =  new MockHandProfile()
                {
                    FingerStates = new Dictionary<HandFinger, VrFingerState>()
                    {
                        { HandFinger.Index, new VrFingerState(){ Tilt = 0.1f }},
                        { HandFinger.Middle, new VrFingerState(){ Tilt = 0.2f }},
                        { HandFinger.Pinky, new VrFingerState(){ Tilt = 0.3f }},
                        { HandFinger.Ring, new VrFingerState(){ Tilt = 0.4f }},
                        { HandFinger.Thumb, new VrFingerState(){ Tilt = 0.5f }},
                    }
                }
            };
            var provider = new MockStateProvider() { Profile =  new MockHandProfile() };
            var driver = new VrHandDriver(defaultProvider);
            
            driver.SetProvider(provider);
            driver.UpdateHand(hand);
            foreach (var finger in hand.MockFingers)
            {
                Assert.IsTrue(finger.UpdatedState.HasValue, $"The state of the {finger.Finger} has not been set!");
                Assert.AreEqual(defaultProvider.Profile.FingerStates[finger.Finger].Tilt, finger.Tilt );
            }
        }
        
        
        [Test]
        public void UpdateHand_WithHandRotationOffset_SetsCorrectOffset()
        {
            var hand = new MockHand();
            var defaultProvider = new MockStateProvider()
            {
                Profile =  new MockHandProfile() { HandRotationOffset = new Quaternion(0f, 0.2f, 0.6f, 1f) }
            };
            var provider = new MockStateProvider()
            {
                Profile = new MockHandProfile() { HandRotationOffset = new Quaternion(0f, 0.5f, 0.1f, 0f) }
            };
            var driver = new VrHandDriver(defaultProvider);
            
            driver.SetProvider(provider);
            driver.UpdateHand(hand);
            Assert.AreEqual(provider.Profile.HandRotationOffset, hand.RotationOffset);
            
            driver.SetProvider(null);
            driver.UpdateHand(hand);
            Assert.AreEqual(defaultProvider.Profile.HandRotationOffset, hand.RotationOffset);
        }

        [Test]
        public void UpdateHand_WithHandPositionOffset_SetsCorrectOffset()
        {
            var hand = new MockHand();
            var defaultProvider = new MockStateProvider()
            {
                Profile =  new MockHandProfile() { HandPositionOffset = Vector3.one }
            };
            var provider = new MockStateProvider()
            {
                Profile = new MockHandProfile() { HandPositionOffset = Vector3.forward }
            };
            var driver = new VrHandDriver(defaultProvider);
            
            driver.SetProvider(provider);
            driver.UpdateHand(hand);
            Assert.AreEqual(provider.Profile.HandPositionOffset, hand.PositionOffset);
            
            driver.SetProvider(null);
            driver.UpdateHand(hand);
            Assert.AreEqual(defaultProvider.Profile.HandPositionOffset, hand.PositionOffset);
        }

        [Test]
        public void UpdateHand_WithVisibilityProfile_SetsCorrectVisibility()
        {
            var hand = new MockHand() { Visible = false };
            var defaultProvider = new MockStateProvider()
            {
                Profile =  new MockHandProfile() { HandVisible = false }
            };
            var provider = new MockStateProvider()
            {
                Profile = new MockHandProfile() { HandVisible = true }
            };
            var driver = new VrHandDriver(defaultProvider);
            
            driver.SetProvider(provider);
            driver.UpdateHand(hand);
            Assert.AreEqual(provider.Profile.HandVisible, hand.Visible);
            
            driver.SetProvider(null);
            driver.UpdateHand(hand);
            Assert.AreEqual(defaultProvider.Profile.HandVisible, hand.Visible);
        }
        
        [Test]
        public void SetProvider_WithNullProvider_ResetsToDefault()
        {
            var defaultProvider = new MockStateProvider();
            var tempProvider = new MockStateProvider();
            var driver = new VrHandDriver(defaultProvider);
            
            driver.SetProvider(tempProvider);
            Assert.AreEqual(tempProvider, driver.CurrentProvider);
            
            driver.SetProvider(null);
            Assert.AreEqual(defaultProvider, driver.CurrentProvider);
        }

        /// <summary>
        /// Mock for IVrHandStateProvider to use in this test
        /// </summary>
        private class MockStateProvider: IVrHandStateProvider
        {
            public IVrHandProfile CurrentProfile => Profile;
            public MockHandProfile Profile { get; set; }
        }
        
        /// <summary>
        /// Mock for IVrHandStateProvider to use in this test
        /// </summary>
        private class MockHandProfile: IVrHandProfile
        {
            public Dictionary<HandFinger, VrFingerState> FingerStates = new();
            public bool OverrideVisibility { get; set; }
            public bool HandVisible { get; set; }
            public bool OverridePosition { get; set; }
            public Vector3 HandPositionOffset { get; set; }
            public bool OverrideRotation { get; set; }
            public Quaternion HandRotationOffset { get; set; }
            public VrFingerState? GetFingerState(HandFinger fingerName, bool onlyActive = false)
            {
                if (!FingerStates.ContainsKey(fingerName) 
                    || (onlyActive && FingerStates[fingerName].Muted))
                    return null;

                return FingerStates[fingerName];
            }
        }
        
        /// <summary>
        /// Mock for IVrHand to use in this test
        /// </summary>
        private class MockHand: IVrHand
        {
            public List<MockFinger> MockFingers = new List<MockFinger>();
            
            public bool Visible { get; set; }
            public Vector3 PositionOffset { get; set; }
            public Quaternion RotationOffset { get; set; }
            public IReadOnlyCollection<IVrFinger> Fingers => MockFingers;
            public IVrFinger GetFinger(HandFinger fingerName)
            {
                return Fingers.First(finger => finger.Finger == fingerName);
            }
        }
        
        /// <summary>
        /// Mock for IVrHand to use in this test
        /// </summary>
        private class MockFinger: IVrFinger
        {
            public HandFinger Finger { get; set; }
            public float Tilt { get; set; }
            public IVrFingerJoint Root { get; set; }

            public VrFingerState? UpdatedState = null;
            
            public void UpdateState(VrFingerState state)
            {
                UpdatedState = state;
                Tilt = state.Tilt;
            }
        }
    }
}