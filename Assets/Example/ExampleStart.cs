using UnityEngine;

namespace MVC.Example
{
    public class ExampleStart : MonoBehaviour
    {
        [SerializeField]
        private TeamController _startingController;

        void Start()
        {
            _startingController.MonsterList();
        }
    }
}

