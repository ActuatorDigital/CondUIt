using UnityEngine;
using NUnit.Framework;

namespace MVC {

    public class ViewTests {

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

            view.transform.SetAsFirstSibling();
            controllerGoOne.transform.SetAsFirstSibling();

            // Act.
            view.Render();

            // Assert.
            var parentIndex = controllerGoOne.transform.GetSiblingIndex() + 1;
            var parentCount = canvasGo.transform.childCount;
            Assert.AreEqual(parentIndex, parentCount);

            var siblingIndex = view.transform.GetSiblingIndex() + 1;
            var siblingCount = controllerGoOne.transform.childCount;
            Assert.AreEqual(siblingIndex, siblingCount);

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