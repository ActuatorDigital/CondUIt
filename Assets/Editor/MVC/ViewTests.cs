using UnityEngine;
using NUnit.Framework;

namespace MVC {

    public class ViewTests {

        [Test]
        public void ControllerCanCallItsViews() {
            // Arrange = First Controller and view.
            var canvasGo = new GameObject();
            var mvc = canvasGo.AddComponent<MVCFramework>();

            var controllerGoOne = new GameObject();
            controllerGoOne.transform.parent = canvasGo.transform;
            var controllerOne = controllerGoOne.AddComponent<TestExclusiveControllerOne>();
            AddPeerViews(controllerGoOne.transform);

            var viewGOOne = new GameObject().AddComponent<TestExclusiveViewOne>();
            viewGOOne.transform.parent = controllerGoOne.transform;

            var initialString = "Test";
            var modelObj = new TestModel(initialString);

            mvc.Initialize<TestExclusiveControllerOne>();

            // Act - Call view from controller.
            controllerOne.View<TestExclusiveViewOne>(modelObj);

            // Assert.
            Assert.AreNotEqual(modelObj.TestString, initialString);
        }

        [Test]
        public void CanHideViews() {
            // Arrange.
            var canvasGo = new GameObject();

            var controllerGoOne = new GameObject();
            controllerGoOne.transform.parent = canvasGo.transform;
            controllerGoOne.AddComponent<TestControllerOne>();
            AddPeerViews(controllerGoOne.transform);

            var viewGo = new GameObject();
            var view = viewGo.AddComponent<TestView>();
            viewGo.transform.parent = controllerGoOne.transform;

            // Act.
            view.Hide();

            // Assert.
            Assert.IsFalse(viewGo.activeInHierarchy);

            GameObject.DestroyImmediate(canvasGo);

        }

        [Test]
        public void CanShowViews() {
            // Arrange.
            var canvasGo = new GameObject();

            var controllerGoOne = new GameObject();
            controllerGoOne.transform.parent = canvasGo.transform;
            controllerGoOne.AddComponent<TestControllerOne>();
            AddPeerViews(controllerGoOne.transform);

            var viewGo = new GameObject();
            var view = viewGo.AddComponent<TestView>();
            viewGo.transform.parent = controllerGoOne.transform;

            // Act.
            view.Render(); 

            // Assert.
            Assert.IsTrue(viewGo.activeInHierarchy);

            GameObject.DestroyImmediate(canvasGo);

        }

        [Test]
        public void CanRenderWithManyControllers() {
            //Arrange.
            var canvasGo = new GameObject();

            var controllerGoOne = new GameObject();
            controllerGoOne.transform.parent = canvasGo.transform;
            controllerGoOne.AddComponent<TestControllerOne>();
            AddPeerViews(controllerGoOne.transform);

            var controllerGoTwo = new GameObject();
            controllerGoTwo.transform.parent = canvasGo.transform;
            controllerGoOne.AddComponent<TestControllerOne>();
            AddPeerViews(controllerGoTwo.transform);

            var viewGo = new GameObject();
            var view = viewGo.AddComponent<TestView>();
            viewGo.transform.parent = controllerGoOne.transform;

            // Act.
            view.Render();

            Assert.IsTrue(viewGo.activeInHierarchy);
            
            GameObject.DestroyImmediate(canvasGo);
        }

        [Test]
        public void ExclusiveControllerHidesOthers() {

            // Arrange First Controller and view.
            var canvasGo = new GameObject();
            var mvc = canvasGo.AddComponent<MVCFramework>();

            var controllerGoOne = new GameObject();
            controllerGoOne.transform.parent = canvasGo.transform;
            var controllerOne = controllerGoOne.AddComponent<TestExclusiveControllerOne>();
            AddPeerViews(controllerGoOne.transform);

            var viewGOOne = new GameObject();
            viewGOOne.AddComponent<TestExclusiveViewOne>();
            viewGOOne.transform.parent = controllerGoOne.transform;

            // Arrange Second Controller and view.
            var controllerGoTwo = new GameObject();
            controllerGoTwo.transform.parent = canvasGo.transform;
            controllerGoTwo.AddComponent<TestExclusiveControllerTwo>();
            AddPeerViews(controllerGoTwo.transform);
            
            var viewGOTwo = new GameObject();
            viewGOTwo.AddComponent<TestExclusiveViewTwo>();
            viewGOTwo.transform.parent = controllerGoOne.transform;

            mvc.Initialize<TestExclusiveControllerOne>();

            // Act.
            controllerOne.Action<TestExclusiveControllerTwo>().FunctionOnControllerTwo();

            Assert.IsTrue(viewGOTwo.activeInHierarchy);
            Assert.IsTrue(!viewGOOne.activeInHierarchy);
            
            GameObject.DestroyImmediate(canvasGo);
        }

        private void AddPeerViews(Transform parent) {
            var childOneGo = new GameObject();
            childOneGo.transform.parent = parent;
            childOneGo.AddComponent<TestView>();
            var childTwoGo = new GameObject();
            childTwoGo.transform.parent = parent;
            childTwoGo.AddComponent<TestView>();
            var childThreeGo = new GameObject();
            childThreeGo.transform.parent = parent;
            childThreeGo.AddComponent<TestView>();
        }
    }
}