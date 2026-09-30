using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VertigoWheel.Core;

namespace VertigoWheel.Tests
{
    public class GameStateMachineTests
    {
        private class TrackingState : GameState
        {
            public int EnterCount;
            public int ExitCount;

            public TrackingState(GameStateMachine machine) : base(null, machine) { }

            public override void Enter() { EnterCount++; }
            public override void Exit() { ExitCount++; }
        }

        private class FirstState : TrackingState { public FirstState(GameStateMachine machine) : base(machine) { } }
        private class SecondState : TrackingState { public SecondState(GameStateMachine machine) : base(machine) { } }

        private GameStateMachine machine;
        private FirstState first;
        private SecondState second;

        [SetUp]
        public void SetUp()
        {
            machine = new GameStateMachine();
            first = new FirstState(machine);
            second = new SecondState(machine);
            machine.AddState(first);
            machine.AddState(second);
            machine.Start<FirstState>();
        }

        [Test]
        public void AllowedTransition_ExitsTheOldState_AndEntersTheNewOne()
        {
            machine.AllowTransition<FirstState, SecondState>();

            machine.ChangeState<SecondState>();

            Assert.AreSame(second, machine.Current);
            Assert.AreEqual(1, first.ExitCount);
            Assert.AreEqual(1, second.EnterCount);
        }

        [Test]
        public void UndeclaredTransition_IsRejected_AndLogged()
        {
            LogAssert.Expect(LogType.Error, new Regex("Gecersiz state gecisi"));

            machine.ChangeState<SecondState>();

            Assert.AreSame(first, machine.Current);
            Assert.AreEqual(0, first.ExitCount);
            Assert.AreEqual(0, second.EnterCount);
        }
    }
}
