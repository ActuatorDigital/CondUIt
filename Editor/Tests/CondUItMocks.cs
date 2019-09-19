using Conduit;

public class TestExclusiveControllerOne : InitialController {

    public override void Routed() {
        UnityEngine.Debug.LogWarning(
            "TODO: Make this implicit " +
            "through view generic type binding!");
        View<TestExclusiveViewOne>(new TestModel(""));
    }

    public override void LoadServices(IServiceLoader services) { }

}

public class TestExclusiveControllerTwo : Controller {

    public override void Routed() { }
    public void FunctionOnControllerTwo() { }

    public override void LoadServices(IServiceLoader services) { }
}

public class TestExclusiveViewOne : View<TestModel, TestExclusiveControllerOne> {
    //public override bool DoesntHideNeighbours { get { return false; } }

    protected override void ClearElements() { }
    protected override void LoadElements() {
        ViewModel.TestString = "Changed";
    }
}

public class TestExclusiveViewTwo : View<TestModel, TestExclusiveControllerOne> {
    //public override bool DoesntHideNeighbours { get { return false; } }

    protected override void ClearElements() { }
    protected override void LoadElements() { }
}

public class TestControllerOne : InitialController {

    public string TestString { get; set; }

    public override void Routed() { }

    public void ChangeTestStr(string test) {
        TestString = test;
    }

    public override void LoadServices(IServiceLoader services) { }
}

public class TestControllerTwo : Controller {

    public override void Routed() { }

    public override void LoadServices(IServiceLoader services) { }

    internal void TriggerAction(string postTest) {
        Route<TestControllerOne>().ChangeTestStr(postTest);
    }
}

public class TestView : View<TestModel, TestControllerOne> {
    //public override bool DoesntHideNeighbours => false;

    protected override void ClearElements() {
        gameObject.SetActive(false);
    }

    protected override void LoadElements() {
        gameObject.SetActive(true);
    }
}

public class TestModel : IContext {
    public string TestString;
    public TestModel(string testString) { TestString = testString; }
}
