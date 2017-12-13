
using JsonFx.Json;

namespace MVC {

    public abstract class Model<P> : IModel
            where P : IModel {

        [JsonIgnore]
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