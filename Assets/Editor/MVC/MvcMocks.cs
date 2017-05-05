namespace MVC {
    public class TestControllerOne : Controller<TestModel> {
        public override void Display() {}

        public void ChangeTestStr(string test) {
            Context.TestString = test;
            SaveContext();
        }
    }

    public class TestControllerTwo : Controller<TestModel> {
        public override void Display() { }

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
