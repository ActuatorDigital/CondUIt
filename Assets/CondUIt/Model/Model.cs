
namespace Conduit {

    public abstract class Model<P> : IModel
            where P : IModel {
        
        public P Parent { get; set; }
        public Model() { }
        public Model(P parent) {
            Parent = parent;
        }
    }

    public abstract class ModelRoot : IModel {
        public ModelRoot() { }
    }
}