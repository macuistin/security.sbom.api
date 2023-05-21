using ExRam.Gremlinq.Core;
using security.sbom.Entities;

namespace security.sbom.Services;

public class SbomService : ISbomService
{
    private readonly IGremlinQuerySource _g;
    private readonly ILogger<SbomService> _logger;

    public SbomService(IGremlinQuerySource g, ILogger<SbomService> logger)
    {
        _g = g;
        _logger = logger;
    }

    public async Task SaveSpbxAsync(string type, SPDX22Document document)
    {
        _logger.LogInformation($"Processing for dependency: {document.DocumentName}");

        var service = new Service(document.DocumentName, "1.0", document.DocumentNamespace, document.CreationInfo.Created);
        await AddService(_g, service);

        await ClearDependencies(_g, service);
        await AddDependentPackagesToGraph(document, _g, service);
        
    }

    private async Task AddService(IStartGremlinQuery g, Service service)
    {
        var serviceVertex = await g.V<Service>().Where(v => v.Id == service.Id).FirstOrDefaultAsync();

        if (serviceVertex == null)
        {
            await g.AddV(service).FirstAsync();
        }
        else
        {
            _logger.LogInformation("Service already exists");
        }
    }

    private async Task ClearDependencies(IStartGremlinQuery g, Vertex service)
    {
        _logger.LogInformation("Clearing the dependencies...");

        var hasOutgoingEdges = await g.V(service.Id).OutE<DependsOn>().FirstOrDefaultAsync();

        if (hasOutgoingEdges != null)
        {
            await g.V(service.Id).OutE<DependsOn>().Drop().FirstOrDefaultAsync();
        }
        else
        {
            _logger.LogInformation("No outgoing edges to drop");
        }
    }

    private async Task AddDependentPackagesToGraph(
        SPDX22Document spdx22Document,
        IStartGremlinQuery startGremlinQuery,
        Vertex service)
    {
        foreach (var docPackage in spdx22Document.Packages)
        {
            if (docPackage.SpdxId == "SPDXRef-RootPackage")
                continue;

            var dependentPackage = new Package(docPackage.Name, docPackage.VersionInfo, docPackage.SpdxId, docPackage.ExternalReferences);

            await AddPackage(startGremlinQuery, dependentPackage);
            await AddEdgeToDependency(startGremlinQuery, service, dependentPackage);
        }
    }

    private async Task AddPackage(IStartGremlinQuery startGremlinQuery, Package dependency)
    {
        var packageVertex = await startGremlinQuery.V<Package>().Where(v => v.Id == dependency.Id)
            .FirstOrDefaultAsync();

        if (packageVertex == null)
        {
            await startGremlinQuery.AddV(dependency).FirstAsync();
        }
    }

    private async Task AddEdgeToDependency(IStartGremlinQuery startGremlinQuery, Vertex service, Vertex dependentPackage)
    {
        await startGremlinQuery.V(service.Id).AddE<DependsOn>().To(__ => __.V(dependentPackage.Id)).FirstAsync();
    }
}
