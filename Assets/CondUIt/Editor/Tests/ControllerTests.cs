﻿using UnityEngine;
using NUnit.Framework;

namespace CondUIt {
    public class ControllerTests {

        [Test]
        public void CallsActionAcrossControllers() {
            // Arrange.
            const string PRE_TEST_STR = "pre", 
                         POST_TEST_STR = "post";

            var canvas = new GameObject("Canvas");
            var conduit = canvas.AddComponent<CondUItFramework>();

            var controllerOne = new GameObject()
                .AddComponent<TestControllerOne>();
            controllerOne.TestString = PRE_TEST_STR;
            controllerOne.gameObject.transform.SetParent(canvas.transform);

            var controllerTwo = new GameObject()
                .AddComponent<TestControllerTwo>();
            controllerTwo.gameObject.transform.SetParent(canvas.transform);

            conduit.InitializeUI<TestControllerOne>();

            try {
                // Act.
                controllerTwo.TriggerAction(POST_TEST_STR);

                // Assert.
                Assert.IsTrue(controllerOne.TestString == POST_TEST_STR);
            } finally {
                Object.DestroyImmediate(controllerOne.gameObject);
                Object.DestroyImmediate(controllerTwo.gameObject);
            }

        }
    }
}