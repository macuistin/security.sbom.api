namespace security.sbom.Entities;
public class Service : Vertex
{
    public Service(string name, string version, string ns, string created)
    {
        Id = $"{name}@{version}";
        Label = Id;
        Name = name;
        Version = version;
        Namespace = ns;
        Created = created;
    }

    public string Namespace { get; set; }

    public string Created { get; set; }
}
