
using System;

namespace MVC {

    public class TestExclusiveControllerOne : Controller<TestModel> {
        public override bool Exclusive {
            get { return true; }
        }

        public override void Display() { }

        public override void LoadServices(IServicesLoader services) { }
    }

    public class TestExclusiveControllerTwo : Controller<TestModel> {
        public override bool Exclusive {
            get { return true; }
        }

        public override void Display() { }
        public void FunctionOnControllerTwo() { }

        public override void LoadServices(IServicesLoader services) { }
    }

    public class TestExclusiveViewOne : View<TestModel, TestExclusiveControllerOne> {
        protected override void ClearElements() { }
        protected override void LoadElements() { }
    }

    public class TestExclusiveViewTwo : View<TestModel, TestExclusiveControllerTwo> {
        protected override void ClearElements() { }
        protected override void LoadElements() { }
    }

    public class TestControllerOne : Controller<TestModel> {
        public override bool Exclusive {
            get { return false; }
        }

        public override void Display() {}

        public void ChangeTestStr(string test) {
            Context.TestString = test;
            SaveContext();
        }

        public override void LoadServices(IServicesLoader services) { }
    }

    public class TestControllerTwo : Controller<TestModel> {
        public override bool Exclusive {
            get { return false; }
        }

        public override void Display() { }

        public override void LoadServices(IServicesLoader services) { }

        internal void TriggerAction(string postTest) {
            Action<TestControllerOne>("ChangeTestStr", Context, postTest);
        }
    }

    public class TestView : View<TestModel, TestControllerOne>
    {
        protected override void ClearElements()
        {
            gameObject.SetActive(false);
        }

        protected override void LoadElements()
        {
            gameObject.SetActive(true);
        }
    }

    public class TestModel : IModel {
        public string TestString;
        public TestModel(string testString) { TestString = testString; }
    }
}
