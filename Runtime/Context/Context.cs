
namespace AIR.Conduit {

    public abstract class Context<P> : IContext
            where P : IContext {

        public P Parent { get; set; }
        public Context() { }
        public Context(P parent) {
            Parent = parent;
        }
    }

    public abstract class ContextRoot : IContext {
        public ContextRoot() { }
    }
}