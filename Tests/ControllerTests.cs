using NUnit.Framework;
using UnityEngine;

namespace AIR.Conduit.Tests {
    
    public class ControllerTests {

        [Test]
        public void CallsActionAcrossControllers() {
            // Arrange.
            const string PRE_TEST_STR = "pre",
                         POST_TEST_STR = "post";


            var canvas = new GameObject("Canvas");

            var controllerOne = new GameObject()
                .AddComponent<TestControllerOne>();
            controllerOne.TestString = PRE_TEST_STR;
            controllerOne.gameObject.transform.SetParent(canvas.transform);

            var controllerTwo = new GameObject()
                .AddComponent<TestControllerTwo>();
            controllerTwo.gameObject.transform.SetParent(canvas.transform);

            var conduit = canvas.AddComponent<ConduitUIFramework>();

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