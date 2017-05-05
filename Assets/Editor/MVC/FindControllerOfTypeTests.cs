using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

namespace MVC {

    public class FindControllerOfTypeTests {

        [Test]
        public void NoResultsWhenMissing() {
            //Arrange
            var gameObject = new GameObject();

            //Act
            var controller = gameObject.FindControllerOfType(typeof(TestModel));

            //Assert
            Assert.IsNull(controller);

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void FindsController() {
            //Arrange
            var gameObject = new GameObject();
            gameObject.AddComponent<TestControllerOne>();

            //Act
            var controller = gameObject.FindControllerOfType(typeof(TestModel));

            //Assert
            Assert.IsNotNull(controller);

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

    }

}
