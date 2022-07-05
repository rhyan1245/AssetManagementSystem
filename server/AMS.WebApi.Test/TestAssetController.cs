using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.WebApi.Controllers;
using AMS.WebApi.DAL;
using AMS.WebApi.Models;
using AMS.WebApi.Models.EnumTypes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace AMS.WebApi.Test;

[TestFixture]
public class TestAssetController
{
  private Mock<IAssetRepository> _repoMock;

  [SetUp]
  public void Init()
  {
    _repoMock = new Mock<IAssetRepository>();
  }

  [Test]
  public async Task GetAssets_ShouldReturnAllAssets()
  {
    _repoMock.Setup(r => r.GetAssetsAsync()).ReturnsAsync(new List<Asset> {
        new Asset
        {
            Id = 1,
          Name = "Laptop",
          Type = AssetType.Hardware,
          Description = "Ullamco excepteur consequat mollit amet tempor."
        },
        new Asset
        {
            Id = 2,
          Name = "Mouse",
          Type = AssetType.Hardware,
          Description = "Ut magna ex aliqua pariatur cillum."
        },
        new Asset
        {
            Id = 3,
          Name = "Windows 10",
          Type = AssetType.Software,
          Description = "Eiusmod aute exercitation deserunt sit elit pariatur est eiusmod non ut incididunt amet."
        }
    });
    var controller = new AssetController(_repoMock.Object);

    // Act
    var response = await controller.GetAssets();
    var content = response.Result as OkObjectResult;
    var assets = content.Value as List<Asset>;

    // Assert
    _repoMock.Verify(r => r.GetAssetsAsync());
    Assert.That(assets, Is.Not.Null);
    Assert.That(3, Is.EqualTo(assets[2].Id));
    Assert.That("Windows 10", Is.EqualTo(assets[2].Name));
    Assert.That(AssetType.Software, Is.EqualTo(assets[2].Type));
    Assert.That("Eiusmod aute exercitation deserunt sit elit pariatur est eiusmod non ut incididunt amet.", Is.EqualTo(assets[2].Description));
  }

  [Test]
  public async Task GetAsset_ShouldReturnAsset()
  {
    // Arrange
    _repoMock.Setup(r => r.GetAssetByIdAsync(1)).ReturnsAsync(new Asset
    {
      Id = 1,
      Name = "Laptop",
      Type = AssetType.Hardware,
      Description = "Ullamco excepteur consequat mollit amet tempor."
    });
    var controller = new AssetController(_repoMock.Object);

    // Act
    var response = await controller.GetAsset(1);
    var content = response.Result as OkObjectResult;
    var asset = content.Value as Asset;

    // Assert
    _repoMock.Verify(r => r.GetAssetByIdAsync(1));
    Assert.That(asset, Is.Not.Null);
    Assert.That(1, Is.EqualTo(asset.Id));
    Assert.That("Laptop", Is.EqualTo(asset.Name));
    Assert.That(AssetType.Hardware, Is.EqualTo(asset.Type));
    Assert.That("Ullamco excepteur consequat mollit amet tempor.", Is.EqualTo(asset.Description));
  }

  [Test]
  public async Task CreateAsset_ShouldCreateAsset()
  {
    // Arrange
    Asset _asset = new Asset
    {
      Id = 4,
      Name = "Keyboard",
      Type = AssetType.Hardware,
      Description = "Velit commodo aliquip adipisicing cupidatat sint Lorem laborum ipsum minim ad mollit."
    };

    _repoMock.Setup(r => r.AddAsset(_asset));
    var controller = new AssetController(_repoMock.Object);

    // Act
    var response = await controller.CreateAsset(_asset);
    var content = response.Result as CreatedAtActionResult;
    var asset = content.Value as Asset;

    // Assert
    _repoMock.Verify(r => r.AddAsset(_asset));
    Assert.That(asset, Is.Not.Null);
    Assert.That(_asset.Id, Is.EqualTo(asset.Id));
    Assert.That(_asset.Name, Is.EqualTo(asset.Name));
    Assert.That(_asset.Type, Is.EqualTo(asset.Type));
    Assert.That(_asset.Description, Is.EqualTo(asset.Description));
  }

  [Test]
  public async Task UpdateAsset_ShouldUpdateAsset()
  {
    Asset _asset = new Asset
    {
      Id = 1,
      Description = "Updated description"
    };

    _repoMock.Setup(r => r.UpdateAsset(_asset));
    _repoMock.Setup(r => r.GetAssetByIdAsync(1)).ReturnsAsync(new Asset
    {
      Id = 1,
      Name = "Laptop",
      Type = AssetType.Hardware,
      Description = "Ullamco excepteur consequat mollit amet tempor."
    });
    var controller = new AssetController(_repoMock.Object);

    // Act
    var response = await controller.UpdateAsset((int)_asset.Id, _asset);
    var content = response.Result as OkObjectResult;
    var asset = content.Value as Asset;

    // Assert
    Assert.That(asset, Is.Not.Null);
    Assert.That(_asset.Id, Is.EqualTo(asset.Id));
    Assert.That("Laptop", Is.EqualTo(asset.Name));
    Assert.That(AssetType.Hardware, Is.EqualTo(asset.Type));
    Assert.That(_asset.Description, Is.EqualTo(asset.Description));
  }

  [Test]
  public async Task UpdateAsset_ShouldReturnNotFound()
  {
    Asset _asset = new Asset
    {
      Id = 5,
      Description = "Updated description"
    };

    _repoMock.Setup(r => r.UpdateAsset(_asset));
    _repoMock.Setup(r => r.GetAssetByIdAsync(5));
    var controller = new AssetController(_repoMock.Object);

    // Act
    var response = await controller.UpdateAsset((int)_asset.Id, _asset);

    // Assert
    Assert.That(response.Result, Is.Not.Null);
    Assert.That(response.Result, Is.InstanceOf<NotFoundResult>());
  }

  [Test]
  public async Task DeleteAsset_ShouldDeleteAsset()
  {
    _repoMock.Setup(r => r.GetAssetByIdAsync(1)).ReturnsAsync(new Asset
    {
      Id = 1,
      Name = "Laptop",
      Type = AssetType.Hardware,
      Description = "Ullamco excepteur consequat mollit amet tempor."
    });
    var controller = new AssetController(_repoMock.Object);

    // Act
    var response = await controller.DeleteAsset(1);

    // Assert
    Assert.That(response, Is.Not.Null);
    Assert.That(response, Is.InstanceOf<NoContentResult>());
  }

}