using MVC.Models;
using System;

namespace MVC.Controllers
{
    public interface IController
    {
        void Init(Action<IModel> context, IModel model);
        void Display();
    }
}
