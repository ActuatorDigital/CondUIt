using MVC;

public class TestExclusiveControllerOne : Controller {
    public override bool Exclusive {
        get { return true; }
    }

    public override void LoadServices(IServicesLoader services) { }

    public void Main() { }
}

public class TestExclusiveControllerTwo : Controller {
    public override bool Exclusive {
        get { return true; }
    }

    public void FunctionOnControllerTwo() { }

    public override void LoadServices(IServicesLoader services) { }
}

public class TestExclusiveViewOne : View<TestModel, TestExclusiveControllerOne> {
    public override bool IsPartial {
        get { return false; }
    }

    protected override void ClearElements() { }
    protected override void LoadElements() {
        Model.TestString = "Changed";
    }
}

public class TestExclusiveViewTwo : View<TestModel, TestExclusiveControllerTwo> {
    public override bool IsPartial {
        get { return false; }
    }

    protected override void ClearElements() { }
    protected override void LoadElements() { }
}

public class TestControllerOne : Controller {

    public string TestString { get; set; }

    public override bool Exclusive {
        get { return false; }
    }

    public void ChangeTestStr(string test) {
        TestString = test;
    }

    public override void LoadServices(IServicesLoader services) { }

    public void Main() { }
}

public class TestControllerTwo : Controller {
    
    public override bool Exclusive {
        get { return false; }
    }

    public override void LoadServices(IServicesLoader services) { }

    internal void TriggerAction(string postTest) {
        Action<TestControllerOne>().ChangeTestStr(postTest);
    }
}

public class TestView : View<TestModel, TestControllerOne>
{
    public override bool IsPartial {
        get { return true; }
    }

    protected override void ClearElements() {
        gameObject.SetActive(false);
    }

    protected override void LoadElements() {
        gameObject.SetActive(true);
    }
}

public class TestModel : IModel {
    public string TestString;
    public TestModel(string testString) { TestString = testString; }
}