using Conduit;

public class TestExclusiveControllerOne : FirstController {
    public override bool Exclusive {
        get { return true; }
    }

    public override void Display() {
        UnityEngine.Debug.LogWarning(
            "TODO: Make this implicit "+
            "through view generic type binding!");
        View<TestExclusiveViewOne>(new TestModel(""));
     }

    public override void LoadServices(IServiceLoader services) { }

}

public class TestExclusiveControllerTwo : Controller {
    public override bool Exclusive {
        get { return true; }
    }

    public override void Display() { }
    public void FunctionOnControllerTwo() { }

    public override void LoadServices(IServiceLoader services) { }
}

public class TestExclusiveViewOne : View<TestModel, TestExclusiveControllerOne> {
    public override bool IsPartial { get { return false; } } 

    protected override void ClearElements() { }
    protected override void LoadElements() {
        ViewModel.TestString = "Changed";
    }
}

public class TestExclusiveViewTwo : View<TestModel, TestExclusiveControllerOne> {
    public override bool IsPartial { get { return false; } } 

    protected override void ClearElements() { }
    protected override void LoadElements() { }
}

public class TestControllerOne : FirstController {

    public string TestString { get; set; }

    public override bool Exclusive {
        get { return false; }
    }

    public override void Display() {}

    public void ChangeTestStr(string test) {
        TestString = test;
    }

    public override void LoadServices(IServiceLoader services) { }
}

public class TestControllerTwo : Controller {

    public override bool Exclusive {
        get { return false; }
    }

    public override void Display() { }

    public override void LoadServices(IServiceLoader services) { }

    internal void TriggerAction(string postTest) {
        Redirect<TestControllerOne>().ChangeTestStr(postTest);
    }
}

public class TestView : View<TestModel, TestControllerOne>
{
    public override bool IsPartial { get { return false; } }

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
