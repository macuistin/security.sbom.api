using Newtonsoft.Json;

namespace security.sbom.Entities;
public class Vertex
{
    public string Id { get; set; }

    public string Label { get; set; }

    public string Name { get; set; }

    public string Version { get; set; }

    public string PartitionKey { get; set; } = "PartitionKey";
}
