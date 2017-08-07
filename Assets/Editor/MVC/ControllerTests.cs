using UnityEngine;
using NUnit.Framework;

namespace MVC {
    public class ControllerTests {

        [Test]
        public void ChangesCanSave() {
            var canvas = new GameObject("Canvas");
            canvas.AddComponent<MVCFramework>();

            // Arrange.
            var gameObject = new GameObject();
            gameObject.transform.parent = canvas.transform;
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

            var canvas = new GameObject("Canvas");
            var mvc = canvas.AddComponent<MVCFramework>();

            var controllerOne = new GameObject()
                .AddComponent<TestControllerOne>();
            controllerOne.Context = new TestModel(PRE_TEST_STR);
            controllerOne.gameObject.transform.parent = canvas.transform;

            var controllerTwo = new GameObject()
                .AddComponent<TestControllerTwo>();
            controllerTwo.Context = new TestModel(POST_TEST_STR);
            controllerTwo.gameObject.transform.parent = canvas.transform;
            controllerTwo.OnSaveChanges += (x) => { };

            mvc.Initialize();

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