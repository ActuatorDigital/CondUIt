using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Model<P> : IModel 
        where P : IModel {
    public P Parent { get; set; }
    public Model(P parent) {
        Parent = parent;
    }
}

public abstract class ModelRoot : IModel { }
