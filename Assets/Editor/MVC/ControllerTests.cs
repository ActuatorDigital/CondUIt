using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace MVC {
    public class ControllerTests {

        [Test]
        public void ChangesCanSave() {
            // Arrange.
            var gameObject = new GameObject();
            var controller = gameObject.AddComponent<TestControllerOne>();
            const string PRE_TEST_STR = "pre", POST_TEST_STR = "post";
            bool changed = false;
            var preModel = new TestModel(PRE_TEST_STR);

            // Act.
            controller.Init((changedModel) => {
                var testStr = (changedModel as TestModel).TestString;
                changed = testStr != PRE_TEST_STR &&
                          testStr == POST_TEST_STR;
            }, preModel);
            controller.ChangeTestStr(POST_TEST_STR); 

            // Assert.
            Assert.IsTrue(changed);

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void CallsActionAcrossControllers() {
            // Arrange.
            const string PRE_TEST_STR = "pre", 
                         POST_TEST_STR = "post";

            var controllerOne = new GameObject()
                .AddComponent<TestControllerOne>();
            controllerOne.ConnectMVC();
            controllerOne.Context = new TestModel(PRE_TEST_STR);

            var controllerTwo = new GameObject()
                .AddComponent<TestControllerTwo>();
            controllerTwo.ConnectMVC();
            controllerTwo.Context = new TestModel(POST_TEST_STR);
            controllerTwo.OnSaveChanges += (x) => { };

            try {
                // Act.
                controllerTwo.TriggerAction(POST_TEST_STR);

                // Assert.
                Assert.IsTrue(controllerTwo.Context.TestString == POST_TEST_STR);
            } finally {
                Object.DestroyImmediate(controllerOne.gameObject);
                Object.DestroyImmediate(controllerTwo.gameObject);
            }

        }
    }
}