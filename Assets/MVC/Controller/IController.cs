using System;

public interface IController {
    void Init(Action<IModel> context, IModel model);
    void Display();
}