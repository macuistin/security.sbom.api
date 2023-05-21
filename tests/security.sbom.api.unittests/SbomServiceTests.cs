using ExRam.Gremlinq.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Sbom.Parsers.Spdx22SbomParser.Entities;
using Moq;
using security.sbom.Entities;
using security.sbom.Services;
using System.Linq.Expressions;

namespace security.sbom.api.unittests;

public class SbomServiceTests
{
    [Fact]
    public async Task SaveSpbxAsync_AddsService_WhenServiceDoesNotExist()
    {
        // Arrange
        var gremlinQuerySourceMock = new Mock<IGremlinQuerySource>();
        var vertexQueryMock = new Mock<IVertexGremlinQuery>();
        var loggerMock = new Mock<ILogger<SbomService>>();

        gremlinQuerySourceMock.Setup(g => g.V<Service>()).Returns(vertexQueryMock.Object);
        vertexQueryMock.Setup(g => g.Where(It.IsAny<Expression<Func<Service, bool>>>())).Returns(vertexQueryMock.Object);
        vertexQueryMock.Setup(g => g.FirstOrDefaultAsync(default)).ReturnsAsync((Service)null);
        vertexQueryMock.Setup(g => g.AddV(It.IsAny<Service>())).Returns(vertexQueryMock.Object);
        vertexQueryMock.Setup(g => g.FirstAsync(default)).ReturnsAsync(new Service());

        var sbomService = new SbomService(gremlinQuerySourceMock.Object, loggerMock.Object);

        var document = new SPDX22Document
        {
            DocumentName = "Test Document",
            DocumentNamespace = "Test Namespace",
            CreationInfo = new CreationInfo
            {
                Created = DateTime.Now.ToString("s")
            }
        };

        // Act
        await sbomService.SaveSpbxAsync("Test Type", document);

        // Assert
        // Verify that AddV was called on the gremlin query source
        gremlinQuerySourceMock.Verify(g => g.AddV(It.IsAny<Service>()), Times.Once);
    }
}